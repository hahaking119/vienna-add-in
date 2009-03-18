/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using EA;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Collections;
using VIENNAAddIn.Utils;
using VIENNAAddIn.CCTS;
using VIENNAAddIn.constants;

namespace VIENNAAddIn.WSDLGenerator
{
    public partial class WSDLGenerator : Form, GeneratorCallBackInterface
    {
        #region Variable
        private bool withGUI = true;
        private GeneratorCallBackInterface caller = null;
        private EA.Repository repository;
        private string scope = "";
        private bool blnAlias = false;
        

        private int counter = 0;
        private string path = "";
        private EA.Element busTransElement;
        private EA.Element initiator;
        private EA.Element responder;
        private EA.Element requestingBusinessActivity;
        private EA.Element respondingBusinessActivity;
        private ArrayList requestingInformationEnvelope = new ArrayList();
        private ArrayList respondingInformationEnvelope = new ArrayList();
        private string busSignalPath = ""; //saving Business Signal Path
        private string schemaColPath = ""; //saving schema collection Path
        private string wsdlPath = ""; //saving wsdl path;

        private string sourceBusSignal = ""; //saving source of business signal path
        private string sourceSchemaCollection = ""; //saving source of schema collection

        private Hashtable importNamespaceList = new Hashtable();
        private ArrayList alreadyIncluded;
        private bool blnCreateStructure = false;
        private bool blnAnyLevel = false;
        private bool blnBindingService = false;

        //Namespace list
        private string wsdlNamespace = "http://schemas.xmlsoap.org/wsdl/";
        private string schemaNamespace = "http://www.w3.org/2001/XMLSchema";
        private string soapNamespace = "http://schemas.xmlsoap.org/wsdl/soap/";

        //Prefix list
        private string targetNamespacePrefix = "tns";
        private string busSignalNamespace = "bs";
        private string importPrefix = "bm";
        private string wsdlPrefix = "wsdl";
        private string schemaPrefix = "xs";
        private string soapPrefix = "soap";
        private string soapAction = "";

        private bool busSignalFound = false;
        private int busSignalPkgId = 0;
        #endregion

        #region Constructor
        
        public WSDLGenerator(EA.Repository repository, string scope, bool blnAnyLevel)
        {
            InitializeComponent();
            this.repository = repository;
            this.scope = scope;
            this.setActivePackageLabel();

            this.blnAnyLevel = blnAnyLevel;
        }

        //for recursive generation
        public WSDLGenerator(EA.Repository repository, string scope, string path, bool bindingService, bool blnAlias, GeneratorCallBackInterface caller)
        {
            //InitializeComponent();
            this.repository = repository;
            this.scope = scope;
            //this.setActivePackageLabel();
            this.path = path;
            this.blnBindingService = bindingService;

            this.blnAlias = blnAlias;
            this.blnAnyLevel = false;
            this.caller = caller;
        }

        #endregion

        #region Button and User interface

        private void btnGenerateWsdl_Click(object sender, EventArgs e)
        {
            resetGenerator(this.scope);

            getCheckedOption();

            EA.Package pkg = this.repository.GetPackageByID(Int32.Parse(this.scope));

            if (txtSavingPath.Text == "")
            {
                this.appendErrorMessage("Target directory can't be empty. Please select target directory first.", pkg.Name);
                return;
            }
            //if this is the first time WSDL generator running on selected folder
            //check if business signal and schema collection folder is not empty
            if (this.blnCreateStructure)
            {
                //Check if business signal is not empty
                if (txtBusinessSignal.Text.Trim() == "")
                {
                    this.appendErrorMessage("Business Signal location can not be empty, because it's the first time generator running on selected folder.", pkg.Name);
                    return;
                }
                else
                    this.sourceBusSignal = txtBusinessSignal.Text.Trim();

                //check if schema collection location is not empty
                if (txtSchemaColLocation.Text == "")
                {
                    this.appendErrorMessage("Schema Location collection can not be empty, because it's the first time generator running on selected folder.", pkg.Name);
                    return;
                }
                else
                    this.sourceSchemaCollection = txtSchemaColLocation.Text.Trim();
            }



            EA.Package selectedPackage = this.repository.GetPackageByID(Int32.Parse(scope));
            bool error = false;
            this.performProgressStep();



            if (this.blnCreateStructure)
            {
                setPath();

                //create folder structure for generated WSDL
                CreateDirectoryStructure();

                //check whether Business Signal WSDL is in the same folder.
                CheckBusinessSignal();

                //Copy the schema collection from the source;
                CopyDirectory(this.sourceSchemaCollection, this.schemaColPath);
            }
            if (!blnAnyLevel)
            {
                try
                {
                    generateWSDL();
                }
                catch (Exception exception)
                {
                    error = true;
                    this.appendErrorMessage(exception.Message, selectedPackage.Name);
                }
                if (!error)
                    this.appendInfoMessage("Successfully generate WSDL.", selectedPackage.Name);
                this.progressBar1.Value = this.progressBar1.Maximum;


                //create structure only for the first generated WSDL on selected folder
                this.blnCreateStructure = false;
                EnableBrowseButton(false);
            }
            else //Any-level generation
            {
                //Get the active Package
                //EA.Package selectedPackage = this.repository.GetPackageByID(Int32.Parse(this.scope));

                try
                {
                    DoRecursivePackageSearch(selectedPackage);
                }
                catch (Exception exception)
                {
                    error = true;
                    this.appendErrorMessage(exception.Message, selectedPackage.Name);
                }

                if (!error)
                    this.appendInfoMessage("Finished generating WSDL.", selectedPackage.Name);
                this.progressBar1.Value = this.progressBar1.Maximum;


                //create structure only for the first generated WSDL on selected folder
                this.blnCreateStructure = false;
                EnableBrowseButton(false);
            }
        }

        private void btnBrowseSchemaCol_Click(object sender, EventArgs e)
        {
            DialogResult result = dlgSchemaColLocation.ShowDialog();
            if (result == DialogResult.OK)
                txtSchemaColLocation.Text = dlgSchemaColLocation.SelectedPath;
        }

