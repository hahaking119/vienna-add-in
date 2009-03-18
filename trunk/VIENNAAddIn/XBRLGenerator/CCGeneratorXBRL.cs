/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Schema;
using System.IO;
using VIENNAAddIn.Utils;
using VIENNAAddIn.Exceptions;
using VIENNAAddIn.CCTS;
using EA;
using VIENNAAddIn.constants;

namespace VIENNAAddIn.XBRLGenerator
{
    public partial class CCGeneratorXBRL : Form, GeneratorCallBackInterface
    {

        #region Variables
        private Repository repository;
        private GeneratorCallBackInterface caller;
        private bool withGUI;
        internal String scope;
        private const String strComplexType = "ComplexType";

        //This ArrayList holds a List of all auxilliary schmeas
        //that had to be created for this schema to be valid
        private System.Collections.ArrayList alreadyCreatedSchemas = new ArrayList();

        private String targetNameSpacePrefix = "bie";

        internal String path = "";

        static int countQDTImports = 0;
        static int countUDTImports = 0;
        static int countENUMImports = 0;
        static int countBIEImports = 0;

        string name = "";
        #endregion

        #region Constructor

        public CCGeneratorXBRL(EA.Repository repository, String scope)
        {
            InitializeComponent();

            this.repository = repository;
            this.scope = scope;
            this.withGUI = true;
            this.setActivePackageLabel();
        }

        public CCGeneratorXBRL(EA.Repository repository, EA.Package package, string path, GeneratorCallBackInterface caller)
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

        #region Method
        public void resetGenerator(String scope)
        {
            this.scope = scope;
            //this.alreadyCreatedSchemas = new ArrayList();
            this.progressBar1.Value = this.progressBar1.Minimum;
            this.statusTextBox.Text = "";
            this.setActivePackageLabel();
        }

        private void setActivePackageLabel()
        {
            EA.Package p = this.repository.GetPackageByID(Int32.Parse(this.scope));
            this.lblSelectedCClibrary.Text = p.Element.Name + "<<" + p.Element.Stereotype.ToString() + ">>";
        }

