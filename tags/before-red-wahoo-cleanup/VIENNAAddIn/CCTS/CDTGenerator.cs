/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Schema;
using VIENNAAddIn.Utils;
using VIENNAAddIn.Exceptions;
using VIENNAAddIn.common;
using VIENNAAddIn.constants;
using EA;

namespace VIENNAAddIn.CCTS {
    public partial class CDTGenerator : Form, GeneratorCallBackInterface
    {
        private static CDTGenerator form;
        public static void ShowForm(Repository repository)
        {
            var scope = repository.DetermineScope();
            if (form == null || form.IsDisposed)
            {
                form = new CDTGenerator(repository, scope, false);
                form.Show();
            }
            else
            {
                form.resetGenerator(scope);
                form.Select();
                form.Focus();
                form.Show();
            }
        }



        #region Variables
        private EA.Repository repository;
        private String scope;
        private bool annotate;
        private bool withGUI;
        private bool blnAlias;
        
        private GeneratorCallBackInterface caller;

        private bool DEBUG;

        //The path where the generated schema(s) will be saved
        private String path = "";
               
        private String targetNameSpacePrefix = "udt";
        #endregion


        #region Constructor
        /// <sUMM2ary>
        /// This constructor creates a regular CDTGenerator with a GUI
        /// </sUMM2ary>
        /// <param name="repository"></param>
        /// <param name="scope"></param>
        /// <param name="annotate"></param>
        public CDTGenerator(EA.Repository repository, String scope, bool annotate) {
            
            //Set the debug mode
            DEBUG = Utility.DEBUG;

            this.repository = repository;
            this.scope = scope;
            this.annotate = annotate;
            
            //With GUI
            this.withGUI = true;
            InitializeComponent();
            this.setActivePackageLabel();
        }


        /// <summary>
        /// This constructor is made special for any-level schema generation
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="scope"></param>
        /// <param name="annotate"></param>
        /// <param name="path"></param>
        public CDTGenerator(EA.Repository repository, String scope, bool annotate, bool blnAlias, string path)
        {

            //Set the debug mode
            DEBUG = Utility.DEBUG;

            this.repository = repository;
            this.scope = scope;
            this.annotate = annotate;
            this.blnAlias = blnAlias;

            //With GUI
            this.withGUI = false;
            this.path = path;   
            InitializeComponent();
            this.setActivePackageLabel();
        }

