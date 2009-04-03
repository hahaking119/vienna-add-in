/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Schema;
using EA;
using VIENNAAddIn.CCTS;
using VIENNAAddIn.constants;
using File=System.IO.File;
using ICollection=System.Collections.ICollection;

namespace VIENNAAddIn.WSDLGenerator
{
    public partial class WSDLGenerator : Form, GeneratorCallBackInterface
    {
        #region Variable

        private ArrayList alreadyIncluded;
        private bool blnAlias = false;
        private bool blnAnyLevel = false;
        private bool blnBindingService = false;
        private bool blnCreateStructure = false;
        private bool busSignalFound = false;
        private string busSignalNamespace = "bs";
        private string busSignalPath = ""; //saving Business Signal Path
        private int busSignalPkgId = 0;

        private Element busTransElement;
        private GeneratorCallBackInterface caller = null;
        private int counter = 0;
        private Hashtable importNamespaceList = new Hashtable();
        private string importPrefix = "bm";
        private Element initiator;
        private string path = "";
        private Repository repository;
        private Element requestingBusinessActivity;
        private ArrayList requestingInformationEnvelope = new ArrayList();
        private Element responder;
        private Element respondingBusinessActivity;
        private ArrayList respondingInformationEnvelope = new ArrayList();
        private string schemaColPath = ""; //saving schema collection Path
        private string schemaNamespace = "http://www.w3.org/2001/XMLSchema";
        private string schemaPrefix = "xs";
        private string scope = "";
        private string soapAction = "";
        private string soapNamespace = "http://schemas.xmlsoap.org/wsdl/soap/";
        private string soapPrefix = "soap";
        private string sourceBusSignal = ""; //saving source of business signal path
        private string sourceSchemaCollection = ""; //saving source of schema collection

        //Prefix list
        private string targetNamespacePrefix = "tns";
        private bool withGUI = true;
        private string wsdlNamespace = "http://schemas.xmlsoap.org/wsdl/";
        private string wsdlPath = ""; //saving wsdl path;
        private string wsdlPrefix = "wsdl";

        #endregion

        private static WSDLGenerator form;

        public static void ShowForm(Repository repository, bool blnAnyLevel)
        {
            string scope = repository.DetermineScope();
            if (form == null || form.IsDisposed)
            {
                form = new WSDLGenerator(repository, scope, blnAnyLevel);
                form.Show();
            }
            else
            {
                form.resetGenerator(scope);
                form.resetBlnAnyLevel(blnAnyLevel);
                form.Select();
                form.Focus();
                form.Show();
            }
        }

        /// <summary>
        /// This methods determins, what should be passed to an auxilliary schema generator
        /// If this class itself has already been called by another class, the calling class is passed
        /// otherwise an instance of this class is passed
        /// </summary>
        /// <returns></returns>
        private GeneratorCallBackInterface getCaller()
        {
            if (caller == null)
                return this;
            else
                return caller;
        }

        #region Constructor

        public WSDLGenerator(Repository repository, string scope, bool blnAnyLevel)
        {
            InitializeComponent();
            this.repository = repository;
            this.scope = scope;
            setActivePackageLabel();

            this.blnAnyLevel = blnAnyLevel;
        }

        //for recursive generation
        public WSDLGenerator(Repository repository, string scope, string path, bool bindingService, bool blnAlias,
                             GeneratorCallBackInterface caller)
        {
            //InitializeComponent();
            this.repository = repository;
            this.scope = scope;
            //this.setActivePackageLabel();
            this.path = path;
            blnBindingService = bindingService;

            this.blnAlias = blnAlias;
            blnAnyLevel = false;
            this.caller = caller;
        }

        #endregion

        #region Button and User interface

        private void btnGenerateWsdl_Click(object sender, EventArgs e)
        {
            resetGenerator(scope);

            getCheckedOption();

            Package pkg = repository.GetPackageByID(Int32.Parse(scope));

            if (txtSavingPath.Text == "")
            {
                appendErrorMessage("Target directory can't be empty. Please select target directory first.", pkg.Name);
                return;
            }
            //if this is the first time WSDL generator running on selected folder
            //check if business signal and schema collection folder is not empty
            if (blnCreateStructure)
            {
                //Check if business signal is not empty
                if (txtBusinessSignal.Text.Trim() == "")
                {
                    appendErrorMessage(
                        "Business Signal location can not be empty, because it's the first time generator running on selected folder.",
                        pkg.Name);
                    return;
                }
                else
                    sourceBusSignal = txtBusinessSignal.Text.Trim();

                //check if schema collection location is not empty
                if (txtSchemaColLocation.Text == "")
                {
                    appendErrorMessage(
                        "Schema Location collection can not be empty, because it's the first time generator running on selected folder.",
                        pkg.Name);
                    return;
                }
                else
                    sourceSchemaCollection = txtSchemaColLocation.Text.Trim();
            }

            Package selectedPackage = repository.GetPackageByID(Int32.Parse(scope));
            bool error = false;
            performProgressStep();

            if (blnCreateStructure)
            {
                setPath();

                //create folder structure for generated WSDL
                CreateDirectoryStructure();

                //check whether Business Signal WSDL is in the same folder.
                CheckBusinessSignal();

                //Copy the schema collection from the source;
                CopyDirectory(sourceSchemaCollection, schemaColPath);
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
                    appendErrorMessage(exception.Message, selectedPackage.Name);
                }
                if (!error)
                    appendInfoMessage("Successfully generate WSDL.", selectedPackage.Name);
                progressBar1.Value = progressBar1.Maximum;

                //create structure only for the first generated WSDL on selected folder
                blnCreateStructure = false;
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
                    appendErrorMessage(exception.Message, selectedPackage.Name);
                }