        private void btnSavingPath_Click(object sender, EventArgs e)
        {
            //resetGenerator(this.scope);

            //clear text 
            this.txtBusinessSignal.Text = "";
            this.txtSavingPath.Text = "";
            this.txtSchemaColLocation.Text = "";

            this.blnCreateStructure = false;

            EnableBrowseButton(false);

            DialogResult result = dlgSavePath.ShowDialog();

            if (result == DialogResult.OK)
            {
                this.path = dlgSavePath.SelectedPath + @"\";
                txtSavingPath.Text = dlgSavePath.SelectedPath;

                if (!CheckSchemaAndWSDLFolderExist())
                //{
                //    EnableBrowseButton(false);
                //}
                //else
                {
                    MessageBox.Show("This is the first time WSDL generator running on selected folder.\n " +
                        "You must specify the business signal location and schema collection location.", "Message", MessageBoxButtons.OK);

                    EnableBrowseButton(true);
                    this.blnCreateStructure = true;
                }
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            DialogResult result = dlgOpenBusinessSignal.ShowDialog();

            if (result == DialogResult.OK)
            {
                txtBusinessSignal.Text = dlgOpenBusinessSignal.FileName;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chkBindingService_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBindingService.Checked)
            {
                DialogResult result = MessageBox.Show("Please note the generator will not fill in the endpoint references in the WSDL." +
                    "\nPlease remember to fill in the endpoint references (marked with '[myEndPointReference]') \nbefore using the WSDL for implementation." +
                    "\nDo you want to continue?",
                    "Warning", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    this.blnBindingService = true;
                }
                else
                    chkBindingService.Checked = false;
            }
            else
                this.blnBindingService = false;
        }
        #endregion

        #region Single WSDL generation
        public void resetGenerator(String scope)
        {
            this.scope = scope;
            this.progressBar1.Value = this.progressBar1.Minimum;
            this.statusTextBox.Text = "";
            this.setActivePackageLabel();
            
            this.requestingInformationEnvelope = new ArrayList();
            this.respondingInformationEnvelope = new ArrayList();
            this.importNamespaceList = new Hashtable();
        }

        public void resetBlnAnyLevel(bool blnAnyLevel)
        {
            this.blnAnyLevel = blnAnyLevel;
            this.chkBindingService.Checked = false;

            this.txtBusinessSignal.Text = "";
            this.txtSavingPath.Text = "";
            this.txtSchemaColLocation.Text = "";
        }

        private void setActivePackageLabel()
        {
            EA.Package pkg = this.repository.GetPackageByID(Int32.Parse(this.scope));
            this.selectedBusinessTransaction.Text = pkg.Name + "<<" + pkg.Element.Stereotype.ToString() + ">>";
        }


        /// <summary>
        /// Setting the path of schema collection, wsdl, and business signal to the new saving path
        /// </summary>
        private void setPath()
        {
            this.schemaColPath = this.path + "Schemas" + @"\";
            this.wsdlPath = this.path + "WSDL" + @"\";
            this.busSignalPath = this.path + @"WSDL\GIEMBusinessSignal.wsdl";
        }

        private void CreateDirectoryStructure()
        {
            //if (this.blnCreateStructure)
            //{
                Directory.CreateDirectory(this.schemaColPath);
                Directory.CreateDirectory(this.wsdlPath);
            //}
        }

        private void EnableBrowseButton(bool blnEnabled)
        {
            this.btnBrowse.Enabled = blnEnabled;
            this.btnBrowseSchemaCol.Enabled = blnEnabled;
        }

        private bool CheckSchemaAndWSDLFolderExist()
        {
            if (Directory.Exists(this.path + "schemas"))
                if (Directory.Exists(this.path + "wsdl"))
                    return true;
                else
                    return false;
            return false;
        }

        /// <summary>
        /// Main WSDL generator
        /// </summary>
        private void generateWSDL()
        {
            //set saving path, schema collection path, business Signal path
            setPath();

            EA.Package pkg = this.repository.GetPackageByID(Int32.Parse(this.scope));
            
            //get element with stereotype <<BusinessTransaction>>
            foreach (EA.Element e in pkg.Elements)
            {
                if (e.Stereotype.Equals("BusinessTransaction",StringComparison.OrdinalIgnoreCase))
                {
                    this.busTransElement = e;
                    break;
                }
            }

            if (this.busTransElement == null)
            {
                throw new Exception("No element with stereotype <<BusinessTransaction>>. Therefore, there is no WSDL schema to generate.");
            }

            
            String schemaName = "";
            schemaName = XMLTools.getSchemaNameWithoutExtention(this.repository.GetPackageByID(pkg.PackageID));
            String filename = this.wsdlPath + schemaName + ".wsdl";
            XmlTextWriter w = new XmlTextWriter(filename, Encoding.UTF8);

            try
            {
                getInitiatorResponderElement();

                generateInitialSchema(w);

                addBusinessSignalImports(w);

                addBusinessMessageType(w);

                generateBusinessMessage(w);

                generatePortType(w);

                if (this.blnBindingService)
                    generateBindingService(w);

            }
            catch (Exception ex)
            {
                w.Close();
                throw new Exception(ex.Message);
            }
            
            w.WriteEndElement();
            w.Flush();
            w.Close();
        }

        private void getCheckedOption()
        {
            if (this.chkUseAlias.Checked)
                this.blnAlias = true;
            else
                this.blnAlias = false;
        }

        private void generateBindingService(XmlTextWriter w)
        {
            string bindingName = "";
            #region ~~Responder Binding~~
            bindingName = this.responder.Name + removeSpace(this.respondingBusinessActivity.Name); //this.responder.Name + removeSpace(this.repository.GetPackageByID(Int32.Parse(this.scope)).Name);

            w.WriteStartElement(this.wsdlPrefix, "binding", this.wsdlNamespace);
            w.WriteAttributeString("name", bindingName + "SOAP");
            w.WriteAttributeString("type", this.targetNamespacePrefix + ":" + bindingName);

            w.WriteStartElement(this.soapPrefix, "binding", this.soapNamespace);
            w.WriteAttributeString("style", "document");
            w.WriteAttributeString("transport", "http://schemas.xmlsoap.org/soap/http");
            w.WriteAttributeString("xmlns",this.soapPrefix, null,this.soapNamespace);
            w.WriteEndElement();

            #region Requesting Business Activity
            foreach (EA.Element el in this.requestingInformationEnvelope)
            {
                w.WriteStartElement(this.wsdlPrefix, "operation", this.wsdlNamespace);
                w.WriteAttributeString("name", el.Name);

                w.WriteStartElement(this.soapPrefix, "operation", this.soapNamespace);
                w.WriteAttributeString("soapAction", this.soapAction);
                w.WriteAttributeString("style", "document");
                w.WriteAttributeString("xmlns", this.soapPrefix, null, this.soapNamespace);
                w.WriteEndElement();

                w.WriteStartElement(this.wsdlPrefix,"input", this.wsdlNamespace);
                w.WriteStartElement(this.soapPrefix, "body", this.soapNamespace);
                w.WriteAttributeString("use", "literal");
                w.WriteAttributeString("xmlns", this.soapPrefix, null, this.soapNamespace);
                w.WriteEndElement();
                w.WriteEndElement();

                w.WriteEndElement();
            }
            #endregion

            string tv = XMLTools.getElementTVValue("businessTransactionType", this.busTransElement);

            if (!(tv.ToLower() == "notification") && !(tv.ToLower() == "informationdistribution"))
            {

                #region ReceiptAcknowledgement
                w.WriteStartElement(this.wsdlPrefix, "operation", this.wsdlNamespace);
                w.WriteAttributeString("name", "ReceiptAcknowledgement");

                w.WriteStartElement(this.soapPrefix, "operation", this.soapNamespace);
                w.WriteAttributeString("soapAction", this.soapAction);
                w.WriteAttributeString("style", "document");
                w.WriteAttributeString("xmlns", this.soapPrefix, null, this.soapNamespace);
                w.WriteEndElement();

                w.WriteStartElement(this.wsdlPrefix, "input", this.wsdlNamespace);
                w.WriteStartElement(this.soapPrefix, "body", this.soapNamespace);
                w.WriteAttributeString("use", "literal");
                w.WriteAttributeString("xmlns", this.soapPrefix, null, this.soapNamespace);
                w.WriteEndElement();
                w.WriteEndElement();

                w.WriteEndElement();
                #endregion

                #region ReceiptException
                w.WriteStartElement(this.wsdlPrefix, "operation", this.wsdlNamespace);
                w.WriteAttributeString("name", "ReceiptException");

                w.WriteStartElement(this.soapPrefix, "operation", this.soapNamespace);
                w.WriteAttributeString("soapAction", this.soapAction);
                w.WriteAttributeString("style", "document");
                w.WriteAttributeString("xmlns", this.soapPrefix, null, this.soapNamespace);
                w.WriteEndElement();

                w.WriteStartElement(this.wsdlPrefix, "input", this.wsdlNamespace);
                w.WriteStartElement(this.soapPrefix, "body", this.soapNamespace);
                w.WriteAttributeString("use", "literal");
                w.WriteAttributeString("xmlns", this.soapPrefix, null, this.soapNamespace);
                w.WriteEndElement();
                w.WriteEndElement();

                w.WriteEndElement();
                #endregion
            }

            #region GeneralException
            w.WriteStartElement(this.wsdlPrefix, "operation", this.wsdlNamespace);
            w.WriteAttributeString("name", "GeneralException");

            w.WriteStartElement(this.soapPrefix, "operation", this.soapNamespace);
            w.WriteAttributeString("soapAction", this.soapAction);
            w.WriteAttributeString("style", "document");
            w.WriteAttributeString("xmlns", this.soapPrefix, null, this.soapNamespace);
            w.WriteEndElement();

            w.WriteStartElement(this.wsdlPrefix, "input", this.wsdlNamespace);
            w.WriteStartElement(this.soapPrefix, "body", this.soapNamespace);
            w.WriteAttributeString("use", "literal");
            w.WriteAttributeString("xmlns", this.soapPrefix, null, this.soapNamespace);
            w.WriteEndElement();
            w.WriteEndElement();

            w.WriteEndElement();
            #endregion

            w.WriteEndElement();
            #endregion

            #region ~~Initiator Binding~~
            bindingName = this.initiator.Name + removeSpace(this.requestingBusinessActivity.Name); //this.initiator.Name + removeSpace(this.repository.GetPackageByID(Int32.Parse(this.scope)).Name);

            w.WriteStartElement(this.wsdlPrefix, "binding", this.wsdlNamespace);
            w.WriteAttributeString("name", this.initiator.Name + removeSpace(this.requestingBusinessActivity.Name) + "SOAP");
            w.WriteAttributeString("type", this.targetNamespacePrefix + ":" + bindingName);

            w.WriteStartElement(this.soapPrefix, "binding", this.soapNamespace);
            w.WriteAttributeString("style", "document");
            w.WriteAttributeString("transport", "http://schemas.xmlsoap.org/soap/http");
            w.WriteAttributeString("xmlns", this.soapPrefix, null, this.soapNamespace);
            w.WriteEndElement();

            #region Requesting Business Activity
            foreach (EA.Element el in this.respondingInformationEnvelope)
            {
                w.WriteStartElement(this.wsdlPrefix, "operation", this.wsdlNamespace);
                w.WriteAttributeString("name", el.Name);

                w.WriteStartElement(this.soapPrefix, "operation", this.soapNamespace);
                w.WriteAttributeString("soapAction", this.soapAction);
                w.WriteAttributeString("style", "document");
                w.WriteAttributeString("xmlns", this.soapPrefix, null, this.soapNamespace);
                w.WriteEndElement();

                w.WriteStartElement(this.wsdlPrefix, "input", this.wsdlNamespace);
                w.WriteStartElement(this.soapPrefix, "body", this.soapNamespace);
                w.WriteAttributeString("use", "literal");
                w.WriteAttributeString("xmlns", this.soapPrefix, null, this.soapNamespace);
                w.WriteEndElement();
                w.WriteEndElement();

                w.WriteEndElement();
            }
            #endregion

            #region ReceiptAcknowledgement
            w.WriteStartElement(this.wsdlPrefix, "operation", this.wsdlNamespace);
            w.WriteAttributeString("name", "ReceiptAcknowledgement");

            w.WriteStartElement(this.soapPrefix, "operation", this.soapNamespace);
            w.WriteAttributeString("soapAction", this.soapAction);
            w.WriteAttributeString("style", "document");
            w.WriteAttributeString("xmlns", this.soapPrefix, null, this.soapNamespace);
            w.WriteEndElement();

            w.WriteStartElement(this.wsdlPrefix, "input", this.wsdlNamespace);
            w.WriteStartElement(this.soapPrefix, "body", this.soapNamespace);
            w.WriteAttributeString("use", "literal");
            w.WriteAttributeString("xmlns", this.soapPrefix, null, this.soapNamespace);
            w.WriteEndElement();
            w.WriteEndElement();

            w.WriteEndElement();
            #endregion

            #region ReceiptException
            w.WriteStartElement(this.wsdlPrefix, "operation", this.wsdlNamespace);
            w.WriteAttributeString("name", "ReceiptException");

            w.WriteStartElement(this.soapPrefix, "operation", this.soapNamespace);
            w.WriteAttributeString("soapAction", this.soapAction);
            w.WriteAttributeString("style", "document");
            w.WriteAttributeString("xmlns", this.soapPrefix, null, this.soapNamespace);
            w.WriteEndElement();

            w.WriteStartElement(this.wsdlPrefix, "input", this.wsdlNamespace);
            w.WriteStartElement(this.soapPrefix, "body", this.soapNamespace);
            w.WriteAttributeString("use", "literal");
            w.WriteAttributeString("xmlns", this.soapPrefix, null, this.soapNamespace);
            w.WriteEndElement();
            w.WriteEndElement();

            w.WriteEndElement();
            #endregion

            #region GeneralException
            w.WriteStartElement(this.wsdlPrefix, "operation", this.wsdlNamespace);
            w.WriteAttributeString("name", "GeneralException");

            w.WriteStartElement(this.soapPrefix, "operation", this.soapNamespace);
            w.WriteAttributeString("soapAction", this.soapAction);
            w.WriteAttributeString("style", "document");
            w.WriteAttributeString("xmlns", this.soapPrefix, null, this.soapNamespace);
            w.WriteEndElement();

            w.WriteStartElement(this.wsdlPrefix, "input", this.wsdlNamespace);
            w.WriteStartElement(this.soapPrefix, "body", this.soapNamespace);
            w.WriteAttributeString("use", "literal");
            w.WriteAttributeString("xmlns", this.soapPrefix, null, this.soapNamespace);
            w.WriteEndElement();
            w.WriteEndElement();

            w.WriteEndElement();
            #endregion

            w.WriteEndElement();
            #endregion

            string serviceName = "";
            #region Responder Service
            serviceName = this.responder.Name + removeSpace(this.respondingBusinessActivity.Name);

            w.WriteStartElement(this.wsdlPrefix, "service", this.wsdlNamespace);
            w.WriteAttributeString("name", serviceName);

            w.WriteStartElement(this.wsdlPrefix, "port", this.wsdlNamespace);
            w.WriteAttributeString("name", serviceName + "SOAP");
            w.WriteAttributeString("binding", this.targetNamespacePrefix + ":" + serviceName + "SOAP");

            w.WriteStartElement(this.soapPrefix, "address", this.soapNamespace);
            w.WriteAttributeString("location", "[myEndPointReference]" + @"/" + serviceName);
            w.WriteAttributeString("xmlns", this.soapPrefix, null, this.soapNamespace);
            w.WriteEndElement();
            w.WriteEndElement();

            w.WriteEndElement();
            #endregion

            #region Initiator Service
            serviceName = this.initiator.Name + removeSpace(this.requestingBusinessActivity.Name);

            w.WriteStartElement(this.wsdlPrefix, "service", this.wsdlNamespace);
            w.WriteAttributeString("name", serviceName);

            w.WriteStartElement(this.wsdlPrefix, "port", this.wsdlNamespace);
            w.WriteAttributeString("name", serviceName + "SOAP");
            w.WriteAttributeString("binding", this.targetNamespacePrefix + ":" + serviceName + "SOAP");

            w.WriteStartElement(this.soapPrefix, "address", this.soapNamespace);
            w.WriteAttributeString("location", "[myEndPointReference]" + @"/" + serviceName);
            w.WriteAttributeString("xmlns", this.soapPrefix, null, this.soapNamespace);
            w.WriteEndElement();
            w.WriteEndElement();

            w.WriteEndElement();
            #endregion
        }


        /// <summary>
        /// Copy a whole directory including subdirectory
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        private static void CopyDirectory(string source, string destination)
        {
            String[] files;

            if (destination[destination.Length - 1] != Path.DirectorySeparatorChar)
                destination += Path.DirectorySeparatorChar;
            if (!Directory.Exists(destination))
                Directory.CreateDirectory(destination);

            files = Directory.GetFileSystemEntries(source);
            foreach (string Element in files)
            {
                // Sub directories
                if (Directory.Exists(Element))
                    CopyDirectory(Element, destination + Path.GetFileName(Element));
                // Files in directory
                else
                    System.IO.File.Copy(Element, destination + Path.GetFileName(Element), true);
            }
        }

        /// <summary>
        /// Method to check whether the WSDL Business Signal is in the same folder with the saving path.
        /// If no, copy the WSDL Business Signal to saving path
        /// </summary>
        private void CheckBusinessSignal()
        {
            System.IO.File.Copy(this.sourceBusSignal, this.wsdlPath + "GIEMBusinessSignal.wsdl", true);

            try
            {
                //finding the location of business signal in the model
                findBusinessSignal();
                //resetting the relative path of imported schema in GIEMBusinessSignal.wsdl
                resetBusSignalRelativePath();
            }
            catch (Exception excp)
            {
                this.appendErrorMessage(excp.Message, "");
            }
        }

        private void findBusinessSignal()
        {
            this.busSignalFound = false;

            for (short a = 0; a < this.repository.Models.Count; a++ )
            {
                if (this.busSignalFound)
                    break;

                EA.Package pkg = (EA.Package)this.repository.Models.GetAt(a);
                
                searchRecursiveInsidePackage(pkg);
            }
        }

        private void searchRecursiveInsidePackage(EA.Package pkg)
        {
            for (short idx = 0; idx < pkg.Packages.Count; idx++)
            {
                if (this.busSignalFound)
                    break;

                EA.Package thePackage = (EA.Package)pkg.Packages.GetAt(idx);
                
                if (thePackage.Name == "Signals.library")
                {
                    this.busSignalFound = true;
                    this.busSignalPkgId = thePackage.PackageID;
                    break;
                }

                if (pkg.Packages.Count > 0)
                    searchRecursiveInsidePackage(thePackage);
            }
        }



        private bool isIncludeBusinessMessage(EA.Element element)
        {
            EA.Package pBusTrans = this.repository.GetPackageByID(Int32.Parse(this.scope));

            string baseURNBusMsg, baseURNBusTrans;

            //get business Transaction "baseURN" tagged value
            baseURNBusTrans = XMLTools.getElementTVValue(CCTS_TV.baseURN, pBusTrans.Element);
            if (baseURNBusTrans == "")
                throw new Exception("'baseURN' tagged value in package " + pBusTrans.Name + " is not found or empty. Please fill add it or fill it.");

            EA.Element infElementClassifier;
            try
            {
                infElementClassifier = this.repository.GetElementByID(element.ClassifierID);
            }
            catch
            {
                throw new Exception("Failed getting classifier of element " + element.Name);
            }

            EA.Package pBusMsg = this.repository.GetPackageByID(infElementClassifier.PackageID); //get DOCLibrary

            //get bussiness Message "baseuURN" tagged value
            baseURNBusMsg = XMLTools.getElementTVValue(CCTS_TV.baseURN, pBusMsg.Element);
            if (baseURNBusMsg == "")
                throw new Exception("'baseURN' tagged value in package " + pBusMsg.Name + " is not found or empty. Please fill or add it.");


            if (baseURNBusMsg == baseURNBusTrans)
                return true; //include
            else
                return false; //import

        }

        private void addBusinessMessageType(XmlTextWriter w)
        {
            w.WriteStartElement("types", this.wsdlNamespace);
            
            w.WriteStartElement ("schema",this.schemaNamespace);
            w.WriteAttributeString("targetNamespace", getNamespace());
            w.WriteAttributeString("elementFormDefault", "qualified");

            ArrayList informationEnvelope = new ArrayList(this.requestingInformationEnvelope);
            informationEnvelope.AddRange(this.respondingInformationEnvelope);

            alreadyIncluded = new ArrayList();

            foreach (EA.Element element in informationEnvelope)
            {
                EA.Element infElementClassifier;

                try
                {
                    infElementClassifier = this.repository.GetElementByID(element.ClassifierID);
                }
                catch
                {
                    throw new Exception("Failed getting classifier of element " + element.Name);
                }

                EA.Package pBusMsg = this.repository.GetPackageByID(infElementClassifier.PackageID); //get DOCLibrary
                

                if (!isAlreadyExist(element))
                {
                    if (isIncludeBusinessMessage(element))
                    {
                        w.WriteStartElement("include", this.schemaNamespace);
                        w.WriteAttributeString("schemaLocation", getSchemaLocation(pBusMsg));
                        w.WriteEndElement();

                        //generateLinkedSchema(pBusMsg);
                    }
                    else
                    {
                        //not sure on this
                        w.WriteStartElement("import", this.schemaNamespace);
                        w.WriteAttributeString("namespace", XMLTools.getNameSpace(this.repository, pBusMsg));
                        w.WriteAttributeString("schemaLocation", getSchemaLocation(pBusMsg));
                        w.WriteEndElement();
                    }
                }

            }

            w.WriteEndElement(); //for schema
            w.WriteEndElement(); //for types
        }

        private void generateLinkedSchema(EA.Package pBusMsg)
        {
            System.Collections.ICollection result = null;

            if (pBusMsg.Element.Stereotype == CCTS_PackageType.DOCLibrary.ToString())
            {
                result = new DOCGenerator(this.repository, pBusMsg.PackageID.ToString(), false).generateSchema(pBusMsg);
            }

            String schemaPath = "";
            String schemaName = "";
            schemaPath = XMLTools.getSavePathForSchema(this.repository.GetPackageByID(pBusMsg.PackageID), repository, this.blnAlias);
            schemaName = XMLTools.getSchemaName(this.repository.GetPackageByID(pBusMsg.PackageID));

            //Write the schema(s)
            foreach (XmlSchema schema1 in result)
            {
                String filename = path + schemaPath + schemaName;
                //Create the path
                System.IO.Directory.CreateDirectory(path + schemaPath);
                Stream outputStream = System.IO.File.Open(filename, FileMode.Create);
                schema1.Write(outputStream);
                outputStream.Close();
            }
        }


        private bool isAlreadyExist(Element element)
        {
            EA.Element infElementClassifier = this.repository.GetElementByID(element.ClassifierID);
            EA.Package pBusMsg = this.repository.GetPackageByID(infElementClassifier.PackageID); //get DOCLibrary

            if (this.alreadyIncluded.Contains(pBusMsg.Name)) //baseURN))
                return true;
            else
                this.alreadyIncluded.Add(pBusMsg.Name);

            return false;
        }

        
        
        private void getInitiatorResponderElement()
        {
            bool swimlaneRequestExist = false;
            bool swimlaneRespondExist = false;

            foreach (EA.Element e in this.busTransElement.Elements)
            {
                if (e.Stereotype.Equals("BusinessTransactionSwimlane"))
                {
                    foreach (EA.Element busActivity in e.Elements)
                    {
                        if (busActivity.Stereotype.Equals("RequestingBusinessActivity"))
                        {
                            swimlaneRequestExist = true;

                            try
                            {
                                this.initiator = this.repository.GetElementByID(e.ClassifierID);
                            }
                            catch 
                            {
                                string erroMsg = "Failed get the classifier of <<BusinessTransactionSwimlane>> that contains a <<RequestingBusinessActivity>> on package " + 
                                    this.repository.GetPackageByID(Int32.Parse(this.scope)).Name +
                                    ". \nIt might be caused by losing reference to classifier or you miss setting it.";
                                throw new Exception(erroMsg);
                            }

                            this.requestingBusinessActivity = busActivity;
                            
                            foreach (EA.Element busEnvelope in busActivity.Elements)
                            {
                                if (busEnvelope.Stereotype.Equals("RequestingInformationEnvelope"))
                                {
                                    requestingInformationEnvelope.Add(busEnvelope);
                                }
                            }
                        }
                        else if (busActivity.Stereotype.Equals("RespondingBusinessActivity"))
                        {
                            swimlaneRespondExist = true;
                            try
                            {
                                responder = this.repository.GetElementByID(e.ClassifierID);
                            }
                            catch 
                            {
                                string errorMsg = "Failed to get the classifier of <<BusinessTransactionSwimlane>> that contains a <<RespondingBusinessActivity>> on package " +
                                    this.repository.GetPackageByID(Int32.Parse(this.scope)).Name +
                                    ". \nIt might be caused by losing reference to classifier or you miss setting it.";
                                throw new Exception(errorMsg);
                            }

                            this.respondingBusinessActivity = busActivity;
                            
                            foreach (EA.Element busEnvelope in busActivity.Elements)
                            {
                                if (busEnvelope.Stereotype.Equals("RespondingInformationEnvelope"))
                                {
                                    respondingInformationEnvelope.Add(busEnvelope);
                                }
                            }
                        }
                    }
                }

                if ((swimlaneRequestExist) && (swimlaneRespondExist))
                    break;
            }

            if (!(swimlaneRequestExist) && !(swimlaneRespondExist))
            {
                string errorMsg = "No element with stereotype <<BusinessTransactionSwimlane>> in package " +
                    this.repository.GetPackageByID(Int32.Parse(this.scope)).Name +
                    ".\n Can't continue to generate WSDL.";
                throw new Exception(errorMsg);
            }
            else if (!(swimlaneRequestExist))
            {
                string errorMsg = "<<BusinessTransactionSwimlane>> that contains <<RequestingBusinessActivity>> doesn't exist." +
                    ". \nCan't continue to generate WSDL.";
                throw new Exception(errorMsg);
            }
            else if (!(swimlaneRespondExist))
            {
                //notification and InformationDistribution don't have RespondingBusinessActivity, they are one-way transaction
                if (!XMLTools.getElementTVValue("businessTransactionType", this.busTransElement).Equals("Notification", StringComparison.OrdinalIgnoreCase) ||
                    !XMLTools.getElementTVValue("businessTransactionType", this.busTransElement).Equals("InformationDistribution", StringComparison.OrdinalIgnoreCase))
                {
                    string errorMsg = "<<BusinessTransactionSwimlane>> that contains <<RespondingBusinessActivity>> doesn't exist." +
                    ". \nCan't continue to generate WSDL.";
                    throw new Exception(errorMsg);
                }
                
            }
        }

        private void addBusinessSignalImports(XmlTextWriter w)
        {
            w.WriteStartElement("import",this.wsdlNamespace);
            
            w.WriteAttributeString("namespace", getNamespaceBusinessSignal());

            //Business Signal Location is now in the same directory with the saving path of Business transaction WSDL - March 21,2008
            w.WriteAttributeString("location", Path.GetFileName(this.busSignalPath)); //getRelativePath(this.schemaPath, this.busSignalPath));

            w.WriteEndElement();
        }


        private string getNamespaceBusinessSignal()
        {
            XmlTextReader reader = new XmlTextReader(this.busSignalPath);
            try
            {
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element: // The node is an element.
                            if (reader.Name.Equals("wsdl:definitions"))
                            {
                                reader.MoveToAttribute("targetNamespace");
                                string val = reader.Value;
                                //reader.Close();
                                return val;
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed getting namespace from file " + this.busSignalPath + ". Error message: " + ex.Message);
            }
            finally
            {
                reader.Close();
            }
            return "";
        }


        private void generateInitialSchema(XmlTextWriter w)
        {
            w.Namespaces = true;
            w.Formatting = Formatting.Indented;
            w.WriteStartDocument();

            w.WriteStartElement(this.wsdlPrefix, "definitions", this.wsdlNamespace);
            w.WriteAttributeString("xmlns", this.soapPrefix, null, this.soapNamespace);
            w.WriteAttributeString("xmlns", "http", null, "http://schemas.xmlsoap.org/wsdl/http/");
            w.WriteAttributeString("xmlns", this.schemaPrefix, null, this.schemaNamespace);
            w.WriteAttributeString("xmlns", "soapenc", null, "http://schemas.xmlsoap.org/soap/encoding/");
            w.WriteAttributeString("xmlns", "mime", null, "http://schemas.xmlsoap.org/wsdl/mime/");
            w.WriteAttributeString("xmlns", this.targetNamespacePrefix, null, getNamespace());
            w.WriteAttributeString("xmlns", this.busSignalNamespace, null, getNamespaceBusinessSignal());
            //TO DO :
            //add namespace for importing business message which is not in the same namespace with WSDL
            addImportBusinessMessageNamespace(w);
            w.WriteAttributeString("name", removeSpace(this.busTransElement.Name));
            w.WriteAttributeString("", "targetNamespace", null, getNamespace());
        }

        private string getNamespace()
        {
            return XMLTools.getNameSpace(this.repository, this.repository.GetPackageByID(this.busTransElement.PackageID));
        }

        private void addImportBusinessMessageNamespace(XmlTextWriter w)
        {
            ArrayList joinArr = new ArrayList(this.requestingInformationEnvelope);
            joinArr.AddRange(this.respondingInformationEnvelope);

            foreach (EA.Element infElement in joinArr)
            {
                if (!isIncludeBusinessMessage(infElement)) 
                {
                    EA.Element infElementClassifier;
                    try
                    {
                        infElementClassifier = this.repository.GetElementByID(infElement.ClassifierID);
                    }
                    catch
                    {
                        string errorMsg = "Failed getting classifier of element " + infElement.Name;
                        throw new Exception(errorMsg);
                    }

                    EA.Package pBusMsg = this.repository.GetPackageByID(infElementClassifier.PackageID); //get DOCLibrary
                    
                    if (!importNamespaceList.Contains(XMLTools.getNameSpace(this.repository, pBusMsg)))
                    {
                        w.WriteAttributeString("xmlns", this.importPrefix + ++counter, null, XMLTools.getNameSpace(this.repository, pBusMsg));

                        //add imported schema and its prefix to a hashtable for further reference.
                        importNamespaceList.Add(XMLTools.getNameSpace(this.repository, pBusMsg), this.importPrefix + counter);
                    }
                }
            }
        }
       
        
        
        private void generateBusinessMessage(XmlTextWriter w)
        {

            foreach (EA.Element reqElement in this.requestingInformationEnvelope)
            {
                EA.Element reqElementClassifier;
                try
                {
                    reqElementClassifier = this.repository.GetElementByID(reqElement.ClassifierID);
                }
                catch
                {
                    throw new Exception("Failed getting classifier of element " + reqElement.Name);
                }

                string abstractName = "";
                abstractName = "RequestMsg";

                w.WriteStartElement("message", wsdlNamespace);
                w.WriteAttributeString("name", abstractName); 

                w.WriteStartElement("part", wsdlNamespace);
                w.WriteAttributeString("name", removeSpace(reqElementClassifier.Name));

                w.WriteAttributeString("element", getQualifiedName(reqElementClassifier)); 
                w.WriteEndElement();

                w.WriteEndElement();
            }

            foreach (EA.Element resElement in this.respondingInformationEnvelope)
            {
                EA.Element resElementClassifier;
                try
                {
                    resElementClassifier = this.repository.GetElementByID(resElement.ClassifierID);
                }
                catch
                {
                    throw new Exception("Failed getting classifier of element " + resElement.Name);
                }

                string abstractName = "PositiveResponseMsg"; //give default value to RespondingInformationEnvelope

                if (XMLTools.getElementTVValue("ispositive",resElement).Equals("false",StringComparison.OrdinalIgnoreCase))
                {
                    abstractName = "NegativeResponseMsg";
                }

                w.WriteStartElement("message", wsdlNamespace);
                w.WriteAttributeString("name", abstractName); 

                w.WriteStartElement("part", wsdlNamespace);
                w.WriteAttributeString("name", removeSpace(resElementClassifier.Name));

                w.WriteAttributeString("element", getQualifiedName(resElementClassifier));
                w.WriteEndElement();

                w.WriteEndElement();
            }

        }

        /// <summary>
        /// Get Qualified Name of the element based on the type
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private string getQualifiedName(EA.Element classifierElement)//, string type)
        {
            string ns = "";

            EA.Package pBusMsg = this.repository.GetPackageByID(classifierElement.PackageID); //get DOCLibrary

            string elementNamespace = XMLTools.getNameSpace(this.repository, pBusMsg);

            if (!importNamespaceList.ContainsKey(elementNamespace)) // in the same namespace
            {
                ns = this.targetNamespacePrefix + ":" + classifierElement.Name;
            }
            else //different namespace
            {
                ns = (string)(importNamespaceList[elementNamespace]) + ":" + classifierElement.Name;
            }

            return ns;
        }


        private void generatePortType(XmlTextWriter w)
        {
            //Initiator
            w.WriteStartElement("portType", this.wsdlNamespace);
            w.WriteAttributeString("name", getInitiatorPortTypeName());
            generateInitiatorOperation(w);
            w.WriteEndElement();

            //Responder
            w.WriteStartElement("portType", this.wsdlNamespace);
            w.WriteAttributeString("name", getResponderPortTypeName());
            generateResponderOperation(w);
            w.WriteEndElement();
        }

        private void generateResponderOperation(XmlTextWriter w)
        {
            foreach (EA.Element requesting in this.requestingInformationEnvelope)
            {
                string abstractName = "RequestMsg";

                w.WriteStartElement("operation", this.wsdlNamespace);
                w.WriteAttributeString("name", removeSpace(requesting.Name));

                w.WriteStartElement("input", this.wsdlNamespace);
                w.WriteAttributeString("message", this.targetNamespacePrefix + ":" + abstractName);//getQualifiedName(classifierElement) + "Msg"); //,"MessageInput"));
                w.WriteEndElement();

                w.WriteEndElement();
            }

            string tv = XMLTools.getElementTVValue("businessTransactionType", this.busTransElement);

            if (!(tv.ToLower() == "notification") && !(tv.ToLower() == "informationdistribution"))
            {
                //Receipt Acknowledgement
                w.WriteStartElement("operation", this.wsdlNamespace);
                w.WriteAttributeString("name", "ReceiptAcknowledgement");

                w.WriteStartElement("input", this.wsdlNamespace);

                w.WriteAttributeString("message", this.busSignalNamespace + ":" + "ReceiptAcknowledgementSignal");
                w.WriteEndElement();

                w.WriteEndElement();

                //Receipt Exception
                w.WriteStartElement("operation", this.wsdlNamespace);
                w.WriteAttributeString("name", "ReceiptException");

                w.WriteStartElement("input", this.wsdlNamespace);

                w.WriteAttributeString("message", this.busSignalNamespace + ":" + "ReceiptExceptionSignal");
                w.WriteEndElement();

                w.WriteEndElement();
            }


            //General Exception must be included in all cases
            w.WriteStartElement("operation", this.wsdlNamespace);
            w.WriteAttributeString("name", "GeneralException");

            w.WriteStartElement("input", this.wsdlNamespace);

            w.WriteAttributeString("message", this.busSignalNamespace + ":" + "GeneralExceptionSignal");
            w.WriteEndElement();

            w.WriteEndElement();
        }

        private void generateInitiatorOperation(XmlTextWriter w)
        {
            //Business Information Envelope
            foreach (EA.Element responder in this.respondingInformationEnvelope)
            {
                string abstractName = "PositiveResponseMsg"; //set default value for RespondingInformationEnvelope

                if (XMLTools.getElementTVValue("ispositive",responder).Equals("false", StringComparison.OrdinalIgnoreCase))//responder.Name.Equals("accept", StringComparison.OrdinalIgnoreCase))
                {
                    abstractName = "NegativeResponseMsg";
                }

                w.WriteStartElement("operation", this.wsdlNamespace);
                w.WriteAttributeString("name", removeSpace(responder.Name));

                w.WriteStartElement("input", this.wsdlNamespace);

                w.WriteAttributeString("message", this.targetNamespacePrefix + ":" + abstractName);//getQualifiedName(classifierElement) + "Msg"); //,"MessageInput"));
                w.WriteEndElement();

                w.WriteEndElement();
            }

            #region old algorithm
            ////Business Signal
            ////If the tagged value “timeToAcknowledgeReceipt” is not null, 
            ////then both a Receipt Acknowledgement and ReceiptException are both required
            //string ack = "";
            //EA.Element element = this.requestingBusinessActivity;
            //ack = XMLTools.getElementTVValue("timeToAcknowledgeReceipt", element);

            //if ((ack != "") || (ack.ToLower() != "null"))
            //{
            //    //Receipt Acknowledgement
            //    w.WriteStartElement("operation", this.wsdlNamespace);
            //    w.WriteAttributeString("name", "ReceiptAcknowledgement");

            //    w.WriteStartElement("input", this.wsdlNamespace);

            //    w.WriteAttributeString("message", this.busSignalNamespace + ":" + "ReceiptAcknowledgementSignal");
            //    w.WriteEndElement();

            //    w.WriteEndElement();

            //    //Receipt Exception
            //    w.WriteStartElement("operation", this.wsdlNamespace);
            //    w.WriteAttributeString("name", "ReceiptException");

            //    w.WriteStartElement("input", this.wsdlNamespace);

            //    w.WriteAttributeString("message", this.busSignalNamespace + ":" + "GeneralExceptionSignal");
            //    w.WriteEndElement();

            //    w.WriteEndElement();
            //}
            #endregion

            //string tv = XMLTools.getElementTVValue("businessTransactionType", this.busTransElement);

            //if ((tv == UMMTransactionPattern.Notification.ToString()) || (tv == UMMTransactionPattern.InformationDistribution.ToString()))
            //{
                //Receipt Acknowledgement
                w.WriteStartElement("operation", this.wsdlNamespace);
                w.WriteAttributeString("name", "ReceiptAcknowledgement");

                w.WriteStartElement("input", this.wsdlNamespace);

                w.WriteAttributeString("message", this.busSignalNamespace + ":" + "ReceiptAcknowledgementSignal");
                w.WriteEndElement();

                w.WriteEndElement();

                //Receipt Exception
                w.WriteStartElement("operation", this.wsdlNamespace);
                w.WriteAttributeString("name", "ReceiptException");

                w.WriteStartElement("input", this.wsdlNamespace);

                w.WriteAttributeString("message", this.busSignalNamespace + ":" + "ReceiptExceptionSignal");
                w.WriteEndElement();

                w.WriteEndElement();
            //}

            //General Exception must be include in all cases
            w.WriteStartElement("operation", this.wsdlNamespace);
            w.WriteAttributeString("name", "GeneralException");

            w.WriteStartElement("input", this.wsdlNamespace);

            w.WriteAttributeString("message", this.busSignalNamespace + ":" + "GeneralExceptionSignal");
            w.WriteEndElement();

            w.WriteEndElement();
        }

        private string getResponderPortTypeName()
        {
            string responderName = this.responder.Name;
            string respondingBusActivityName = this.respondingBusinessActivity.Name;

            return removeSpace(responderName) + removeSpace(respondingBusActivityName);
        }

        private string getInitiatorPortTypeName()
        {
            string initiatorName = this.initiator.Name;
            string requestingBusActivityName = this.requestingBusinessActivity.Name;

            return removeSpace(initiatorName) + removeSpace(requestingBusActivityName);
        }


        private string getSchemaLocation(EA.Package pkg)
        {
            String schemaName = "";
            schemaName = XMLTools.getSchemaName(pkg);

            string schemaSavePath = XMLTools.getSavePathForSchema(pkg, repository, this.blnAlias) + schemaName;
            string relativePath = getRelativePath(this.wsdlPath, this.schemaColPath);

            return relativePath + schemaSavePath.Replace(@"\",@"/");
        }

        /// <summary>
        /// Returns the name of the current package
        /// </summary>
        /// <returns></returns>
        private String getPackageName()
        {
            return this.repository.GetPackageByID(Int32.Parse(this.scope)).Name;
        }

        private string removeSpace(string name)
        {
            name = name.Replace(" ", "");
            return name;
        }

        private string getRelativePath(string savePath, string busSignalPath)
        {
            string[] firstPathParts = savePath.Trim(Path.DirectorySeparatorChar).Split(Path.DirectorySeparatorChar);
            string[] secondPathParts = busSignalPath.Trim(Path.DirectorySeparatorChar).Split(Path.DirectorySeparatorChar);

            int sameCounter = 0;
            for (int i = 0; i < Math.Min(firstPathParts.Length, secondPathParts.Length); i++)
            {
                if (!firstPathParts[i].ToLower().Equals(secondPathParts[i].ToLower()))
                {
                    break;
                }
                sameCounter++;
            }

            if (sameCounter == 0)
            {
                return busSignalPath;
            }

            string newPath = String.Empty;
            for (int i = sameCounter; i < firstPathParts.Length; i++)
            {
                if (i > sameCounter)
                {
                    newPath += "/"; //Path.DirectorySeparatorChar;
                }
                newPath += "..";
            }

            if (newPath.Length == 0)
            {
                newPath = ".";
            }

            for (int i = sameCounter; i < secondPathParts.Length; i++)
            {
                newPath += "/"; //Path.DirectorySeparatorChar;
                newPath += secondPathParts[i];
            }

            return newPath + "/";
        }

        private void resetBusSignalRelativePath()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(this.busSignalPath);                      

            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(doc.NameTable);
            namespaceManager.AddNamespace("xs", "http://www.w3.org/2001/XMLSchema");
            XmlNodeList nodeList = doc.SelectNodes("//xs:include", namespaceManager);

            ArrayList doclibSignal = new ArrayList();

            //get DOCLibrary name under the busSignalPkgId
            foreach (EA.Package doclib in this.repository.GetPackageByID(this.busSignalPkgId).Packages)
            {
                if (doclib.Element.Stereotype == CCTS_PackageType.DOCLibrary.ToString())
                    doclibSignal.Add(doclib.Name);
            }

            bool save = false;
            if (doclibSignal.Count == nodeList.Count)
            {
                int idx = 0;
                
                foreach (XmlNode node in nodeList)
                {
                    //string fileName = Path.GetFileName(node.Attributes["schemaLocation"].Value);
                    //string relativePath = getRelativePath(this.wsdlPath, this.schemaColPath) +
                    //        getStructureOfBusSignal(this.busSignalPkgId) + fileName;
                    //@"/NatCore/Signals/" + fileName;

                    string relativePath = getRelativePath(this.wsdlPath, this.schemaColPath) +
                                getStructureOfBusSignal(this.busSignalPkgId) + doclibSignal[idx].ToString() + ".xsd" ;

                    idx++;
                    if (!(relativePath == node.Attributes["schemaLocation"].Value))
                    {
                        node.Attributes["schemaLocation"].Value = relativePath;
                        save = true;
                    }
                }
            }

            if (save)
            {
                XmlTextWriter wrtr = new XmlTextWriter(this.busSignalPath, Encoding.UTF8);
                wrtr.Formatting = Formatting.Indented;
                doc.WriteTo(wrtr);
                wrtr.Close();
            }
        }

        private string getStructureOfBusSignal(int pkgId)
        {
            EA.Package currentPkg = this.repository.GetPackageByID(pkgId);
            string structurePath = "";

            while (currentPkg.ParentID != 0)
            {
                if (currentPkg.Element != null) 
                {
                    if (this.blnAlias && (currentPkg.Alias != ""))
                    {
                        structurePath = currentPkg.Alias + "/" + structurePath;
                    }
                    else
                        structurePath = XMLTools.getXMLName(currentPkg.Name) + "/" + structurePath;
                }
                else
                    break;

                currentPkg = this.repository.GetPackageByID(currentPkg.ParentID);
            }
            return structurePath;
        }

        #region Error Message

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

        #endregion

        #region GeneratorCallBackInterface Members

        public void appendMessage(string type, string message, string packageName)
        {
            if (type == "info")
                this.appendInfoMessage(message, packageName);
            else if (type == "warn")
                this.appendWarnMessage(message, packageName);
            else if (type == "error")
                this.appendErrorMessage(message, packageName);
        }

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

        #endregion

        #region Any-level WSDL generation
        
        private void DoRecursivePackageSearch(EA.Package p)
        {
            if (p.Packages.Count > 0)
            {
                foreach (EA.Package pkg in p.Packages)
                {
                    DoRecursivePackageSearch(pkg);
                }
            }
            else
            {
                if (p.Element.Stereotype.Equals("BusinessTransactionView", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        string scp = p.PackageID.ToString();
                        WSDLGenerator wsdlGenerator = new WSDLGenerator(repository, scp, this.path, blnBindingService, this.blnAlias, getCaller());
                        wsdlGenerator.generateWSDL();
                    }
                    catch (Exception exception)
                    {
                        this.appendErrorMessage(exception.Message, p.Name);
                    }
                }
            }
        }

        #endregion

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

    }
}