        /// <sUMM2ary>
        /// This constructor creates a CDTGenerator with no GUI
        /// </sUMM2ary>
        /// <param name="repository"></param>
        /// <param name="scope"></param>
        /// <param name="annotate"></param>
        /// <param name="hidden"></param>
        public CDTGenerator(EA.Repository repository, String scope, bool annotate, String path, GeneratorCallBackInterface caller) {
            
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






        /// <sUMM2ary>
        /// Setze den Generator zurück
        /// </sUMM2ary>
        /// <param name="scope"></param>
        public void resetGenerator(String scope) {
            this.scope = scope;
            
            if (this.withGUI) {
                this.progressBar1.Value = this.progressBar1.Minimum;
                this.setActivePackageLabel();
                this.statusTextBox.Text = "";
            }
        }




        /// <sUMM2ary>
        /// Setze den Text der ausgewählten BIELibrary
        /// </sUMM2ary>
        private void setActivePackageLabel() {
            if (this.withGUI) {
                EA.Package p = this.repository.GetPackageByID(Int32.Parse(this.scope));
                this.selectedBIELibrary.Text = p.Element.Name + "<<" + p.Element.Stereotype.ToString() + ">>";
            }
        }


        /// <sUMM2ary>
        /// Performs a step with the progress bar
        /// </sUMM2ary>
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


        public static void ValidationCallbackOne(object sender, ValidationEventArgs args) {
            throw new XmlSchemaException(args.Message + args.Exception.StackTrace);
        }

        /// <sUMM2ary>
        /// Generate a Schema from the CDTs
        /// </sUMM2ary>
        /// <param name="p"></param>
        /// <returns></returns>
        public System.Collections.ICollection generateSchema(EA.Package p) {

            XmlSchema schema = new XmlSchema();
            schema.ElementFormDefault = XmlSchemaForm.Qualified;
            schema.AttributeFormDefault = XmlSchemaForm.Unqualified;
            schema.Version = XMLTools.getSchemaVersionFromPackage(p);

            //Add the necessary namespaces for the CDT schema
            addNameSpaces(schema);

            //Add the imports
            addImports(schema);

            int elementCount = 0;
            //Iterate through the CDTs
            foreach (EA.Element e in p.Elements) {
                elementCount++;

                if (e.Stereotype.Equals(CCTS_Types.CDT.ToString())) {
                    schema.Items.Add(getSchemaElement(e, schema));
                }
                //After every 20th iteration we perform a progress step
                if (elementCount % 20 == 0)
                    this.performProgressStep();
            }

            //Validate the Schema
            XmlSchemaSet xsdSet = new XmlSchemaSet();
            xsdSet.ValidationEventHandler += new ValidationEventHandler(ValidationCallbackOne);
            try {
                xsdSet.Add(schema);
                xsdSet.Compile();
            }
            catch (XmlSchemaException xse) {
                Console.Write(xse.Message.ToString());
                Console.Write(xse.StackTrace.ToString());
                throw xse;
            }


            return xsdSet.Schemas();
        }


        /// <sUMM2ary>
        /// Returns a Schema Element
        /// </sUMM2ary>
        /// <param name="e"></param>
        /// <param name="schema"></param>
        /// <returns></returns>
        private XmlSchemaComplexType getSchemaElement(EA.Element e, XmlSchema schema) {

            XmlSchemaComplexType complexType = new XmlSchemaComplexType();
            complexType.Name = XMLTools.getXMLName(e.Name) + "Type";

            //Annotate it?
            if (annotate)
                complexType.Annotation = this.getCDTAnnotation(e);

            XmlSchemaSimpleContent simpleContent = new XmlSchemaSimpleContent();
            XmlSchemaSimpleContentExtension simpleContent_extension = new XmlSchemaSimpleContentExtension();
            simpleContent_extension.BaseTypeName = getBaseTypeName(e);

            //Iterate through the properties of the CDT
            foreach (EA.Attribute attribute in e.Attributes) {
                if (attribute.Stereotype.ToString().Equals(CCTS_Types.SUP.ToString())) {
                    XmlSchemaAttribute att = new XmlSchemaAttribute();
                    //Annotate the attribute
                    if (annotate)
                        att.Annotation = getAttributeAnnoation(attribute);
                    att.Name = attribute.Name;
                    att.SchemaTypeName = new XmlQualifiedName(getXSDType(attribute, e), "http://www.w3.org/2001/XMLSchema");
                    getOptionalOrRequired(attribute, att);
                    simpleContent_extension.Attributes.Add(att);
                }
            }

            simpleContent.Content = simpleContent_extension;
            complexType.ContentModel = simpleContent;

            return complexType;
        }


        /// <sUMM2ary>
        /// Determine wheter the attribute has use=optional or use=required
        /// </sUMM2ary>
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
            catch (Exception e) {}

        }





        /// <sUMM2ary>
        /// Returns the base type of this cdt element
        /// </sUMM2ary>
        /// <param name="e"></param>
        /// <returns></returns>
        private XmlQualifiedName getBaseTypeName(EA.Element e) {

            //Locate the Attribute stereotyped as CON and determine its type
            EA.Attribute a = null;
            bool found = false;
            foreach (EA.Attribute attribute in e.AttributesEx) {
                if (attribute.Stereotype != null && attribute.Stereotype.ToString().Equals(CCTS_Types.CON.ToString())) {
                    a = attribute;
                    found = true;
                    break;
                }
            }

            if (found) {
                return new XmlQualifiedName(getXSDType(a, e), "http://www.w3.org/2001/XMLSchema");
            }
            else {
                appendWarnMessage("Unable to find CON Attribute for the CDT " +  e.Name + ". Taking xsd:string as basetype for the CDT instead.", this.getPackageName());
                return new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema");
            }
                       
        }

            
        /// <sUMM2ary>
        /// Returns the XSD-Type for the passed EA Type
        /// </sUMM2ary>
        /// <returns></returns>
        private String getXSDType(EA.Attribute a, EA.Element e) {
            String determinedElementType = "";
            try {
                determinedElementType = XMLTools.getXSDType(a, e);
            }
            catch (XMLException xe) {
                this.appendWarnMessage(xe.Message, this.getPackageName());
                determinedElementType = "string";
            }
            return determinedElementType;
        }



