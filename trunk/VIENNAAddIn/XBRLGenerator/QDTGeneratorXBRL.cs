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
using VIENNAAddIn.common;
using VIENNAAddIn.constants;

namespace VIENNAAddIn.XBRLGenerator
{
    public partial class QDTGeneratorXBRL : Form, GeneratorCallBackInterface
    {

        #region Variables
        private Repository repository;
        private String scope;
        private bool annotate = true;
        private bool withGUI;
        private GeneratorCallBackInterface caller;

        private bool DEBUG;

        //The path where the generated schema(s) will be saved
        internal String path = "";       
        private String targetNameSpacePrefix = "qdt";

        //These variables are used to make a distinction between namespaces of the same type        
        static int countUDTImports = 0;
        static int countENUMImports = 0;
        static int countBIEImports = 0;

        //This ArrayList holds a List of all auxilliary schmeas
        //that had to be created for this schema to be valid
        private System.Collections.ArrayList alreadyCreatedSchemas = new ArrayList();
        #endregion

        #region Constructor

        /// <summary>
        /// This constructor constructs a regular QDTGenerator with a GUI
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="scope"></param>
        public QDTGeneratorXBRL(EA.Repository repository, String scope,bool annotate)
        {

            //Set the debug mode
            DEBUG = Utility.DEBUG;

            this.repository = repository;
            this.scope = scope;
            this.annotate = annotate;
            this.withGUI = true;

            InitializeComponent();

            this.setActivePackageLabel();
        }

        /// <summary>
        /// This constructor constructs a QDTGenerator without a GUI
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="scope"></param>
        public QDTGeneratorXBRL(EA.Repository repository, String scope, bool annotate, String path, GeneratorCallBackInterface caller)
        {
            //Set the debug mode
            DEBUG = Utility.DEBUG;

            this.caller = caller;
            this.repository = repository;
            this.scope = scope;
            this.annotate = annotate;
            this.withGUI = false;
            //Set the path from the caller because the CDT Generator now operates in not visible
            //mode and hence no path is determined in this class
            this.path = path;

        }

        #endregion

        #region public methods


        /// <summary>
        /// Setze den Generator zurück
        /// </summary>
        /// <param name="scope"></param>
        public void resetGenerator(String scope) {
            this.scope = scope;
            this.alreadyCreatedSchemas = new ArrayList();
            if (this.withGUI) {
                this.progressBar1.Value = this.progressBar1.Minimum;
                this.setActivePackageLabel();
                this.statusTextBox.Text = "";
            }
        }
        
        public String TargetNameSpacePrefix {
            get { return targetNameSpacePrefix; }
            set { targetNameSpacePrefix = value; }
        }

        /// <summary>
        /// Generates a schema from the QDTs
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public System.Collections.ICollection generateXBRLschema(EA.Package p)
        {
            
            XmlSchema schema = new XmlSchema();
            schema.ElementFormDefault = XmlSchemaForm.Qualified;
            schema.AttributeFormDefault = XmlSchemaForm.Unqualified;
            schema.Version = XMLTools.getSchemaVersionFromPackage(p);

            //Add the necessary namespaces for the QDT schema
            addNameSpaces(schema);

            //Add the imports
            addImports(schema);


            int elementCount = 0;
            //Iterate through the CDTs
            foreach (EA.Element e in p.Elements) {
                elementCount++;
                if (e.Stereotype.Equals(CCTS_Types.QDT.ToString())) {

                    schema.Items.Add(getSchemaElement(e, schema));
                }
                //After every 20th iteration we perform a progress step
                if (elementCount % 20 == 0)
                    this.performProgressStep();
            }


            //Validate the Schema
            XmlSchemaSet xsdSet = new XmlSchemaSet();
            xsdSet.XmlResolver = null;//Since schemaLocation for XBRL is abstract, the schema set's default XmlResolver (XmlUrlResolver) will not know how to get them, so set it to null
            xsdSet.ValidationEventHandler += new ValidationEventHandler(ValidationCallbackOne);
            try {
                xsdSet.Add(schema);
                //xsdSet.Compile();
            }
            catch (XmlSchemaException xse) {
                Console.Write(xse.Message.ToString());
                Console.Write(xse.StackTrace.ToString());
                throw xse;
            }
                       
            return xsdSet.Schemas();
        }