        public System.Collections.ICollection generateXBRLschema(EA.Package pkg)
        {
            #region main schema
            //schema for element ACC
            XmlSchema schema = new XmlSchema();
            addXBRLNamespace(schema);
            addXBRLImport(schema);
            //string schemaLocation = XMLTools.getSchemaNameWithoutExtention(this.repository.GetPackageByID(Int32.Parse(this.scope))) + "-structure" + ".xsd";
            //addXBRLInclude(schema, schemaLocation);
            ////create annotation for the linkbase
            //addAnnotationLinkbase(schema);
            #endregion

            #region schema for structure
            //schema for structure
            XmlSchema ctSchema = new XmlSchema();
            addXBRLNamespace(ctSchema);
            addXBRLImport(ctSchema);
            addXBRLInclude(ctSchema, XMLTools.getSchemaName(this.repository.GetPackageByID(Int32.Parse(this.scope))));
            this.performProgressStep();
            #endregion

            #region Create Taxonomy schema
            this.appendInfoMessage("Generating schema file...", pkg.Name);
            int elementCount = 0;
            //Iterate through the ACCs
            foreach (EA.Element element in pkg.Elements)
            {
                elementCount++;
                if (element.Stereotype.Equals(CCTS_Types.ACC.ToString()))
                {
                    try
                    {
                        getSchemaElement(element, schema);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

                //After every 20th iteration we  perform one progress step
                if (elementCount % 20 == 0)
                    this.performProgressStep();
            }
            #endregion

            #region write the schema
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

            if (ctSchema.Items.Count != 0)
            {
                //create annotation for the linkbase
                addAnnotationLinkbase(schema);

                filename = path + schemaPath + XMLTools.getSchemaNameWithoutExtention(this.repository.GetPackageByID(Int32.Parse(this.scope))) + "-structure" + ".xsd"; ;
                WriteSchemaToFile(filename, ctSchema);
            }
            #endregion

            return xsdSet.Schemas();

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


            //label
            XmlElement e = doc.CreateElement("xbrll", "linkbaseRef", "http://www.xbrl.org/2003/linkbase");
            string href = XMLTools.getSchemaNameWithoutExtention(this.repository.GetPackageByID(Int32.Parse(scope))) + "-label.xml";
            e.SetAttribute("type", "http://www.w3.org/1999/xlink", "simple");
            e.SetAttribute("href", "http://www.w3.org/1999/xlink", href);
            e.SetAttribute("title", "http://www.w3.org/1999/xlink", "Label Links, all");
            e.SetAttribute("role", "http://www.w3.org/1999/xlink", "http://www.xbrl.org/2003/role/labelLinkbaseRef");
            e.SetAttribute("arcrole", "http://www.w3.org/1999/xlink", "http://www.w3.org/1999/xlink/properties/linkbase");
            annNodes[0] = e;

            ////presentation
            //XmlElement e1 = doc.CreateElement("xbrll", "linkbaseRef", "http://www.xbrl.org/2003/linkbase");
            //href = XMLTools.getSchemaNameWithoutExtention(this.repository.GetPackageByID(Int32.Parse(scope))) + "-presentation.xml";
            //e1.SetAttribute("type", "http://www.w3.org/1999/xlink", "simple");
            //e1.SetAttribute("href", "http://www.w3.org/1999/xlink", href);
            //e1.SetAttribute("title", "http://www.w3.org/1999/xlink", "Presentation Links, all");
            //e1.SetAttribute("role", "http://www.w3.org/1999/xlink", "http://www.xbrl.org/2003/role/presentationLinkbaseRef");
            //e1.SetAttribute("arcrole", "http://www.w3.org/1999/xlink", "http://www.w3.org/1999/xlink/properties/linkbase");
            //annNodes[1] = e1;


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
            //schema for the element file




            if (e.Connectors.Count > 0)
            {
                bool isRoot = true;
                foreach (EA.Connector con in e.Connectors)
                {
                    //if current class is a client of another class in the same package --> not a root
                    if ((con.ClientID == e.ElementID) &&
                        (this.repository.GetElementByID(con.SupplierID).PackageID == e.PackageID))
                    {
                        isRoot = false;
                        break;
                    }
                }

                if (isRoot)
                {
                    #region generate element ACC for this root
                    XmlSchemaElement xmlElement = new XmlSchemaElement();
                    //for validation in UBMatrix, element's ID and element's Name must be the same
                    xmlElement.Name = XMLTools.getXMLName(e.Name);
                    xmlElement.Id = xmlElement.Name;
                    xmlElement.IsNillable = true;

                    if (isTuple(e))
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

                    #endregion

                    #region element for BCC for this root

                    foreach (EA.Attribute attr in e.Attributes)
                    {
                        XmlSchemaElement el = new XmlSchemaElement();
                        //for validation in UBMatrix, element's ID and element's Name must be the same
                        el.Name = XMLTools.getXBRLBbieName(e, attr);
                        el.Id = el.Name;
                        el.IsNillable = true;
                        setBCCType(el, attr, schema, e);
                        el.SubstitutionGroup = new XmlQualifiedName("item", "http://www.xbrl.org/2003/instance");

                        XmlDocument dummy2 = new XmlDocument();
                        XmlAttribute dummyAttr2 = dummy2.CreateAttribute("periodType", "http://www.xbrl.org/2003/instance");
                        dummyAttr2.Value = "instant";
                        el.UnhandledAttributes = new XmlAttribute[] { dummyAttr2 };
                        schema.Items.Add(el);
                    }
                    #endregion

                    //then generate element that related to this class recursively
                    ArrayList name = new ArrayList();
                    name.Add(e.Name);

                    //Create ComplexType if isTuple == true

                    if (isTuple(e))
                    {
                        generateTuple(e, schema, name);
                    }

                    GenerateSchemaRecursively(e, schema, name);
                }
            }

        }


        private void generateTuple(Element element, XmlSchema schema, ArrayList arrName)
        {
            //create BCC
            XmlSchemaSequence sequence = new XmlSchemaSequence();

            foreach (EA.Attribute bcc in element.Attributes)
            {
                XmlSchemaElement el = new XmlSchemaElement();
                string bccName = XMLTools.getXBRLBbieName(bcc, arrName);
                el.RefName = new XmlQualifiedName(bccName, schema.TargetNamespace); //Ref attribute
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
            restrict.BaseTypeName = new XmlQualifiedName("stringItemType", "http://www.xbrl.org/2003/instance");

            XmlSchemaComplexType complexType = new XmlSchemaComplexType();
            complexType.IsMixed = false;
            complexType.Name = XMLTools.getXBRLAsbieName(arrName);//element.Name;

            restrict.Particle = sequence;
            complexContent.Content = restrict;
            complexType.ContentModel = complexContent;

            schema.Items.Add(complexType);
        }

        private void GenerateSchemaRecursively(EA.Element element, XmlSchema schema, ArrayList name)
        {
            EA.Element e = element;

            foreach (EA.Connector con in e.Connectors)
            {
                ArrayList tempName = new ArrayList(name);
                if (con.Type == EA_Element.Aggregation.ToString() && con.Stereotype.ToString() == CCTS_Types.ASCC.ToString()
                        && isOutgoing(con, e))
                {
                    EA.Element client = this.repository.GetElementByID(con.ClientID);

                    if (con.ClientEnd.Role != "")
                        tempName.Add(con.ClientEnd.Role);
                    tempName.Add(client.Name);

                    if (!isInSamePackage(client))
                    {
                        //For ASCC that connect to ACC in other packages, just generate definitions for the ASCC but not for the ACC/BCC in the target package
                        #region ASCC that connect to ACC in other packages

                        //Generate ASCC
                        XmlSchemaElement conEl = new XmlSchemaElement();
                        conEl.Name = XMLTools.getXBRLAsbieName(tempName);
                        conEl.Id = conEl.Name;
                        conEl.IsNillable = true;
                        conEl.SchemaTypeName = new XmlQualifiedName("stringItemType", "http://www.xbrl.org/2003/instance");
                        //conEl.SchemaTypeName = new XmlQualifiedName(conEl.Id + "ItemType", XMLTools.getNameSpace(this.repository, this.repository.GetPackageByID(Int32.Parse(this.scope))));
                        conEl.SubstitutionGroup = new XmlQualifiedName("item", "http://www.xbrl.org/2003/instance");

                        XmlDocument dummy = new XmlDocument();
                        XmlAttribute dummyAttr = dummy.CreateAttribute("periodType", "http://www.xbrl.org/2003/instance");
                        dummyAttr.Value = "instant";
                        conEl.UnhandledAttributes = new XmlAttribute[] { dummyAttr };

                        schema.Items.Add(conEl);

                        //generate BCC
                        foreach (EA.Attribute attribute in client.Attributes)
                        {
                            XmlSchemaElement conEl2 = new XmlSchemaElement();
                            conEl2.Name = XMLTools.getXBRLBbieName(attribute, tempName);
                            conEl2.Id = conEl2.Name;
                            conEl2.IsNillable = true;
                            conEl2.SubstitutionGroup = new XmlQualifiedName("item", "http://www.xbrl.org/2003/instance");
                            setBCCType(conEl2, attribute, schema, element);

                            XmlDocument dummy2 = new XmlDocument();
                            XmlAttribute dummyAttr2 = dummy2.CreateAttribute("periodType", "http://www.xbrl.org/2003/instance");
                            dummyAttr2.Value = "instant";
                            conEl2.UnhandledAttributes = new XmlAttribute[] { dummyAttr2 };
                            schema.Items.Add(conEl2);
                        }

                        #endregion
                    }
                    else
                    {
                        //follow ASCC's to any depth until you reach an ACC with no further ASCC.
                        #region ASCC that connect to ACC in the same package

                        //generate ASCC for this connector
                        XmlSchemaElement conEl = new XmlSchemaElement();
                        conEl.Name = XMLTools.getXBRLAsbieName(tempName); //namanya kayaknya masih salah, ntar mbenerinnya klo dah dapet logicnya
                        conEl.Id = conEl.Name;
                        conEl.IsNillable = true;
                        conEl.SchemaTypeName = new XmlQualifiedName("stringItemType", "http://www.xbrl.org/2003/instance");
                        //conEl.SchemaTypeName = new XmlQualifiedName(conEl.Id + "ItemType", XMLTools.getNameSpace(this.repository, this.repository.GetPackageByID(Int32.Parse(this.scope))));
                        conEl.SubstitutionGroup = new XmlQualifiedName("item", "http://www.xbrl.org/2003/instance");
                        XmlDocument dummy = new XmlDocument();
                        XmlAttribute dummyAttr = dummy.CreateAttribute("periodType", "http://www.xbrl.org/2003/instance");
                        dummyAttr.Value = "instant";
                        conEl.UnhandledAttributes = new XmlAttribute[] { dummyAttr };
                        schema.Items.Add(conEl);


                        //generate BCC
                        foreach (EA.Attribute attribute in client.Attributes)
                        {
                            XmlSchemaElement conEl2 = new XmlSchemaElement();
                            conEl2.Name = XMLTools.getXBRLBbieName(attribute, tempName);
                            conEl2.Id = conEl2.Name;
                            conEl2.IsNillable = true;
                            conEl2.SubstitutionGroup = new XmlQualifiedName("item", "http://www.xbrl.org/2003/instance");
                            setBCCType(conEl2, attribute, schema, element);

                            XmlDocument dummy2 = new XmlDocument();
                            XmlAttribute dummyAttr2 = dummy2.CreateAttribute("periodType", "http://www.xbrl.org/2003/instance");
                            dummyAttr2.Value = "instant";
                            conEl2.UnhandledAttributes = new XmlAttribute[] { dummyAttr2 };
                            schema.Items.Add(conEl2);
                        }

                        #region Create ComplexType if isTuple == true
                        if (isTuple(client))
                        {
                            generateTuple(client, schema, tempName);
                        }
                        #endregion

                        //to avoid never ending looping from connector which point to itself
                        if (client.ElementID != element.ElementID)
                        {
                            //generate ASCC of related ACC.
                            GenerateSchemaRecursively(client, schema, tempName);
                        }
                        #endregion
                    }
                }
            }
        }


        /// <summary>
        /// Determine the element is a tuple or not
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private bool isTuple(Element e)
        {
            foreach (EA.TaggedValue tv in e.TaggedValues)
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


        

        private bool isInSamePackage(EA.Element e)
        {
            if (Int32.Parse(scope) == e.PackageID)
                return true;
            else
                return false;
        }

        private XmlQualifiedName createAndImportAuxilliarySchema(EA.Element e, XmlSchema schema)
        {
            EA.Package p = this.repository.GetPackageByID(e.PackageID);
            String pName = p.Name;

            //Is this schema already included?
            String s = "";
            if ((s = isSchemaAlreadyIncluded(p.Name)) == "")
            {
                System.Collections.ICollection result = new BIEGeneratorXBRL(this.repository, p, this.path, getCaller()).generateXBRLschema(p);
                //Create an Auxilliary schema and store it in the collection - later we have to
                //add it to the main schema collection
                AuxilliarySchema aux = new AuxilliarySchema();
                aux.Namespace = XMLTools.getNameSpace(this.repository, p);
                aux.NamespacePrefix = XMLTools.getNameSpacePrefix(p, "bie" + ++countBIEImports);
                aux.PackageOfOrigin = p.Name.ToString();
                aux.Schemas = result;
                this.alreadyCreatedSchemas.Add(aux);

                //Write the schema(s)
                String schemaPath = "";
                String schemaName = "";
                schemaPath = XMLTools.getSavePathForSchema(this.repository.GetPackageByID(p.PackageID), repository);
                schemaName = XMLTools.getSchemaName(this.repository.GetPackageByID(p.PackageID));
                String filename = path + schemaPath + schemaName;

                //if (CC_Utils.blnLinkedSchema)
                //{
                foreach (XmlSchema schema1 in result)
                {

                    //Create the path
                    System.IO.Directory.CreateDirectory(path + schemaPath);
                    Stream outputStream = System.IO.File.Open(filename, FileMode.Create);
                    schema1.Write(outputStream);
                    outputStream.Close();
                }
                //}
                //We need to add an import statement for the newly generated schema
                //to the original schema
                String importPath = XMLTools.getImportPathForSchema(p, repository, schemaName, scope);

                addXBRLImport(schema, importPath, aux.Namespace);

                //We need to add the namespace of this schema to the main schema
                schema.Namespaces.Add(aux.NamespacePrefix, aux.Namespace);

                //Set the namespace to the variable which is then used down below
                s = aux.Namespace;
            }

            return new XmlQualifiedName(e.Name + "ItemTypeASCC", s);
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

        /// <summary>
        /// Sets the type of the given bcc
        /// If the type refers to a cdt/qdt the method first checks,
        /// whether the relevant schema does already exist
        /// If not the schema is created and a reference via import statement
        /// is made
        /// </summary>
        /// <param name="e"></param>
        /// <param name="bcc"></param>
        private void setBCCType(XmlSchemaElement e, EA.Attribute bcc, XmlSchema schema, EA.Element element)
        {

            try
            {

                String type = bcc.Type;
                int classifierID = bcc.ClassifierID;
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


                e.SchemaTypeName = new XmlQualifiedName(bcc.Type + "Type", s);


            }
            catch (Exception ex)
            {
                this.appendWarnMessage("Unable to determine correct datatype for attribute " + bcc.Name + " in element " + element.Name + ". Taking xsd:string instead. ", this.getPackageName());
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

        }

        private bool isRootElement(Element rootEl)
        {
            foreach (EA.TaggedValue tv in rootEl.TaggedValues)
            {
                if (tv.Name.Equals("isRoot", StringComparison.OrdinalIgnoreCase) &&
                    (tv.Value.Equals("true", StringComparison.OrdinalIgnoreCase) || (tv.Value.Equals("1", StringComparison.OrdinalIgnoreCase))))
                    return true;
            }
            return false;
        }

        #endregion

        #region Event Handler
        
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
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

        #endregion
    }
}