        /// <sUMM2ary>
        /// Return the Annotation for a given cdt
        /// </sUMM2ary>
        /// <param name="name"></param>
        /// <returns></returns>
        private XmlSchemaAnnotation getCDTAnnotation(EA.Element cdt) {
            XmlSchemaAnnotation ann = new XmlSchemaAnnotation();
            XmlSchemaDocumentation doc = new XmlSchemaDocumentation();
            doc.Language = "en";
            XmlDocument xml = new XmlDocument();

            String name = cdt.Name;

            //These arrays hold the names and the values of the annotation
            String[] nodes = { "*UniqueID", 
                "*Acronym", 
                "*DictionaryEntryName", 
                "*Version", 
                "*Definition", 
                "*PrimaryRepresentationTerm", 
                "*PrimitiveType", 
                "*UsageRule", 
                "*BusinessTerm", 
                "*Example"};

            String[] values = { XMLTools.getElementTVValue(CCTS_TV.UniqueID, cdt),
                "CCT", 
                XMLTools.getElementTVValue(CCTS_TV.DictionaryEntryName, cdt), 
                XMLTools.getElementTVValue(CCTS_TV.Version, cdt), 
                XMLTools.getElementTVValue(CCTS_TV.Definition, cdt), 
                getCONAttributeTypeFromElement(cdt),
                XMLTools.getElementTVValue(CCTS_TV.PrimitiveType, cdt),
                XMLTools.getElementTVValue(CCTS_TV.UsageRule, cdt),
                XMLTools.getElementTVValue(CCTS_TV.BusinessTerm, cdt),
                XMLTools.getElementTVValue(CCTS_TV.Example, cdt)            
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


        /// <sUMM2ary>
        /// Returns the annoation for an attribute
        /// </sUMM2ary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        private XmlSchemaAnnotation getAttributeAnnoation(EA.Attribute attribute) {
            XmlSchemaAnnotation ann = new XmlSchemaAnnotation();
            XmlSchemaDocumentation doc = new XmlSchemaDocumentation();
            doc.Language = "en";

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





        /// <sUMM2ary>
        /// Returns the type of the CON element of the passed EA.Element
        /// </sUMM2ary>
        /// <param name="e"></param>
        /// <returns></returns>
        private String getCONAttributeTypeFromElement(EA.Element e) {

            foreach (EA.Attribute attribute in e.Attributes) {
                if (attribute.Stereotype.ToString().Equals(CCTS_Types.CON.ToString()))
                    return attribute.Type;
            }
            return "";
        }


        private void getCheckedOption()
        {
            if (this.annotateElementBox.Checked)
                this.annotate = true;
            else
                this.annotate = false;

            if (this.chkUseAlias.Checked)
                this.blnAlias = true;
            else
                this.blnAlias = false;
        }



        /// <sUMM2ary>
        /// Generate the Schema
        /// </sUMM2ary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e) {

            this.resetGenerator(this.scope);

            getCheckedOption();

            //if (this.annotateElementBox.Checked)
            //    this.annotate = true;
            //else
            //    this.annotate = false;

            //if (this.chkUseAlias.Checked)
            //    CC_Utils.blnUseAlias = true;
            //else
            //    CC_Utils.blnUseAlias = false;

            if (DEBUG) {
                path = "C:\\Dokumente und Einstellungen\\pliegl\\Desktop\\xsd-schema";
            }
            else {
                DialogResult dr = this.folderBrowserDialog1.ShowDialog(this);

                if (dr.Equals(DialogResult.Cancel)) {
                    folderBrowserDialog1.Dispose();
                    return;
                } 

                path = this.folderBrowserDialog1.SelectedPath;
            }

            if (this.path == null || this.path.Equals("")) {
                this.appendErrorMessage("Please select a location for the generated schemas first.", this.getPackageName());
            }
            else {

                this.path = this.path + "\\";
                //Get the active Package
                EA.Package p = this.repository.GetPackageByID(Int32.Parse(this.scope));

                this.performProgressStep();

                String error = "";
                System.Collections.ICollection result = null;

                this.appendInfoMessage("Starting CDT schema creation. Please wait.", this.getPackageName());

                try {
                    result = generateSchema(p);
                }
                catch (Exception exc) {

                    error = exc.Message;
                }

                //Kein Fehler aufgetreten - schreibe das Ergebnis
                if (error == "") {
                    foreach (XmlSchema schema in result) {
                        String schemaPath = XMLTools.getSavePathForSchema(this.repository.GetPackageByID(Int32.Parse(this.scope)), repository, this.blnAlias);
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


        /// <sUMM2ary>
        /// Append an error message to the status box
        /// </sUMM2ary>
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
        /// <sUMM2ary>
        /// Show a info message in the status box
        /// </sUMM2ary>
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
        /// <sUMM2ary>
        /// Show a warn message in the status box
        /// </sUMM2ary>
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


        /// <sUMM2ary>
        /// Reset the Text of the StatusTextBox
        /// </sUMM2ary>
        private void resetMessageText() {
            if (this.withGUI)
                this.statusTextBox.Text = "";
        }


        /// <sUMM2ary>
        /// Adds the necessary Namespaces to the Schema
        /// </sUMM2ary>
        /// <param name="schema"></param>
        private void addNameSpaces(XmlSchema schema) {

            string schemaNamespace = "";

            schema.Namespaces.Add("xsd", "http://www.w3.org/2001/XMLSchema");
            schema.Namespaces.Add("ccts", "urn:un:unece:uncefact:documentation:standard:CoreComponentsTechnicalSpecification:2");

            //Catch if the baseURN tagged value is empty
            schemaNamespace = XMLTools.getNameSpace(this.repository, this.repository.GetPackageByID(Int32.Parse(this.scope)));
            if (schemaNamespace == "")
                throw new Exception("Please fill the 'baseURN' tagged value of the package. 'baseURN' tagged value can not be empty.");

            schema.Namespaces.Add(XMLTools.getNameSpacePrefix(this.repository.GetPackageByID(Int32.Parse(this.scope)), this.TargetNameSpacePrefix), schemaNamespace);
            schema.TargetNamespace = schemaNamespace;
        }



        /// <sUMM2ary>
        /// Add the necessary Imports
        /// </sUMM2ary>
        /// <param name="schema"></param>
        private void addImports(XmlSchema schema) {
            //Nothing to do so far
        }






        /// <sUMM2ary>
        /// Returns the name of the current package
        /// </sUMM2ary>
        /// <returns></returns>
        private String getPackageName() {
            return this.repository.GetPackageByID(Int32.Parse(this.scope)).Name.ToString();
        }




        public String TargetNameSpacePrefix {
            get { return targetNameSpacePrefix; }
            set { targetNameSpacePrefix = value; }
        }

        /// <sUMM2ary>
        /// Cancel Button
        /// </sUMM2ary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e) {
            this.Visible = false;
        }


        /// <sUMM2ary>
        /// This methods determins, what should be passed to an auxilliary schema generator
        /// If this class itself has already been called by another class, the calling class is passed
        /// otherwise an instance of this class is passed
        /// </sUMM2ary>
        /// <returns></returns>
        private GeneratorCallBackInterface getCaller() {
            if (this.caller == null)
                return this;
            else
                return caller;
        }

        /// <sUMM2ary>
        /// Is called from extern to append a message to this GUI
        /// </sUMM2ary>
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




    }
}