        /// <summary>
        /// Is called from extern to append a message to this GUI
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        public void appendMessage(String type, String message, String packageName) {
            if (type == "info")
                this.appendInfoMessage(message, packageName);
            else if (type == "warn")
                this.appendWarnMessage(message, packageName);
            else if (type == "error")
                this.appendErrorMessage(message, packageName);

        }


        #endregion

        #region private methods

        /// <summary>
        /// Returns a Schema Element
        /// This method can either return a complexType or a SimpleType,
        /// depending on the basis of the QDT
        /// </summary>
        /// <param name="e"></param>
        /// <param name="schema"></param>
        /// <returns></returns>
        private XmlSchemaType getSchemaElement(EA.Element e, XmlSchema schema) {

            object[] obj = getBaseTypeName(e, schema);
            
            XmlQualifiedName xmlQualName = (XmlQualifiedName)obj[0];

            //Build in data type
            if (((Boolean)obj[1]) == true) {
                XmlSchemaComplexType complexType = new XmlSchemaComplexType();
                complexType.Name = XMLTools.getXMLName(e.Name) + "Type";

                //Annotate it?
                if (annotate)
                    complexType.Annotation = this.getQDTAnnotation(e);

                XmlSchemaSimpleContent simpleContent = new XmlSchemaSimpleContent();
                XmlSchemaSimpleContentRestriction simpleContent_restriction = new XmlSchemaSimpleContentRestriction();
                simpleContent_restriction.BaseTypeName = xmlQualName;

                //Is there an OCL constraint we have to incorporate?
                String CONType = ((EA.Attribute)obj[3]).Type;
                OCLMapper.processOCL(simpleContent_restriction, e, this.repository.GetPackageByID(e.PackageID), this, CONType);

                //In order to restrict the usage of attributes to the ones specified
                //in the QDT we have to add the prohibited attributes here
                IList attributes = (ArrayList)obj[2];
                if (attributes != null && attributes.Count != 0) {
                    foreach (String s in attributes) {
                        XmlSchemaAttribute at = new XmlSchemaAttribute();                                                
                        at.Use = XmlSchemaUse.Prohibited;
                        at.Name = s;
                        simpleContent_restriction.Attributes.Add(at);
                    }
                }

                //Those attributes whose multiplicity is 1..1 must be added here as well (with use=required)
                foreach (EA.Attribute attr in e.Attributes) {
                    if (attr.Stereotype.ToString().Equals(CCTS_Types.SUP.ToString()) && 
                        isRequired(attr)) {
                        XmlSchemaAttribute a = new XmlSchemaAttribute();                        
                        a.Name = attr.Name;
                        a.SchemaTypeName = new XmlQualifiedName(getXBRLType(attr, e), "http://www.xbrl.org/2003/instance");
                        a.Use = XmlSchemaUse.Required;
                        simpleContent_restriction.Attributes.Add(a);
                    }
                }


                simpleContent.Content = simpleContent_restriction;
                complexType.ContentModel = simpleContent;

                return complexType;
            }
            ////No build in datatype
            else {
                XmlSchemaComplexType complexType = new XmlSchemaComplexType();
                complexType.Name = XMLTools.getXMLName(e.Name) + "Type";

                ////Annotate it?
                if (annotate)
                    complexType.Annotation = this.getQDTAnnotation(e);

                XmlSchemaSimpleContent simpleContent = new XmlSchemaSimpleContent();
                XmlSchemaSimpleContentRestriction simpleContent_restriction = new XmlSchemaSimpleContentRestriction();                                
                simpleContent_restriction.BaseTypeName = xmlQualName;

                //Iterate through the properties of the QDT
                foreach (EA.Attribute attribute in e.Attributes) {
                    if (attribute.Stereotype.ToString().Equals(CCTS_Types.SUP.ToString())) {
                        XmlSchemaAttribute att = new XmlSchemaAttribute();
                        //Annotate the attribute
                        if (annotate)
                            att.Annotation = this.getAttributeAnnotation(attribute);
                        att.Name = attribute.Name;
                        att.SchemaTypeName = new XmlQualifiedName(getXBRLType(attribute, e), "http://www.xbrl.org/2003/instance");                        
                        //If the mulitplicity of the attribute is 1..1 then the use
                        //attribute must be set to required
                        if (isRequired(attribute))
                            att.Use = XmlSchemaUse.Required;
                        simpleContent_restriction.Attributes.Add(att);
                    }
                }

                simpleContent.Content = simpleContent_restriction;
                complexType.ContentModel = simpleContent;

                return complexType;
            }
        }


