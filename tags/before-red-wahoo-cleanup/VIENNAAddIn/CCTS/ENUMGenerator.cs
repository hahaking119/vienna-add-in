/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using EA;
using VIENNAAddIn.Utils;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using VIENNAAddIn.Exceptions;
using VIENNAAddIn.CCTS.CCTSBIE_MetaModel;
using VIENNAAddIn.common;
using VIENNAAddIn.constants;



namespace VIENNAAddIn.CCTS
{
    public partial class ENUMGenerator : Form, GeneratorCallBackInterface {

        private static ENUMGenerator form;
        public static void ShowForm(Repository repository)
        {
            var scope = repository.DetermineScope();
            if (form == null || form.IsDisposed)
            {
                form = new ENUMGenerator(repository, scope, false);
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

        private EA.Repository repository = null;
        private String scope = "";
        private bool annotate;
        private bool withGUI;
        private bool blnAlias;
        
        private GeneratorCallBackInterface caller;

        private bool DEBUG;

        //The path where the generated schema(s) will be saved
        private String path = "";
        private String targetNameSpacePrefix = "enum";

        /// <sUMM2ary>
        /// The constructor creates a regular ENUMGenerator with a GUI
        /// </sUMM2ary>
        /// <param name="repository"></param>
        /// <param name="scope"></param>
        /// <param name="annotate"></param>
        public ENUMGenerator(EA.Repository repository, String scope, bool annotate)  {
            //Set the DEBUG mode
            DEBUG = Utility.DEBUG;

            this.repository = repository;
            this.scope = scope;
            this.annotate = annotate;
            
            //With GUI
            this.withGUI = true;
            InitializeComponent();
            setActivePackageLabel();

        }
        /// <summary>
        /// This constructor is for any-level schema generation, with no GUI
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="scope"></param>
        /// <param name="annotate"></param>
        /// <param name="blnAlias"></param>
        /// <param name="path"></param>
        public ENUMGenerator(EA.Repository repository, String scope, bool annotate, bool blnAlias, string path)
        {
            //Set the DEBUG mode
            DEBUG = Utility.DEBUG;

            this.repository = repository;
            this.scope = scope;
            this.annotate = annotate;
            this.blnAlias = blnAlias;
            this.path = path;

            //With GUI
            this.withGUI = false;
            //InitializeComponent();
            //setActivePackageLabel();

        }

        /// <sUMM2ary>
        /// This constructor creates a EnumGenerator with no GUI which is linked from another schema
        /// or 
        /// </sUMM2ary>
        /// <param name="repository"></param>
        /// <param name="scope"></param>
        /// <param name="annotate"></param>
        /// <param name="hidden"></param>
        public ENUMGenerator(EA.Repository repository, String scope, bool annotate, String path, GeneratorCallBackInterface caller) {
            
            //Set the debug mode
            DEBUG = Utility.DEBUG;

            this.caller = caller;
            this.repository = repository;
            this.scope = scope;
            this.annotate = annotate;
            this.withGUI = false;
            //Set the path from the caller because the ENUM Generator now operates in not visible
            //mode and hence no path is determined in this class
            this.path = path;
        }

        public String TargetNameSpacePrefix {
            get { return targetNameSpacePrefix; }
            set { targetNameSpacePrefix = value; }
        }
        
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
                this.selectedEnumLibrary.Text = p.Element.Name + " <<" + p.Element.Stereotype.ToString() + ">>";
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


        private static void ValidationCallbackOne(object sender, ValidationEventArgs args) {
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
            //Iterate through the ENUMs
            foreach (EA.Element e in p.Elements) {
                elementCount++;
                if (e.Stereotype.Equals(CCTS_Types.ENUM.ToString())) {
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
        private XmlSchemaSimpleType getSchemaElement(EA.Element e, XmlSchema schema) {

            XmlSchemaSimpleType simpleType = new XmlSchemaSimpleType();
            simpleType.Name = XMLTools.getXMLName(e.Name) + "Type";

            //Annotate it?
            //if (annotate)
                //complexType.Annotation = this.getCDTAnnotation(e);

            XmlSchemaSimpleTypeRestriction simpleType_restriction = new XmlSchemaSimpleTypeRestriction();
            simpleType_restriction.BaseTypeName = new XmlQualifiedName("token", "http://www.w3.org/2001/XMLSchema");



            //Iterate through the properties of the ENUM
            foreach (EA.Attribute attribute in e.Attributes) {
                XmlSchemaEnumerationFacet enumeration = new XmlSchemaEnumerationFacet();
                enumeration.Value = XMLTools.getXMLName(attribute.Name);
                if (annotate)
                    enumeration.Annotation = this.getEnumerationAnnotation(attribute);
                simpleType_restriction.Facets.Add(enumeration);           
                
  
                
            }
            
            simpleType.Content = simpleType_restriction;            
            return simpleType;
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
        /// Start the schema generation
        /// </sUMM2ary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void generateButton_Click(object sender, EventArgs e) {
            this.resetGenerator(this.scope);

            getCheckedOption();

            //if (this.annotateElementBox.Checked)
            //    this.annotate = true;
            //else
            //    this.annotate = false;

            //////use alias for package or not?
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
                this.appendErrorMessage("Plese select a location for the generated schemas first.", this.getPackageName());
            }
            else {

                this.path = this.path + "\\";
                //Get the active Package
                EA.Package p = this.repository.GetPackageByID(Int32.Parse(this.scope));

                this.performProgressStep();

                String error = "";
                System.Collections.ICollection result = null;

                this.appendInfoMessage("Starting ENUM schema creation. Please wait.", this.getPackageName());

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
        /// Returns the annoation for an attribute
        /// </sUMM2ary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        private XmlSchemaAnnotation getEnumerationAnnotation(EA.Attribute attribute) {
            XmlSchemaAnnotation ann = new XmlSchemaAnnotation();
            XmlSchemaDocumentation doc = new XmlSchemaDocumentation();
            doc.Language = "en";
            XmlDocument xml = new XmlDocument();

            //These arrays hold the names and the values of the annotation
            String[] nodes = { "CodeName", "CodeDescription"};
            String[] values = { attribute.Name, attribute.Default };

            XmlNode[] annNodes = new XmlNode[nodes.Length];
            for (int i = 0; i < nodes.Length; i++) {
                XmlNode node = xml.CreateElement("ccts", nodes[i], "urn:un:unece:uncefact:documentation:standard:CoreComponentsTechnicalSpecification:2");
                if (values[i] != "")
                    node.InnerText = values[i];
                annNodes[i] = node;
            }

            doc.Markup = annNodes;
            ann.Items.Add(doc);

            return ann;
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
                throw new Exception("Please fill the 'baseURN' tagged value of the package. 'baseURN' can not be empty.");

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

        ///// <sUMM2ary>
        ///// Replaces white spaces in the parameter n with _
        ///// </sUMM2ary>
        ///// <param name="n"></param>
        ///// <returns></returns>
        //private String getXMLName(String n) {
        //    if (n != null && n != "")
        //        return n.Replace(' ', '_');
        //    else
        //        return "";
        //}




        private void cancelButton_Click(object sender, EventArgs e) {
            this.Close();
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


        /// <sUMM2ary>
        /// Returns the name of the current package
        /// </sUMM2ary>
        /// <returns></returns>
        private String getPackageName() {
            return this.repository.GetPackageByID(Int32.Parse(this.scope)).Name.ToString();
        }

        
    }
}