                if (!error)
                    appendInfoMessage("Finished generating WSDL.", selectedPackage.Name);
                progressBar1.Value = progressBar1.Maximum;

                //create structure only for the first generated WSDL on selected folder
                blnCreateStructure = false;
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
            txtBusinessSignal.Text = "";
            txtSavingPath.Text = "";
            txtSchemaColLocation.Text = "";

            blnCreateStructure = false;

            EnableBrowseButton(false);

            DialogResult result = dlgSavePath.ShowDialog();

            if (result == DialogResult.OK)
            {
                path = dlgSavePath.SelectedPath + @"\";
                txtSavingPath.Text = dlgSavePath.SelectedPath;

                if (!CheckSchemaAndWSDLFolderExist())
                    //{
                    //    EnableBrowseButton(false);
                    //}
                    //else
                {
                    MessageBox.Show("This is the first time WSDL generator running on selected folder.\n " +
                                    "You must specify the business signal location and schema collection location.",
                                    "Message", MessageBoxButtons.OK);

                    EnableBrowseButton(true);
                    blnCreateStructure = true;
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
            Close();
        }

        private void chkBindingService_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBindingService.Checked)
            {
                DialogResult result =
                    MessageBox.Show("Please note the generator will not fill in the endpoint references in the WSDL." +
                                    "\nPlease remember to fill in the endpoint references (marked with '[myEndPointReference]') \nbefore using the WSDL for implementation." +
                                    "\nDo you want to continue?",
                                    "Warning", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    blnBindingService = true;
                }
                else
                    chkBindingService.Checked = false;
            }
            else
                blnBindingService = false;
        }

        #endregion

        #region Single WSDL generation

        public void appendMessage(string type, string message, string packageName)
        {
            if (type == "info")
                appendInfoMessage(message, packageName);
            else if (type == "warn")
                appendWarnMessage(message, packageName);
            else if (type == "error")
                appendErrorMessage(message, packageName);
        }

        public void performProgressStep()
        {
            if (withGUI)
            {
                progressBar1.PerformStep();
            }
            else
            {
                //Is there a caller for this class?
                if (caller != null)
                    caller.performProgressStep();
            }
        }

        public void resetGenerator(String scope)
        {
            this.scope = scope;
            progressBar1.Value = progressBar1.Minimum;
            statusTextBox.Text = "";
            setActivePackageLabel();

            requestingInformationEnvelope = new ArrayList();
            respondingInformationEnvelope = new ArrayList();
            importNamespaceList = new Hashtable();
        }

        public void resetBlnAnyLevel(bool blnAnyLevel)
        {
            this.blnAnyLevel = blnAnyLevel;
            chkBindingService.Checked = false;

            txtBusinessSignal.Text = "";
            txtSavingPath.Text = "";
            txtSchemaColLocation.Text = "";
        }

        private void setActivePackageLabel()
        {
            Package pkg = repository.GetPackageByID(Int32.Parse(scope));
            selectedBusinessTransaction.Text = pkg.Name + "<<" + pkg.Element.Stereotype.ToString() + ">>";
        }

        /// <summary>
        /// Setting the path of schema collection, wsdl, and business signal to the new saving path
        /// </summary>
        private void setPath()
        {
            schemaColPath = path + "Schemas" + @"\";
            wsdlPath = path + "WSDL" + @"\";
            busSignalPath = path + @"WSDL\GIEMBusinessSignal.wsdl";
        }

        private void CreateDirectoryStructure()
        {
            //if (this.blnCreateStructure)
            //{
            Directory.CreateDirectory(schemaColPath);
            Directory.CreateDirectory(wsdlPath);
            //}
        }

        private void EnableBrowseButton(bool blnEnabled)
        {
            btnBrowse.Enabled = blnEnabled;
            btnBrowseSchemaCol.Enabled = blnEnabled;
        }