        /// <summary>
        /// Determines whether the use of the Attribute is required or not
        /// The use of the attribute is required if the multiplicity is set to 1..1
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        private bool isRequired(EA.Attribute a) {
            bool b = false;
            if ((a.LowerBound != null && a.LowerBound.Equals("1")) &&
                (a.UpperBound != null && a.UpperBound.Equals("1")))
                b = true;
            return b;
        }





        /// <summary>
        /// Returns the base type of this qdt element
        /// The base type is the type of the CON attribute
        /// </summary>
        /// <param name="e"></param>
        /// <returns>object array
        /// [0] = QualifiedName (XmlQualifiedName)
        /// [1] = buildInType (true/false)
        /// [2] = AttributeList (List)
        /// [3] = the CON Attribute
        ///</returns>
        private object [] getBaseTypeName(EA.Element e, XmlSchema schema) {

            XmlQualifiedName xmlQualifiedName = null;
            System.Collections.IList attributesOfCDT = null;
            bool isBuildIn = true;
            EA.Attribute conAttribute = null;

            try {

                //Get the CON Attribute
                foreach (EA.Attribute attribute in e.Attributes) {
                    if (attribute.Stereotype.ToString().Equals(CCTS_Types.CON.ToString())) {
                        conAttribute = attribute;
                        break;
                    }
                }

                //No CON Attribute - throw an exception
                if (conAttribute == null)
                    throw new XMLException("The element " + e + " has no CON attribute. Unable to proceed with schema generation");

                //If the CON Attribute has no classifierID, a build in type has been taken
                EA.Element el = null;
                try {
                    el = this.repository.GetElementByID(conAttribute.ClassifierID);
                    isBuildIn = false;
                }
                catch (Exception eex) {
                    this.appendErrorMessage(eex.Message, this.repository.GetPackageByID(e.PackageID).Name);
                    throw new UnexpectedElementException(eex.Message,"Error occurs when trying to get classifier ID fo element " 
                        + el.Name + " in package " + this.repository.GetPackageByID(el.PackageID).Name);
                }

                EA.Element classifierElement = null;

                if (!isBuildIn) {
                    //Get the classifier element
                    //In the case of a non build in type this is the ENUM
                    classifierElement = this.repository.GetElementByID(conAttribute.ClassifierID);
                }
                else {
                    //Get the classifier element
                    //In the case of a build in type this is the underlying CDT
                    classifierElement = getCDTofQDT(e);
                    if (classifierElement == null)
                        this.appendErrorMessage("Could not find the underlying CDT for the QDT " + e.Name, this.scope.ToString());

                    //We have to get the attributes of the CDT
                    attributesOfCDT = getProhibitedAttributesOfCDT(e);
                }


                //Is there already an XML schema for this library?
                EA.Package importPackage = this.repository.GetPackageByID(classifierElement.PackageID);
                String stereotype = importPackage.Element.Stereotype.ToString();

                //Is this schema already included?
                String s = "";
                if ((s = isSchemaAlreadyIncluded(importPackage.Name)) == "") {

                    //Create an Auxilliary schema and store it in the collection - later we have to
                    //add it to the main schema collection
                    AuxilliarySchema aux = new AuxilliarySchema();
                    System.Collections.ICollection result = null;

                    if (stereotype.Equals(CCTS_Types.CDTLibrary.ToString())) {
                        result = new CDTGeneratorXBRL(this.repository, importPackage.PackageID.ToString(), annotate, this.path, getCaller()).generateSchema(importPackage);
                        aux.NamespacePrefix = XMLTools.getNameSpacePrefix(importPackage,"udt" + ++countUDTImports);
                    }
                    else if (stereotype.Equals(CCTS_Types.ENUMLibrary.ToString())) {
                        result = new ENUMGeneratorXBRL(this.repository, importPackage, this.path, getCaller()).generateXBRLschema(importPackage);
                        aux.NamespacePrefix = XMLTools.getNameSpacePrefix(importPackage,"enum" + ++countENUMImports);
                    }
                    else {
                        //If the stereotype is neither CDT nor QDT we have to raise an error here
                        this.appendErrorMessage("The CON Attribute of the element " + e.Name + " points to an element in the following library ("+stereotype+"): " + importPackage.Name, importPackage.Name);
                        throw new XMLException("");
                    }

                    aux.Namespace = XMLTools.getNameSpace(this.repository,importPackage);
                    aux.PackageOfOrigin = importPackage.Name.ToString();
                    aux.Schemas = result;
                    this.alreadyCreatedSchemas.Add(aux);
                    
                    //Write the schema(s)

                    String schemaPath = "";
                    String schemaName = "";
                    schemaName = XMLTools.getSchemaName(this.repository.GetPackageByID(importPackage.PackageID));
                    schemaPath = XMLTools.getSavePathForSchema(this.repository.GetPackageByID(importPackage.PackageID), repository);
                    String filename = path + schemaPath + schemaName;

                    foreach (XmlSchema schema1 in result)
                    {
                        //Create the path
                        System.IO.Directory.CreateDirectory(path + schemaPath);
                        Stream outputStream = System.IO.File.Open(filename, FileMode.Create);
                        schema1.Write(outputStream);
                        outputStream.Close();
                    }

                    //We need to add an import statement for the newly generated schema
                    //to the original schema
                    String importPath = XMLTools.getImportPathForSchema(this.repository.GetPackageByID(importPackage.PackageID), repository, schemaName,scope);

                    addImport(schema, importPath, aux.Namespace);

                    //We need to add the namespace of this schema to the main schema
                    schema.Namespaces.Add(aux.NamespacePrefix, aux.Namespace);

                    //Set the namespace to the variable which is then used down below
                    s = aux.Namespace;
                }

                xmlQualifiedName = new XmlQualifiedName(classifierElement.Name + "Type", s);

            }
            catch (XMLException xmlException) {               
                throw xmlException;
            }
            catch (Exception ex) {
                this.appendWarnMessage(ex.Message, this.getPackageName());
                xmlQualifiedName = new XmlQualifiedName("stringItemType", "http://www.xbrl.org/2003/instance");
            }

