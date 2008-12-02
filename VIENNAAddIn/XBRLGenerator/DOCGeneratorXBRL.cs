/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using EA;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Schema;
using VIENNAAddIn.Utils;
using VIENNAAddIn.Exceptions;
using VIENNAAddIn.CCTS;
using VIENNAAddIn.constants;

namespace VIENNAAddIn.XBRLGenerator
{
    public partial class DOCGeneratorXBRL : Form, GeneratorCallBackInterface
    {
        #region Variable
        private Repository repository;
        private GeneratorCallBackInterface caller;
        private bool withGUI;
        private String scope;
        //private const String strComplexType = "ComplexType";
        internal string path = "";
        private Label label1;
        private Label selectedBIELibrary;
        private RichTextBox statusTextBox;
        private ProgressBar progressBar1;
        private Button btnCancel;
        private Button btnGenerateSchema;
        private FolderBrowserDialog dlgSavePath;
        
        //This ArrayList holds a List of all auxilliary schmeas
        //that had to be created for this schema to be valid
        private System.Collections.ArrayList alreadyCreatedSchemas = new ArrayList();

        static int countBIEImports = 0;
        static int countQDTImports = 0;
        static int countUDTImports = 0;
        static int countENUMImports = 0;

        private String targetNameSpacePrefix = "doc";
        #endregion

        #region Constructor
        public DOCGeneratorXBRL(EA.Repository repository, String scope)
        {
            InitializeComponent();

            this.repository = repository;
            this.scope = scope;
            this.withGUI = true;
            this.setActivePackageLabel();
        }

        public DOCGeneratorXBRL(EA.Repository repository, EA.Package package, string path, GeneratorCallBackInterface caller)
        {
            this.caller = caller;
            this.repository = repository;
            this.scope = package.PackageID.ToString();
            this.withGUI = false;
            //Set the path from the caller because the CDT Generator now operates in not visible
            //mode and hence no path is determined in this class
            this.path = path;
        }
        #endregion

        #region Implement
        /// <summary>
        /// Append an error message to the status box
        /// </summary>
        /// <param name="msg"></param>
        private void appendErrorMessage(String msg, String packageName)
        {
            if (caller != null)
            {
                caller.appendMessage("error", msg, this.getPackageName());
            }
            else
            {
                if (this.withGUI)
                {
                    this.statusTextBox.Text += "ERROR: (Package: " + packageName + ") " + msg + "\n\n";
                }
            }
        }

        /// <summary>
        /// Show a info message in the status box
        /// </summary>
        /// <param name="msg"></param>
        private void appendInfoMessage(String msg, String packageName)
        {
            if (caller != null)
            {
                caller.appendMessage("info", msg, this.getPackageName());
            }
            else
            {
                if (this.withGUI)
                {
                    this.statusTextBox.Text += "INFO: (Package: " + packageName + ") " + msg + "\n\n";
                }
            }
        }

        /// <summary>
        /// Show a warn message in the status box
        /// </summary>
        /// <param name="msg"></param>
        private void appendWarnMessage(String msg, String packageName)
        {
            if (caller != null)
            {
                caller.appendMessage("warn", msg, this.getPackageName());
            }
            else
            {
                if (this.withGUI)
                {    
                    this.statusTextBox.Text += "WARN: (Package: " + packageName + ") " + msg + "\n\n";
                }
            }
        }
        
        /// <summary>
        /// Is called from extern to append a message to this GUI
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        public void appendMessage(String type, String message, String packageName)
        {
            if (type == "info")
                this.appendInfoMessage(message, packageName);
            else if (type == "warn")
                this.appendWarnMessage(message, packageName);
            else if (type == "error")
                this.appendErrorMessage(message, packageName);

        }

        /// <summary>
        /// Performs a step with the progress bar
        /// </summary>
        public void performProgressStep()
        {
            if (this.withGUI)
            {
                this.progressBar1.PerformStep();
            }
            else
            {
                //Is there a caller for this class?
                if (caller != null)
                    caller.performProgressStep();
            }
        }
        #endregion

        public void resetGenerator(String scope)
        {
            this.scope = scope;
            //this.alreadyCreatedSchemas = new ArrayList();
            this.progressBar1.Value = this.progressBar1.Minimum;
            this.statusTextBox.Text = "";
            this.setActivePackageLabel();
            this.alreadyCreatedSchemas = new ArrayList();
        }

        private void setActivePackageLabel()
        {
            EA.Package p = this.repository.GetPackageByID(Int32.Parse(this.scope));
            this.selectedBIELibrary.Text = p.Element.Name + "<<" + p.Element.Stereotype.ToString() + ">>";
        }

