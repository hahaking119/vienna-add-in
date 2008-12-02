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
using VIENNAAddIn.constants;
using VIENNAAddIn.Exceptions;
using VIENNAAddIn.common;
using EA;
using VIENNAAddIn.CCTS;


namespace VIENNAAddIn.XBRLGenerator
{
    public partial class CDTGeneratorXBRL : Form, GeneratorCallBackInterface
    {



        private EA.Repository repository;
        private String scope;
        private bool annotate;
        private bool withGUI;
        private GeneratorCallBackInterface caller;

        private bool DEBUG;

        //The path where the generated schema(s) will be saved
        internal String path = "";

        private String targetNameSpacePrefix = "udt";



        /// <summary>
        /// This constructor creates a regular CDTGenerator with a GUI
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="scope"></param>
        /// <param name="annotate"></param>
        public CDTGeneratorXBRL(EA.Repository repository, String scope, bool annotate)
        {

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
        /// This constructor creates a CDTGenerator with no GUI
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="scope"></param>
        /// <param name="annotate"></param>
        /// <param name="hidden"></param>
        public CDTGeneratorXBRL(EA.Repository repository, String scope, bool annotate, String path, GeneratorCallBackInterface caller)
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







        /// <summary>
        /// Setze den Generator zurück
        /// </summary>
        /// <param name="scope"></param>
        public void resetGenerator(String scope)
        {
            this.scope = scope;

            if (this.withGUI)
            {
                this.progressBar1.Value = this.progressBar1.Minimum;
                this.setActivePackageLabel();
                this.statusTextBox.Text = "";
            }
        }




        /// <summary>
        /// Setze den Text der ausgewählten BIELibrary
        /// </summary>
        private void setActivePackageLabel()
        {
            if (this.withGUI)
            {
                EA.Package p = this.repository.GetPackageByID(Int32.Parse(this.scope));
                this.selectedBIELibrary.Text = p.Element.Name + "<<" + p.Element.Stereotype.ToString() + ">>";
            }
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


        public static void ValidationCallbackOne(object sender, ValidationEventArgs args)
        {
            throw new XmlSchemaException(args.Message + args.Exception.StackTrace);
        }

        /// <summary>
        /// Generate a Schema from the CDTs
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public System.Collections.ICollection generateSchema(EA.Package p)
        {

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
            foreach (EA.Element e in p.Elements)
            {
                elementCount++;

                if (e.Stereotype.Equals(CCTS_Types.CDT.ToString()))
                {
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
            try
            {
                xsdSet.Add(schema);
                //xsdSet.Compile();
            }
            catch (XmlSchemaException xse)
            {
                Console.Write(xse.Message.ToString());
                Console.Write(xse.StackTrace.ToString());
                throw xse;
            }


            return xsdSet.Schemas();
        }


        /// <summary>
        /// Returns a Schema Element
        /// </summary>
        /// <param name="e"></param>
        /// <param name="schema"></param>
        /// <returns></returns>
        private XmlSchemaComplexType getSchemaElement(EA.Element e, XmlSchema schema)
        {

            XmlSchemaComplexType complexType = new XmlSchemaComplexType();
            complexType.Name = XMLTools.getXMLName(e.Name) + "Type";

            //Annotate it?
            if (annotate)
                complexType.Annotation = this.getCDTAnnotation(e);

            XmlSchemaSimpleContent simpleContent = new XmlSchemaSimpleContent();
            XmlSchemaSimpleContentRestriction simpleContent_restriction = new XmlSchemaSimpleContentRestriction();
            simpleContent_restriction.BaseTypeName = getBaseTypeName(e);

            //Iterate through the properties of the CDT
            foreach (EA.Attribute attribute in e.Attributes)
            {
                if (attribute.Stereotype.ToString().Equals(CCTS_Types.SUP.ToString()))
                {
                    XmlSchemaAttribute att = new XmlSchemaAttribute();
                    //Annotate the attribute
                    if (annotate)
                        att.Annotation = getAttributeAnnoation(attribute);
                    att.Name = attribute.Name;
                    att.SchemaTypeName = new XmlQualifiedName(getXBRLType(attribute, e), "http://www.xbrl.org/2003/instance");
                    getOptionalOrRequired(attribute, att);
                    simpleContent_restriction.Attributes.Add(att);
                }
            }

            simpleContent.Content = simpleContent_restriction;
            complexType.ContentModel = simpleContent;

            return complexType;
        }


        /// <summary>
        /// Determine wheter the attribute has use=optional or use=required
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="schemaAttribute"></param>
        private void getOptionalOrRequired(EA.Attribute attribute, XmlSchemaAttribute schemaAttribute)
        {

            //Get the lower bound of the attribute
            String lowerBound = attribute.LowerBound;
            schemaAttribute.Use = XmlSchemaUse.Required;
            try
            {
                int i = Int32.Parse(lowerBound);
                if (i == 0)
                    schemaAttribute.Use = XmlSchemaUse.Optional;
            }
            catch (Exception e) { }

        }





        /// <summary>
        /// Returns the base type of this cdt element
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private XmlQualifiedName getBaseTypeName(EA.Element e)
        {

            //Locate the Attribute stereotyped as CON and determine its type
            EA.Attribute a = null;
            bool found = false;
            foreach (EA.Attribute attribute in e.AttributesEx)
            {
                if (attribute.Stereotype != null && attribute.Stereotype.ToString().Equals(CCTS_Types.CON.ToString()))
                {
                    a = attribute;
                    found = true;
                    break;
                }
            }

            if (found)
            {
                return new XmlQualifiedName(getXBRLType(a, e), "http://www.xbrl.org/2003/instance");
            }
            else
            {
                appendWarnMessage("Unable to find CON Attribute for the CDT " + e.Name + ". Taking xbrli:stringItemType as basetype for the CDT instead.", this.getPackageName());
                return new XmlQualifiedName("stringItemType", "http://www.xbrl.org/2003/instance");
            }

        }


        /// <summary>
        /// Returns the XSD-Type for the passed EA Type
        /// </summary>
        /// <returns></returns>
        private String getXBRLType(EA.Attribute a, EA.Element e)
        {
            String determinedElementType = "";
            try
            {
                determinedElementType = XMLTools.getXBRLType(a, e);
            }
            catch (XMLException xe)
            {
                this.appendWarnMessage(xe.Message, this.getPackageName());
                determinedElementType = "stringItemType";
            }
            return determinedElementType;
        }



        /// <summary>
        /// Return the Annotation for a given cdt
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private XmlSchemaAnnotation getCDTAnnotation(EA.Element cdt)
        {
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
            for (int i = 0; i < nodes.Length; i++)
            {
                //If a node is optional (a node is optional if its name starts with a *)
                //we only include it, if a value is specified
                bool include = true;
                if (nodes[i].Substring(0, 1) == "*" && values[i] == "")
                {
                    include = false;
                }
                if (include)
                {
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
        private XmlSchemaAnnotation getAttributeAnnoation(EA.Attribute attribute)
        {
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
            for (int i = 0; i < nodes.Length; i++)
            {
                //If a node is optional (a node is optional if its name starts with a *)
                //we only include it, if a value is specified
                bool include = true;
                if (nodes[i].Substring(0, 1) == "*" && values[i] == "")
                {
                    include = false;
                }
                if (include)
                {
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
        private String getCONAttributeTypeFromElement(EA.Element e)
        {

            foreach (EA.Attribute attribute in e.Attributes)
            {
                if (attribute.Stereotype.ToString().Equals(CCTS_Types.CON.ToString()))
                    return attribute.Type;
            }
            return "";
        }


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
                    this.statusTextBox.Text += "WARN: (Package: " + packageName + ") " + msg + "\n\n";
            }
        }


        /// <summary>
        /// Reset the Text of the StatusTextBox
        /// </summary>
        private void resetMessageText()
        {
            if (this.withGUI)
                this.statusTextBox.Text = "";
        }


        /// <summary>
        /// Adds the necessary Namespaces to the Schema
        /// </summary>
        /// <param name="schema"></param>
        private void addNameSpaces(XmlSchema schema)
        {
            schema.Namespaces.Add("", "http://www.w3.org/2001/XMLSchema");
            schema.Namespaces.Add("xbrll", "http://www.xbrl.org/2003/linkbase");
            schema.Namespaces.Add("xlink", "http://www.w3.org/1999/xlink");
            schema.Namespaces.Add("xbrli", "http://www.xbrl.org/2003/instance");
            schema.Namespaces.Add(XMLTools.getNameSpacePrefix(this.repository.GetPackageByID(Int32.Parse(this.scope)), this.targetNameSpacePrefix), XMLTools.getNameSpace(this.repository, this.repository.GetPackageByID(Int32.Parse(this.scope))));
            schema.TargetNamespace = XMLTools.getNameSpace(this.repository, this.repository.GetPackageByID(Int32.Parse(this.scope)));

            schema.Namespaces.Add("ccts", "urn:un:unece:uncefact:documentation:standard:CoreComponentsTechnicalSpecification:2");
        }



        /// <summary>
        /// Add the necessary Imports
        /// </summary>
        /// <param name="schema"></param>
        private void addImports(XmlSchema schema)
        {
            XmlSchemaImport import = new XmlSchemaImport();
            import.Namespace = "http://www.xbrl.org/2003/instance";
            import.SchemaLocation = "http://www.xbrl.org/2003/xbrl-instance-2003-12-31.xsd";
            schema.Includes.Add(import);
        }






        /// <summary>
        /// Returns the name of the current package
        /// </summary>
        /// <returns></returns>
        private String getPackageName()
        {
            return this.repository.GetPackageByID(Int32.Parse(this.scope)).Name.ToString();
        }




        public String TargetNameSpacePrefix
        {
            get { return targetNameSpacePrefix; }
            set { targetNameSpacePrefix = value; }
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

        




    }
}