        private bool CheckSchemaAndWSDLFolderExist()
        {
            if (Directory.Exists(path + "schemas"))
                if (Directory.Exists(path + "wsdl"))
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

            Package pkg = repository.GetPackageByID(Int32.Parse(scope));

            //get element with stereotype <<BusinessTransaction>>
            foreach (Element e in pkg.Elements)
            {
                if (e.Stereotype.Equals("BusinessTransaction", StringComparison.OrdinalIgnoreCase))
                {
                    busTransElement = e;
                    break;
                }
            }

            if (busTransElement == null)
            {
                throw new Exception(
                    "No element with stereotype <<BusinessTransaction>>. Therefore, there is no WSDL schema to generate.");
            }

            String schemaName = "";
            schemaName = XMLTools.getSchemaNameWithoutExtention(repository.GetPackageByID(pkg.PackageID));
            String filename = wsdlPath + schemaName + ".wsdl";
            var w = new XmlTextWriter(filename, Encoding.UTF8);

            try
            {
                getInitiatorResponderElement();

                generateInitialSchema(w);

                addBusinessSignalImports(w);

                addBusinessMessageType(w);

                generateBusinessMessage(w);

                generatePortType(w);

                if (blnBindingService)
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
            if (chkUseAlias.Checked)
                blnAlias = true;
            else
                blnAlias = false;
        }

        private void generateBindingService(XmlTextWriter w)
        {
            string bindingName = "";

            #region ~~Responder Binding~~

            bindingName = responder.Name + removeSpace(respondingBusinessActivity.Name);
                //this.responder.Name + removeSpace(this.repository.GetPackageByID(Int32.Parse(this.scope)).Name);

            w.WriteStartElement(wsdlPrefix, "binding", wsdlNamespace);
            w.WriteAttributeString("name", bindingName + "SOAP");
            w.WriteAttributeString("type", targetNamespacePrefix + ":" + bindingName);

            w.WriteStartElement(soapPrefix, "binding", soapNamespace);
            w.WriteAttributeString("style", "document");
            w.WriteAttributeString("transport", "http://schemas.xmlsoap.org/soap/http");
            w.WriteAttributeString("xmlns", soapPrefix, null, soapNamespace);
            w.WriteEndElement();

            #region Requesting Business Activity

            foreach (Element el in requestingInformationEnvelope)
            {
                w.WriteStartElement(wsdlPrefix, "operation", wsdlNamespace);
                w.WriteAttributeString("name", el.Name);

                w.WriteStartElement(soapPrefix, "operation", soapNamespace);
                w.WriteAttributeString("soapAction", soapAction);
                w.WriteAttributeString("style", "document");
                w.WriteAttributeString("xmlns", soapPrefix, null, soapNamespace);
                w.WriteEndElement();

                w.WriteStartElement(wsdlPrefix, "input", wsdlNamespace);
                w.WriteStartElement(soapPrefix, "body", soapNamespace);
                w.WriteAttributeString("use", "literal");
                w.WriteAttributeString("xmlns", soapPrefix, null, soapNamespace);
                w.WriteEndElement();
                w.WriteEndElement();

                w.WriteEndElement();
            }

            #endregion

            string tv = XMLTools.getElementTVValue("businessTransactionType", busTransElement);

            if (!(tv.ToLower() == "notification") && !(tv.ToLower() == "informationdistribution"))
            {
                #region ReceiptAcknowledgement

                w.WriteStartElement(wsdlPrefix, "operation", wsdlNamespace);
                w.WriteAttributeString("name", "ReceiptAcknowledgement");

                w.WriteStartElement(soapPrefix, "operation", soapNamespace);
                w.WriteAttributeString("soapAction", soapAction);
                w.WriteAttributeString("style", "document");
                w.WriteAttributeString("xmlns", soapPrefix, null, soapNamespace);
                w.WriteEndElement();

                w.WriteStartElement(wsdlPrefix, "input", wsdlNamespace);
                w.WriteStartElement(soapPrefix, "body", soapNamespace);
                w.WriteAttributeString("use", "literal");
                w.WriteAttributeString("xmlns", soapPrefix, null, soapNamespace);
                w.WriteEndElement();
                w.WriteEndElement();

                w.WriteEndElement();

                #endregion

                #region ReceiptException

                w.WriteStartElement(wsdlPrefix, "operation", wsdlNamespace);
                w.WriteAttributeString("name", "ReceiptException");

                w.WriteStartElement(soapPrefix, "operation", soapNamespace);
                w.WriteAttributeString("soapAction", soapAction);
                w.WriteAttributeString("style", "document");
                w.WriteAttributeString("xmlns", soapPrefix, null, soapNamespace);
                w.WriteEndElement();

                w.WriteStartElement(wsdlPrefix, "input", wsdlNamespace);
                w.WriteStartElement(soapPrefix, "body", soapNamespace);
                w.WriteAttributeString("use", "literal");
                w.WriteAttributeString("xmlns", soapPrefix, null, soapNamespace);
                w.WriteEndElement();
                w.WriteEndElement();

                w.WriteEndElement();

                #endregion
            }

            #region GeneralException

            w.WriteStartElement(wsdlPrefix, "operation", wsdlNamespace);
            w.WriteAttributeString("name", "GeneralException");

            w.WriteStartElement(soapPrefix, "operation", soapNamespace);
            w.WriteAttributeString("soapAction", soapAction);
            w.WriteAttributeString("style", "document");
            w.WriteAttributeString("xmlns", soapPrefix, null, soapNamespace);
            w.WriteEndElement();

            w.WriteStartElement(wsdlPrefix, "input", wsdlNamespace);
            w.WriteStartElement(soapPrefix, "body", soapNamespace);
            w.WriteAttributeString("use", "literal");
            w.WriteAttributeString("xmlns", soapPrefix, null, soapNamespace);
            w.WriteEndElement();
            w.WriteEndElement();

            w.WriteEndElement();

            #endregion

            w.WriteEndElement();

            #endregion

            #region ~~Initiator Binding~~

            bindingName = initiator.Name + removeSpace(requestingBusinessActivity.Name);
                //this.initiator.Name + removeSpace(this.repository.GetPackageByID(Int32.Parse(this.scope)).Name);

            w.WriteStartElement(wsdlPrefix, "binding", wsdlNamespace);
            w.WriteAttributeString("name", initiator.Name + removeSpace(requestingBusinessActivity.Name) + "SOAP");
            w.WriteAttributeString("type", targetNamespacePrefix + ":" + bindingName);

            w.WriteStartElement(soapPrefix, "binding", soapNamespace);
            w.WriteAttributeString("style", "document");
            w.WriteAttributeString("transport", "http://schemas.xmlsoap.org/soap/http");
            w.WriteAttributeString("xmlns", soapPrefix, null, soapNamespace);
            w.WriteEndElement();

            #region Requesting Business Activity

            foreach (Element el in respondingInformationEnvelope)
            {
                w.WriteStartElement(wsdlPrefix, "operation", wsdlNamespace);
                w.WriteAttributeString("name", el.Name);

                w.WriteStartElement(soapPrefix, "operation", soapNamespace);
                w.WriteAttributeString("soapAction", soapAction);
                w.WriteAttributeString("style", "document");
                w.WriteAttributeString("xmlns", soapPrefix, null, soapNamespace);
                w.WriteEndElement();

                w.WriteStartElement(wsdlPrefix, "input", wsdlNamespace);
                w.WriteStartElement(soapPrefix, "body", soapNamespace);
                w.WriteAttributeString("use", "literal");
                w.WriteAttributeString("xmlns", soapPrefix, null, soapNamespace);
                w.WriteEndElement();
                w.WriteEndElement();

                w.WriteEndElement();
            }

            #endregion

            #region ReceiptAcknowledgement

            w.WriteStartElement(wsdlPrefix, "operation", wsdlNamespace);
            w.WriteAttributeString("name", "ReceiptAcknowledgement");

            w.WriteStartElement(soapPrefix, "operation", soapNamespace);
            w.WriteAttributeString("soapAction", soapAction);
            w.WriteAttributeString("style", "document");
            w.WriteAttributeString("xmlns", soapPrefix, null, soapNamespace);
            w.WriteEndElement();

            w.WriteStartElement(wsdlPrefix, "input", wsdlNamespace);
            w.WriteStartElement(soapPrefix, "body", soapNamespace);
            w.WriteAttributeString("use", "literal");
            w.WriteAttributeString("xmlns", soapPrefix, null, soapNamespace);
            w.WriteEndElement();
            w.WriteEndElement();

            w.WriteEndElement();

            #endregion

            #region ReceiptException

            w.WriteStartElement(wsdlPrefix, "operation", wsdlNamespace);
            w.WriteAttributeString("name", "ReceiptException");

            w.WriteStartElement(soapPrefix, "operation", soapNamespace);
            w.WriteAttributeString("soapAction", soapAction);
            w.WriteAttributeString("style", "document");
            w.WriteAttributeString("xmlns", soapPrefix, null, soapNamespace);
            w.WriteEndElement();

            w.WriteStartElement(wsdlPrefix, "input", wsdlNamespace);
            w.WriteStartElement(soapPrefix, "body", soapNamespace);
            w.WriteAttributeString("use", "literal");
            w.WriteAttributeString("xmlns", soapPrefix, null, soapNamespace);
            w.WriteEndElement();
            w.WriteEndElement();

            w.WriteEndElement();

            #endregion

            #region GeneralException

            w.WriteStartElement(wsdlPrefix, "operation", wsdlNamespace);
            w.WriteAttributeString("name", "GeneralException");

            w.WriteStartElement(soapPrefix, "operation", soapNamespace);
            w.WriteAttributeString("soapAction", soapAction);
            w.WriteAttributeString("style", "document");
            w.WriteAttributeString("xmlns", soapPrefix, null, soapNamespace);
            w.WriteEndElement();

            w.WriteStartElement(wsdlPrefix, "input", wsdlNamespace);
            w.WriteStartElement(soapPrefix, "body", soapNamespace);
            w.WriteAttributeString("use", "literal");
            w.WriteAttributeString("xmlns", soapPrefix, null, soapNamespace);
            w.WriteEndElement();
            w.WriteEndElement();

            w.WriteEndElement();

            #endregion

            w.WriteEndElement();

            #endregion

            string serviceName = "";

            #region Responder Service

            serviceName = responder.Name + removeSpace(respondingBusinessActivity.Name);

            w.WriteStartElement(wsdlPrefix, "service", wsdlNamespace);
            w.WriteAttributeString("name", serviceName);

            w.WriteStartElement(wsdlPrefix, "port", wsdlNamespace);
            w.WriteAttributeString("name", serviceName + "SOAP");
            w.WriteAttributeString("binding", targetNamespacePrefix + ":" + serviceName + "SOAP");

            w.WriteStartElement(soapPrefix, "address", soapNamespace);
            w.WriteAttributeString("location", "[myEndPointReference]" + @"/" + serviceName);
            w.WriteAttributeString("xmlns", soapPrefix, null, soapNamespace);
            w.WriteEndElement();
            w.WriteEndElement();

            w.WriteEndElement();

            #endregion

            #region Initiator Service

            serviceName = initiator.Name + removeSpace(requestingBusinessActivity.Name);

            w.WriteStartElement(wsdlPrefix, "service", wsdlNamespace);
            w.WriteAttributeString("name", serviceName);

            w.WriteStartElement(wsdlPrefix, "port", wsdlNamespace);
            w.WriteAttributeString("name", serviceName + "SOAP");
            w.WriteAttributeString("binding", targetNamespacePrefix + ":" + serviceName + "SOAP");

            w.WriteStartElement(soapPrefix, "address", soapNamespace);
            w.WriteAttributeString("location", "[myEndPointReference]" + @"/" + serviceName);
            w.WriteAttributeString("xmlns", soapPrefix, null, soapNamespace);
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
                    File.Copy(Element, destination + Path.GetFileName(Element), true);
            }
        }