            object[] rv = { xmlQualifiedName, isBuildIn, attributesOfCDT, conAttribute };
            return rv;
        }


        /// <summary>
        /// Get the attributes of the CDT
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private IList getProhibitedAttributesOfCDT(EA.Element e) {
            IList list = null;
            //Get the CDT on which the QDT is based on via the basedOn dependency
            foreach (EA.Connector con in e.Connectors) {
                if (con.Stereotype.ToString() == CCTS_Types.basedOn.ToString()) {
                    //Get the supplier of the connector
                    EA.Element cdt = this.repository.GetElementByID(con.SupplierID);
                    list = new ArrayList();
                    //Copy the SUP attributes of the QDT in a List
                    IList qdtAttributes = new ArrayList();
                    foreach (EA.Attribute a in e.Attributes) {
                        if (a.Stereotype.ToString() == CCTS_Types.SUP.ToString())
                            qdtAttributes.Add(a.Name);
                    }
                    
                    //copy the attributes of the cdt which are not contained
                    //in the qdt
                    foreach (EA.Attribute attribute in cdt.Attributes) {
                        if (attribute.Stereotype.ToString() == CCTS_Types.SUP.ToString()) {
                            if (!qdtAttributes.Contains(attribute.Name))
                                list.Add(attribute.Name);
                        }
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// Get the CDT which is the basis for the passed QDT
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private EA.Element getCDTofQDT(EA.Element e) {
            //Get the CDT on which the QDT is based on via the basedOn dependency
            foreach (EA.Connector con in e.Connectors) {
                if (con.Stereotype.ToString() == CCTS_Types.basedOn.ToString()) {
                    //Get the supplier of the connector
                    return this.repository.GetElementByID(con.SupplierID);                    
                }
            }
            return null;
        }

        

        /// <summary>
        /// Returns the XSD-Type for the passed EA Type
        /// </summary>
        /// <returns></returns>
        private String getXBRLType(EA.Attribute a, EA.Element e) {
            String determinedElementType = "";
            try {
                determinedElementType = XMLTools.getXBRLType(a,e);
            }
            catch (XMLException xe) {
                this.appendWarnMessage(xe.Message, this.getPackageName());
                determinedElementType = "stringItemType";
            }
            return determinedElementType;
        }

        /// <summary>
        /// Adds the necessary Namespaces to the Schema
        /// </summary>
        /// <param name="schema"></param>
        private void addNameSpaces(XmlSchema schema) {

            schema.Namespaces.Add("", "http://www.w3.org/2001/XMLSchema");
            
            schema.Namespaces.Add("ccts", "urn:un:unece:uncefact:documentation:standard:CoreComponentsTechnicalSpecification:2");

            schema.Namespaces.Add("xbrll", "http://www.xbrl.org/2003/linkbase");
            schema.Namespaces.Add("xlink", "http://www.w3.org/1999/xlink");
            schema.Namespaces.Add("xbrli", "http://www.xbrl.org/2003/instance");
            schema.Namespaces.Add(XMLTools.getNameSpacePrefix(this.repository.GetPackageByID(Int32.Parse(this.scope)), this.targetNameSpacePrefix), XMLTools.getNameSpace(this.repository, this.repository.GetPackageByID(Int32.Parse(this.scope))));
            schema.TargetNamespace = XMLTools.getNameSpace(this.repository, this.repository.GetPackageByID(Int32.Parse(this.scope)));
        }

        /// <summary>
        /// Generate a schema from the QDT library
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGenerateSchema_Click(object sender, EventArgs e)
        {
            this.resetGenerator(this.scope);

          
            if (DEBUG) {
                path = "C:\\Dokumente und Einstellungen\\pliegl\\Desktop\\xsd-schema";
            }
            else {
                DialogResult dr = this.dlgSavePath.ShowDialog(this);

                if (dr.Equals(DialogResult.Cancel)) {
                    this.dlgSavePath.Dispose();
                    return;
                } 

                path = this.dlgSavePath.SelectedPath;
            }

            if (this.path == null || this.path.Equals("")) {
                this.appendErrorMessage("Please select a location for the generated schemas first.",this.getPackageName());
            }
            else {

                this.path = this.path + "\\";
                //Get the active Package
                EA.Package p = this.repository.GetPackageByID(Int32.Parse(this.scope));

                this.performProgressStep();

                String error = "";
                System.Collections.ICollection result = null;

                this.appendInfoMessage("Starting QDT schema creation. Please wait.", this.getPackageName());

                try {
                    result = generateXBRLschema(p);
                }
                catch (Exception exc) {
                    error = exc.Message;
                }

                //Kein Fehler aufgetreten - schreibe das Ergebnis
                if (error == "") {
                    foreach (XmlSchema schema in result) {
                        
                        String schemaPath = XMLTools.getSavePathForSchema(this.repository.GetPackageByID(Int32.Parse(this.scope)), repository);
                        String filename = path + schemaPath + XMLTools.getSchemaName(this.repository.GetPackageByID(Int32.Parse(this.scope)));
                        //Create the path
                        System.IO.Directory.CreateDirectory(path + schemaPath);
                        Stream outputStream = System.IO.File.Open(filename, FileMode.Create);
                        schema.Write(outputStream);
                        outputStream.Close();
                    }
                    this.appendInfoMessage("The schema was created successfully.", this.getPackageName());
                }
                else {
                    this.appendErrorMessage(error, this.getPackageName());
                }

                this.progressBar1.Value = this.progressBar1.Maximum;
            }
        }

        /// <summary>
        /// Determine wheter the attribute has use=optional or use=required
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="schemaAttribute"></param>
        private void getOptionalOrRequired(EA.Attribute attribute, XmlSchemaAttribute schemaAttribute) {

            //Get the lower bound of the attribute
            String lowerBound = attribute.LowerBound;
            schemaAttribute.Use = XmlSchemaUse.Required;
            try {
                int i = Int32.Parse(lowerBound);
                if (i == 0)
                    schemaAttribute.Use = XmlSchemaUse.Optional;
            }
            catch (Exception e) { }

        }


        private static void ValidationCallbackOne(object sender, ValidationEventArgs args) {
            throw new XmlSchemaException(args.Message + args.Exception.StackTrace);
        }

        /// <summary>
        /// Setze den Text der ausgewählten QDTLibrary
        /// </summary>
        private void setActivePackageLabel() {
            EA.Package p = this.repository.GetPackageByID(Int32.Parse(this.scope));
            this.selectedQDTLibrary.Text = p.Element.Name + "<<" + p.Element.Stereotype.ToString() + ">>";
        }

        /// <summary>
        /// Performs a step with the progress bar
        /// </summary>
        public void performProgressStep() {
            if (this.withGUI) {
                this.progressBar1.PerformStep();
            }
            else {
                //Is there a caller for this class?
                if (caller != null)
                    caller.performProgressStep();
            }
        }

        /// <summary>
        /// Append an error message to the status box
        /// </summary>
        /// <param name="msg"></param>
        private void appendErrorMessage(String msg, String packageName) {
            if (caller != null) {
                caller.appendMessage("error", msg, this.getPackageName());
            }
            else {
                if (this.withGUI) {
                    this.statusTextBox.Text += "ERROR: (Package: " + packageName + ") " + msg + "\n\n";
                }
            }
        }
        /// <summary>
        /// Show a info message in the status box
        /// </summary>
        /// <param name="msg"></param>
        private void appendInfoMessage(String msg, String packageName) {
            if (caller != null) {
                caller.appendMessage("info", msg, this.getPackageName());
            }
            else {
                if (this.withGUI) {
                    this.statusTextBox.Text += "INFO: (Package: " + packageName + ") " + msg + "\n\n";
                }
            }
        }
        /// <summary>
        /// Show a warn message in the status box
        /// </summary>
        /// <param name="msg"></param>
        private void appendWarnMessage(String msg, String packageName) {
            if (caller != null) {
                caller.appendMessage("warn", msg, this.getPackageName());
            }
            else {
                if (this.withGUI)
                    this.statusTextBox.Text += "WARN: (Package: " + packageName + ") " + msg + "\n\n";
            }
        }


        /// <summary>
        /// Reset the Text of the StatusTextBox
        /// </summary>
        private void resetMessageText() {
            if (this.withGUI)
                this.statusTextBox.Text = "";
        }


        /// <summary>
        /// Add the necessary Imports
        /// </summary>
        /// <param name="schema"></param>
        private void addImports(XmlSchema schema) {
            XmlSchemaImport import = new XmlSchemaImport();
            import.Namespace = "http://www.xbrl.org/2003/instance";
            import.SchemaLocation = "http://www.xbrl.org/2003/xbrl-instance-2003-12-31.xsd";
            schema.Includes.Add(import);
        }

        /// <summary>
        /// Return the Annotation for a given cdt
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private XmlSchemaAnnotation getQDTAnnotation(EA.Element qdt) {
            XmlSchemaAnnotation ann = new XmlSchemaAnnotation();
            XmlSchemaDocumentation doc = new XmlSchemaDocumentation();
            doc.Language = "en";
            XmlDocument xml = new XmlDocument();

            String name = qdt.Name;

            //These arrays hold the names and the values of the annotation
            String[] nodes = { "*UniqueID", 
                "*Acronym", 
                "*DictionaryEntryName", 
                "*Version", 
                "*Definition", 
                "*PrimaryRepresentationTerm", 
                "*DataTypeQualifierTerm", 
                "*PrimitiveType", 
                "*BusinessProcessContextValue", 
                "*GeopoliticalOrRegionContextValue", 
                "*OfficialConstraintContextValue", 
                "*ProductContextValue", 
                "*IndustryContextValue", 
                "*BusinessProcessRoleContextValue", 
                "*SupportingRoleContextValue", 
                "*SystemCapabilitiesContextValue", 
                "*UsageRule", 
                "*BusinessTerm", 
                "*Example" };

            String[] values = { XMLTools.getElementTVValue(CCTS_TV.UniqueID, qdt), 
                "QDT", 
                XMLTools.getElementTVValue(CCTS_TV.DictionaryEntryName, qdt), 
                XMLTools.getElementTVValue(CCTS_TV.Version, qdt), 
                XMLTools.getElementTVValue(CCTS_TV.Definition, qdt), 
                XMLTools.getElementTVValue(CCTS_TV.PrimaryRepresentationTerm, qdt),
                XMLTools.getElementTVValue(CCTS_TV.DataTypeQualifierTerm, qdt),
                XMLTools.getElementTVValue(CCTS_TV.PrimitiveType, qdt),
                XMLTools.getElementTVValue(CCTS_TV.BusinessProcessContextValue, qdt),
                XMLTools.getElementTVValue(CCTS_TV.GeopoliticalOrRegionContextValue, qdt),
                XMLTools.getElementTVValue(CCTS_TV.OfficialConstraintContextValue, qdt),
                XMLTools.getElementTVValue(CCTS_TV.ProductContextValue, qdt),
                XMLTools.getElementTVValue(CCTS_TV.IndustryContextValue, qdt),
                XMLTools.getElementTVValue(CCTS_TV.BusinessProcessRoleContextValue, qdt),
                XMLTools.getElementTVValue(CCTS_TV.SupportingRoleContextValue, qdt),
                XMLTools.getElementTVValue(CCTS_TV.SystemCapabilitiesContextValue, qdt),
                XMLTools.getElementTVValue(CCTS_TV.UsageRule, qdt),
                XMLTools.getElementTVValue(CCTS_TV.BusinessTerm, qdt),
                XMLTools.getElementTVValue(CCTS_TV.Example, qdt)
                };

            XmlNode[] annNodes = new XmlNode[nodes.Length];
            for (int i = 0; i < nodes.Length; i++) {
                //If a node is optional (a node is optional if its name starts with a *)
                //we only include it, if a value is specified
                bool include = true;
                if (nodes[i].Substring(0, 1) == "*" && values[i] == "") {
                    include = false;
                }
                if (include) {
                    XmlNode node = xml.CreateElement("ccts", nodes[i].Replace("*", ""), "urn:un:unece:uncefact:documentation:standard:CoreComponentsTechnicalSpecification:2");
                    if (values[i] != "")
                        node.InnerText = values[i];
                    annNodes[i] = node;
                }
            }

            doc.Markup = annNodes;
            ann.Items.Add(doc);

            return ann;
        }


        /// <summary>
        /// Returns the annoation for an attribute
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        private XmlSchemaAnnotation getAttributeAnnotation(EA.Attribute attribute) {
            XmlSchemaAnnotation ann = new XmlSchemaAnnotation();
            XmlSchemaDocumentation doc = new XmlSchemaDocumentation();

            XmlDocument xml = new XmlDocument();

            String name = attribute.Name;
            String card = attribute.LowerBound + ".." + attribute.UpperBound;

            //These arrays hold the names and the values of the annotation
            String[] nodes = { 
                "*Acronym",                  
                "*Name",
                "*Definition", 
                "*Cardinality", 
                "*ObjectClassTerm", 
                "*PropertyTerm", 
                "*PrimitiveType", 
                "*Example" };

            String[] values = {                  
                "SC", 
                XMLTools.getAttributeTVValue(CCTS_TV.Name, attribute), 
                XMLTools.getAttributeTVValue(CCTS_TV.Definition, attribute), 
                card, 
                XMLTools.getAttributeTVValue(CCTS_TV.ObjectClassTerm, attribute),  
                XMLTools.getAttributeTVValue(CCTS_TV.PropertyTerm, attribute),
                XMLTools.getAttributeTVValue(CCTS_TV.PrimitiveType, attribute),
                XMLTools.getAttributeTVValue(CCTS_TV.Example, attribute)
                };

            XmlNode[] annNodes = new XmlNode[nodes.Length];
            for (int i = 0; i < nodes.Length; i++) {
                //If a node is optional (a node is optional if its name starts with a *)
                //we only include it, if a value is specified
                bool include = true;
                if (nodes[i].Substring(0, 1) == "*" && values[i] == "") {
                    include = false;
                }
                if (include) {
                    XmlNode node = xml.CreateElement("ccts", nodes[i].Replace("*", ""), "urn:un:unece:uncefact:documentation:standard:CoreComponentsTechnicalSpecification:2");
                    if (values[i] != "")
                        node.InnerText = values[i];
                    annNodes[i] = node;
                }
            }

            doc.Markup = annNodes;
            ann.Items.Add(doc);
            return ann;
        }


        /// <summary>
        /// Returns the type of the CON element of the passed EA.Element
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private String getCONAttributeTypeFromElement(EA.Element e) {

            foreach (EA.Attribute attribute in e.Attributes) {
                if (attribute.Stereotype.ToString().Equals(CCTS_Types.CON.ToString()))
                    return attribute.Type;
            }
            return "";
        }


        /// <summary>
        /// Add a new import to the schema
        /// </summary>
        /// <param name="schema"></param>
        private void addImport(XmlSchema schema, String schemaLocation, String namespace_) {
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
        private String isSchemaAlreadyIncluded(String packageName) {
            foreach (AuxilliarySchema aux in this.alreadyCreatedSchemas) {
                if (aux.PackageOfOrigin == packageName)
                    return aux.Namespace;
            }
            return "";
        }


        /// <summary>
        /// Cancel Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e) {
            this.Visible = false;
        }

        /// <summary>
        /// Returns the name of the current package
        /// </summary>
        /// <returns></returns>
        private String getPackageName() {
            return this.repository.GetPackageByID(Int32.Parse(this.scope)).Name.ToString();
        }



        /// <summary>
        /// This methods determins, what should be passed to an auxilliary schema generator
        /// If this class itself has already been called by another class, the calling class is passed
        /// otherwise an instance of this class is passed
        /// </summary>
        /// <returns></returns>
        private GeneratorCallBackInterface getCaller() {
            if (this.caller == null)
                return this;
            else
                return caller;
        }


        #endregion
    }
 
}