        public System.Collections.ICollection generateXBRLschema(EA.Package pkg)
        {

            #region main schema
            XmlSchema schema = new XmlSchema();
            addXBRLNamespace(schema);
            addXBRLImport(schema);
            string schemaLocation = XMLTools.getSchemaNameWithoutExtention(this.repository.GetPackageByID(Int32.Parse(this.scope))) + "-structure" + ".xsd";
            //addXBRLInclude(schema, schemaLocation);
            ////create annotation for the linkbase
            addAnnotationLinkbase(schema);
            #endregion 

            #region schema for structure
            //schema for BBIE
            XmlSchema ctSchema = new XmlSchema();
            addXBRLNamespace(ctSchema);
            addXBRLImport(ctSchema);
            addXBRLInclude(ctSchema, XMLTools.getSchemaName(this.repository.GetPackageByID(Int32.Parse(this.scope))));
            this.performProgressStep();
            #endregion

            #region Create Taxonomy schema
            this.appendInfoMessage("Generating schema file...", pkg.Name);
            int elementCount = 0;
            //Iterate through the ABIEs
            foreach (EA.Element element in pkg.Elements)
            {
                elementCount++;
                if (element.Stereotype.Equals(CCTS_Types.ABIE.ToString()) && isRootElement(element))
                {
                    try
                    {
                        getSchemaElement(element, schema);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //After every 20th iteration we  perform one progress step
                if (elementCount % 20 == 0)
                    this.performProgressStep();
            }
            #endregion

            //Validate the Schema
            XmlSchemaSet xsdSet = new XmlSchemaSet();
            xsdSet.XmlResolver = null;
            xsdSet.ValidationEventHandler += new ValidationEventHandler(ValidationCallbackOne);
            
            try
            {
                //Add the actual schema
                xsdSet.Add(schema);
            }
            catch (XmlSchemaException xse)
            {
                throw xse;
            }

            String filename = "";
            String schemaPath = "";
            String schemaName = "";
            schemaPath = XMLTools.getSavePathForSchema(this.repository.GetPackageByID(pkg.PackageID), repository);
            schemaName = XMLTools.getSchemaNameWithoutExtention(this.repository.GetPackageByID(pkg.PackageID));
            System.IO.Directory.CreateDirectory(path + schemaPath);

            this.appendInfoMessage("Saving schema files...", pkg.Name);
            filename = path + schemaPath + XMLTools.getSchemaName(this.repository.GetPackageByID(Int32.Parse(this.scope)));
            WriteSchemaToFile(filename, schema);

            return xsdSet.Schemas();
        }

        private void generateTuple(Element element, XmlSchema schema, ArrayList arrName)
        {
            //create BBIE
            XmlSchemaSequence sequence = new XmlSchemaSequence();

            foreach (EA.Attribute bbie in element.Attributes)
            {
                XmlSchemaElement el = new XmlSchemaElement();
                string bbieName = XMLTools.getXBRLBbieName(bbie, arrName);
                el.RefName = new XmlQualifiedName(bbieName, schema.TargetNamespace); //Ref attribute
                el.MaxOccurs = 1;
                el.MinOccurs = 0;
                sequence.Items.Add(el);
            }

            foreach (EA.Connector con in element.Connectors)
            {
                if (isOutgoing(con, element))
                {
                    XmlSchemaElement el = new XmlSchemaElement();

                    string conName = "";

                    if (con.ClientEnd.Role != "")
                        conName = XMLTools.getXBRLAsbieName(arrName) + "." + con.ClientEnd.Role + "." + this.repository.GetElementByID(con.ClientID).Name;
                    else
                        conName = XMLTools.getXBRLAsbieName(arrName) + "." + this.repository.GetElementByID(con.ClientID).Name;

                    el.RefName = new XmlQualifiedName(conName, schema.TargetNamespace); //Ref attribute
                    el.MaxOccurs = 1;
                    el.MinOccurs = 0;
                    sequence.Items.Add(el);
                }
            }

            //create complex type;
            XmlSchemaComplexContent complexContent = new XmlSchemaComplexContent();

            XmlSchemaComplexContentRestriction restrict = new XmlSchemaComplexContentRestriction();
            restrict.BaseTypeName = new XmlQualifiedName("stringItemType","http://www.xbrl.org/2003/instance");

            XmlSchemaComplexType complexType = new XmlSchemaComplexType();
            complexType.IsMixed = false;
            complexType.Name = XMLTools.getXBRLAsbieName(arrName);//element.Name;

            restrict.Particle = sequence;
            complexContent.Content = restrict;
            complexType.ContentModel = complexContent;

            schema.Items.Add(complexType);
        }

        private bool isRootElement(Element rootEl)
        {
            foreach (EA.TaggedValue tv in rootEl.TaggedValues)
            {
                if ( tv.Name.Equals("isRoot", StringComparison.OrdinalIgnoreCase) && 
                    (tv.Value.Equals("true", StringComparison.OrdinalIgnoreCase) || (tv.Value.Equals("1", StringComparison.OrdinalIgnoreCase))))
                    return true;
            }
            return false;
        }

        private void WriteSchemaToFile(string filename, XmlSchema schema)
        {
            Stream outputStream = System.IO.File.Open(filename, FileMode.Create);
            schema.Write(outputStream);
            outputStream.Close();
        }

        private void addAnnotationLinkbase(XmlSchema schema)
        {
            XmlSchemaAnnotation ann = new XmlSchemaAnnotation();
            XmlSchemaAppInfo appInfo = new XmlSchemaAppInfo();

            XmlDocument doc = new XmlDocument();
            XmlDocument doc1 = new XmlDocument();

            XmlNode[] annNodes = new XmlNode[2];
            ////XmlNode node = doc.CreateElement("link", "linkbaseRef", "http://www.xbrl.org/2003/linkbase");
            //doc.CreateElement("link", "linkbaseRef", "http://www.xbrl.org/2003/linkbase");
            //XmlAttribute attr = doc.CreateAttribute("type", "http://www.w3.org/1999/xlink");
            //attr.Value = "simple";

            //XmlNode node = doc;

            //presentation
            XmlElement e1 = doc.CreateElement("xbrll", "linkbaseRef", "http://www.xbrl.org/2003/linkbase");
            string href = XMLTools.getSchemaNameWithoutExtention(this.repository.GetPackageByID(Int32.Parse(scope))) + "-presentation.xml";
            e1.SetAttribute("type", "http://www.w3.org/1999/xlink", "simple");
            e1.SetAttribute("href", "http://www.w3.org/1999/xlink", href);
            e1.SetAttribute("title", "http://www.w3.org/1999/xlink", "Presentation Links, all");
            e1.SetAttribute("role", "http://www.w3.org/1999/xlink", "http://www.xbrl.org/2003/role/presentationLinkbaseRef");
            e1.SetAttribute("arcrole", "http://www.w3.org/1999/xlink", "http://www.w3.org/1999/xlink/properties/linkbase");
            annNodes[0] = e1;

            //label
            XmlElement e = doc.CreateElement("xbrll", "linkbaseRef", "http://www.xbrl.org/2003/linkbase");
            href = XMLTools.getSchemaNameWithoutExtention(this.repository.GetPackageByID(Int32.Parse(scope))) + "-label.xml";
            e.SetAttribute("type", "http://www.w3.org/1999/xlink", "simple");
            e.SetAttribute("href", "http://www.w3.org/1999/xlink", href);
            e.SetAttribute("title", "http://www.w3.org/1999/xlink", "Label Links, all");
            e.SetAttribute("role", "http://www.w3.org/1999/xlink", "http://www.xbrl.org/2003/role/labelLinkbaseRef");
            e.SetAttribute("arcrole", "http://www.w3.org/1999/xlink", "http://www.w3.org/1999/xlink/properties/linkbase");
            annNodes[1] = e;

            appInfo.Markup = annNodes;
            ann.Items.Add(appInfo);
            schema.Items.Add(ann);
        }

        private void addXBRLImport(XmlSchema schema)
        {
            XmlSchemaImport import = new XmlSchemaImport();
            import.Namespace = "http://www.xbrl.org/2003/instance";
            import.SchemaLocation = "http://www.xbrl.org/2003/xbrl-instance-2003-12-31.xsd";
            schema.Includes.Add(import);
        }

        private void addXBRLInclude(XmlSchema schema, string schemaLoc)
        {
            XmlSchemaInclude include = new XmlSchemaInclude();
            include.SchemaLocation = schemaLoc;
            schema.Includes.Add(include);
        }


        /// <summary>
        /// Add Namespace
        /// </summary>
        /// <param name="schema"></param>
        private void addXBRLNamespace(XmlSchema schema)
        {
            schema.ElementFormDefault = XmlSchemaForm.Qualified;
            schema.AttributeFormDefault = XmlSchemaForm.Unqualified;

            schema.Namespaces.Add("", "http://www.w3.org/2001/XMLSchema");
            schema.Namespaces.Add("xbrll", "http://www.xbrl.org/2003/linkbase");
            schema.Namespaces.Add("xlink", "http://www.w3.org/1999/xlink");
            schema.Namespaces.Add("xbrli", "http://www.xbrl.org/2003/instance");
            schema.Namespaces.Add(XMLTools.getNameSpacePrefix(this.repository.GetPackageByID(Int32.Parse(this.scope)), this.targetNameSpacePrefix), XMLTools.getNameSpace(this.repository, this.repository.GetPackageByID(Int32.Parse(this.scope))));
            schema.TargetNamespace = XMLTools.getNameSpace(this.repository, this.repository.GetPackageByID(Int32.Parse(this.scope)));
        }


        private bool isSchemaObjectAlreadyExist(XmlSchema schema, XmlSchemaComplexType obj)
        {
            return schema.Items.Contains(obj);
        }

        /// <summary>
        /// Creates a Schema Element for the passed EA.Element
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private void getSchemaElement(EA.Element e, XmlSchema schema)
        {
            ArrayList name = new ArrayList();
            name.Add(e.Name);

            #region generate element ABIE for this class

            createABIE(e, schema, name);

            #endregion

            #region element for BBIE for this class

            createBBIE(e, schema, name);

            //foreach (EA.Attribute attr in e.Attributes)
            //{
            //    XmlSchemaElement el = new XmlSchemaElement();
            //    //for validation in UBMatrix, element's ID and element's Name must be the same
            //    el.Name = XMLTools.getXBRLBbieName(e, attr);
            //    el.Id = el.Name;
            //    el.IsNillable = true;
            //    setBBIEType(el, attr, schema, e);
            //    el.SubstitutionGroup = new XmlQualifiedName("item", "http://www.xbrl.org/2003/instance");

            //    XmlDocument dummy2 = new XmlDocument();
            //    XmlAttribute dummyAttr2 = dummy2.CreateAttribute("periodType", "http://www.xbrl.org/2003/instance");
            //    dummyAttr2.Value = "instant";
            //    el.UnhandledAttributes = new XmlAttribute[] { dummyAttr2 };
            //    schema.Items.Add(el);
            //}
            #endregion

            

            #region Create ComplexType if isTuple == true
            
            if (isTuple(e))
            {
                generateTuple(e, schema, name);
            }

            #endregion

            if (e.Connectors.Count > 0)
            {
                #region OLD-generate element ABIE for this class

                ////for validation in UBMatrix, element's ID and element's Name must be the same
                //xmlElement.Name = XMLTools.getXMLName(e.Name);
                //xmlElement.Id = xmlElement.Name;
                //xmlElement.IsNillable = true;
                //xmlElement.SubstitutionGroup = new XmlQualifiedName("item", "http://www.xbrl.org/2003/instance");
                ////xmlElement.SchemaTypeName = new XmlQualifiedName(xmlElement.Id + "ItemType", XMLTools.getNameSpace(this.repository, this.repository.GetPackageByID(Int32.Parse(this.scope)))); //'Type' attribute
                //xmlElement.SchemaTypeName = new XmlQualifiedName("stringItemType", "http://www.xbrl.org/2003/instance");

                //XmlDocument dummy = new XmlDocument();
                //XmlAttribute dummyAttr = dummy.CreateAttribute("periodType", "http://www.xbrl.org/2003/instance");
                //dummyAttr.Value = "instant";
                //xmlElement.UnhandledAttributes = new XmlAttribute[] { dummyAttr };
                //schema.Items.Add(xmlElement);

                //#endregion

                //#region element for BBIE

                //foreach (EA.Attribute attr in e.Attributes)
                //{
                //    XmlSchemaElement el = new XmlSchemaElement();
                //    //for validation in UBMatrix, element's ID and element's Name must be the same
                //    el.Name = XMLTools.getXBRLBbieName(e, attr);
                //    el.Id = el.Name;
                //    el.IsNillable = true;
                //    setBBIEType(el, attr, schema, e);
                //    //el.SchemaTypeName = new XmlQualifiedName(el.Id + "ItemType", XMLTools.getNameSpace(this.repository, this.repository.GetPackageByID(Int32.Parse(this.scope)))); //'Type' attribute
                //    el.SubstitutionGroup = new XmlQualifiedName("item", "http://www.xbrl.org/2003/instance");

                //    XmlDocument dummy2 = new XmlDocument();
                //    XmlAttribute dummyAttr2 = dummy2.CreateAttribute("periodType", "http://www.xbrl.org/2003/instance");
                //    dummyAttr2.Value = "instant";
                //    el.UnhandledAttributes = new XmlAttribute[] { dummyAttr2 };
                //    schema.Items.Add(el);


                //}
                #endregion

                //then generate element that related to this class recursively
                GenerateSchemaRecursively(e, schema, name);
            }

            #region old code
            ////schema for the element file
            //XmlSchemaElement element = new XmlSchemaElement();

            ////create element for ABIE
            //element.Name = XMLTools.getXMLName(e.Name);
            //element.Id = element.Name;
            //element.IsAbstract = false;
            //element.IsNillable = true;

            ////Check if the element contain tagged value "isTuple", if yes, create "tuple" for subtitutionGroup else "item"
            //if (isTuple(e))
            //    element.SubstitutionGroup = new XmlQualifiedName("tuple", "http://www.xbrl.org/2003/instance");
            //else
            //    element.SubstitutionGroup = new XmlQualifiedName("item","http://www.xbrl.org/2003/instance");

            //element.SchemaTypeName = new XmlQualifiedName(XMLTools.getXMLName(e.Name) + strComplexType, XMLTools.getNameSpace(this.repository, this.repository.GetPackageByID(Int32.Parse(this.scope))));
            //schema.Items.Add(element);

            ////create element for BBIE
            //foreach (EA.Attribute attr in e.Attributes)
            //{
            //    XmlSchemaElement el = new XmlSchemaElement();
            //    //for validation in UBMatrix, element's ID and element's Name must be the same
            //    el.Name = XMLTools.getXBRLBbieName(e, attr); 
            //    el.Id = el.Name;
            //    el.IsNillable = true;
            //    el.SchemaTypeName = new XmlQualifiedName(el.Id + "ItemType", XMLTools.getNameSpace(this.repository, this.repository.GetPackageByID(Int32.Parse(this.scope)))); //'Type' attribute
            //    el.SubstitutionGroup = new XmlQualifiedName("item", "http://www.xbrl.org/2003/instance");

            //    XmlDocument dummy = new XmlDocument();
            //    XmlAttribute dummyAttr = dummy.CreateAttribute("periodType", "http://www.xbrl.org/2003/instance");
            //    dummyAttr.Value = "instant";
            //    el.UnhandledAttributes = new XmlAttribute[] { dummyAttr };
            //    schema.Items.Add(el);
            //}



            //#region connector
            ////create element for ASBIE
            //foreach (EA.Connector con in e.Connectors)
            //{
            //    if (con.Type == EA_Element.Aggregation.ToString() && con.Stereotype.ToString() == CCTS_Types.ASBIE.ToString()
            //            && isOutgoing(con, e))
            //    {
            //        EA.Element client = this.repository.GetElementByID(con.ClientID);
            //        EA.Package p = this.repository.GetPackageByID(client.PackageID);


            //        if (!isInSamePackage(client))
            //        {
            //            #region ASBIE that connect to ABIE in other packages
            //            //For ASBIE that connect to ABIE in other packages, just generate definitions for the ASBIE but not for the ABIE/BBIE in the target package

            //            //Generate ASBIE
            //            XmlSchemaElement conEl = new XmlSchemaElement();
            //            //temp by Kristina conEl.Name = XMLTools.getXBRLAsbieName(e,con, repository,"");
            //            conEl.Name = "abc";
            //            conEl.Id = conEl.Name;
            //            conEl.IsNillable = true;
            //            conEl.SchemaTypeName = new XmlQualifiedName(conEl.Id + "ItemType", XMLTools.getNameSpace(this.repository, this.repository.GetPackageByID(Int32.Parse(this.scope))));
            //            conEl.SubstitutionGroup = new XmlQualifiedName("item", "http://www.xbrl.org/2003/instance");
                        
            //            XmlDocument dummy = new XmlDocument();
            //            XmlAttribute dummyAttr = dummy.CreateAttribute("periodType", "http://www.xbrl.org/2003/instance");
            //            dummyAttr.Value = "instant";
            //            conEl.UnhandledAttributes = new XmlAttribute[] { dummyAttr };
                        
            //            schema.Items.Add(conEl);

            //            //if (!p.Element.Stereotype.Equals(CCTS_Types.BIELibrary.ToString()) && !p.Element.Stereotype.Equals(CCTS_Types.DOCLibrary.ToString()))
            //            //{
            //            //    continue;
            //            //}
            //            //else if (p.Element.Stereotype.Equals(CCTS_Types.BIELibrary.ToString()))                        
            //            //{
            //            //    //1. generate that BIELibrary
            //            //    BIEGeneratorXBRL nestedBIE = new BIEGeneratorXBRL(this.repository, client.PackageID.ToString());
            //            //    System.Collections.ICollection schemaCollection = nestedBIE.generateXBRLschema(p);

            //            //    //get path of schema
            //            //    String schemaPath = "";
            //            //    String schemaName = "";
            //            //    schemaPath = XMLTools.getSavePathForSchema(this.repository.GetPackageByID(p.PackageID), repository);
            //            //    schemaName = XMLTools.getSchemaNameWithoutExtention(this.repository.GetPackageByID(p.PackageID));
            //            //    String filename = path + schemaPath + schemaName + "-structure.xsd";

            //            //    //2. Include the structure file to current schema
            //            //    addXBRLInclude(schema, filename);

            //            //    //3. set the "type" of element to that BIE Type.
            //            //}
            //            #endregion
            //        }
            //        else
            //        {
            //            #region ASBIE that connect to ABIE in the same package
            //            //follow ASBIE's to any depth until you reach an ABIE with no further ASBIE.
            //            GenerateSchemaRecursively(con);
            //            #endregion
            //        }
                    
            //        ////create the target BBIE (of ASBIE)
            //        //foreach (EA.Attribute attr in client.Attributes)
            //        //{
            //        //    XmlSchemaElement conEl2 = new XmlSchemaElement();
            //        //    conEl2.Name = e.Name + "_" + con.ClientEnd.Role + "_" + client.Name + "_" + attr.Name;
            //        //    conEl2.Id = conEl2.Name;
            //        //    conEl2.IsNillable = true;
            //        //    conEl2.SubstitutionGroup = new XmlQualifiedName("tuple", "http://www.xbrl.org/2003/instance");
            //        //    string targetType2 = client.Name + strComplexType;
            //        //    conEl2.SchemaTypeName = new XmlQualifiedName(targetType2, XMLTools.getNameSpace(this.repository, this.repository.GetPackageByID(Int32.Parse(this.scope))));
            //        //    schema.Items.Add(conEl2);
            //        //}



            //    }
            //}
            //#endregion
            #endregion 
        }



        /// <summary>
        /// Create ABIE for root element or  any link ABIE
        /// </summary>
        /// <param name="element"></param>
        /// <param name="schema"></param>
        private void createABIE(EA.Element element, XmlSchema schema, ArrayList arrName)
        {
            XmlSchemaElement xmlElement = new XmlSchemaElement();

            //for validation in UBMatrix, element's ID and element's Name must be the same
            xmlElement.Name = XMLTools.getXBRLAsbieName(arrName); //XMLTools.getXMLName(element.Name);
            xmlElement.Id = xmlElement.Name;
            xmlElement.IsNillable = true;
            if (isTuple(element))
            {
                xmlElement.SubstitutionGroup = new XmlQualifiedName("tuple", "http://www.xbrl.org/2003/instance");
            }
            else
            {
                xmlElement.SubstitutionGroup = new XmlQualifiedName("item", "http://www.xbrl.org/2003/instance");
            }

            xmlElement.SchemaTypeName = new XmlQualifiedName("stringItemType", "http://www.xbrl.org/2003/instance");

            XmlDocument dummy = new XmlDocument();
            XmlAttribute dummyAttr = dummy.CreateAttribute("periodType", "http://www.xbrl.org/2003/instance");
            dummyAttr.Value = "instant";
            xmlElement.UnhandledAttributes = new XmlAttribute[] { dummyAttr };
            schema.Items.Add(xmlElement);



            ////generate ASBIE for this connector
            //XmlSchemaElement conEl = new XmlSchemaElement();
            //conEl.Name = XMLTools.getXBRLAsbieName(tempName);
            //conEl.Id = conEl.Name;
            //conEl.IsNillable = true;
            //conEl.SchemaTypeName = new XmlQualifiedName("stringItemType", "http://www.xbrl.org/2003/instance");

            //if (isTuple(element))
            //{
            //    conEl.SubstitutionGroup = new XmlQualifiedName("tuple", "http://www.xbrl.org/2003/instance");
            //}
            //else
            //{
            //    conEl.SubstitutionGroup = new XmlQualifiedName("item", "http://www.xbrl.org/2003/instance");
            //}

            //XmlDocument dummy = new XmlDocument();
            //XmlAttribute dummyAttr = dummy.CreateAttribute("periodType", "http://www.xbrl.org/2003/instance");
            //dummyAttr.Value = "instant";
            //conEl.UnhandledAttributes = new XmlAttribute[] { dummyAttr };
            //schema.Items.Add(conEl);
        }

        /// <summary>
        /// Create BBIE for root element or any link ABIE
        /// </summary>
        /// <param name="element"></param>
        /// <param name="schema"></param>
        /// <param name="arrName"></param>
        private void createBBIE(EA.Element e, XmlSchema schema, ArrayList arrName)
        {
            foreach (EA.Attribute attr in e.Attributes)
            {
                XmlSchemaElement el = new XmlSchemaElement();
                //for validation in UBMatrix, element's ID and element's Name must be the same
                el.Name = XMLTools.getXBRLBbieName(attr, arrName);
                el.Id = el.Name;
                el.IsNillable = true;
                setBBIEType(el, attr, schema, e);
                el.SubstitutionGroup = new XmlQualifiedName("item", "http://www.xbrl.org/2003/instance");

                XmlDocument dummy2 = new XmlDocument();
                XmlAttribute dummyAttr2 = dummy2.CreateAttribute("periodType", "http://www.xbrl.org/2003/instance");
                dummyAttr2.Value = "instant";
                el.UnhandledAttributes = new XmlAttribute[] { dummyAttr2 };
                schema.Items.Add(el);
            }

            //foreach (EA.Attribute attribute in element.Attributes)
            //{
            //    XmlSchemaElement conEl2 = new XmlSchemaElement();
            //    conEl2.Name = XMLTools.getXBRLBbieName(attribute, arrName);
            //    conEl2.Id = conEl2.Name;
            //    conEl2.IsNillable = true;
            //    conEl2.SubstitutionGroup = new XmlQualifiedName("item", "http://www.xbrl.org/2003/instance");
            //    setBBIEType(conEl2, attribute, schema, element);

            //    XmlDocument dummy2 = new XmlDocument();
            //    XmlAttribute dummyAttr2 = dummy2.CreateAttribute("periodType", "http://www.xbrl.org/2003/instance");
            //    dummyAttr2.Value = "instant";
            //    conEl2.UnhandledAttributes = new XmlAttribute[] { dummyAttr2 };
            //    schema.Items.Add(conEl2);
            //}
            
        }


        private void GenerateSchemaRecursively(EA.Element element, XmlSchema schema, ArrayList name)
        {
            EA.Element e = element;

            foreach (EA.Connector con in e.Connectors)
            {
                ArrayList tempName = new ArrayList(name);
                if (con.Type == EA_Element.Aggregation.ToString() && con.Stereotype.ToString() == CCTS_Types.ASBIE.ToString()
                        && isOutgoing(con, e))
                {
                    EA.Element client = this.repository.GetElementByID(con.ClientID);

                    if (con.ClientEnd.Role != "")
                        tempName.Add(con.ClientEnd.Role);
                    tempName.Add(client.Name);

                    if (!isInSamePackage(client))
                    {
                        //For ASBIE that connect to ABIE in other packages, just generate definitions for the ASBIE but not for the ABIE/BBIE in the target package
                        #region ASBIE that connect to ABIE in other packages

                        //Generate ASBIE
                        XmlSchemaElement conEl = new XmlSchemaElement();
                        conEl.Name = XMLTools.getXBRLAsbieName(tempName);
                        conEl.Id = conEl.Name;
                        conEl.IsNillable = true;
                        conEl.SchemaTypeName = new XmlQualifiedName("stringItemType", "http://www.xbrl.org/2003/instance");
                        conEl.SubstitutionGroup = new XmlQualifiedName("item", "http://www.xbrl.org/2003/instance");

                        XmlDocument dummy = new XmlDocument();
                        XmlAttribute dummyAttr = dummy.CreateAttribute("periodType", "http://www.xbrl.org/2003/instance");
                        dummyAttr.Value = "instant";
                        conEl.UnhandledAttributes = new XmlAttribute[] { dummyAttr };

                        schema.Items.Add(conEl);

                        #endregion
                    }
                    else
                    {
                        //follow ASBIE's to any depth until you reach an ABIE with no further ASBIE.
                        #region ASBIE that connect to ABIE in the same package

                        ////generate ASBIE for this connector
                        createABIE(element, schema, tempName);

                        //generate BBIE
                        createBBIE(client, schema, tempName);

                        //foreach (EA.Attribute attribute in client.Attributes)
                        //{
                        //    XmlSchemaElement conEl2 = new XmlSchemaElement();
                        //    conEl2.Name = XMLTools.getXBRLBbieName(attribute, tempName);
                        //    conEl2.Id = conEl2.Name;
                        //    conEl2.IsNillable = true;
                        //    conEl2.SubstitutionGroup = new XmlQualifiedName("item", "http://www.xbrl.org/2003/instance");
                        //    setBBIEType(conEl2, attribute, schema, client); //element);

                        //    XmlDocument dummy2 = new XmlDocument();
                        //    XmlAttribute dummyAttr2 = dummy2.CreateAttribute("periodType", "http://www.xbrl.org/2003/instance");
                        //    dummyAttr2.Value = "instant";
                        //    conEl2.UnhandledAttributes = new XmlAttribute[] { dummyAttr2 };
                        //    schema.Items.Add(conEl2);
                        //}

                        #region Create ComplexType if isTuple == true
                        if (isTuple(client))
                        {
                            generateTuple(client, schema,tempName);
                        }
                        #endregion

                        //generate ASBIE of related ABIE.
                        GenerateSchemaRecursively(client, schema, tempName);
                        #endregion
                    }
                }
            }
        }


        /// <summary>
        /// Check if there is 'isTuple' tagged value in the elemet
        /// </summary>
        /// <param name="element"></param>
        /// <returns>boolean value, whether there is 'isTuple' tagged value</returns>
        private bool isTuple(EA.Element element)
        {
            foreach (EA.TaggedValue tv in element.TaggedValues)
            {
                if (tv.Name.Equals("isTuple", StringComparison.OrdinalIgnoreCase) &&
                    (tv.Value.Equals("true", StringComparison.OrdinalIgnoreCase) || tv.Value.Equals("1")))
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// Returns the name of the current package
        /// </summary>
        /// <returns></returns>
        private String getPackageName()
        {
            return this.repository.GetPackageByID(Int32.Parse(this.scope)).Name;
        }

        public static void ValidationCallbackOne(object sender, ValidationEventArgs args)
        {
            throw new XmlSchemaException(args.Message + args.Exception.StackTrace);
        }

        /// <summary>
        /// returns true if the connector is outgoing of the element
        /// </summary>
        /// <param name="c"></param>
        /// <param name="el"></param>
        /// <returns></returns>
        private bool isOutgoing(EA.Connector c, EA.Element el)
        {
            if (c.SupplierID == el.ElementID)
                return true;
            else
                return false;
        }

        private void btnGenerateSchema_Click(object sender, EventArgs e)
        {
            resetGenerator(this.scope);
            DialogResult result = this.dlgSavePath.ShowDialog(this);
            string packageName = this.repository.GetPackageByID(Int32.Parse(scope)).Name;
            EA.Package pkg = this.repository.GetPackageByID(Int32.Parse(scope));
            System.Collections.ICollection schemaCollection = null;

            if (result == DialogResult.Cancel)
                dlgSavePath.Dispose();
            else
            {
                path = dlgSavePath.SelectedPath + "\\";
                this.performProgressStep();

                
                try
                {
                    schemaCollection = this.generateXBRLschema(pkg);
                }
                catch (Exception exception)
                {
                    this.appendErrorMessage(exception.Message, packageName);
                }
                this.appendInfoMessage("Successfully generate XBRL taxonomy.", packageName);
                this.progressBar1.Value = this.progressBar1.Maximum;
            }

        }

        private bool isInSamePackage(EA.Element e)
        {
            if (Int32.Parse(scope) == e.PackageID)
                return true;
            else
                return false;
        }

        

        private void addXBRLImport(XmlSchema schema, String schemaLocation, String namespace_)
        {
            XmlSchemaImport xsi1 = new XmlSchemaImport();
            xsi1.SchemaLocation = schemaLocation;
            xsi1.Namespace = namespace_;
            schema.Includes.Add(xsi1);
        }

        /// <summary>
        /// Returns an empty String if the schema is not included
        /// If the schema is included, the namespace is returned
        /// </summary>
        /// <param name="packageName"></param>
        /// <returns></returns>
        private String isSchemaAlreadyIncluded(String packageName)
        {
            foreach (AuxilliarySchema aux in this.alreadyCreatedSchemas)
            {
                if (aux.PackageOfOrigin == packageName)
                    return aux.Namespace;
            }
            return "";
        }

        /// <summary>
        /// This methods determins, what should be passed to an auxilliary schema generator
        /// If this class itself has already been called by another class, the calling class is passed
        /// otherwise an instance of this class is passed
        /// </summary>
        /// <returns></returns>
        private GeneratorCallBackInterface getCaller()
        {
            if (this.caller == null)
                return this;
            else
                return caller;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Sets the type of the given bbie
        /// If the type refers to a cdt/qdt the method first checks,
        /// whether the relevant schema does already exist
        /// If not the schema is created and a reference via import statement
        /// is made
        /// </summary>
        /// <param name="e"></param>
        /// <param name="bbie"></param>
        private void setBBIEType(XmlSchemaElement e, EA.Attribute bbie, XmlSchema schema, EA.Element element)
        {

            try
            {

                String type = bbie.Type;
                int classifierID = bbie.ClassifierID;
                EA.Element classifierElement = this.repository.GetElementByID(classifierID);
                EA.Package classifierPackage = this.repository.GetPackageByID(classifierElement.PackageID);
                String stereotype = classifierPackage.Element.Stereotype;
                String s = "";



                //Is this schema already included?
                if ((s = isSchemaAlreadyIncluded(classifierPackage.Name)) == "")
                {
                    if (!CC_Utils.blnLinkedSchema)
                    {
                        #region Include linked schema unchecked - for local generation only
                        AuxilliarySchema aux = new AuxilliarySchema();
                        aux.Namespace = XMLTools.getNameSpace(this.repository, classifierPackage);
                        aux.NamespacePrefix = XMLTools.getNameSpacePrefix(classifierPackage, DeterminePrefix(classifierPackage));
                        aux.PackageOfOrigin = classifierPackage.Name.ToString();
                        this.alreadyCreatedSchemas.Add(aux);

                        String schemaName = "";
                        schemaName = XMLTools.getSchemaName(this.repository.GetPackageByID(classifierPackage.PackageID));

                        String importPath = XMLTools.getImportPathForSchema(this.repository.GetPackageByID(classifierPackage.PackageID), repository, schemaName, scope);

                        addXBRLImport(schema, importPath, aux.Namespace);

                        //We need to add the namespace of this schema to the main schema
                        schema.Namespaces.Add(aux.NamespacePrefix, aux.Namespace);

                        //Set the namespace to the variable which is then used down below
                        s = aux.Namespace;
                        #endregion
                    }
                    else
                    {
                        #region Included linked schema
                        //Create an Auxilliary schema and store it in the collection - later we have to
                        //add it to the main schema collection
                        AuxilliarySchema aux = new AuxilliarySchema();
                        System.Collections.ICollection result = new ArrayList();
                        XmlSchema linkedSchema = new XmlSchema();
                        if (stereotype.Equals(CCTS_Types.CDTLibrary.ToString()))
                        {
                            result = new CDTGeneratorXBRL(this.repository, classifierPackage.PackageID.ToString(), true, this.path, getCaller()).generateSchema(classifierPackage);
                            aux.NamespacePrefix = XMLTools.getNameSpacePrefix(classifierPackage, "udt" + ++countUDTImports);
                            //return;
                        }
                        else if (stereotype.Equals(CCTS_Types.QDTLibrary.ToString()))
                        {
                            result = new QDTGeneratorXBRL(this.repository, classifierPackage.PackageID.ToString(), true, this.path, getCaller()).generateXBRLschema(classifierPackage);
                            aux.NamespacePrefix = XMLTools.getNameSpacePrefix(classifierPackage, "qdt" + ++countQDTImports);
                        }
                        else if (stereotype.Equals(CCTS_Types.ENUMLibrary.ToString()))
                        {
                            result = new ENUMGeneratorXBRL(this.repository, classifierPackage, this.path, getCaller()).generateXBRLschema(classifierPackage);
                            aux.NamespacePrefix = XMLTools.getNameSpacePrefix(classifierPackage, "enum" + ++countENUMImports);
                        }
                        else
                        {
                            throw new XMLException("");
                        }

                        //We get the namespace for the newly created QDTLibrary-Schema from the package name
                        aux.Namespace = XMLTools.getNameSpace(this.repository, classifierPackage);
                        aux.PackageOfOrigin = classifierPackage.Name.ToString();
                        aux.Schemas = result;

                        this.alreadyCreatedSchemas.Add(aux);

                        String schemaPath = "";
                        String schemaName = "";
                        schemaPath = XMLTools.getSavePathForSchema(this.repository.GetPackageByID(classifierPackage.PackageID), repository);
                        schemaName = XMLTools.getSchemaName(this.repository.GetPackageByID(classifierPackage.PackageID));

                        foreach (XmlSchema schema1 in result)
                        {

                            //It is possible, that more than one schema is returned here (because the
                            //auxilliary schemas may have interdependencies
                            //write only the one where the namespace fits to the one we initally created
                            if (schema1.TargetNamespace.Equals(aux.Namespace))
                            {
                                String filename = path + schemaPath + schemaName;
                                //Create the path
                                System.IO.Directory.CreateDirectory(path + schemaPath);
                                Stream outputStream = System.IO.File.Open(filename, FileMode.Create);
                                schema1.Write(outputStream);
                                outputStream.Close();
                            }
                        }

                        //We need to add an import statement for the newly generated schema
                        //to the original schema
                        String importPath = XMLTools.getImportPathForSchema(classifierPackage, repository, schemaName, scope);
                        addXBRLImport(schema, importPath, aux.Namespace);
                        //We need to add the namespace of this schema to the main schema
                        schema.Namespaces.Add(aux.NamespacePrefix, aux.Namespace);
                        //Set the namespace to the variable which is then used down below
                        s = aux.Namespace;
                        #endregion
                    }
                }


                e.SchemaTypeName = new XmlQualifiedName(bbie.Type + "Type", s);


            }
            catch (Exception ex)
            {
                this.appendWarnMessage("Unable to determine correct datatype for attribute " + bbie.Name + " in element " + element.Name + ". Taking xsd:string instead. ", this.getPackageName());
                e.SchemaTypeName = new XmlQualifiedName("stringItemType", "http://www.xbrl.org/2003/instance");
            }


        }


        private string DeterminePrefix(Package importPackage)
        {
            if (importPackage.Element.Stereotype.Equals(CCTS_Types.CDTLibrary.ToString()))
                return "udt" + ++countUDTImports;
            else
                if (importPackage.Element.Stereotype.Equals(CCTS_Types.ENUMLibrary.ToString()))
                    return "enum" + ++countENUMImports;
                else
                    if (importPackage.Element.Stereotype.Equals(CCTS_Types.QDTLibrary.ToString()))
                        return "qdt" + ++countQDTImports;
                    else
                        throw new XMLException("");

            //return "";
        }

        #region OBSOLETE CODE, put it here

        //private XmlQualifiedName createAndImportAuxilliarySchema(EA.Element e, XmlSchema schema)
        //{
        //    EA.Package p = this.repository.GetPackageByID(e.PackageID);
        //    String pName = p.Name;

        //    //Is this schema already included?
        //    String s = "";
        //    if ((s = isSchemaAlreadyIncluded(p.Name)) == "")
        //    {
        //        System.Collections.ICollection result = new BIEGeneratorXBRL(this.repository, p, this.path, getCaller()).generateXBRLschema(p);
        //        //Create an Auxilliary schema and store it in the collection - later we have to
        //        //add it to the main schema collection
        //        AuxilliarySchema aux = new AuxilliarySchema();
        //        aux.Namespace = XMLTools.getNameSpace(this.repository, p);
        //        aux.NamespacePrefix = XMLTools.getNameSpacePrefix(p, "bie" + ++countBIEImports);
        //        aux.PackageOfOrigin = p.Name.ToString();
        //        aux.Schemas = result;
        //        this.alreadyCreatedSchemas.Add(aux);

        //        //Write the schema(s)
        //        String schemaPath = "";
        //        String schemaName = "";
        //        schemaPath = XMLTools.getSavePathForSchema(this.repository.GetPackageByID(p.PackageID), repository);
        //        schemaName = XMLTools.getSchemaName(this.repository.GetPackageByID(p.PackageID));
        //        String filename = path + schemaPath + schemaName;

        //        if (CC_Utils.blnLinkedSchema)
        //        {
        //            foreach (XmlSchema schema1 in result)
        //            {
        //                //Create the path
        //                System.IO.Directory.CreateDirectory(path + schemaPath);
        //                Stream outputStream = System.IO.File.Open(filename, FileMode.Create);
        //                schema1.Write(outputStream);
        //                outputStream.Close();
        //            }
        //        }
        //        //We need to add an import statement for the newly generated schema
        //        //to the original schema
        //        String importPath = XMLTools.getImportPathForSchema(p, repository, schemaName, scope);

        //        addXBRLImport(schema, importPath, aux.Namespace);

        //        //We need to add the namespace of this schema to the main schema
        //        schema.Namespaces.Add(aux.NamespacePrefix, aux.Namespace);

        //        //Set the namespace to the variable which is then used down below
        //        s = aux.Namespace;
        //    }

        //    return new XmlQualifiedName(e.Name + "ItemTypeASBIE", s);
        //}

        //private void generatePresentationFile()
        //{
        //    EA.Package pkg = this.repository.GetPackageByID(Int32.Parse(this.scope));

        //    String schemaPath = "";
        //    String schemaName = "";
        //    schemaPath = XMLTools.getSavePathForSchema(this.repository.GetPackageByID(pkg.PackageID), repository);
        //    schemaName = XMLTools.getSchemaNameWithoutExtention(this.repository.GetPackageByID(pkg.PackageID));
        //    String filename = path + schemaPath + schemaName + "-presentation.xml";
        //    System.IO.Directory.CreateDirectory(path + schemaPath);

        //    XmlTextWriter w = new XmlTextWriter(filename, null);
        //    w.Namespaces = true;
        //    w.Formatting = Formatting.Indented;
        //    w.WriteStartDocument();
        //    w.WriteStartElement("", "linkbase", "http://www.xbrl.org/2003/linkbase");
        //    w.WriteAttributeString("xmlns", "", null, "http://www.xbrl.org/2003/linkbase");
        //    w.WriteAttributeString("xmlns", "xbrli", null, "http://www.xbrl.org/2003/instance");
        //    w.WriteAttributeString("xmlns", "xlink", null, "http://www.w3.org/1999/xlink");
        //    w.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
        //    w.WriteAttributeString("xsi", "schemaLocation", null, "http://www.xbrl.org/2003/linkbase http://www.xbrl.org/2003/xbrl-linkbase-2003-12-31.xsd");
        //    w.WriteAttributeString("xmlns", XMLTools.getNameSpacePrefix(this.repository.GetPackageByID(Int32.Parse(this.scope)),
        //        this.targetNameSpacePrefix), null, XMLTools.getNameSpace(this.repository, this.repository.GetPackageByID(Int32.Parse(scope))));

        //    w.WriteStartElement("presentationLink", "http://www.xbrl.org/2003/linkbase");
        //    w.WriteAttributeString("type", "http://www.w3.org/1999/xlink", "extended");
        //    w.WriteAttributeString("role", "http://www.w3.org/1999/xlink", "http://www.xbrl.org/2003/role/link");

        //    #region Looping parts

        //    foreach (EA.Element abie in pkg.Elements)
        //    {
        //        generatePresentationLocatorForABIE(abie, w);

        //        int order = 0;
        //        foreach (EA.Attribute bbie in abie.Attributes)
        //        {
        //            order++;
        //            generatePresentationLocatorForBBIE(bbie, w, order);
        //        }

        //        foreach (EA.Connector con in abie.Connectors)
        //        {
        //            if (con.Type == EA_Element.Aggregation.ToString() && con.Stereotype.ToString() == CCTS_Types.ASBIE.ToString()
        //                && isOutgoing(con, abie))
        //            {
        //                EA.Element client = this.repository.GetElementByID(con.ClientID);
        //                EA.Package p = this.repository.GetPackageByID(client.PackageID);

        //                order++;
        //                //Connecting ASBIE to the ABIE in presentation linkbase


        //                if (!isInSamePackage(client))
        //                {
        //                    if (!p.Element.Stereotype.Equals(CCTS_Types.BIELibrary.ToString()) && !p.Element.Stereotype.Equals(CCTS_Types.DOCLibrary.ToString()))
        //                    {
        //                        continue;
        //                    }
        //                }

        //                //create presentation Arc from ABIE to ASBIE
        //                ConnectAsbieToAbie(con, w, order);
        //                int bbieOrder = 0;
        //                foreach (EA.Attribute attr in client.Attributes)
        //                {
        //                    bbieOrder++;
        //                    generatePresentationLocatorForTargetBBIE(con, attr, w, bbieOrder);
        //                }

        //                //if (!isInSamePackage(client))
        //                //{
        //                //    if (p.Element.Stereotype.Equals(CCTS_Types.BIELibrary.ToString()))
        //                //    {
        //                //        int bbieOrder = 0;
        //                //        foreach (EA.Attribute attr in client.Attributes)
        //                //        {
        //                //            bbieOrder++;
        //                //            generatePresentationLocatorForTargetBBIE(con, attr, w, bbieOrder);
        //                //        }
        //                //    }
        //                //}
        //                //else
        //                //{
        //                //    //looping to create the target BBIE 
        //                //    int bbieOrder = 0;
        //                //    foreach (EA.Attribute attr in client.Attributes)
        //                //    {
        //                //        bbieOrder++;
        //                //        generatePresentationLocatorForTargetBBIE(con, attr, w, bbieOrder);
        //                //    }
        //                //}
        //            }
        //        }
        //    }

        //    #endregion

        //    w.WriteEndElement();

        //    w.Flush();
        //    w.Close();
        //}

        //private void ConnectAsbieToAbie(Connector con, XmlTextWriter w, int order)
        //{
        //    string supplierName = this.repository.GetElementByID(con.SupplierID).Name;
        //    string clientName = this.repository.GetElementByID(con.ClientID).Name;
        //    string roleName = con.ClientEnd.Role;

        //    string locatorName = supplierName + "_" + roleName + "_" + clientName;
        //    string href = XMLTools.getSchemaName(this.repository.GetPackageByID(Int32.Parse(scope))) + "#" + locatorName;

        //    //locator
        //    w.WriteStartElement("loc", "http://www.xbrl.org/2003/linkbase");
        //    w.WriteAttributeString("type", "http://www.w3.org/1999/xlink", "locator");
        //    w.WriteAttributeString("href", "http://www.w3.org/1999/xlink", href);
        //    w.WriteAttributeString("label", "http://www.w3.org/1999/xlink", locatorName);
        //    w.WriteAttributeString("title", "http://www.w3.org/1999/xlink", "presentation : " + supplierName + " to " + locatorName);
        //    w.WriteEndElement();

        //    //presentationArc
        //    w.WriteStartElement("presentationArc", "http://www.xbrl.org/2003/linkbase");
        //    w.WriteAttributeString("type", "http://www.w3.org/1999/xlink", "arc");
        //    w.WriteAttributeString("arcrole", "http://www.w3.org/1999/xlink", "http://www.xbrl.org/2003/arcrole/parent-child");
        //    w.WriteAttributeString("from", "http://www.w3.org/1999/xlink", supplierName);
        //    w.WriteAttributeString("to", "http://www.w3.org/1999/xlink", locatorName);
        //    w.WriteAttributeString("use", "optional");
        //    w.WriteAttributeString("order", order.ToString());
        //    w.WriteEndElement();
        //}

        ///// <summary>
        ///// To create bbie of the target ABIE
        ///// </summary>
        ///// <param name="abie"></param>
        ///// <param name="w"></param>
        //private void generatePresentationLocatorForTargetBBIE(EA.Connector con, EA.Attribute attr, XmlTextWriter w, int order)
        //{
        //    string supplierName = this.repository.GetElementByID(con.SupplierID).Name;
        //    string clientName = this.repository.GetElementByID(con.ClientID).Name;
        //    string roleName = con.ClientEnd.Role;

        //    //locator
        //    string from = supplierName + "_" + roleName + "_" + clientName;
        //    string locatorName = supplierName + "_" + roleName + "_" + clientName + "_" + attr.Name;
        //    string href = XMLTools.getSchemaName(this.repository.GetPackageByID(Int32.Parse(scope))) + "#" + locatorName;

        //    w.WriteStartElement("loc", "http://www.xbrl.org/2003/linkbase");
        //    w.WriteAttributeString("type", "http://www.w3.org/1999/xlink", "locator");
        //    w.WriteAttributeString("href", "http://www.w3.org/1999/xlink", href);
        //    w.WriteAttributeString("label", "http://www.w3.org/1999/xlink", locatorName);
        //    w.WriteAttributeString("title", "http://www.w3.org/1999/xlink", "presentation : " + from + " to " + locatorName);
        //    w.WriteEndElement();

        //    //presentationArc
        //    w.WriteStartElement("presentationArc", "http://www.xbrl.org/2003/linkbase");
        //    w.WriteAttributeString("type", "http://www.w3.org/1999/xlink", "arc");
        //    w.WriteAttributeString("arcrole", "http://www.w3.org/1999/xlink", "http://www.xbrl.org/2003/arcrole/parent-child");
        //    w.WriteAttributeString("from", "http://www.w3.org/1999/xlink", from);
        //    w.WriteAttributeString("to", "http://www.w3.org/1999/xlink", locatorName);
        //    w.WriteAttributeString("use", "optional");
        //    w.WriteAttributeString("order", order.ToString());
        //    w.WriteEndElement();
        //}

        ///// <summary>
        ///// To create bbie of element
        ///// </summary>
        ///// <param name="bbie"></param>
        ///// <param name="w"></param>
        ///// <param name="order"></param>
        //private void generatePresentationLocatorForBBIE(EA.Attribute bbie, XmlTextWriter w, int order)
        //{
        //    EA.Element parentElement = this.repository.GetElementByID(bbie.ParentID);
        //    //locator
        //    string locatorName = XMLTools.getXBRLBbieName(parentElement, bbie);    //parentElement.Name + "_" + bbie.Name;
        //    string href = XMLTools.getSchemaName(this.repository.GetPackageByID(Int32.Parse(scope))) + "#" + locatorName;

        //    w.WriteStartElement("loc", "http://www.xbrl.org/2003/linkbase");
        //    w.WriteAttributeString("type", "http://www.w3.org/1999/xlink", "locator");
        //    w.WriteAttributeString("href", "http://www.w3.org/1999/xlink", href);
        //    w.WriteAttributeString("label", "http://www.w3.org/1999/xlink", locatorName);
        //    w.WriteAttributeString("title", "http://www.w3.org/1999/xlink", "presentation : " + parentElement.Name + " to " + locatorName);
        //    w.WriteEndElement();

        //    //presentationArc
        //    w.WriteStartElement("presentationArc", "http://www.xbrl.org/2003/linkbase");
        //    w.WriteAttributeString("type", "http://www.w3.org/1999/xlink", "arc");
        //    w.WriteAttributeString("arcrole", "http://www.w3.org/1999/xlink", "http://www.xbrl.org/2003/arcrole/parent-child");
        //    w.WriteAttributeString("from", "http://www.w3.org/1999/xlink", parentElement.Name);
        //    w.WriteAttributeString("to", "http://www.w3.org/1999/xlink", locatorName);
        //    w.WriteAttributeString("use", "optional");
        //    w.WriteAttributeString("order", order.ToString());
        //    w.WriteEndElement();
        //}

        //private void generatePresentationLocatorForABIE(EA.Element element, XmlTextWriter w)
        //{
        //    //locator 1
        //    //href must be composition of : schemafile + "#" + ID-attribute of the element
        //    string href = XMLTools.getSchemaName(this.repository.GetPackageByID(Int32.Parse(scope))) + "#" + element.Name;
        //    string locatorName = element.Name;
        //    w.WriteStartElement("loc", "http://www.xbrl.org/2003/linkbase");
        //    w.WriteAttributeString("type", "http://www.w3.org/1999/xlink", "locator");
        //    w.WriteAttributeString("href", "http://www.w3.org/1999/xlink", href);
        //    w.WriteAttributeString("label", "http://www.w3.org/1999/xlink", locatorName);
        //    w.WriteAttributeString("title", "http://www.w3.org/1999/xlink", "presentation : ABIE to BBIE");
        //    w.WriteEndElement();

        //}
        ///// <summary>
        ///// Create the "packagename" + "-structure.xsd"
        ///// </summary>
        ///// <param name="element"></param>
        ///// <param name="ctSchema"></param>
        //private void getCTSchemaElement(Element element, XmlSchema ctSchema)
        //{

        //    XmlSchemaSequence sequence = new XmlSchemaSequence();

        //    #region create element for abie
        //    foreach (EA.Attribute bbie in element.Attributes)
        //    {
        //        //for BBIE ComplexType
        //        XmlSchemaElement el = new XmlSchemaElement();
        //        el.RefName = new XmlQualifiedName(element.Name + "_" + bbie.Name, XMLTools.getNameSpace(this.repository, this.repository.GetPackageByID(Int32.Parse(this.scope)))); //Ref attribute
        //        el.MaxOccurs = 1;
        //        el.MinOccurs = 0;
        //        sequence.Items.Add(el);

        //        //for bbie simple content
        //        XmlSchemaComplexType complexTypeBBIE = new XmlSchemaComplexType();
        //        complexTypeBBIE.Name = element.Name + "_" + bbie.Name + "ItemType";

        //        XmlSchemaSimpleContentRestriction stRestriction = new XmlSchemaSimpleContentRestriction();
        //        stRestriction.BaseTypeName = new XmlQualifiedName("stringItemType", "http://www.xbrl.org/2003/instance"); //namanya ntar menyusul

        //        XmlSchemaSimpleContent simpleContent = new XmlSchemaSimpleContent();
        //        simpleContent.Content = stRestriction;

        //        complexTypeBBIE.ContentModel = simpleContent;
        //        ctSchema.Items.Add(complexTypeBBIE);


        //    }
        //    #endregion

        //    #region create element for connector
        //    foreach (EA.Connector con in element.Connectors)
        //    {
        //        if (con.Type == EA_Element.Aggregation.ToString() && con.Stereotype.ToString() == CCTS_Types.ASBIE.ToString()
        //            && isOutgoing(con, element))
        //        {
        //            EA.Element client = this.repository.GetElementByID(con.ClientID);
        //            //string clientName = this.repository.GetElementByID(con.ClientID).Name;
        //            string refName = element.Name + "_" + con.ClientEnd.Role + "_" + client.Name;

        //            if (!isInSamePackage(client))
        //            {
        //                //We only import other elements if they are located in a BIELibrary
        //                EA.Package p = this.repository.GetPackageByID(client.PackageID);

        //                if (p.Element.Stereotype.Equals(CCTS_Types.BIELibrary.ToString()) || p.Element.Stereotype.Equals(CCTS_Types.DOCLibrary.ToString()))
        //                {
        //                    #region old code

        //                    //XmlQualifiedName qName = createAndImportAuxilliarySchema(client, ctSchema);
        //                    ////create connector in complex type
        //                    //XmlSchemaElement el = new XmlSchemaElement();

        //                    //el.RefName = qName; //new XmlQualifiedName(refName, XMLTools.getNameSpace(this.repository, this.repository.GetPackageByID(Int32.Parse(this.scope))));
        //                    //el.MaxOccurs = 1;
        //                    //el.MinOccurs = 0;
        //                    //sequence.Items.Add(el);

        //                    //for target of connector 
        //                    XmlSchemaComplexType complexTypeBBIE = new XmlSchemaComplexType();
        //                    complexTypeBBIE.Name = client.Name + "ComplexType";

        //                    XmlSchemaComplexContent complexContentBBIE = new XmlSchemaComplexContent();

        //                    XmlSchemaComplexContentRestriction restrictBBIE = new XmlSchemaComplexContentRestriction();
        //                    restrictBBIE.BaseTypeName = new XmlQualifiedName("anyType");

        //                    XmlSchemaSequence seq = new XmlSchemaSequence();

        //                    restrictBBIE.Particle = seq;
        //                    complexContentBBIE.Content = restrictBBIE;
        //                    complexTypeBBIE.ContentModel = complexContentBBIE;

        //                    bool exist = XMLTools.isElementAlreadyIncludedInSchema(ctSchema, complexTypeBBIE.Name);
        //                    if (!exist)
        //                        ctSchema.Items.Add(complexTypeBBIE);

        //                    #endregion

        //                    foreach (EA.Attribute attr in client.Attributes)
        //                    {
        //                        XmlSchemaElement conEl2 = new XmlSchemaElement();
        //                        string targetName = refName + "_" + attr.Name;
        //                        conEl2.RefName = new XmlQualifiedName(targetName, XMLTools.getNameSpace(this.repository, this.repository.GetPackageByID(Int32.Parse(this.scope)))); //Ref attribute
        //                        conEl2.MaxOccurs = 1;
        //                        conEl2.MinOccurs = 0;
        //                        sequence.Items.Add(conEl2);
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                XmlSchemaElement conEl = new XmlSchemaElement();
        //                conEl.RefName = new XmlQualifiedName(refName, XMLTools.getNameSpace(this.repository, this.repository.GetPackageByID(Int32.Parse(this.scope)))); //Ref attribute
        //                conEl.MaxOccurs = 1;
        //                conEl.MinOccurs = 0;
        //                sequence.Items.Add(conEl);
        //            }
        //        }
        //    }
        //    #endregion

        //    XmlSchemaComplexContent complexContent = new XmlSchemaComplexContent();

        //    XmlSchemaComplexContentRestriction restrict = new XmlSchemaComplexContentRestriction();
        //    restrict.BaseTypeName = new XmlQualifiedName("anyType");

        //    XmlSchemaComplexType complexType = new XmlSchemaComplexType();
        //    complexType.IsMixed = false;
        //    complexType.Name = element.Name + strComplexType;

        //    restrict.Particle = sequence;
        //    complexContent.Content = restrict;
        //    complexType.ContentModel = complexContent;
        //    ctSchema.Items.Add(complexType);
        //}

        //private void generateLabelFile()
        //{
        //    EA.Package pkg = this.repository.GetPackageByID(Int32.Parse(this.scope));

        //    String schemaPath = "";
        //    String schemaName = "";
        //    schemaPath = XMLTools.getSavePathForSchema(this.repository.GetPackageByID(pkg.PackageID), repository);
        //    schemaName = XMLTools.getSchemaNameWithoutExtention(this.repository.GetPackageByID(pkg.PackageID));
        //    String filename = path + schemaPath + schemaName + "-label.xml";
        //    System.IO.Directory.CreateDirectory(path + schemaPath);

        //    XmlTextWriter w = new XmlTextWriter(filename, null);
        //    w.Namespaces = true;
        //    w.Formatting = Formatting.Indented;
        //    w.WriteStartDocument();
        //    w.WriteStartElement("", "linkbase", "http://www.xbrl.org/2003/linkbase");
        //    w.WriteAttributeString("xmlns", "", null, "http://www.xbrl.org/2003/linkbase");
        //    w.WriteAttributeString("xmlns", "xbrli", null, "http://www.xbrl.org/2003/instance");
        //    w.WriteAttributeString("xmlns", "xlink", null, "http://www.w3.org/1999/xlink");
        //    w.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
        //    w.WriteAttributeString("xsi", "schemaLocation", null, "http://www.xbrl.org/2003/linkbase http://www.xbrl.org/2003/xbrl-linkbase-2003-12-31.xsd");
        //    w.WriteAttributeString("xmlns", XMLTools.getNameSpacePrefix(this.repository.GetPackageByID(Int32.Parse(this.scope)),
        //        this.targetNameSpacePrefix), null, XMLTools.getNameSpace(this.repository, this.repository.GetPackageByID(Int32.Parse(scope))));

        //    w.WriteStartElement("labelLink", "http://www.xbrl.org/2003/linkbase");
        //    w.WriteAttributeString("type", "http://www.w3.org/1999/xlink", "extended");
        //    w.WriteAttributeString("role", "http://www.w3.org/1999/xlink", "http://www.xbrl.org/2003/role/link");

        //    #region Looping parts

        //    foreach (EA.Element abie in pkg.Elements)
        //    {
        //        generateLabelLocatorForABIE(abie, w);

        //        foreach (EA.Attribute bbie in abie.Attributes)
        //        {
        //            generateLabelLocatorForBBIE(bbie, w);
        //        }

        //        foreach (EA.Connector con in abie.Connectors)
        //        {
        //            if (con.Type == EA_Element.Aggregation.ToString() && con.Stereotype.ToString() == CCTS_Types.ASBIE.ToString()
        //                && isOutgoing(con, abie))
        //            {
        //                EA.Element client = this.repository.GetElementByID(con.ClientID);
        //                EA.Package p = this.repository.GetPackageByID(client.PackageID);

        //                if (!isInSamePackage(client))
        //                {
        //                    if (!p.Element.Stereotype.Equals(CCTS_Types.BIELibrary.ToString()) && !p.Element.Stereotype.Equals(CCTS_Types.DOCLibrary.ToString()))
        //                    {
        //                        continue;
        //                    }
        //                }
        //                generateLabelFileLocatorForASBIEName(con, w);
        //                foreach (EA.Attribute attr in client.Attributes)
        //                {
        //                    generateLabelLocatorForTargetBBIE(con, attr, w);
        //                }
        //            }
        //        }
        //    }

        //    #endregion

        //    w.WriteEndElement();

        //    w.Flush();
        //    w.Close();
        //}

        //private void generateLabelFileLocatorForASBIEName(Connector con, XmlTextWriter w)
        //{
        //    string supplierName = this.repository.GetElementByID(con.SupplierID).Name;
        //    string clientName = this.repository.GetElementByID(con.ClientID).Name;
        //    string roleName = con.ClientEnd.Role;
        //    //locator
        //    string locatorName = supplierName + "_" + roleName + "_" + clientName;
        //    string href = XMLTools.getSchemaName(this.repository.GetPackageByID(Int32.Parse(scope))) + "#" + locatorName;
        //    w.WriteStartElement("loc", "http://www.xbrl.org/2003/linkbase");
        //    w.WriteAttributeString("type", "http://www.w3.org/1999/xlink", "locator");
        //    w.WriteAttributeString("href", "http://www.w3.org/1999/xlink", href);
        //    w.WriteAttributeString("label", "http://www.w3.org/1999/xlink", locatorName);
        //    w.WriteEndElement();

        //    //label
        //    string labelName = locatorName + "_lbl";
        //    w.WriteStartElement("label", "http://www.xbrl.org/2003/linkbase");
        //    w.WriteAttributeString("type", "http://www.w3.org/1999/xlink", "resource");
        //    w.WriteAttributeString("label", "http://www.w3.org/1999/xlink", labelName);
        //    w.WriteAttributeString("role", "http://www.w3.org/1999/xlink", "http://www.xbrl.org/2003/role/documentation");
        //    w.WriteAttributeString("xml", "lang", "http://www.w3.org/XML/1998/namespace", "en");
        //    w.WriteString("ASBIE from " + supplierName + " to " + clientName); //to be filled with information of connector, not needed for now
        //    w.WriteEndElement();//closing tag of label

        //    //labelArc
        //    w.WriteStartElement("labelArc", "http://www.xbrl.org/2003/linkbase");
        //    w.WriteAttributeString("type", "http://www.w3.org/1999/xlink", "arc");
        //    w.WriteAttributeString("arcrole", "http://www.w3.org/1999/xlink", "http://www.xbrl.org/2003/arcrole/concept-label");
        //    w.WriteAttributeString("from", "http://www.w3.org/1999/xlink", locatorName);
        //    w.WriteAttributeString("to", "http://www.w3.org/1999/xlink", labelName);
        //    w.WriteEndElement();
        //}

        //private void generateLabelLocatorForTargetBBIE(EA.Connector con, EA.Attribute attr, XmlTextWriter w)
        //{
        //    //EA.Element parentElement = this.repository.GetElementByID(attr.ParentID);
        //    string supplierName = this.repository.GetElementByID(con.SupplierID).Name;
        //    string clientName = this.repository.GetElementByID(con.ClientID).Name;
        //    string roleName = con.ClientEnd.Role;
        //    //locator
        //    string locatorName = supplierName + "_" + roleName + "_" + clientName + "_" + attr.Name;
        //    string href = XMLTools.getSchemaName(this.repository.GetPackageByID(Int32.Parse(scope))) + "#" + locatorName;
        //    w.WriteStartElement("loc", "http://www.xbrl.org/2003/linkbase");
        //    w.WriteAttributeString("type", "http://www.w3.org/1999/xlink", "locator");
        //    w.WriteAttributeString("href", "http://www.w3.org/1999/xlink", href);
        //    w.WriteAttributeString("label", "http://www.w3.org/1999/xlink", locatorName);
        //    w.WriteEndElement();

        //    //label
        //    string labelName = locatorName + "_lbl";
        //    w.WriteStartElement("label", "http://www.xbrl.org/2003/linkbase");
        //    w.WriteAttributeString("type", "http://www.w3.org/1999/xlink", "resource");
        //    w.WriteAttributeString("label", "http://www.w3.org/1999/xlink", labelName);
        //    w.WriteAttributeString("role", "http://www.w3.org/1999/xlink", "http://www.xbrl.org/2003/role/documentation");
        //    w.WriteAttributeString("xml", "lang", "http://www.w3.org/XML/1998/namespace", "en");
        //    w.WriteString("Attribute of " + clientName + ": " + attr.Name); //to be filled with information of connector, not needed for now
        //    w.WriteEndElement();//closing tag of label

        //    //labelArc
        //    w.WriteStartElement("labelArc", "http://www.xbrl.org/2003/linkbase");
        //    w.WriteAttributeString("type", "http://www.w3.org/1999/xlink", "arc");
        //    w.WriteAttributeString("arcrole", "http://www.w3.org/1999/xlink", "http://www.xbrl.org/2003/arcrole/concept-label");
        //    w.WriteAttributeString("from", "http://www.w3.org/1999/xlink", locatorName);
        //    w.WriteAttributeString("to", "http://www.w3.org/1999/xlink", labelName);
        //    w.WriteEndElement();
        //}

        //private void generateLabelLocatorForBBIE(EA.Attribute attr, XmlTextWriter w)
        //{
        //    EA.Element parentElement = this.repository.GetElementByID(attr.ParentID);
        //    //locator
        //    string locatorName = XMLTools.getXBRLBbieName(parentElement, attr);    //parentElement.Name + "_" + attr.Name;
        //    string href = XMLTools.getSchemaName(this.repository.GetPackageByID(Int32.Parse(scope))) + "#" + locatorName;
        //    w.WriteStartElement("loc", "http://www.xbrl.org/2003/linkbase");
        //    w.WriteAttributeString("type", "http://www.w3.org/1999/xlink", "locator");
        //    w.WriteAttributeString("href", "http://www.w3.org/1999/xlink", href);
        //    w.WriteAttributeString("label", "http://www.w3.org/1999/xlink", locatorName);
        //    w.WriteEndElement();

        //    //label
        //    string labelName = locatorName + "_lbl";
        //    w.WriteStartElement("label", "http://www.xbrl.org/2003/linkbase");
        //    w.WriteAttributeString("type", "http://www.w3.org/1999/xlink", "resource");
        //    w.WriteAttributeString("label", "http://www.w3.org/1999/xlink", labelName);
        //    w.WriteAttributeString("role", "http://www.w3.org/1999/xlink", "http://www.xbrl.org/2003/role/documentation");
        //    w.WriteAttributeString("xml", "lang", "http://www.w3.org/XML/1998/namespace", "en");
        //    w.WriteString(XMLTools.getAttributeTVValue(CCTS_TV.Definition, attr) == "" ? attr.Name : XMLTools.getAttributeTVValue(CCTS_TV.Definition, attr)); // get the Definition Tagged Value
        //    w.WriteEndElement();//closing tag of label

        //    //labelArc
        //    w.WriteStartElement("labelArc", "http://www.xbrl.org/2003/linkbase");
        //    w.WriteAttributeString("type", "http://www.w3.org/1999/xlink", "arc");
        //    w.WriteAttributeString("arcrole", "http://www.w3.org/1999/xlink", "http://www.xbrl.org/2003/arcrole/concept-label");
        //    w.WriteAttributeString("from", "http://www.w3.org/1999/xlink", locatorName);
        //    w.WriteAttributeString("to", "http://www.w3.org/1999/xlink", labelName);
        //    w.WriteEndElement();
        //}

        //private void generateLabelLocatorForABIE(EA.Element element, XmlTextWriter w)
        //{
        //    //locator
        //    string href = XMLTools.getSchemaName(this.repository.GetPackageByID(Int32.Parse(scope))) + "#" + element.Name;
        //    string locatorName = element.Name;
        //    w.WriteStartElement("loc", "http://www.xbrl.org/2003/linkbase");
        //    w.WriteAttributeString("type", "http://www.w3.org/1999/xlink", "locator");
        //    w.WriteAttributeString("href", "http://www.w3.org/1999/xlink", href);
        //    w.WriteAttributeString("label", "http://www.w3.org/1999/xlink", locatorName);
        //    w.WriteEndElement();

        //    //label
        //    string labelName = element.Name + "_lbl";
        //    w.WriteStartElement("label", "http://www.xbrl.org/2003/linkbase");
        //    w.WriteAttributeString("type", "http://www.w3.org/1999/xlink", "resource");
        //    w.WriteAttributeString("label", "http://www.w3.org/1999/xlink", labelName);
        //    w.WriteAttributeString("role", "http://www.w3.org/1999/xlink", "http://www.xbrl.org/2003/role/documentation");
        //    w.WriteAttributeString("xml", "lang", "http://www.w3.org/XML/1998/namespace", "en");
        //    w.WriteString(XMLTools.getElementTVValue(CCTS_TV.Definition, element) == "" ? element.Name : XMLTools.getElementTVValue(CCTS_TV.Definition, element)); // get label from definition tagged value
        //    w.WriteEndElement();//closing tag of label

        //    //labelArc
        //    w.WriteStartElement("labelArc", "http://www.xbrl.org/2003/linkbase");
        //    w.WriteAttributeString("type", "http://www.w3.org/1999/xlink", "arc");
        //    w.WriteAttributeString("arcrole", "http://www.w3.org/1999/xlink", "http://www.xbrl.org/2003/arcrole/concept-label");
        //    w.WriteAttributeString("from", "http://www.w3.org/1999/xlink", locatorName);
        //    w.WriteAttributeString("to", "http://www.w3.org/1999/xlink", labelName);
        //    w.WriteEndElement();
        //}
        #endregion
    }
}