        /// <summary>
        /// Method to check whether the WSDL Business Signal is in the same folder with the saving path.
        /// If no, copy the WSDL Business Signal to saving path
        /// </summary>
        private void CheckBusinessSignal()
        {
            File.Copy(sourceBusSignal, wsdlPath + "GIEMBusinessSignal.wsdl", true);

            try
            {
                //finding the location of business signal in the model
                findBusinessSignal();
                //resetting the relative path of imported schema in GIEMBusinessSignal.wsdl
                resetBusSignalRelativePath();
            }
            catch (Exception excp)
            {
                appendErrorMessage(excp.Message, "");
            }
        }

        private void findBusinessSignal()
        {
            busSignalFound = false;

            for (short a = 0; a < repository.Models.Count; a++)
            {
                if (busSignalFound)
                    break;

                var pkg = (Package) repository.Models.GetAt(a);

                searchRecursiveInsidePackage(pkg);
            }
        }

        private void searchRecursiveInsidePackage(Package pkg)
        {
            for (short idx = 0; idx < pkg.Packages.Count; idx++)
            {
                if (busSignalFound)
                    break;

                var thePackage = (Package) pkg.Packages.GetAt(idx);

                if (thePackage.Name == "Signals.library")
                {
                    busSignalFound = true;
                    busSignalPkgId = thePackage.PackageID;
                    break;
                }

                if (pkg.Packages.Count > 0)
                    searchRecursiveInsidePackage(thePackage);
            }
        }

