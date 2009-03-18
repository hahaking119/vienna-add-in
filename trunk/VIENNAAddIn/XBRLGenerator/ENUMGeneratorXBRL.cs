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
    public partial class ENUMGeneratorXBRL : Form, GeneratorCallBackInterface
    {
        #region Variables
        private Repository repository;
        private GeneratorCallBackInterface caller;
        private bool withGUI;
        internal String scope;
        private const String strComplexType = "ComplexType";
        internal string path = "C:\\";

        private String targetNameSpacePrefix = "enum";
        #endregion

        #region Constructor
        public ENUMGeneratorXBRL(EA.Repository repository, String scope)
        {
            InitializeComponent();
            this.repository = repository;
            this.scope = scope;
            resetGenerator(scope);
        }

        public ENUMGeneratorXBRL(EA.Repository repository, EA.Package package, string path, GeneratorCallBackInterface caller)
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
                caller.appendMessage("error", msg, packageName);
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
                caller.appendMessage("info", msg, packageName);
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
                caller.appendMessage("warn", msg, packageName);
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

        public System.Collections.ICollection generateXBRLschema(EA.Package pkg)
        {
            #region XBRL ENUM
            //XmlSchema schema = new XmlSchema();
            ////schema.ElementFormDefault = XmlSchemaForm.Qualified;
            ////schema.AttributeFormDefault = XmlSchemaForm.Unqualified;
            ////schema.Version = XMLTools.getSchemaVersionFromPackage(pkg);

            //addXBRLNamespace(schema);

            //addXBRLImport(schema);

            //int elementCount = 0;
            ////Iterate through the ENUMs
            //foreach (EA.Element e in pkg.Elements)
            //{
            //    elementCount++;
            //    if (e.Stereotype.Equals(CCTS_Types.ENUM.ToString()))
            //    {
            //        //schema.Items.Add(getSchemaElement(e, schema));
            //        getSchemaElement(e, schema);
            //    }
            //    //After every 20th iteration we perform a progress step
            //    if (elementCount % 20 == 0)
            //        this.performProgressStep();
            //}

            //    ////It was true but, remove it for nested generation
            ////String filename = "";
            ////String schemaPath = "";
            ////String schemaName = "";
            ////schemaPath = XMLTools.getSavePathForSchema(this.repository.GetPackageByID(pkg.PackageID), repository);
            ////schemaName = XMLTools.getSchemaNameWithoutExtention(this.repository.GetPackageByID(pkg.PackageID));
            ////System.IO.Directory.CreateDirectory(path + schemaPath);

            ////this.appendInfoMessage("Saving schema files...", pkg.Name);
            ////filename = path + schemaPath + XMLTools.getSchemaName(this.repository.GetPackageByID(Int32.Parse(this.scope)));
            ////WriteSchemaToFile(filename, schema);
            #endregion

            XmlSchema schema = new XmlSchema();
            schema.ElementFormDefault = XmlSchemaForm.Qualified;
            schema.AttributeFormDefault = XmlSchemaForm.Unqualified;
            schema.Version = XMLTools.getSchemaVersionFromPackage(pkg);

            //Add the necessary namespaces for the CDT schema
            addXBRLNamespace(schema);

            //Add the imports
            addXBRLImport(schema);

            int elementCount = 0;
            //Iterate through the ENUMs
            foreach (EA.Element e in pkg.Elements)
            {
                elementCount++;
                if (e.Stereotype.Equals(CCTS_Types.ENUM.ToString()))
                {
                    schema.Items.Add(getSchemaElement(e, schema));
                }
                //After every 20th iteration we perform a progress step
                if (elementCount % 20 == 0)
                    this.performProgressStep();
            }

            ////Validate the Schema
            XmlSchemaSet xsdSet = new XmlSchemaSet();
            xsdSet.XmlResolver = null; //Since schemaLocation for XBRL is abstract, the schema set's default XmlResolver (XmlUrlResolver) will not know how to get them, so set it to null
            xsdSet.ValidationEventHandler += new ValidationEventHandler(ValidationCallbackOne);
            
            try
            {
                xsdSet.Add(schema);
                //xsdSet.Compile();
            }
            catch (XmlSchemaException xse)
            {
                //WriteSchemaToFile(@"C:\mytemp.xsd", schema);
                Console.Write(xse.Message.ToString());
                Console.Write(xse.StackTrace.ToString());
                throw xse;
            }

            return xsdSet.Schemas();
            //return null;
            
        }

        private void WriteSchemaToFile(string filename, XmlSchema schema)
        {
            Stream outputStream = System.IO.File.Open(filename, FileMode.Create);
            schema.Write(outputStream);
            outputStream.Close();
        }

        private void addXBRLImport(XmlSchema schema)
        {
            XmlSchemaImport import = new XmlSchemaImport();
            import.Namespace = "http://www.xbrl.org/2003/instance";
            import.SchemaLocation = "http://www.xbrl.org/2003/xbrl-instance-2003-12-31.xsd";
            schema.Includes.Add(import);
        }


        /// <summary>
        /// Add Namespace
        /// </summary>
        /// <param name="schema"></param>
        private void addXBRLNamespace(XmlSchema schema)
        {
            ////XBRL ENUM

            schema.Namespaces.Add("", "http://www.w3.org/2001/XMLSchema");
            
            schema.Namespaces.Add("ccts", "urn:un:unece:uncefact:documentation:standard:CoreComponentsTechnicalSpecification:2");

            schema.Namespaces.Add("xbrll", "http://www.xbrl.org/2003/linkbase");
            schema.Namespaces.Add("xlink", "http://www.w3.org/1999/xlink");
            schema.Namespaces.Add("xbrli", "http://www.xbrl.org/2003/instance");
            schema.Namespaces.Add(XMLTools.getNameSpacePrefix(this.repository.GetPackageByID(Int32.Parse(this.scope)), this.targetNameSpacePrefix), XMLTools.getNameSpace(this.repository, this.repository.GetPackageByID(Int32.Parse(this.scope))));
            schema.TargetNamespace = XMLTools.getNameSpace(this.repository, this.repository.GetPackageByID(Int32.Parse(this.scope)));
        }

        /// <summary>
        /// Returns a Schema Element
        /// </summary>
        /// <param name="e"></param>
        /// <param name="schema"></param>
        /// <returns></returns>
        private XmlSchemaComplexType getSchemaElement(EA.Element e, XmlSchema schema)
        {
            XmlSchemaComplexType complextype = new XmlSchemaComplexType();
            complextype.Name = XMLTools.getXMLName(e.Name) + "Type";

            XmlSchemaSimpleContent simplecontent = new XmlSchemaSimpleContent();
            XmlSchemaSimpleContentRestriction restriction = new XmlSchemaSimpleContentRestriction();
            restriction.BaseTypeName = new XmlQualifiedName("tokenItemType", "http://www.xbrl.org/2003/instance");

            foreach (EA.Attribute attr in e.Attributes)
            {
                XmlSchemaEnumerationFacet en = new XmlSchemaEnumerationFacet();
                en.Value = XMLTools.getXMLName(attr.Name);
                restriction.Facets.Add(en);
            }

            simplecontent.Content = restriction;
            complextype.ContentModel = simplecontent;
            //schema.Items.Add(complextype);

            return complextype;
        }

        private static void ValidationCallbackOne(object sender, ValidationEventArgs args)
        {
            throw new XmlSchemaException(args.Message + args.Exception.StackTrace);
        }

        private void btnGenerateSchema_Click(object sender, EventArgs e)
        {
            string error = "";
            System.Collections.ICollection schemaCollection  = null;
            EA.Package pkg = this.repository.GetPackageByID(Int32.Parse(scope));

            resetGenerator(this.scope);

            DialogResult dr = this.dlgSavePath.ShowDialog(this);
            if (dr.Equals(DialogResult.Cancel))
            {
                dlgSavePath.Dispose();
                return;
            }

            this.path = this.dlgSavePath.SelectedPath + "\\";

            try
            {
                schemaCollection = generateXBRLschema(pkg);
            }
            catch (Exception exc)
            {
                error = exc.Message;
            }

            if (error == "")
            {
                foreach (XmlSchema schema in schemaCollection)
                {
                    Stream outputStream = System.IO.File.Open(path + XMLTools.getSchemaName(pkg), FileMode.Create);
                    schema.Write(outputStream);
                    outputStream.Close();
                }
            }
        }

        public void resetGenerator(String scope)
        {
            this.scope = scope;
            this.progressBar1.Value = this.progressBar1.Minimum;
            this.statusTextBox.Text = "";
            this.setActivePackageLabel();
        }

        private void setActivePackageLabel()
        {
            EA.Package p = this.repository.GetPackageByID(Int32.Parse(this.scope));
            this.selectedBIELibrary.Text = p.Element.Name + "<<" + p.Element.Stereotype.ToString() + ">>";
        }

        /// <summary>
        /// Replaces white spaces in the parameter n with _
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        private String getXMLName(String n)
        {
            if (n != null && n != "")
                return n.Replace(' ', '_');
            else
                return "";
        }
    }
}