        private bool isIncludeBusinessMessage(Element element)
        {
            Package pBusTrans = repository.GetPackageByID(Int32.Parse(scope));

            string baseURNBusMsg, baseURNBusTrans;

            //get business Transaction "baseURN" tagged value
            baseURNBusTrans = XMLTools.getElementTVValue(CCTS_TV.baseURN, pBusTrans.Element);
            if (baseURNBusTrans == "")
                throw new Exception("'baseURN' tagged value in package " + pBusTrans.Name +
                                    " is not found or empty. Please fill add it or fill it.");

            Element infElementClassifier;
            try
            {
                infElementClassifier = repository.GetElementByID(element.ClassifierID);
            }
            catch
            {
                throw new Exception("Failed getting classifier of element " + element.Name);
            }

            Package pBusMsg = repository.GetPackageByID(infElementClassifier.PackageID); //get DOCLibrary

            //get bussiness Message "baseuURN" tagged value
            baseURNBusMsg = XMLTools.getElementTVValue(CCTS_TV.baseURN, pBusMsg.Element);
            if (baseURNBusMsg == "")
                throw new Exception("'baseURN' tagged value in package " + pBusMsg.Name +
                                    " is not found or empty. Please fill or add it.");

            if (baseURNBusMsg == baseURNBusTrans)
                return true; //include
            else
                return false; //import
        }

        private void addBusinessMessageType(XmlTextWriter w)
        {
            w.WriteStartElement("types", wsdlNamespace);

            w.WriteStartElement("schema", schemaNamespace);
            w.WriteAttributeString("targetNamespace", getNamespace());
            w.WriteAttributeString("elementFormDefault", "qualified");

            var informationEnvelope = new ArrayList(requestingInformationEnvelope);
            informationEnvelope.AddRange(respondingInformationEnvelope);

            alreadyIncluded = new ArrayList();

            foreach (Element element in informationEnvelope)
            {
                Element infElementClassifier;

                try
                {
                    infElementClassifier = repository.GetElementByID(element.ClassifierID);
                }
                catch
                {
                    throw new Exception("Failed getting classifier of element " + element.Name);
                }

                Package pBusMsg = repository.GetPackageByID(infElementClassifier.PackageID); //get DOCLibrary

                if (!isAlreadyExist(element))
                {
                    if (isIncludeBusinessMessage(element))
                    {
                        w.WriteStartElement("include", schemaNamespace);
                        w.WriteAttributeString("schemaLocation", getSchemaLocation(pBusMsg));
                        w.WriteEndElement();

                        //generateLinkedSchema(pBusMsg);
                    }
                    else
                    {
                        //not sure on this
                        w.WriteStartElement("import", schemaNamespace);
                        w.WriteAttributeString("namespace", XMLTools.getNameSpace(repository, pBusMsg));
                        w.WriteAttributeString("schemaLocation", getSchemaLocation(pBusMsg));
                        w.WriteEndElement();
                    }
                }
            }

            w.WriteEndElement(); //for schema
            w.WriteEndElement(); //for types
        }

        private void generateLinkedSchema(Package pBusMsg)
        {
            ICollection result = null;

            if (pBusMsg.Element.Stereotype == CCTS_PackageType.DOCLibrary.ToString())
            {
                result = new DOCGenerator(repository, pBusMsg.PackageID.ToString(), false).generateSchema(pBusMsg);
            }

            String schemaPath = "";
            String schemaName = "";
            schemaPath = XMLTools.getSavePathForSchema(repository.GetPackageByID(pBusMsg.PackageID), repository,
                                                       blnAlias);
            schemaName = XMLTools.getSchemaName(repository.GetPackageByID(pBusMsg.PackageID));

            //Write the schema(s)
            foreach (XmlSchema schema1 in result)
            {
                String filename = path + schemaPath + schemaName;
                //Create the path
                Directory.CreateDirectory(path + schemaPath);
                Stream outputStream = File.Open(filename, FileMode.Create);
                schema1.Write(outputStream);
                outputStream.Close();
            }
        }

        private bool isAlreadyExist(Element element)
        {
            Element infElementClassifier = repository.GetElementByID(element.ClassifierID);
            Package pBusMsg = repository.GetPackageByID(infElementClassifier.PackageID); //get DOCLibrary

            if (alreadyIncluded.Contains(pBusMsg.Name)) //baseURN))
                return true;
            else
                alreadyIncluded.Add(pBusMsg.Name);

            return false;
        }

        private void getInitiatorResponderElement()
        {
            bool swimlaneRequestExist = false;
            bool swimlaneRespondExist = false;

            foreach (Element e in busTransElement.Elements)
            {
                if (e.Stereotype.Equals("BusinessTransactionSwimlane"))
                {
                    foreach (Element busActivity in e.Elements)
                    {
                        if (busActivity.Stereotype.Equals("RequestingBusinessActivity"))
                        {
                            swimlaneRequestExist = true;

                            try
                            {
                                initiator = repository.GetElementByID(e.ClassifierID);
                            }
                            catch
                            {
                                string erroMsg =
                                    "Failed get the classifier of <<BusinessTransactionSwimlane>> that contains a <<RequestingBusinessActivity>> on package " +
                                    repository.GetPackageByID(Int32.Parse(scope)).Name +
                                    ". \nIt might be caused by losing reference to classifier or you miss setting it.";
                                throw new Exception(erroMsg);
                            }

                            requestingBusinessActivity = busActivity;

                            foreach (Element busEnvelope in busActivity.Elements)
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
                                responder = repository.GetElementByID(e.ClassifierID);
                            }
                            catch
                            {
                                string errorMsg =
                                    "Failed to get the classifier of <<BusinessTransactionSwimlane>> that contains a <<RespondingBusinessActivity>> on package " +
                                    repository.GetPackageByID(Int32.Parse(scope)).Name +
                                    ". \nIt might be caused by losing reference to classifier or you miss setting it.";
                                throw new Exception(errorMsg);
                            }

                            respondingBusinessActivity = busActivity;

                            foreach (Element busEnvelope in busActivity.Elements)
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
                                  repository.GetPackageByID(Int32.Parse(scope)).Name +
                                  ".\n Can't continue to generate WSDL.";
                throw new Exception(errorMsg);
            }
            else if (!(swimlaneRequestExist))
            {
                string errorMsg =
                    "<<BusinessTransactionSwimlane>> that contains <<RequestingBusinessActivity>> doesn't exist." +
                    ". \nCan't continue to generate WSDL.";
                throw new Exception(errorMsg);
            }
            else if (!(swimlaneRespondExist))
            {
                //notification and InformationDistribution don't have RespondingBusinessActivity, they are one-way transaction
                if (
                    !XMLTools.getElementTVValue("businessTransactionType", busTransElement).Equals("Notification",
                                                                                                   StringComparison.
                                                                                                       OrdinalIgnoreCase) ||
                    !XMLTools.getElementTVValue("businessTransactionType", busTransElement).Equals(
                         "InformationDistribution", StringComparison.OrdinalIgnoreCase))
                {
                    string errorMsg =
                        "<<BusinessTransactionSwimlane>> that contains <<RespondingBusinessActivity>> doesn't exist." +
                        ". \nCan't continue to generate WSDL.";
                    throw new Exception(errorMsg);
                }
            }
        }

        private void addBusinessSignalImports(XmlTextWriter w)
        {
            w.WriteStartElement("import", wsdlNamespace);

            w.WriteAttributeString("namespace", getNamespaceBusinessSignal());

            //Business Signal Location is now in the same directory with the saving path of Business transaction WSDL - March 21,2008
            w.WriteAttributeString("location", Path.GetFileName(busSignalPath));
                //getRelativePath(this.schemaPath, this.busSignalPath));

            w.WriteEndElement();
        }

        private string getNamespaceBusinessSignal()
        {
            var reader = new XmlTextReader(busSignalPath);
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
                throw new Exception("Failed getting namespace from file " + busSignalPath + ". Error message: " +
                                    ex.Message);
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

            w.WriteStartElement(wsdlPrefix, "definitions", wsdlNamespace);
            w.WriteAttributeString("xmlns", soapPrefix, null, soapNamespace);
            w.WriteAttributeString("xmlns", "http", null, "http://schemas.xmlsoap.org/wsdl/http/");
            w.WriteAttributeString("xmlns", schemaPrefix, null, schemaNamespace);
            w.WriteAttributeString("xmlns", "soapenc", null, "http://schemas.xmlsoap.org/soap/encoding/");
            w.WriteAttributeString("xmlns", "mime", null, "http://schemas.xmlsoap.org/wsdl/mime/");
            w.WriteAttributeString("xmlns", targetNamespacePrefix, null, getNamespace());
            w.WriteAttributeString("xmlns", busSignalNamespace, null, getNamespaceBusinessSignal());
            //TO DO :
            //add namespace for importing business message which is not in the same namespace with WSDL
            addImportBusinessMessageNamespace(w);
            w.WriteAttributeString("name", removeSpace(busTransElement.Name));
            w.WriteAttributeString("", "targetNamespace", null, getNamespace());
        }

        private string getNamespace()
        {
            return XMLTools.getNameSpace(repository, repository.GetPackageByID(busTransElement.PackageID));
        }

        private void addImportBusinessMessageNamespace(XmlTextWriter w)
        {
            var joinArr = new ArrayList(requestingInformationEnvelope);
            joinArr.AddRange(respondingInformationEnvelope);

            foreach (Element infElement in joinArr)
            {
                if (!isIncludeBusinessMessage(infElement))
                {
                    Element infElementClassifier;
                    try
                    {
                        infElementClassifier = repository.GetElementByID(infElement.ClassifierID);
                    }
                    catch
                    {
                        string errorMsg = "Failed getting classifier of element " + infElement.Name;
                        throw new Exception(errorMsg);
                    }

                    Package pBusMsg = repository.GetPackageByID(infElementClassifier.PackageID); //get DOCLibrary

                    if (!importNamespaceList.Contains(XMLTools.getNameSpace(repository, pBusMsg)))
                    {
                        w.WriteAttributeString("xmlns", importPrefix + ++counter, null,
                                               XMLTools.getNameSpace(repository, pBusMsg));

                        //add imported schema and its prefix to a hashtable for further reference.
                        importNamespaceList.Add(XMLTools.getNameSpace(repository, pBusMsg), importPrefix + counter);
                    }
                }
            }
        }

        private void generateBusinessMessage(XmlTextWriter w)
        {
            foreach (Element reqElement in requestingInformationEnvelope)
            {
                Element reqElementClassifier;
                try
                {
                    reqElementClassifier = repository.GetElementByID(reqElement.ClassifierID);
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

            foreach (Element resElement in respondingInformationEnvelope)
            {
                Element resElementClassifier;
                try
                {
                    resElementClassifier = repository.GetElementByID(resElement.ClassifierID);
                }
                catch
                {
                    throw new Exception("Failed getting classifier of element " + resElement.Name);
                }

                string abstractName = "PositiveResponseMsg"; //give default value to RespondingInformationEnvelope

                if (XMLTools.getElementTVValue("ispositive", resElement).Equals("false",
                                                                                StringComparison.OrdinalIgnoreCase))
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
        private string getQualifiedName(Element classifierElement) //, string type)
        {
            string ns = "";

            Package pBusMsg = repository.GetPackageByID(classifierElement.PackageID); //get DOCLibrary

            string elementNamespace = XMLTools.getNameSpace(repository, pBusMsg);

            if (!importNamespaceList.ContainsKey(elementNamespace)) // in the same namespace
            {
                ns = targetNamespacePrefix + ":" + classifierElement.Name;
            }
            else //different namespace
            {
                ns = (string) (importNamespaceList[elementNamespace]) + ":" + classifierElement.Name;
            }

            return ns;
        }

        private void generatePortType(XmlTextWriter w)
        {
            //Initiator
            w.WriteStartElement("portType", wsdlNamespace);
            w.WriteAttributeString("name", getInitiatorPortTypeName());
            generateInitiatorOperation(w);
            w.WriteEndElement();

            //Responder
            w.WriteStartElement("portType", wsdlNamespace);
            w.WriteAttributeString("name", getResponderPortTypeName());
            generateResponderOperation(w);
            w.WriteEndElement();
        }

        private void generateResponderOperation(XmlTextWriter w)
        {
            foreach (Element requesting in requestingInformationEnvelope)
            {
                string abstractName = "RequestMsg";

                w.WriteStartElement("operation", wsdlNamespace);
                w.WriteAttributeString("name", removeSpace(requesting.Name));

                w.WriteStartElement("input", wsdlNamespace);
                w.WriteAttributeString("message", targetNamespacePrefix + ":" + abstractName);
                    //getQualifiedName(classifierElement) + "Msg"); //,"MessageInput"));
                w.WriteEndElement();

                w.WriteEndElement();
            }

            string tv = XMLTools.getElementTVValue("businessTransactionType", busTransElement);

            if (!(tv.ToLower() == "notification") && !(tv.ToLower() == "informationdistribution"))
            {
                //Receipt Acknowledgement
                w.WriteStartElement("operation", wsdlNamespace);
                w.WriteAttributeString("name", "ReceiptAcknowledgement");

                w.WriteStartElement("input", wsdlNamespace);

                w.WriteAttributeString("message", busSignalNamespace + ":" + "ReceiptAcknowledgementSignal");
                w.WriteEndElement();

                w.WriteEndElement();

                //Receipt Exception
                w.WriteStartElement("operation", wsdlNamespace);
                w.WriteAttributeString("name", "ReceiptException");

                w.WriteStartElement("input", wsdlNamespace);

                w.WriteAttributeString("message", busSignalNamespace + ":" + "ReceiptExceptionSignal");
                w.WriteEndElement();

                w.WriteEndElement();
            }

            //General Exception must be included in all cases
            w.WriteStartElement("operation", wsdlNamespace);
            w.WriteAttributeString("name", "GeneralException");

            w.WriteStartElement("input", wsdlNamespace);

            w.WriteAttributeString("message", busSignalNamespace + ":" + "GeneralExceptionSignal");
            w.WriteEndElement();

            w.WriteEndElement();
        }

        private void generateInitiatorOperation(XmlTextWriter w)
        {
            //Business Information Envelope
            foreach (Element responder in respondingInformationEnvelope)
            {
                string abstractName = "PositiveResponseMsg"; //set default value for RespondingInformationEnvelope

                if (XMLTools.getElementTVValue("ispositive", responder).Equals("false",
                                                                               StringComparison.OrdinalIgnoreCase))
                    //responder.Name.Equals("accept", StringComparison.OrdinalIgnoreCase))
                {
                    abstractName = "NegativeResponseMsg";
                }

                w.WriteStartElement("operation", wsdlNamespace);
                w.WriteAttributeString("name", removeSpace(responder.Name));

                w.WriteStartElement("input", wsdlNamespace);

                w.WriteAttributeString("message", targetNamespacePrefix + ":" + abstractName);
                    //getQualifiedName(classifierElement) + "Msg"); //,"MessageInput"));
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
            w.WriteStartElement("operation", wsdlNamespace);
            w.WriteAttributeString("name", "ReceiptAcknowledgement");

            w.WriteStartElement("input", wsdlNamespace);

            w.WriteAttributeString("message", busSignalNamespace + ":" + "ReceiptAcknowledgementSignal");
            w.WriteEndElement();

            w.WriteEndElement();

            //Receipt Exception
            w.WriteStartElement("operation", wsdlNamespace);
            w.WriteAttributeString("name", "ReceiptException");

            w.WriteStartElement("input", wsdlNamespace);

            w.WriteAttributeString("message", busSignalNamespace + ":" + "ReceiptExceptionSignal");
            w.WriteEndElement();

            w.WriteEndElement();
            //}

            //General Exception must be include in all cases
            w.WriteStartElement("operation", wsdlNamespace);
            w.WriteAttributeString("name", "GeneralException");

            w.WriteStartElement("input", wsdlNamespace);

            w.WriteAttributeString("message", busSignalNamespace + ":" + "GeneralExceptionSignal");
            w.WriteEndElement();

            w.WriteEndElement();
        }

        private string getResponderPortTypeName()
        {
            string responderName = responder.Name;
            string respondingBusActivityName = respondingBusinessActivity.Name;

            return removeSpace(responderName) + removeSpace(respondingBusActivityName);
        }

        private string getInitiatorPortTypeName()
        {
            string initiatorName = initiator.Name;
            string requestingBusActivityName = requestingBusinessActivity.Name;

            return removeSpace(initiatorName) + removeSpace(requestingBusActivityName);
        }

        private string getSchemaLocation(Package pkg)
        {
            String schemaName = "";
            schemaName = XMLTools.getSchemaName(pkg);

            string schemaSavePath = XMLTools.getSavePathForSchema(pkg, repository, blnAlias) + schemaName;
            string relativePath = getRelativePath(wsdlPath, schemaColPath);

            return relativePath + schemaSavePath.Replace(@"\", @"/");
        }

        /// <summary>
        /// Returns the name of the current package
        /// </summary>
        /// <returns></returns>
        private String getPackageName()
        {
            return repository.GetPackageByID(Int32.Parse(scope)).Name;
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
            var doc = new XmlDocument();
            doc.Load(busSignalPath);

            var namespaceManager = new XmlNamespaceManager(doc.NameTable);
            namespaceManager.AddNamespace("xs", "http://www.w3.org/2001/XMLSchema");
            XmlNodeList nodeList = doc.SelectNodes("//xs:include", namespaceManager);

            var doclibSignal = new ArrayList();

            //get DOCLibrary name under the busSignalPkgId
            foreach (Package doclib in repository.GetPackageByID(busSignalPkgId).Packages)
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

                    string relativePath = getRelativePath(wsdlPath, schemaColPath) +
                                          getStructureOfBusSignal(busSignalPkgId) + doclibSignal[idx].ToString() +
                                          ".xsd";

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
                var wrtr = new XmlTextWriter(busSignalPath, Encoding.UTF8);
                wrtr.Formatting = Formatting.Indented;
                doc.WriteTo(wrtr);
                wrtr.Close();
            }
        }

        private string getStructureOfBusSignal(int pkgId)
        {
            Package currentPkg = repository.GetPackageByID(pkgId);
            string structurePath = "";

            while (currentPkg.ParentID != 0)
            {
                if (currentPkg.Element != null)
                {
                    if (blnAlias && (currentPkg.Alias != ""))
                    {
                        structurePath = currentPkg.Alias + "/" + structurePath;
                    }
                    else
                        structurePath = XMLTools.getXMLName(currentPkg.Name) + "/" + structurePath;
                }
                else
                    break;

                currentPkg = repository.GetPackageByID(currentPkg.ParentID);
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
                caller.appendMessage("error", msg, getPackageName());
            }
            else
            {
                if (withGUI)
                {
                    statusTextBox.Text += "ERROR: (Package: " + packageName + ") " + msg + "\n\n";
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
                caller.appendMessage("info", msg, getPackageName());
            }
            else
            {
                if (withGUI)
                {
                    statusTextBox.Text += "INFO: (Package: " + packageName + ") " + msg + "\n\n";
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
                caller.appendMessage("warn", msg, getPackageName());
            }
            else
            {
                if (withGUI)
                {
                    statusTextBox.Text += "WARN: (Package: " + packageName + ") " + msg + "\n\n";
                }
            }
        }

        #endregion

        #endregion

        #region Any-level WSDL generation

        private void DoRecursivePackageSearch(Package p)
        {
            if (p.Packages.Count > 0)
            {
                foreach (Package pkg in p.Packages)
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
                        var wsdlGenerator = new WSDLGenerator(repository, scp, path, blnBindingService, blnAlias,
                                                              getCaller());
                        wsdlGenerator.generateWSDL();
                    }
                    catch (Exception exception)
                    {
                        appendErrorMessage(exception.Message, p.Name);
                    }
                }
            }
        }

        #endregion
    }
}