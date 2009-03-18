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
using System.IO;
using System.Xml;
using System.Collections;
using System.Text.RegularExpressions;
using VIENNAAddIn.common;
using VIENNAAddIn.CCTS;
using VIENNAAddIn.Utils;
using VIENNAAddIn.constants;

namespace VIENNAAddIn.WSDLGenerator
{
    public partial class TransactionModuleArtefact : Form, GeneratorCallBackInterface
    {
        #region Variables
        private bool withGUI = true;
        private GeneratorCallBackInterface caller = null;
        EA.Repository repository;
        private string scope = "";
        private EA.Element busTransElement;
        private EA.Element initiator;
        private EA.Element responder;
        private EA.Element requestingBusinessActivity;
        private EA.Element respondingBusinessActivity;
        private string path = "";
        private string wsdlPath = "";
        private string schemaColPath = "";
        private string bpelPath = "";
        private string xsltPath = "";
        private string sourceWSDLPath = "";
        private string sourceSchemaColPath = "";
        private string tmPath = "";
        private string transModulePath = "";
        private string busTransPath = "";
        private ArrayList requestingInformationEnvelope = new ArrayList();
        private ArrayList respondingInformationEnvelope = new ArrayList();

        //private string schemaNamespace = "http://www.w3.org/2001/XMLSchema";
        ArrayList variables = new ArrayList();
        private bool blnAnyLevel = false;
        private bool blnCreateSchemaFolder = false;
        private bool blnCreateWSDLFolder = false;
        private bool blnBindingService = false;

        //Initiator Variable
        private string namespace_Transaction = "";
        private string namespace_BusinessTransaction = "";
        private string namespace_BusinessSignal = "";
        private string namespace_TransactionModule = "";
        private string WSDL_TM_FileLocation = "";
        private string WSDL_BT_FileLocation = "";
        private string WSDL_BS_FileLocation = "";
        private string WSDL_TMM_FileLocation = "";

        private string partnerLinkType_RequestingPrivate = "";
        private string partnerLinkType_Public = "";
        private string partnerLinkTypeName_PublicIRLink = "";
        private string partnerLinkTypeName_RequestingPrivateTMLink = "";
        private string portType_RequestingBusinessActivity_PrivateFacing = "";
        private string portType_RequestingBusinessActivity_BA = "";
        private string portType_RequestingBusinessActivity = "";
        private string portType_RespondingBusinessActivity = "";
        private string messagePartName_RequestingMsg = "";
        private string operation_Requesting_RequestingPrivate = "";
        private string operation_Requesting_Public = "";
        private string operation_PositiveResponse_Public = "";
        private string operation_NegativeResponse_Public = "";
        private string operation_PositiveResponse_BA = "";
        private string operation_NegativeResponse_BA = "";
        private string QOS_Requesting_TimeToAckReceipt = "";
        private string QOS_Responding_TimeToAckReceipt = "";
        private string QOS_Requesting_TimeToRespond = "";
        private string QOS_Responding_TimeToRespond = "";
        private string QOS_Requesting_IsIntelligibleCheck = "";
        private string QOS_Responding_IsIntelligibleCheck = "";
        private string xslFilePath_MappingProtocolFailure = "";
        private string xslFilePath_MappingProtocolSuccess = "";
        private string xslFilePath_MappingReceiptAcknowledgement = "";
        private string xslFilePath_MappingReceiptException = "";
        private string xslFilePath_MappingGeneralException = "";

        //Responder Variable
        private string partnerLinkType_RespondingPrivate = "";
        private string portType_RespondingBusinessActivity_PrivateFacing = "";
        private string portType_RespondingBusinessActivity_BA = "";
        private string operation_Requesting_BA = "";
        private string operation_PositiveResponse_RespondingPrivate = "";
        private string operation_NegativeResponse_RespondingPrivate = "";
        private string partnerLinkTypeName_RespondingPrivateTMLink = "";

        //Prefix list
        private string wsdlPrefix = "wsdl";
        private string schemaPrefix = "xs";
        private string targetNamespacePrefix = "tns";
        private string transModulePrefix = "tm";
        private string busTransPrefix = "bt";
        private string partnerLinkPrefix = "plnk";
        private string msgPropertyPrefix = "vprop";
        private string sbdHeaderPrefix = "sbd";
        private string busSignalPrefix = "bs";
        private string soapPrefix = "soap";

        //Namspace list
        private string wsdlNamespace = "http://schemas.xmlsoap.org/wsdl/";
        private string schemaNamespace = "http://www.w3.org/2001/XMLSchema";
        private string partnerLinkNamespace = "http://docs.oasis-open.org/wsbpel/2.0/plnktype";
        private string msgPropertyNamespace = "http://docs.oasis-open.org/wsbpel/2.0/varprop";
        //private string busSignalNamespace = "urn:xml-gov-au:draft:sig:BusinessSignal:0.1";
        private string soapNamespace = "http://schemas.xmlsoap.org/wsdl/soap/";

        private string soapAction = "";
        //Path
        private string sourceBusTrans = "";
        private string sourceTransModule = "";

        private bool transModuleFound = false;
        private int transModulePkgId = 0;
        private bool blnAlias = false;

        #endregion

        #region Constructor
        public TransactionModuleArtefact(EA.Repository repo, string scope, bool blnAnyLevel)
        {
            InitializeComponent();
            

            this.repository = repo;
            this.scope = scope;
            this.blnAnyLevel = blnAnyLevel;

            this.setActivePackageLabel();
        }

        ///for recursive generation
        public TransactionModuleArtefact(EA.Repository repository, string scope, string path, bool bindingService, 
            bool blnAlias, GeneratorCallBackInterface caller)
        {
            this.repository = repository;
            this.scope = scope;
            this.path = path;
            this.blnAnyLevel = false;
            this.blnBindingService = bindingService;
            this.blnAlias = blnAlias;
            this.caller = caller;
        }
        #endregion

        #region Method BPEL Generator
        private void generateBPEL()
        {
            setBPELPath();

            EA.Package pkg = this.repository.GetPackageByID(Int32.Parse(this.scope));

            string fileName = this.wsdlPath + "TM" + removeSpace(pkg.Name) + ".wsdl";

            //find the Transaction Module file on the target directory
            if (!System.IO.File.Exists(fileName))
            {
                throw new Exception("Can't find Transaction Module named TM" + removeSpace(pkg.Name) + ".wsdl" +
                    " in " + this.wsdlPath);
            }
            else
                this.wsdlPath = fileName;

            //get element with stereotype <<BusinessTransaction>>
            foreach (EA.Element e in pkg.Elements)
            {
                if (e.Stereotype.Equals("BusinessTransaction", StringComparison.OrdinalIgnoreCase))
                {
                    this.busTransElement = e;
                    break;
                }
            }

            if (this.busTransElement == null)
            {
                throw new Exception("No element with stereotype <<BusinessTransaction>>. Therefore, there is no WSDL schema to generate.");
            }

            try
            {
                //getInitiatorResponderElement();

                generateInitiatorBPEL();

                generateResponderBPEL();
            }
            catch (Exception ex)
            {
                this.appendErrorMessage(ex.Message, pkg.Name);
            }
        }

        private void copyXSLT()
        {
            string path = WindowsRegistryLoader.getBpelTemplateLocation();
            path += "Mappings";
            CopyDirectory(path, this.xsltPath);
        }

        private void generateResponderBPEL()
        {
            resetVariables();
            setResponderBPELVariable();
            fillHashTableResponder();

            string path = WindowsRegistryLoader.getBpelTemplateLocation();
            //set the default file
            string defaultFile = "";

            string template = determineBPELTemplate();
            if (template == "oneWay")
                defaultFile = path + "TemplateResponderOneWay.bpel";
            else if (template == "twoWay")
                defaultFile = path + "TemplateResponder.bpel";
            else if (template == "special")
                defaultFile = path + "TemplateResponderSpecialCase.bpel";



            string textLine = "";
            string fullText = "";
            TextReader reader = null;
            TextWriter writer = null;
            try
            {
                //read template file
                reader = new StreamReader(defaultFile);
                while ((textLine = reader.ReadLine()) != null)
                {
                    //write to output file
                    fullText += textLine + "\n";
                }
                fullText = replaceValue(fullText);

                EA.Package pkg = this.repository.GetPackageByID(Int32.Parse(this.scope));
                string tempPath = this.bpelPath + "Responder" + removeSpace(pkg.Name) + ".bpel";

                //write BPEL file
                writer = new StreamWriter(tempPath);
                writer.Write(fullText);
                writer.Flush();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (writer != null)
                    writer.Close();
            }
        }

        private void generateInitiatorBPEL()
        {
            resetVariables();
            setInitiatorBPELVariable();
            fillHashTableInitiator();

            string path = WindowsRegistryLoader.getBpelTemplateLocation();
            //set the default file
            string defaultFile = "";

            string template = determineBPELTemplate();
            if (template == "oneWay")
                defaultFile = path + "TemplateInitiatorOneWay.bpel";
            else if (template == "twoWay")
                defaultFile = path + "TemplateInitiator.bpel";
            else if (template == "special")
                defaultFile = path + "TemplateInitiatorSpecialCase.bpel";



            //just for cross checking
            //printHashTable(variables);

            string textLine = "";
            string fullText = "";
            TextReader reader = null;
            TextWriter writer = null;
            try
            {
                //read template file
                reader = new StreamReader(defaultFile);
                while ((textLine = reader.ReadLine()) != null)
                {
                    //write to output file
                    fullText += textLine + "\n";
                }

                //replace the variables
                fullText = replaceValue(fullText);

                EA.Package pkg = this.repository.GetPackageByID(Int32.Parse(this.scope));
                string tempPath = this.bpelPath + "Initiator" + removeSpace(pkg.Name) + ".bpel";

                //write BPEL file
                writer = new StreamWriter(tempPath);
                writer.Write(fullText);
                writer.Flush();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (writer != null)
                    writer.Close();
            }
        }

        private string determineBPELTemplate()
        {
            if (XMLTools.getElementTVValue("businessTransactionType", this.busTransElement).Equals("Notification", StringComparison.OrdinalIgnoreCase) ||
                XMLTools.getElementTVValue("businessTransactionType", this.busTransElement).Equals("InformationDistribution", StringComparison.OrdinalIgnoreCase))
            {
                return "oneWay";
            }
            else if (this.respondingInformationEnvelope.Count == 1)
            {
                EA.Element resElement = ((EA.Element)this.respondingInformationEnvelope[0]);
                if (XMLTools.getElementTVValue("ispositive", resElement).Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                    return "special";
                }
            }
            return "twoWay";
        }

        private string replaceValue(string input)
        {
            StringBuilder sb = new StringBuilder(input);

            foreach (string[] couple in variables)
            {
                string pattern = couple[0];
                string replace = couple[1];

                foreach (Match match in Regex.Matches(input, pattern, RegexOptions.IgnoreCase))
                {
                    sb.Replace(match.Value, replace);
                }
            }

            return sb.ToString();
        }

        private void fillHashTableInitiator()
        {
            //please put the longest variable which has the similar name on the top of the collection
            //to avoid miss-replaced on regex
            string[] var = { 
                "namespace_TransactionModule",
                "namespace_Transaction", "namespace_BusinessTransaction", 
                "namespace_BusinessSignal",  
                "WSDL_TM_FileLocation", "WSDL_BT_FileLocation", 
                "WSDL_BS_FileLocation", "WSDL_TMM_FileLocation",
                "partnerLinkTypeName_RequestingPrivateTMLink",
                "partnerLinkTypeName_PublicIRLink", "partnerLinkType_Public", 
                "partnerLinkType_RequestingPrivate", "portType_RequestingBusinessActivity_PrivateFacing", 
                "portType_RequestingBusinessActivity_BA", "portType_RequestingBusinessActivity",
                "portType_RespondingBusinessActivity", "messagePartName_RequestingMsg", 
                "operation_Requesting_RequestingPrivate", "operation_Requesting_Public", 
                "operation_PositiveResponse_BA", "operation_PositiveResponse_Public",
                "operation_NegativeResponse_BA", "operation_NegativeResponse_Public",
                "QOS_Requesting_TimeToAckReceipt", "QOS_Responding_TimeToAckReceipt",
                "QOS_Requesting_TimeToRespond", "QOS_Requesting_IsIntelligibleCheck",
                "XslFilePath_MappingProtocolFailure","XslFilePath_MappingProtocolSuccess",
                "XslFilePath_MappingReceiptAcknowledgement", "XslFilePath_MappingReceiptException",
                "XslFilePath_MappingGeneralException"
                };

            string[] value = { 
                this.namespace_TransactionModule,
                this.namespace_Transaction, this.namespace_BusinessTransaction, 
                this.namespace_BusinessSignal,  
                this.WSDL_TM_FileLocation, this.WSDL_BT_FileLocation, 
                this.WSDL_BS_FileLocation, this.WSDL_TMM_FileLocation,
                this.partnerLinkTypeName_RequestingPrivateTMLink,
                this.partnerLinkTypeName_PublicIRLink, this.partnerLinkType_Public,
                this.partnerLinkType_RequestingPrivate, this.portType_RequestingBusinessActivity_PrivateFacing, 
                this.portType_RequestingBusinessActivity_BA, this.portType_RequestingBusinessActivity,
                this.portType_RespondingBusinessActivity, this.messagePartName_RequestingMsg, 
                this.operation_Requesting_RequestingPrivate, this.operation_Requesting_Public, 
                this.operation_PositiveResponse_BA, this.operation_PositiveResponse_Public, 
                this.operation_NegativeResponse_BA, this.operation_NegativeResponse_Public,
                this.QOS_Requesting_TimeToAckReceipt, this.QOS_Responding_TimeToAckReceipt,
                this.QOS_Requesting_TimeToRespond, this.QOS_Requesting_IsIntelligibleCheck,
                this.xslFilePath_MappingProtocolFailure, this.xslFilePath_MappingProtocolSuccess,
                this.xslFilePath_MappingReceiptAcknowledgement, this.xslFilePath_MappingReceiptException,
                this.xslFilePath_MappingGeneralException
                };

            for (int count = 0; count <= var.Length - 1; count++)
            {
                string[] temp = new string[2];
                temp[0] = "@@" + var[count];
                temp[1] = value[count];
                variables.Add(temp);
            }

        }

        private void fillHashTableResponder()
        {
            string[] var = { 
                "namespace_TransactionModule",
                "namespace_Transaction", "namespace_BusinessTransaction", 
                "namespace_BusinessSignal",  
                "WSDL_TM_FileLocation", "WSDL_BT_FileLocation", 
                "WSDL_BS_FileLocation", "WSDL_TMM_FileLocation",
                "partnerLinkType_RespondingPrivate",
                "partnerLinkType_Public", "partnerLinkTypeName_PublicIRLink", 
                "portType_RespondingBusinessActivity_PrivateFacing", "partnerLinkTypeName_RespondingPrivateTMLink", 
                "portType_RespondingBusinessActivity_BA", "portType_RequestingBusinessActivity",
                "portType_RespondingBusinessActivity", "operation_Requesting_Public", 
                "operation_Requesting_BA", "operation_PositiveResponse_RespondingPrivate", 
                "operation_NegativeResponse_RespondingPrivate", "messagePartName_RequestingMsg",
                "QOS_Requesting_TimeToAckReceipt", "QOS_Responding_TimeToAckReceipt",
                "QOS_Responding_TimeToRespond", "QOS_Responding_IsIntelligibleCheck",
                "XslFilePath_MappingProtocolFailure","XslFilePath_MappingProtocolSuccess",
                "XslFilePath_MappingReceiptAcknowledgement", "XslFilePath_MappingReceiptException",
                "XslFilePath_MappingGeneralException"
                };

            string[] value = { 
                this.namespace_TransactionModule,
                this.namespace_Transaction, this.namespace_BusinessTransaction, 
                this.namespace_BusinessSignal,  
                this.WSDL_TM_FileLocation, this.WSDL_BT_FileLocation, 
                this.WSDL_BS_FileLocation, this.WSDL_TMM_FileLocation, 
                this.partnerLinkType_RespondingPrivate,
                this.partnerLinkType_Public, this.partnerLinkTypeName_PublicIRLink, 
                this.portType_RespondingBusinessActivity_PrivateFacing, this.partnerLinkTypeName_RespondingPrivateTMLink, 
                this.portType_RespondingBusinessActivity_BA, this.portType_RequestingBusinessActivity,
                this.portType_RespondingBusinessActivity, this.operation_Requesting_Public, 
                this.operation_Requesting_BA, this.operation_PositiveResponse_RespondingPrivate, 
                this.operation_NegativeResponse_RespondingPrivate, this.messagePartName_RequestingMsg,
                this.QOS_Requesting_TimeToAckReceipt, this.QOS_Responding_TimeToAckReceipt,
                this.QOS_Responding_TimeToRespond, this.QOS_Responding_IsIntelligibleCheck,
                this.xslFilePath_MappingProtocolFailure, this.xslFilePath_MappingProtocolSuccess,
                this.xslFilePath_MappingReceiptAcknowledgement, this.xslFilePath_MappingReceiptException,
                this.xslFilePath_MappingGeneralException
                };

            for (int count = 0; count <= var.Length - 1; count++)
            {
                string[] temp = new string[2];
                temp[0] = "@@" + var[count];
                temp[1] = value[count];
                variables.Add(temp);
            }

        }

        private void setInitiatorBPELVariable()
        {
            this.namespace_Transaction = getNamespaceFromWSDLFile(this.wsdlPath);
            this.namespace_BusinessTransaction = this.namespace_Transaction.Replace(":tm", ":bt");
            this.namespace_BusinessSignal = "urn:xml-gov-au:draft:sig:BusinessSignal:0.1";
            this.namespace_TransactionModule = "urn:xml-gov-au:draft:tm:TransactionModule:0.1";

            this.WSDL_TM_FileLocation = "../WSDL/" + System.IO.Path.GetFileName(this.wsdlPath);
            this.WSDL_BT_FileLocation = "../WSDL/" + removeSpace(this.repository.GetPackageByID(Int32.Parse(this.scope)).Name) + ".wsdl";
            this.WSDL_BS_FileLocation = "../WSDL/" + "GIEMBusinessSignal.wsdl";
            this.WSDL_TMM_FileLocation = "../WSDL/" + "GIEMTransactionModuleMessage.wsdl";

            string BTname = removeSpace(this.requestingBusinessActivity.Name);
            //string BTnameResponder = removeSpace(this.respondingBusinessActivity.Name);

            this.partnerLinkType_RequestingPrivate = "RequestingTM" + BTname;
            this.partnerLinkTypeName_RequestingPrivateTMLink = this.partnerLinkType_RequestingPrivate + "TMLink";
            this.partnerLinkType_Public = BTname;//removeSpace(this.respondingBusinessActivity.Name);//removeSpace(this.repository.GetElementByID(((EA.Element)this.requestingInformationEnvelope[0]).ClassifierID).Name);
            this.partnerLinkTypeName_PublicIRLink = this.partnerLinkType_Public + "IRLink";

            this.portType_RequestingBusinessActivity = removeSpace(this.initiator.Name) + removeSpace(this.requestingBusinessActivity.Name);
            this.portType_RespondingBusinessActivity = removeSpace(this.responder.Name) + removeSpace(this.respondingBusinessActivity.Name);

            this.portType_RequestingBusinessActivity_PrivateFacing = "TM" + removeSpace(this.initiator.Name) + removeSpace(this.requestingBusinessActivity.Name);
            this.portType_RequestingBusinessActivity_BA = "BA" + removeSpace(this.initiator.Name) + removeSpace(this.requestingBusinessActivity.Name);

            this.messagePartName_RequestingMsg = this.repository.GetElementByID(((EA.Element)this.requestingInformationEnvelope[0]).ClassifierID).Name; // removeSpace(this.requestingBusinessActivity.Name); //ProposeCreateApplicationTransaction

            this.operation_Requesting_RequestingPrivate = ((EA.Element)this.requestingInformationEnvelope[0]).Name;

            this.operation_Requesting_Public = this.operation_Requesting_RequestingPrivate;

            foreach (EA.Element responder in this.respondingInformationEnvelope)
            {
                if (XMLTools.getElementTVValue("ispositive", responder).Equals("false", StringComparison.OrdinalIgnoreCase))//responder.Name.Equals("accept", StringComparison.OrdinalIgnoreCase))
                {
                    this.operation_NegativeResponse_Public = responder.Name;
                    this.operation_NegativeResponse_BA = responder.Name;
                }
                else
                {
                    this.operation_PositiveResponse_Public = responder.Name;
                    this.operation_PositiveResponse_BA = responder.Name;
                }
            }

            this.QOS_Requesting_IsIntelligibleCheck = getQOSValue("isIntelligibleCheckRequired", this.requestingBusinessActivity);
            this.QOS_Requesting_TimeToAckReceipt = getQOSValue("timeToAcknowledgeReceipt", this.requestingBusinessActivity);
            this.QOS_Requesting_TimeToRespond = getQOSValue("timeToRespond", this.requestingBusinessActivity);
            this.QOS_Responding_TimeToAckReceipt = getQOSValue("timeToAcknowledgeReceipt", this.respondingBusinessActivity);

            this.xslFilePath_MappingGeneralException = "Mappings/" + "MappingGeneralException.xslt";
            this.xslFilePath_MappingProtocolFailure = "Mappings/" + "MappingTransactionProtocolFailure.xslt";
            this.xslFilePath_MappingProtocolSuccess = "Mappings/" + "MappingTransactionProtocolSuccess.xslt";
            this.xslFilePath_MappingReceiptAcknowledgement = "Mappings/" + "MappingReceiptAcknowledgement.xslt";
            this.xslFilePath_MappingReceiptException = "Mappings/" + "MappingReceiptException.xslt";
        }

        private void setResponderBPELVariable()
        {
            this.namespace_Transaction = getNamespaceFromWSDLFile(this.wsdlPath);
            this.namespace_BusinessTransaction = this.namespace_Transaction.Replace(":tm", ":bt");
            this.namespace_BusinessSignal = "urn:xml-gov-au:draft:sig:BusinessSignal:0.1";
            this.namespace_TransactionModule = "urn:xml-gov-au:draft:tm:TransactionModule:0.1";

            this.WSDL_TM_FileLocation = "../WSDL/" + System.IO.Path.GetFileName(this.wsdlPath);
            this.WSDL_BT_FileLocation = "../WSDL/" + removeSpace(this.repository.GetPackageByID(Int32.Parse(this.scope)).Name) + ".wsdl";
            this.WSDL_BS_FileLocation = "../WSDL/" + "GIEMBusinessSignal.wsdl";
            this.WSDL_TMM_FileLocation = "../WSDL/" + "GIEMTransactionModuleMessage.wsdl";

            string BTname = removeSpace(this.requestingBusinessActivity.Name);
            string BTnameResponder = removeSpace(this.respondingBusinessActivity.Name);

            this.partnerLinkType_RespondingPrivate = "RespondingTM" + BTnameResponder;
            this.partnerLinkType_Public = BTname;
            this.partnerLinkTypeName_PublicIRLink = this.partnerLinkType_Public + "IRLink";
            this.partnerLinkTypeName_RespondingPrivateTMLink = this.partnerLinkType_RespondingPrivate + "TMLink";

            this.portType_RespondingBusinessActivity_PrivateFacing = "TM" + removeSpace(this.responder.Name) + removeSpace(this.respondingBusinessActivity.Name);
            this.portType_RespondingBusinessActivity_BA = "BA" + removeSpace(this.responder.Name) + removeSpace(this.respondingBusinessActivity.Name);
            this.portType_RequestingBusinessActivity = removeSpace(this.initiator.Name) + removeSpace(this.requestingBusinessActivity.Name);
            this.portType_RespondingBusinessActivity = removeSpace(this.responder.Name) + removeSpace(this.respondingBusinessActivity.Name);
            this.operation_Requesting_Public = ((EA.Element)this.requestingInformationEnvelope[0]).Name;
            this.operation_Requesting_BA = ((EA.Element)this.requestingInformationEnvelope[0]).Name;

            foreach (EA.Element responder in this.respondingInformationEnvelope)
            {
                if (XMLTools.getElementTVValue("ispositive", responder).Equals("false", StringComparison.OrdinalIgnoreCase))//responder.Name.Equals("accept", StringComparison.OrdinalIgnoreCase))
                {
                    this.operation_NegativeResponse_RespondingPrivate = responder.Name;
                }
                else
                {
                    this.operation_PositiveResponse_RespondingPrivate = responder.Name;
                }
            }

            this.messagePartName_RequestingMsg = this.repository.GetElementByID(((EA.Element)this.requestingInformationEnvelope[0]).ClassifierID).Name; //+  removeSpace(this.respondingBusinessActivity.Name); //ProposeCreateApplicationTransaction

            this.QOS_Responding_IsIntelligibleCheck = getQOSValue("isIntelligibleCheckRequired", this.respondingBusinessActivity);
            this.QOS_Requesting_TimeToAckReceipt = getQOSValue("timeToAcknowledgeReceipt", this.respondingBusinessActivity);
            this.QOS_Responding_TimeToAckReceipt = getQOSValue("timeToAcknowledgeReceipt", this.requestingBusinessActivity);
            this.QOS_Responding_TimeToRespond = getQOSValue("timeToRespond", this.requestingBusinessActivity);

            this.xslFilePath_MappingGeneralException = "Mappings/" + "MappingGeneralException.xslt";
            this.xslFilePath_MappingProtocolFailure = "Mappings/" + "MappingTransactionProtocolFailure.xslt";
            this.xslFilePath_MappingProtocolSuccess = "Mappings/" + "MappingTransactionProtocolSuccess.xslt";
            this.xslFilePath_MappingReceiptAcknowledgement = "Mappings/" + "MappingReceiptAcknowledgement.xslt";
            this.xslFilePath_MappingReceiptException = "Mappings/" + "MappingReceiptException.xslt";
        }

        private string getQOSValue(string taggedValue, EA.Element element)
        {
            string tempTV = XMLTools.getElementTVValue(taggedValue, element);
            if (tempTV == "")
                return "PT0H";

            return tempTV;
        }

        

        private bool checkSchemaFolderExist()
        {
            if (Directory.Exists(this.path + "schemas"))
                return true;
            return false;
        }

        private bool checkWSDLFolderExist()
        {
            if (Directory.Exists(this.path + "wsdl"))
                return true;
            return false;
        }

        private void EnableBrowseButton(bool blnEnabled)
        {
            this.btnSchemaLocation.Enabled = blnEnabled;
            this.btnWSDLLocation.Enabled = blnEnabled;
        }

        public void clearInput()
        {
            this.txtSchemaLocation.Text = "";
            this.txtSavingPath.Text = "";
            this.txtWSDLLocation.Text = "";
            this.statusTextBox.Text = "";
            this.progressBar1.Value = progressBar1.Minimum;

            this.blnCreateSchemaFolder = false;
            this.blnCreateWSDLFolder = false;
        }

        public void resetGenerator(String scope)
        {
            this.scope = scope;
            this.progressBar1.Value = this.progressBar1.Minimum;
            this.statusTextBox.Text = "";
            this.setActivePackageLabel();

            this.requestingInformationEnvelope = new ArrayList();
            this.respondingInformationEnvelope = new ArrayList();
        }

        private void resetVariables()
        {
            this.variables = new ArrayList();

            namespace_Transaction = "";
            namespace_BusinessTransaction = "";
            namespace_BusinessSignal = "";
            namespace_TransactionModule = "";
            WSDL_TM_FileLocation = "";
            WSDL_BT_FileLocation = "";
            WSDL_BS_FileLocation = "";
            WSDL_TMM_FileLocation = "";

            partnerLinkType_RequestingPrivate = "";
            partnerLinkType_Public = "";
            partnerLinkTypeName_PublicIRLink = "";
            partnerLinkTypeName_RequestingPrivateTMLink = "";
            portType_RequestingBusinessActivity_PrivateFacing = "";
            portType_RequestingBusinessActivity_BA = "";
            portType_RequestingBusinessActivity = "";
            portType_RespondingBusinessActivity = "";
            messagePartName_RequestingMsg = "";
            operation_Requesting_RequestingPrivate = "";
            operation_Requesting_Public = "";
            operation_PositiveResponse_Public = "";
            operation_NegativeResponse_Public = "";
            operation_PositiveResponse_BA = "";
            operation_NegativeResponse_BA = "";
            QOS_Requesting_TimeToAckReceipt = "";
            QOS_Responding_TimeToAckReceipt = "";
            QOS_Requesting_TimeToRespond = "";
            QOS_Responding_TimeToRespond = "";
            QOS_Requesting_IsIntelligibleCheck = "";
            QOS_Responding_IsIntelligibleCheck = "";

            xslFilePath_MappingGeneralException = "";
            xslFilePath_MappingProtocolFailure = "";
            xslFilePath_MappingProtocolSuccess = "";
            xslFilePath_MappingReceiptAcknowledgement = "";
            xslFilePath_MappingReceiptException = "";

            //Responder Variable
            partnerLinkType_RespondingPrivate = "";
            portType_RespondingBusinessActivity_PrivateFacing = "";
            portType_RespondingBusinessActivity_BA = "";
            operation_Requesting_BA = "";
            operation_PositiveResponse_RespondingPrivate = "";
            operation_NegativeResponse_RespondingPrivate = "";
            partnerLinkTypeName_RespondingPrivateTMLink = "";
        }

        private void setActivePackageLabel()
        {
            EA.Package pkg = this.repository.GetPackageByID(Int32.Parse(this.scope));
            this.selectedBusinessTransaction.Text = pkg.Name + "<<" + pkg.Element.Stereotype.ToString() + ">>";
        }

        private void CopyWSDLFolder()
        {
            if (this.sourceWSDLPath != this.wsdlPath)
                //Copy the schema collection from the source;
                CopyDirectory(this.sourceWSDLPath, this.wsdlPath);
        }

        private void CopySchemaCollectionFolder()
        {
            if (this.sourceSchemaColPath != this.schemaColPath)
                //Copy the schema collection from the source;
                CopyDirectory(this.sourceSchemaColPath, this.schemaColPath);
        }

        

        #endregion

        #region Button & Event

        private void btnGenerateTMArtefact_Click(object sender, EventArgs e)
        {
            resetGenerator(scope);
            getCheckedOption(); 

            setBPELPath(); //for BPEL
            setPath(); //for TM WSDL

            EA.Package pkg = this.repository.GetPackageByID(Int32.Parse(this.scope));

            //check for input
            if (txtSavingPath.Text == "")
            {
                this.appendErrorMessage("Target directory must not be empty. Please choose the directory where to save generated WSDL", pkg.Name);
                return;
            }

            if (this.blnCreateSchemaFolder && this.blnCreateWSDLFolder)
            {
                #region Create structure
                //Check if schema path is not empty
                if (txtSchemaLocation.Text.Trim() == "")
                {
                    this.appendErrorMessage("Schema Location can not be empty, because it's the first time generator running on selected folder.", pkg.Name);
                    return;
                }

                //Check if WSDL path is empty
                if (txtWSDLLocation.Text == "")
                {
                    this.appendErrorMessage("WSDL Location can not be empty, because it's the first time generator running on selected folder.", pkg.Name);
                    return;
                }

                try
                {
                    //create folder structure
                    Directory.CreateDirectory(this.schemaColPath);
                    Directory.CreateDirectory(this.wsdlPath);

                    CopyWSDLFolder();

                    //Check GIEMTransactionModuleMessage.wsdl path of "include" tag
                    CheckAndResetTransactionModule();

                    CopySchemaCollectionFolder();
                }
                catch (Exception ex)
                {
                    this.appendErrorMessage(ex.Message, pkg.Name);
                    return;
                }
                #endregion
            }
            else if (this.blnCreateSchemaFolder)
            {
                #region Create Schema Folder
                if (txtSchemaLocation.Text.Trim() == "")
                {
                    this.appendErrorMessage("Schema Location can not be empty, because it's the first time generator running on selected folder.", pkg.Name);
                    return;
                }

                try
                {
                    //create folder structure
                    Directory.CreateDirectory(this.schemaColPath);

                    CopySchemaCollectionFolder();
                }
                catch (Exception ex)
                {
                    this.appendErrorMessage(ex.Message, pkg.Name);
                    return;
                }

                #endregion
            }
            else if (this.blnCreateWSDLFolder)
            {
                #region Create WSDL Folder
                if (txtWSDLLocation.Text == "")
                {
                    this.appendErrorMessage("WSDL Location can not be empty, because it's the first time generator running on selected folder.", pkg.Name);
                    return;
                }
                try
                {
                    //create folder structure
                    Directory.CreateDirectory(this.wsdlPath);

                    CopyWSDLFolder();
                }
                catch (Exception ex)
                {
                    this.appendErrorMessage(ex.Message, pkg.Name);
                    return;
                }
                #endregion
            }

            Directory.CreateDirectory(this.bpelPath);
            Directory.CreateDirectory(this.xsltPath);
            copyXSLT();

            this.appendInfoMessage("Start generating Transaction Module Artefact", pkg.Name);
            this.performProgressStep();

            if (!blnAnyLevel)
            {
                try
                {
                    generateArtefact();
                }
                catch (Exception ex)
                {
                    this.appendErrorMessage("Generator encounter an error caused by :" + ex.Message, pkg.Name);
                    this.progressBar1.Value = this.progressBar1.Maximum;
                    return;
                }
                this.progressBar1.Value = this.progressBar1.Maximum;
                this.appendInfoMessage("Finished generating Transaction Module Artefact.", pkg.Name);
            }
            else
            {
                try
                {
                    DoRecursivePackageSearch(pkg);
                }
                catch (Exception exception)
                {
                    this.appendErrorMessage("Generator encounter an error caused by :" + exception.Message, pkg.Name);
                    this.progressBar1.Value = this.progressBar1.Maximum;
                    return;
                }

                this.appendInfoMessage("Finished generating Transaction Module Artefact.", pkg.Name);
                this.progressBar1.Value = this.progressBar1.Maximum;
            }
        }

        private void btnSchemaLocation_Click(object sender, EventArgs e)
        {
            DialogResult result = dlgSchemaLocation.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtSchemaLocation.Text = dlgSchemaLocation.SelectedPath;
                this.sourceSchemaColPath = txtSchemaLocation.Text;
            }
        }

        private void btnWSDLLocation_Click(object sender, EventArgs e)
        {
            DialogResult result = dlgWSDLLocation.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtWSDLLocation.Text = dlgWSDLLocation.SelectedPath;
                this.sourceWSDLPath = dlgWSDLLocation.SelectedPath;
            }
        }

        private void btnSavingPath_Click(object sender, EventArgs e)
        {
            DialogResult result = dlgSavePath.ShowDialog();

            if (result == DialogResult.OK)
            {
                clearInput();
                EnableBrowseButton(false);

                this.path = dlgSavePath.SelectedPath + @"\";
                txtSavingPath.Text = dlgSavePath.SelectedPath;

                if (!checkSchemaFolderExist() && !checkWSDLFolderExist())
                {
                    MessageBox.Show("This is the first time WSDL generator running on selected folder.\n " +
                        "You must specify the WSDL file location and schema collection location.", "Message", MessageBoxButtons.OK);

                    EnableBrowseButton(true);
                    this.blnCreateSchemaFolder = true;
                    this.blnCreateWSDLFolder = true;
                }
                else if (!checkSchemaFolderExist())
                {
                    MessageBox.Show("Schema folder doesn't exist on selected folder." +
                        "You must specify the schema collection location.", "Message", MessageBoxButtons.OK);

                    this.btnSchemaLocation.Enabled = true;
                    this.blnCreateSchemaFolder = true;
                }
                else if (!checkWSDLFolderExist())
                {
                    MessageBox.Show("WSDL folder doesn't exist on selected folder." +
                        "You must specify the WSDL location.", "Message", MessageBoxButtons.OK);

                    this.btnWSDLLocation.Enabled = true;
                    this.blnCreateWSDLFolder = true;
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chkBindingService_CheckStateChanged(object sender, EventArgs e)
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

        private String getPackageName()
        {
            return this.repository.GetPackageByID(Int32.Parse(this.scope)).Name;
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
                        TransactionModuleArtefact tmArtefact = new TransactionModuleArtefact(repository, scp, this.path, this.blnBindingService, this.blnAlias, getCaller());
                        tmArtefact.generateArtefact();
                    }
                    catch (Exception exception)
                    {
                        this.appendErrorMessage(exception.Message, p.Name);
                    }
                }
            }
        }

        #endregion

        #region Method Transaction Module Generator
        private void generateTransactionModule()
        {
            EA.Package pkg = this.repository.GetPackageByID(Int32.Parse(this.scope));

            this.sourceTransModule = this.path + @"WSDL\GIEMTransactionModuleMessage.wsdl";
            this.sourceBusTrans = this.path + @"WSDL\" + removeSpace(pkg.Name) + ".wsdl";
            
            setPath();

            //Check for existence of {Business Transaction Name}.wsdl
            if (!System.IO.File.Exists(this.busTransPath)) //pathBusTrans))
            {
                string name = removeSpace(pkg.Name) + ".wsdl";
                throw new Exception("Business Transaction " + name + " doesn't exist in folder " +
                    this.wsdlPath + ". Please make sure the file exists.\nSkipped.");
            }

            String schemaName = "";
            schemaName = XMLTools.getSchemaNameWithoutExtention(this.repository.GetPackageByID(pkg.PackageID));
            String filename = this.wsdlPath + this.transModulePrefix.ToUpper() + schemaName + ".wsdl";
            XmlTextWriter w = new XmlTextWriter(filename, Encoding.UTF8);

            try
            {
                //getInitiatorResponderElement();

                generateInitialSchema(w);

                addTransactionModuleImports(w);

                generatePortType(w);

                if (this.blnBindingService)
                {
                    //Generate Binding
                    generateBinding(w);

                    //Generate Service
                    generateService(w);
                }

                //Generate Partner Link
                generatePartnerLink(w);

                //generate message property
                generateMessageProperty(w);
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

        private void generateService(XmlTextWriter w)
        {
            string serviceName = "";

            #region TM Responder Service
            serviceName = "TM" + this.responder.Name + removeSpace(this.respondingBusinessActivity.Name);//removeSpace(this.repository.GetPackageByID(Int32.Parse(this.scope)).Name);

            w.WriteStartElement(this.wsdlPrefix, "service", this.wsdlNamespace);
            w.WriteAttributeString("name", serviceName);

            w.WriteStartElement(this.wsdlPrefix, "port", this.wsdlNamespace);
            w.WriteAttributeString("name", serviceName);
            w.WriteAttributeString("binding", this.targetNamespacePrefix + ":" + serviceName + "SOAP");

            w.WriteStartElement(this.soapPrefix, "address", this.soapNamespace);
            w.WriteAttributeString("location", "[myEndPointReference]" + @"/" + serviceName);
            w.WriteAttributeString("xmlns", this.soapPrefix, null, this.soapNamespace);
            w.WriteEndElement();
            w.WriteEndElement();

            w.WriteEndElement();
            #endregion

            #region TM Initiator Service
            serviceName = "TM" + this.initiator.Name + removeSpace(this.requestingBusinessActivity.Name);//removeSpace(this.repository.GetPackageByID(Int32.Parse(this.scope)).Name);

            w.WriteStartElement(this.wsdlPrefix, "service", this.wsdlNamespace);
            w.WriteAttributeString("name", serviceName);

            w.WriteStartElement(this.wsdlPrefix, "port", this.wsdlNamespace);
            w.WriteAttributeString("name", serviceName);
            w.WriteAttributeString("binding", this.targetNamespacePrefix + ":" + serviceName + "SOAP");

            w.WriteStartElement(this.soapPrefix, "address", this.soapNamespace);
            w.WriteAttributeString("location", "[myEndPointReference]" + @"/" + serviceName);
            w.WriteAttributeString("xmlns", this.soapPrefix, null, this.soapNamespace);
            w.WriteEndElement();
            w.WriteEndElement();

            w.WriteEndElement();
            #endregion

            #region BA Responder Service
            serviceName = "BA" + this.responder.Name + removeSpace(this.respondingBusinessActivity.Name);//removeSpace(this.repository.GetPackageByID(Int32.Parse(this.scope)).Name);

            w.WriteStartElement(this.wsdlPrefix, "service", this.wsdlNamespace);
            w.WriteAttributeString("name", serviceName);

            w.WriteStartElement(this.wsdlPrefix, "port", this.wsdlNamespace);
            w.WriteAttributeString("name", serviceName);
            w.WriteAttributeString("binding", this.targetNamespacePrefix + ":" + serviceName + "SOAP");

            w.WriteStartElement(this.soapPrefix, "address", this.soapNamespace);
            w.WriteAttributeString("location", "[myEndPointReference]" + @"/" + serviceName);
            w.WriteAttributeString("xmlns", this.soapPrefix, null, this.soapNamespace);
            w.WriteEndElement();
            w.WriteEndElement();

            w.WriteEndElement();
            #endregion

            #region BA Initiator Service
            serviceName = "BA" + this.initiator.Name + removeSpace(this.requestingBusinessActivity.Name);//removeSpace(this.repository.GetPackageByID(Int32.Parse(this.scope)).Name);

            w.WriteStartElement(this.wsdlPrefix, "service", this.wsdlNamespace);
            w.WriteAttributeString("name", serviceName);

            w.WriteStartElement(this.wsdlPrefix, "port", this.wsdlNamespace);
            w.WriteAttributeString("name", serviceName);
            w.WriteAttributeString("binding", this.targetNamespacePrefix + ":" + serviceName + "SOAP");

            w.WriteStartElement(this.soapPrefix, "address", this.soapNamespace);
            w.WriteAttributeString("location", "[myEndPointReference]" + @"/" + serviceName);
            w.WriteAttributeString("xmlns", this.soapPrefix, null, this.soapNamespace);
            w.WriteEndElement();
            w.WriteEndElement();

            w.WriteEndElement();
            #endregion
        }

        private void generateMessageProperty(XmlTextWriter w)
        {
            w.WriteStartElement("property", this.msgPropertyNamespace);
            //w.WriteAttributeString("xmlns", this.partnerLinkPrefix, null, this.partnerLinkNamespace);
            w.WriteAttributeString("xmlns", this.msgPropertyPrefix, null, this.msgPropertyNamespace);
            w.WriteAttributeString("name", "messageID");
            w.WriteAttributeString("type", this.schemaPrefix + ":" + "string");
            w.WriteEndElement();

            #region Requesting
            foreach (EA.Element requesting in this.requestingInformationEnvelope)
            {
                EA.Element classifierElement;
                try
                {
                    classifierElement = this.repository.GetElementByID(requesting.ClassifierID);
                }
                catch
                {
                    throw new Exception("Failed getting classifier of element " + responder.Name);
                }

                w.WriteStartElement("propertyAlias", this.msgPropertyNamespace);
                w.WriteAttributeString("xmlns", this.msgPropertyPrefix, null, this.msgPropertyNamespace);
                w.WriteAttributeString("messageType", this.busTransPrefix + ":" + "RequestMsg");//requesting.Name);
                w.WriteAttributeString("part", removeSpace(classifierElement.Name));
                w.WriteAttributeString("propertyName", this.targetNamespacePrefix + ":" + "messageID");

                w.WriteStartElement("query", this.msgPropertyNamespace);
                string value1 = this.busTransPrefix + @":StandardBusinessMessageHeader/" + this.sbdHeaderPrefix
                    + @":BusinessScope/" + this.sbdHeaderPrefix + ":InstanceIdentifier";
                w.WriteValue(value1);
                w.WriteEndElement();

                w.WriteEndElement();
            }
            #endregion

            #region Responding
            foreach (EA.Element responding in this.respondingInformationEnvelope)
            {
                EA.Element classifierElement;
                try
                {
                    classifierElement = this.repository.GetElementByID(responding.ClassifierID);
                }
                catch
                {
                    throw new Exception("Failed getting classifier of element " + responder.Name);
                }

                w.WriteStartElement("propertyAlias", this.msgPropertyNamespace);
                w.WriteAttributeString("xmlns", this.msgPropertyPrefix, null, this.msgPropertyNamespace);

                string abstractName = "PositiveResponseMsg"; //give default value to RespondingInformationEnvelope
                if (XMLTools.getElementTVValue("ispositive", responding).Equals("false", StringComparison.OrdinalIgnoreCase))
                {
                    abstractName = "NegativeResponseMsg";
                }

                w.WriteAttributeString("messageType", this.busTransPrefix + ":" + abstractName);//responding.Name);
                w.WriteAttributeString("part", removeSpace(classifierElement.Name));
                w.WriteAttributeString("propertyName", this.targetNamespacePrefix + ":" + "messageID");

                w.WriteStartElement("query", this.msgPropertyNamespace);
                string value2 = this.busTransPrefix + @":StandardBusinessMessageHeader/" + this.sbdHeaderPrefix
                    + @":BusinessScope/" + this.sbdHeaderPrefix + ":InstanceIdentifier";
                w.WriteValue(value2);
                w.WriteEndElement();

                w.WriteEndElement();
            }
            #endregion

            string value = "";

            #region Transaction Protocol Failure Msg
            w.WriteStartElement("propertyAlias", this.msgPropertyNamespace);
            w.WriteAttributeString("xmlns", this.msgPropertyPrefix, null, this.msgPropertyNamespace);
            w.WriteAttributeString("messageType", this.transModulePrefix + ":" + "TransactionProtocolFailureMsg");
            w.WriteAttributeString("part", "TransactionProtocolFailure");
            w.WriteAttributeString("propertyName", this.targetNamespacePrefix + ":" + "messageID");

            w.WriteStartElement("query", this.msgPropertyNamespace);
            value = this.transModulePrefix + @":StandardBusinessMessageHeader/" + this.sbdHeaderPrefix
                    + @":BusinessScope/" + this.sbdHeaderPrefix + ":InstanceIdentifier";
            w.WriteValue(value);
            w.WriteEndElement();

            w.WriteEndElement();
            #endregion

            #region Transaction Protocol Success Msg
            w.WriteStartElement("propertyAlias", this.msgPropertyNamespace);
            w.WriteAttributeString("xmlns", this.msgPropertyPrefix, null, this.msgPropertyNamespace);
            w.WriteAttributeString("messageType", this.transModulePrefix + ":" + "TransactionProtocolSuccessMsg");
            w.WriteAttributeString("part", "TransactionProtocolSuccess");
            w.WriteAttributeString("propertyName", this.targetNamespacePrefix + ":" + "messageID");

            w.WriteStartElement("query", this.msgPropertyNamespace);
            w.WriteValue(value);
            w.WriteEndElement();

            w.WriteEndElement();
            #endregion

            #region General Exception Signal
            w.WriteStartElement("propertyAlias", this.msgPropertyNamespace);
            w.WriteAttributeString("xmlns", this.msgPropertyPrefix, null, this.msgPropertyNamespace);
            w.WriteAttributeString("messageType", this.busSignalPrefix + ":" + "GeneralExceptionSignal");
            w.WriteAttributeString("part", "GeneralException");
            w.WriteAttributeString("propertyName", this.targetNamespacePrefix + ":" + "messageID");

            w.WriteStartElement("query", this.msgPropertyNamespace);
            value = this.busSignalPrefix + @":StandardBusinessMessageHeader/" + this.sbdHeaderPrefix
                    + @":BusinessScope/" + this.sbdHeaderPrefix + ":InstanceIdentifier";
            w.WriteValue(value);
            w.WriteEndElement();

            w.WriteEndElement();
            #endregion

            #region Receipt Acknowledgement Signal
            w.WriteStartElement("propertyAlias", this.msgPropertyNamespace);
            w.WriteAttributeString("xmlns", this.msgPropertyPrefix, null, this.msgPropertyNamespace);
            w.WriteAttributeString("messageType", this.busSignalPrefix + ":" + "ReceiptAcknowledgementSignal");
            w.WriteAttributeString("part", "ReceiptAcknowledgement");
            w.WriteAttributeString("propertyName", this.targetNamespacePrefix + ":" + "messageID");

            w.WriteStartElement("query", this.msgPropertyNamespace);
            w.WriteValue(value);
            w.WriteEndElement();

            w.WriteEndElement();
            #endregion

            #region Receipt Exception Signal
            w.WriteStartElement("propertyAlias", this.msgPropertyNamespace);
            w.WriteAttributeString("xmlns", this.msgPropertyPrefix, null, this.msgPropertyNamespace);
            w.WriteAttributeString("messageType", this.busSignalPrefix + ":" + "ReceiptExceptionSignal");
            w.WriteAttributeString("part", "ReceiptException");
            w.WriteAttributeString("propertyName", this.targetNamespacePrefix + ":" + "messageID");

            w.WriteStartElement("query", this.msgPropertyNamespace);
            w.WriteValue(value);
            w.WriteEndElement();

            w.WriteEndElement();
            #endregion
        }

        private void generatePartnerLink(XmlTextWriter w)
        {
            string BTname = removeSpace(this.requestingBusinessActivity.Name);//removeSpace(this.repository.GetPackageByID(Int32.Parse(this.scope)).Name);
            string BTnameResponder = removeSpace(this.respondingBusinessActivity.Name);
            string temp = "";

            #region Partnet Link : Business Application of Requesting Authorisation Role
            w.WriteStartElement("partnerLinkType", this.partnerLinkNamespace);
            w.WriteAttributeString("xmlns", this.partnerLinkPrefix, null, this.partnerLinkNamespace);
            w.WriteAttributeString("name", "RequestingTM" + BTname); //put the name here

            //BA Initiator
            w.WriteStartElement("role", this.partnerLinkNamespace);
            temp = "BA" + this.initiator.Name + BTname;
            w.WriteAttributeString("name", temp);
            w.WriteAttributeString("portType", this.targetNamespacePrefix + ":" + temp);
            w.WriteEndElement();

            //TM Initiator
            w.WriteStartElement("role", this.partnerLinkNamespace);
            temp = "TM" + this.initiator.Name + BTname;
            w.WriteAttributeString("name", temp);
            w.WriteAttributeString("portType", this.targetNamespacePrefix + ":" + temp);
            w.WriteEndElement();

            w.WriteEndElement();
            #endregion

            #region Partnet Link : Public Process of Requesting Authorisation Role
            w.WriteStartElement("partnerLinkType", this.partnerLinkNamespace);
            w.WriteAttributeString("xmlns", this.partnerLinkPrefix, null, this.partnerLinkNamespace);
            w.WriteAttributeString("name", BTname);

            //BA Initiator
            w.WriteStartElement("role", this.partnerLinkNamespace);
            temp = this.responder.Name + BTnameResponder;
            w.WriteAttributeString("name", temp);
            w.WriteAttributeString("portType", this.busTransPrefix + ":" + temp);
            w.WriteEndElement();

            //TM Initiator
            w.WriteStartElement("role", this.partnerLinkNamespace);
            temp = this.initiator.Name + BTname;
            w.WriteAttributeString("name", temp);
            w.WriteAttributeString("portType", this.busTransPrefix + ":" + temp);
            w.WriteEndElement();

            w.WriteEndElement();
            #endregion

            #region Partnet Link : Business Application of Responding Authorisation Role

            w.WriteStartElement("partnerLinkType", this.partnerLinkNamespace);
            w.WriteAttributeString("xmlns", this.partnerLinkPrefix, null, this.partnerLinkNamespace);
            w.WriteAttributeString("name", "RespondingTM" + BTnameResponder); //put the name here

            //BA Responder
            w.WriteStartElement("role", this.partnerLinkNamespace);
            temp = "BA" + this.responder.Name + BTnameResponder;
            w.WriteAttributeString("name", temp);
            w.WriteAttributeString("portType", this.targetNamespacePrefix + ":" + temp);
            w.WriteEndElement();

            //TM Responder
            w.WriteStartElement("role", this.partnerLinkNamespace);
            temp = "TM" + this.responder.Name + BTnameResponder;
            w.WriteAttributeString("name", temp);
            w.WriteAttributeString("portType", this.targetNamespacePrefix + ":" + temp);
            w.WriteEndElement();

            w.WriteEndElement();
            #endregion
        }

        private void generateBinding(XmlTextWriter w)
        {
            string bindingName = "";
            #region ~~ TM Responder Binding~~
            bindingName = "TM" + this.responder.Name + removeSpace(this.respondingBusinessActivity.Name);//removeSpace(this.repository.GetPackageByID(Int32.Parse(this.scope)).Name);

            w.WriteStartElement(this.wsdlPrefix, "binding", this.wsdlNamespace);
            w.WriteAttributeString("name", bindingName + "SOAP");
            w.WriteAttributeString("type", this.targetNamespacePrefix + ":" + bindingName);

            w.WriteStartElement(this.soapPrefix, "binding", this.soapNamespace);
            w.WriteAttributeString("style", "document");
            w.WriteAttributeString("transport", "http://schemas.xmlsoap.org/soap/http");
            w.WriteAttributeString("xmlns", this.soapPrefix, null, this.soapNamespace);
            w.WriteEndElement();

            foreach (EA.Element el in this.respondingInformationEnvelope)
            {
                w.WriteStartElement(this.wsdlPrefix, "operation", this.wsdlNamespace);
                w.WriteAttributeString("name", el.Name);

                w.WriteStartElement(this.soapPrefix, "operation", this.soapNamespace);
                w.WriteAttributeString("soapAction", this.soapAction);
                w.WriteAttributeString("style", "document");
                w.WriteAttributeString("xmlns", this.soapPrefix, null, this.soapNamespace);
                w.WriteEndElement();

                //input
                w.WriteStartElement(this.wsdlPrefix, "input", this.wsdlNamespace);
                w.WriteStartElement(this.soapPrefix, "body", this.soapNamespace);
                w.WriteAttributeString("use", "literal");
                w.WriteAttributeString("xmlns", this.soapPrefix, null, this.soapNamespace);
                w.WriteEndElement();
                w.WriteEndElement();

                //output
                w.WriteStartElement(this.wsdlPrefix, "output", this.wsdlNamespace);
                w.WriteStartElement(this.soapPrefix, "body", this.soapNamespace);
                w.WriteAttributeString("use", "literal");
                w.WriteAttributeString("xmlns", this.soapPrefix, null, this.soapNamespace);
                w.WriteEndElement();
                w.WriteEndElement();

                //fault
                w.WriteStartElement(this.wsdlPrefix, "fault", this.wsdlNamespace);
                w.WriteAttributeString("name", "ExceptionResponse");
                w.WriteEndElement();

                w.WriteEndElement();
            }

            w.WriteEndElement();
            #endregion

            #region ~~TM Initiator Binding~~
            bindingName = "TM" + this.initiator.Name + removeSpace(this.requestingBusinessActivity.Name);//removeSpace(this.repository.GetPackageByID(Int32.Parse(this.scope)).Name);

            w.WriteStartElement(this.wsdlPrefix, "binding", this.wsdlNamespace);
            w.WriteAttributeString("name", bindingName + "SOAP");
            w.WriteAttributeString("type", this.targetNamespacePrefix + ":" + bindingName);

            w.WriteStartElement(this.soapPrefix, "binding", this.soapNamespace);
            w.WriteAttributeString("style", "document");
            w.WriteAttributeString("transport", "http://schemas.xmlsoap.org/soap/http");
            w.WriteAttributeString("xmlns", this.soapPrefix, null, this.soapNamespace);
            w.WriteEndElement();

            foreach (EA.Element el in this.requestingInformationEnvelope)
            {
                w.WriteStartElement(this.wsdlPrefix, "operation", this.wsdlNamespace);
                w.WriteAttributeString("name", el.Name);

                w.WriteStartElement(this.soapPrefix, "operation", this.soapNamespace);
                w.WriteAttributeString("soapAction", this.soapAction);
                w.WriteAttributeString("style", "document");
                w.WriteAttributeString("xmlns", this.soapPrefix, null, this.soapNamespace);
                w.WriteEndElement();

                //input
                w.WriteStartElement(this.wsdlPrefix, "input", this.wsdlNamespace);
                w.WriteStartElement(this.soapPrefix, "body", this.soapNamespace);
                w.WriteAttributeString("use", "literal");
                w.WriteAttributeString("xmlns", this.soapPrefix, null, this.soapNamespace);
                w.WriteEndElement();
                w.WriteEndElement();

                //output
                w.WriteStartElement(this.wsdlPrefix, "output", this.wsdlNamespace);
                w.WriteStartElement(this.soapPrefix, "body", this.soapNamespace);
                w.WriteAttributeString("use", "literal");
                w.WriteAttributeString("xmlns", this.soapPrefix, null, this.soapNamespace);
                w.WriteEndElement();
                w.WriteEndElement();

                //fault
                w.WriteStartElement(this.wsdlPrefix, "fault", this.wsdlNamespace);
                w.WriteAttributeString("name", "ExceptionResponse");
                w.WriteEndElement();

                w.WriteEndElement();
            }

            w.WriteEndElement();
            #endregion

            #region ~~BA Responder Binding~~
            bindingName = "BA" + this.responder.Name + removeSpace(this.respondingBusinessActivity.Name);//removeSpace(this.repository.GetPackageByID(Int32.Parse(this.scope)).Name);

            w.WriteStartElement(this.wsdlPrefix, "binding", this.wsdlNamespace);
            w.WriteAttributeString("name", bindingName + "SOAP");
            w.WriteAttributeString("type", this.targetNamespacePrefix + ":" + bindingName);

            w.WriteStartElement(this.soapPrefix, "binding", this.soapNamespace);
            w.WriteAttributeString("style", "document");
            w.WriteAttributeString("transport", "http://schemas.xmlsoap.org/soap/http");
            w.WriteAttributeString("xmlns", this.soapPrefix, null, this.soapNamespace);
            w.WriteEndElement();

            foreach (EA.Element el in this.requestingInformationEnvelope)
            {
                w.WriteStartElement(this.wsdlPrefix, "operation", this.wsdlNamespace);
                w.WriteAttributeString("name", el.Name);

                w.WriteStartElement(this.soapPrefix, "operation", this.soapNamespace);
                w.WriteAttributeString("soapAction", this.soapAction);
                w.WriteAttributeString("style", "document");
                w.WriteAttributeString("xmlns", this.soapPrefix, null, this.soapNamespace);
                w.WriteEndElement();

                //input
                w.WriteStartElement(this.wsdlPrefix, "input", this.wsdlNamespace);
                w.WriteStartElement(this.soapPrefix, "body", this.soapNamespace);
                w.WriteAttributeString("use", "literal");
                w.WriteAttributeString("xmlns", this.soapPrefix, null, this.soapNamespace);
                w.WriteEndElement();
                w.WriteEndElement();

                //output
                w.WriteStartElement(this.wsdlPrefix, "output", this.wsdlNamespace);
                w.WriteStartElement(this.soapPrefix, "body", this.soapNamespace);
                w.WriteAttributeString("use", "literal");
                w.WriteAttributeString("xmlns", this.soapPrefix, null, this.soapNamespace);
                w.WriteEndElement();
                w.WriteEndElement();

                //fault
                w.WriteStartElement(this.wsdlPrefix, "fault", this.wsdlNamespace);
                w.WriteAttributeString("name", "ExceptionResponse");
                w.WriteEndElement();

                w.WriteEndElement();
            }

            //TransactionProtocolSuccess
            w.WriteStartElement(this.wsdlPrefix, "operation", this.wsdlNamespace);
            w.WriteAttributeString("name", "TransactionProtocolSuccess");

            w.WriteStartElement(this.soapPrefix, "operation", this.soapNamespace);
            w.WriteAttributeString("soapAction", this.soapAction);
            w.WriteAttributeString("style", "document");
            w.WriteAttributeString("xmlns", this.soapPrefix, null, this.soapNamespace);
            w.WriteEndElement();

            //input
            w.WriteStartElement(this.wsdlPrefix, "input", this.wsdlNamespace);
            w.WriteStartElement(this.soapPrefix, "body", this.soapNamespace);
            w.WriteAttributeString("use", "literal");
            w.WriteAttributeString("xmlns", this.soapPrefix, null, this.soapNamespace);
            w.WriteEndElement();
            w.WriteEndElement();

            //output
            w.WriteStartElement(this.wsdlPrefix, "output", this.wsdlNamespace);
            w.WriteStartElement(this.soapPrefix, "body", this.soapNamespace);
            w.WriteAttributeString("use", "literal");
            w.WriteAttributeString("xmlns", this.soapPrefix, null, this.soapNamespace);
            w.WriteEndElement();
            w.WriteEndElement();

            //fault
            w.WriteStartElement(this.wsdlPrefix, "fault", this.wsdlNamespace);
            w.WriteAttributeString("name", "ExceptionResponse");
            w.WriteEndElement();

            w.WriteEndElement();

            //TransactionProtocolFailure
            w.WriteStartElement(this.wsdlPrefix, "operation", this.wsdlNamespace);
            w.WriteAttributeString("name", "TransactionProtocolFailure");

            w.WriteStartElement(this.soapPrefix, "operation", this.soapNamespace);
            w.WriteAttributeString("soapAction", this.soapAction);
            w.WriteAttributeString("style", "document");
            w.WriteAttributeString("xmlns", this.soapPrefix, null, this.soapNamespace);
            w.WriteEndElement();

            //input
            w.WriteStartElement(this.wsdlPrefix, "input", this.wsdlNamespace);
            w.WriteStartElement(this.soapPrefix, "body", this.soapNamespace);
            w.WriteAttributeString("use", "literal");
            w.WriteAttributeString("xmlns", this.soapPrefix, null, this.soapNamespace);
            w.WriteEndElement();
            w.WriteEndElement();

            //output
            w.WriteStartElement(this.wsdlPrefix, "output", this.wsdlNamespace);
            w.WriteStartElement(this.soapPrefix, "body", this.soapNamespace);
            w.WriteAttributeString("use", "literal");
            w.WriteAttributeString("xmlns", this.soapPrefix, null, this.soapNamespace);
            w.WriteEndElement();
            w.WriteEndElement();

            //fault
            w.WriteStartElement(this.wsdlPrefix, "fault", this.wsdlNamespace);
            w.WriteAttributeString("name", "ExceptionResponse");
            w.WriteEndElement();

            w.WriteEndElement();


            w.WriteEndElement();
            #endregion

            #region ~~BA Initiator Binding~~
            bindingName = "BA" + this.initiator.Name + removeSpace(this.requestingBusinessActivity.Name);//removeSpace(this.repository.GetPackageByID(Int32.Parse(this.scope)).Name);

            w.WriteStartElement(this.wsdlPrefix, "binding", this.wsdlNamespace);
            w.WriteAttributeString("name", bindingName + "SOAP");
            w.WriteAttributeString("type", this.targetNamespacePrefix + ":" + bindingName);

            w.WriteStartElement(this.soapPrefix, "binding", this.soapNamespace);
            w.WriteAttributeString("style", "document");
            w.WriteAttributeString("transport", "http://schemas.xmlsoap.org/soap/http");
            w.WriteAttributeString("xmlns", this.soapPrefix, null, this.soapNamespace);
            w.WriteEndElement();

            foreach (EA.Element el in this.respondingInformationEnvelope)
            {
                w.WriteStartElement(this.wsdlPrefix, "operation", this.wsdlNamespace);
                w.WriteAttributeString("name", el.Name);

                w.WriteStartElement(this.soapPrefix, "operation", this.soapNamespace);
                w.WriteAttributeString("soapAction", this.soapAction);
                w.WriteAttributeString("style", "document");
                w.WriteAttributeString("xmlns", this.soapPrefix, null, this.soapNamespace);
                w.WriteEndElement();

                //input
                w.WriteStartElement(this.wsdlPrefix, "input", this.wsdlNamespace);
                w.WriteStartElement(this.soapPrefix, "body", this.soapNamespace);
                w.WriteAttributeString("use", "literal");
                w.WriteAttributeString("xmlns", this.soapPrefix, null, this.soapNamespace);
                w.WriteEndElement();
                w.WriteEndElement();

                //output
                w.WriteStartElement(this.wsdlPrefix, "output", this.wsdlNamespace);
                w.WriteStartElement(this.soapPrefix, "body", this.soapNamespace);
                w.WriteAttributeString("use", "literal");
                w.WriteAttributeString("xmlns", this.soapPrefix, null, this.soapNamespace);
                w.WriteEndElement();
                w.WriteEndElement();

                //fault
                w.WriteStartElement(this.wsdlPrefix, "fault", this.wsdlNamespace);
                w.WriteAttributeString("name", "ExceptionResponse");
                w.WriteEndElement();

                w.WriteEndElement();
            }

            //TransactionProtocolSuccess
            w.WriteStartElement(this.wsdlPrefix, "operation", this.wsdlNamespace);
            w.WriteAttributeString("name", "TransactionProtocolSuccess");

            w.WriteStartElement(this.soapPrefix, "operation", this.soapNamespace);
            w.WriteAttributeString("soapAction", this.soapAction);
            w.WriteAttributeString("style", "document");
            w.WriteAttributeString("xmlns", this.soapPrefix, null, this.soapNamespace);
            w.WriteEndElement();

            //input
            w.WriteStartElement(this.wsdlPrefix, "input", this.wsdlNamespace);
            w.WriteStartElement(this.soapPrefix, "body", this.soapNamespace);
            w.WriteAttributeString("use", "literal");
            w.WriteAttributeString("xmlns", this.soapPrefix, null, this.soapNamespace);
            w.WriteEndElement();
            w.WriteEndElement();

            //output
            w.WriteStartElement(this.wsdlPrefix, "output", this.wsdlNamespace);
            w.WriteStartElement(this.soapPrefix, "body", this.soapNamespace);
            w.WriteAttributeString("use", "literal");
            w.WriteAttributeString("xmlns", this.soapPrefix, null, this.soapNamespace);
            w.WriteEndElement();
            w.WriteEndElement();

            //fault
            w.WriteStartElement(this.wsdlPrefix, "fault", this.wsdlNamespace);
            w.WriteAttributeString("name", "ExceptionResponse");
            w.WriteEndElement();

            w.WriteEndElement();

            //TransactionProtocolFailure
            w.WriteStartElement(this.wsdlPrefix, "operation", this.wsdlNamespace);
            w.WriteAttributeString("name", "TransactionProtocolFailure");

            w.WriteStartElement(this.soapPrefix, "operation", this.soapNamespace);
            w.WriteAttributeString("soapAction", this.soapAction);
            w.WriteAttributeString("style", "document");
            w.WriteAttributeString("xmlns", this.soapPrefix, null, this.soapNamespace);
            w.WriteEndElement();

            //input
            w.WriteStartElement(this.wsdlPrefix, "input", this.wsdlNamespace);
            w.WriteStartElement(this.soapPrefix, "body", this.soapNamespace);
            w.WriteAttributeString("use", "literal");
            w.WriteAttributeString("xmlns", this.soapPrefix, null, this.soapNamespace);
            w.WriteEndElement();
            w.WriteEndElement();

            //output
            w.WriteStartElement(this.wsdlPrefix, "output", this.wsdlNamespace);
            w.WriteStartElement(this.soapPrefix, "body", this.soapNamespace);
            w.WriteAttributeString("use", "literal");
            w.WriteAttributeString("xmlns", this.soapPrefix, null, this.soapNamespace);
            w.WriteEndElement();
            w.WriteEndElement();

            //fault
            w.WriteStartElement(this.wsdlPrefix, "fault", this.wsdlNamespace);
            w.WriteAttributeString("name", "ExceptionResponse");
            w.WriteEndElement();

            w.WriteEndElement();


            w.WriteEndElement();
            #endregion

        }

        private void generatePortType(XmlTextWriter w)
        {
            //Transaction Module Initiator
            w.WriteStartElement("portType", this.wsdlNamespace);
            w.WriteAttributeString("name", getInitiatorPortTypeName(this.transModulePrefix.ToUpper()));
            generateInitiatorOperation(w);
            w.WriteEndElement();

            //Transaction Module Responder
            w.WriteStartElement("portType", this.wsdlNamespace);
            w.WriteAttributeString("name", getResponderPortTypeName(this.transModulePrefix.ToUpper()));
            generateResponderOperation(w);
            w.WriteEndElement();

            //Business Application Initiator
            w.WriteStartElement("portType", this.wsdlNamespace);
            w.WriteAttributeString("name", getInitiatorPortTypeName("BA"));
            generateBAInitiatorOperation(w);
            w.WriteEndElement();

            //Business Application Responder
            w.WriteStartElement("portType", this.wsdlNamespace);
            w.WriteAttributeString("name", getResponderPortTypeName("BA"));
            generateBAResponderOperation(w);
            w.WriteEndElement();
        }

        private void generateBAResponderOperation(XmlTextWriter w)
        {
            foreach (EA.Element requesting in this.requestingInformationEnvelope)
            {
                string abstractName = "RequestMsg";

                w.WriteStartElement("operation", this.wsdlNamespace);
                w.WriteAttributeString("name", removeSpace(requesting.Name));

                w.WriteStartElement("input", this.wsdlNamespace);
                w.WriteAttributeString("message", this.busTransPrefix + ":" + abstractName);
                w.WriteEndElement();

                w.WriteStartElement("output", this.wsdlNamespace);
                w.WriteAttributeString("message", this.transModulePrefix + ":" + "TechnicalReceiptAcknowledgementMsg");
                w.WriteEndElement();

                w.WriteStartElement("fault", this.wsdlNamespace);
                w.WriteAttributeString("name", "ExceptionResponse");
                w.WriteAttributeString("message", this.transModulePrefix + ":" + "TechnicalReceiptFailureMsg");//getQualifiedName(classifierElement) + "Msg"); //,"MessageInput"));
                w.WriteEndElement();

                w.WriteEndElement();
            }

            //Transaction Protocol Success
            w.WriteStartElement("operation", this.wsdlNamespace);
            w.WriteAttributeString("name", "TransactionProtocolSuccess");

            w.WriteStartElement("input", this.wsdlNamespace);
            w.WriteAttributeString("message", this.transModulePrefix + ":" + "TransactionProtocolSuccessMsg");
            w.WriteEndElement();

            w.WriteStartElement("output", this.wsdlNamespace);
            w.WriteAttributeString("message", this.transModulePrefix + ":" + "TechnicalReceiptAcknowledgementMsg");
            w.WriteEndElement();

            w.WriteStartElement("fault", this.wsdlNamespace);
            w.WriteAttributeString("name", "ExceptionResponse");
            w.WriteAttributeString("message", this.transModulePrefix + ":" + "TechnicalReceiptFailureMsg");
            w.WriteEndElement();

            w.WriteEndElement();

            //Transacation Protocol Failure
            w.WriteStartElement("operation", this.wsdlNamespace);
            w.WriteAttributeString("name", "TransactionProtocolFailure");

            w.WriteStartElement("input", this.wsdlNamespace);
            w.WriteAttributeString("message", this.transModulePrefix + ":" + "TransactionProtocolFailureMsg");
            w.WriteEndElement();

            w.WriteStartElement("output", this.wsdlNamespace);
            w.WriteAttributeString("message", this.transModulePrefix + ":" + "TechnicalReceiptAcknowledgementMsg");
            w.WriteEndElement();

            w.WriteStartElement("fault", this.wsdlNamespace);
            w.WriteAttributeString("name", "ExceptionResponse");
            w.WriteAttributeString("message", this.transModulePrefix + ":" + "TechnicalReceiptFailureMsg");
            w.WriteEndElement();

            w.WriteEndElement();
        }

        private void generateBAInitiatorOperation(XmlTextWriter w)
        {
            //Business Information Envelope
            foreach (EA.Element responder in this.respondingInformationEnvelope)
            {
                try
                {
                    EA.Element classifierElement = this.repository.GetElementByID(responder.ClassifierID);
                }
                catch
                {
                    throw new Exception("Failed getting classifier of element " + responder.Name);
                }

                string abstractName = "PositiveResponseMsg"; //set default value for RespondingInformationEnvelope

                if (XMLTools.getElementTVValue("ispositive", responder).Equals("false", StringComparison.OrdinalIgnoreCase))//responder.Name.Equals("accept", StringComparison.OrdinalIgnoreCase))
                {
                    abstractName = "NegativeResponseMsg";
                }

                w.WriteStartElement("operation", this.wsdlNamespace);
                w.WriteAttributeString("name", removeSpace(responder.Name));

                w.WriteStartElement("input", this.wsdlNamespace);
                w.WriteAttributeString("message", this.busTransPrefix + ":" + abstractName);
                w.WriteEndElement();

                w.WriteStartElement("output", this.wsdlNamespace);
                w.WriteAttributeString("message", this.transModulePrefix + ":" + "TechnicalReceiptAcknowledgementMsg");
                w.WriteEndElement();

                w.WriteStartElement("fault", this.wsdlNamespace);
                w.WriteAttributeString("name", "ExceptionResponse");
                w.WriteAttributeString("message", this.transModulePrefix + ":" + "TechnicalReceiptFailureMsg");
                w.WriteEndElement();

                w.WriteEndElement();
            }

            //Transaction Protocol Success
            w.WriteStartElement("operation", this.wsdlNamespace);
            w.WriteAttributeString("name", "TransactionProtocolSuccess");

            w.WriteStartElement("input", this.wsdlNamespace);
            w.WriteAttributeString("message", this.transModulePrefix + ":" + "TransactionProtocolSuccessMsg");
            w.WriteEndElement();

            w.WriteStartElement("output", this.wsdlNamespace);
            w.WriteAttributeString("message", this.transModulePrefix + ":" + "TechnicalReceiptAcknowledgementMsg");
            w.WriteEndElement();

            w.WriteStartElement("fault", this.wsdlNamespace);
            w.WriteAttributeString("name", "ExceptionResponse");
            w.WriteAttributeString("message", this.transModulePrefix + ":" + "TechnicalReceiptFailureMsg");
            w.WriteEndElement();

            w.WriteEndElement();

            //Transacation Protocol Fail
            w.WriteStartElement("operation", this.wsdlNamespace);
            w.WriteAttributeString("name", "TransactionProtocolFailure");

            w.WriteStartElement("input", this.wsdlNamespace);
            w.WriteAttributeString("message", this.transModulePrefix + ":" + "TransactionProtocolFailureMsg");
            w.WriteEndElement();

            w.WriteStartElement("output", this.wsdlNamespace);
            w.WriteAttributeString("message", this.transModulePrefix + ":" + "TechnicalReceiptAcknowledgementMsg");
            w.WriteEndElement();

            w.WriteStartElement("fault", this.wsdlNamespace);
            w.WriteAttributeString("name", "ExceptionResponse");
            w.WriteAttributeString("message", this.transModulePrefix + ":" + "TechnicalReceiptFailureMsg");
            w.WriteEndElement();

            w.WriteEndElement();

        }

        private void generateResponderOperation(XmlTextWriter w)
        {
            //Business Information Envelope
            foreach (EA.Element responder in this.respondingInformationEnvelope)
            {
                try
                {
                    EA.Element classifierElement = this.repository.GetElementByID(responder.ClassifierID);
                }
                catch
                {
                    throw new Exception("Failed getting classifier of element " + responder.Name);
                }

                string abstractName = "PositiveResponseMsg"; //set default value for RespondingInformationEnvelope

                if (XMLTools.getElementTVValue("ispositive", responder).Equals("false", StringComparison.OrdinalIgnoreCase))//responder.Name.Equals("accept", StringComparison.OrdinalIgnoreCase))
                {
                    abstractName = "NegativeResponseMsg";
                }

                w.WriteStartElement("operation", this.wsdlNamespace);
                w.WriteAttributeString("name", removeSpace(responder.Name));

                w.WriteStartElement("input", this.wsdlNamespace);
                w.WriteAttributeString("message", this.busTransPrefix + ":" + abstractName);
                w.WriteEndElement();

                w.WriteStartElement("output", this.wsdlNamespace);
                w.WriteAttributeString("message", this.transModulePrefix + ":" + "TechnicalReceiptAcknowledgementMsg");
                w.WriteEndElement();
                w.WriteStartElement("fault", this.wsdlNamespace);
                w.WriteAttributeString("name", "ExceptionResponse");
                w.WriteAttributeString("message", this.transModulePrefix + ":" + "TechnicalReceiptFailureMsg");
                w.WriteEndElement();

                w.WriteEndElement();
            }
        }

        private void generateInitiatorOperation(XmlTextWriter w)
        {
            foreach (EA.Element requesting in this.requestingInformationEnvelope)
            {
                string abstractName = "RequestMsg";

                w.WriteStartElement("operation", this.wsdlNamespace);
                w.WriteAttributeString("name", removeSpace(requesting.Name));

                w.WriteStartElement("input", this.wsdlNamespace);
                w.WriteAttributeString("message", this.busTransPrefix + ":" + abstractName);
                w.WriteEndElement();

                w.WriteStartElement("output", this.wsdlNamespace);
                w.WriteAttributeString("message", this.transModulePrefix + ":" + "TechnicalReceiptAcknowledgementMsg");
                w.WriteEndElement();

                w.WriteStartElement("fault", this.wsdlNamespace);
                w.WriteAttributeString("name", "ExceptionResponse");
                w.WriteAttributeString("message", this.transModulePrefix + ":" + "TechnicalReceiptFailureMsg");//getQualifiedName(classifierElement) + "Msg"); //,"MessageInput"));
                w.WriteEndElement();

                w.WriteEndElement();
            }
        }


        private string getInitiatorPortTypeName(string prefix)
        {
            string initiatorName = this.initiator.Name;
            //string requestingBusActivityName = this.requestingBusinessActivity.Name;
            string name = this.requestingBusinessActivity.Name;//this.repository.GetPackageByID(Int32.Parse(this.scope)).Name;
            return prefix + removeSpace(initiatorName) + removeSpace(name);//requestingBusActivityName);
        }

        private string getResponderPortTypeName(string prefix)
        {
            string responderName = this.responder.Name;
            //string respondingBusActivityName = this.respondingBusinessActivity.Name;
            string name = this.respondingBusinessActivity.Name; //this.repository.GetPackageByID(Int32.Parse(this.scope)).Name;
            return prefix + removeSpace(responderName) + removeSpace(name);//respondingBusActivityName);
        }

        private void addTransactionModuleImports(XmlTextWriter w)
        {
            //import Transaction Module Message
            w.WriteStartElement("import", this.wsdlNamespace);
            w.WriteAttributeString("namespace", getNamespaceFromWSDLFile(this.transModulePath));
            w.WriteAttributeString("location", Path.GetFileName(this.transModulePath));
            w.WriteEndElement();

            //import Business Transaction
            w.WriteStartElement("import", this.wsdlNamespace);
            w.WriteAttributeString("namespace", getNamespaceFromWSDLFile(this.busTransPath));
            w.WriteAttributeString("location", Path.GetFileName(this.busTransPath));
            w.WriteEndElement();
        }

        private void generateInitialSchema(XmlTextWriter w)
        {
            w.Namespaces = true;
            w.Formatting = Formatting.Indented;
            w.WriteStartDocument();

            w.WriteStartElement(this.wsdlPrefix, "definitions", this.wsdlNamespace);
            w.WriteAttributeString("xmlns", "soap", null, "http://schemas.xmlsoap.org/wsdl/soap/");
            w.WriteAttributeString("xmlns", "soapenc", null, "http://schemas.xmlsoap.org/soap/encoding/");
            w.WriteAttributeString("xmlns", "http", null, "http://schemas.xmlsoap.org/wsdl/http/");
            w.WriteAttributeString("xmlns", this.schemaPrefix, null, this.schemaNamespace);
            w.WriteAttributeString("xmlns", "mime", null, "http://schemas.xmlsoap.org/wsdl/mime/");
            w.WriteAttributeString("xmlns", this.partnerLinkPrefix, null, this.partnerLinkNamespace);
            w.WriteAttributeString("xmlns", this.msgPropertyPrefix, null, this.msgPropertyNamespace);
            w.WriteAttributeString("xmlns", this.targetNamespacePrefix, null, getNamespace().Replace(":bt:", ":" + this.transModulePrefix + ":"));

            w.WriteAttributeString("xmlns", this.transModulePrefix, null, getNamespaceFromWSDLFile(this.transModulePath));
            w.WriteAttributeString("xmlns", this.busTransPrefix, null, getNamespaceFromWSDLFile(this.busTransPath));
            w.WriteAttributeString("xmlns", this.busSignalPrefix, null, getNamespaceFromWSDLFile(this.wsdlPath + "GIEMBusinessSignal.wsdl"));
            w.WriteAttributeString("xmlns", this.sbdHeaderPrefix, null, "urn:xml-gov-au:draft:data:messagingAggregates:1.1");//getNamespaceFromXSDFile(this.schemaColPath + @"NatCore\data\SBDHeader.xsd"));


            //TO DO :
            //add namespace for importing business message which is not in the same namespace with WSDL
            addImportBusinessMessageNamespace(w);
            w.WriteAttributeString("name", this.transModulePrefix.ToUpper() + removeSpace(this.repository.GetPackageByID(Int32.Parse(this.scope)).Name));
            w.WriteAttributeString("", "targetNamespace", null, getNamespace().Replace(":bt:", ":" + this.transModulePrefix + ":"));
        }

        private void addImportBusinessMessageNamespace(XmlTextWriter w)
        {
            //still don't know what to import
        }

        

        private void setPath()
        {
            this.wsdlPath = this.path + "WSDL" + @"\";
            this.schemaColPath = this.path + "Schemas" + @"\";
            this.busTransPath = this.path + "WSDL" + @"\" + removeSpace(this.repository.GetPackageByID(Int32.Parse(this.scope)).Name) + ".wsdl";
            this.transModulePath = this.path + @"WSDL\GIEMTransactionModuleMessage.wsdl";
        }



        private string getNamespace()
        {
            return XMLTools.getNameSpace(this.repository, this.repository.GetPackageByID(this.busTransElement.PackageID));
        }

        private void CheckAndResetTransactionModule()
        {
            if (System.IO.File.Exists(this.wsdlPath + "GIEMTransactionModuleMessage.wsdl"))
            //if (this.sourceTransModule != this.transModulePath)
            {
                //System.IO.File.Copy(this.sourceTransModule, this.transModulePath, true);

                try
                {
                    findTransactionModule();
                    //resetting the relative path of imported schema in GIEMTransactionModuleMessage.wsdl
                    resetRelativePath();
                }
                catch (Exception excp)
                {
                    throw new Exception("Failed change relative path on GIEM Transaction Module Message caused by " + excp.Message);
                }
            }

            else
            {
                throw new Exception("Can not find GIEMTransactionModuleMessage.wsdl in " + this.wsdlPath);
            }
        }

        /// <summary>
        /// Method to check whether the WSDL Business Signal is in the same folder with the saving path.
        /// If no, copy the WSDL Business Signal to saving path
        /// </summary>
        private void CopyBusinessTransaction()
        {
            if (this.sourceBusTrans != this.wsdlPath)
                CopyDirectory(this.sourceBusTrans, this.wsdlPath);
        }

        private void CreateDirectoryStructure()
        {
            Directory.CreateDirectory(this.schemaColPath);
            Directory.CreateDirectory(this.wsdlPath);
        }

        /// <summary>
        /// Resetting relative path of of imported schema in Business Signal or Transaction Module to current structure
        /// </summary>
        private void resetRelativePath()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(this.transModulePath);

            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(doc.NameTable);
            namespaceManager.AddNamespace("xs", "http://www.w3.org/2001/XMLSchema");
            XmlNodeList nodeList = doc.SelectNodes("//xs:include", namespaceManager);

            ArrayList transModule = new ArrayList();

            //get DOCLibrary name under the busSignalPkgId
            foreach (EA.Package doclib in this.repository.GetPackageByID(this.transModulePkgId).Packages)
            {
                if (doclib.Element.Stereotype == CCTS_PackageType.DOCLibrary.ToString())
                    transModule.Add(doclib.Name);
            }

            bool save = false;
            if (transModule.Count == nodeList.Count)
            {
                int idx = 0;
                foreach (XmlNode node in nodeList)
                {
                    //string fileName = Path.GetFileName(node.Attributes["schemaLocation"].Value);
                    //string relativePath = getRelativePath(this.wsdlPath, this.schemaColPath) + @"/NatCore/TransMod/" + fileName;
                    string relativePath = getRelativePath(this.wsdlPath, this.schemaColPath) +
                                    getStructureOfBusSignal(this.transModulePkgId) + transModule[idx].ToString() + ".xsd";
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
                XmlTextWriter wrtr = new XmlTextWriter(this.transModulePath, Encoding.UTF8);
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


        private void findTransactionModule()
        {
            this.transModuleFound = false;

            for (short a = 0; a < this.repository.Models.Count; a++)
            {
                if (this.transModuleFound)
                    break;

                EA.Package pkg = (EA.Package)this.repository.Models.GetAt(a);

                searchRecursiveInsidePackage(pkg);
            }
        }

        private void searchRecursiveInsidePackage(EA.Package pkg)
        {
            for (short idx = 0; idx < pkg.Packages.Count; idx++)
            {
                if (this.transModuleFound)
                    break;

                EA.Package thePackage = (EA.Package)pkg.Packages.GetAt(idx);

                if (thePackage.Name == "TransactionModule.library")
                {
                    this.transModuleFound = true;
                    this.transModulePkgId = thePackage.PackageID;
                    break;
                }

                if (pkg.Packages.Count > 0)
                    searchRecursiveInsidePackage(thePackage);
            }
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

        

        private void CopySchemaCollection()
        {
            string source = "";
            if (blnAnyLevel)
            {
                if (sourceBusTrans.Contains("WSDL") || sourceBusTrans.Contains("wsdl"))
                {
                    int idx = this.sourceBusTrans.IndexOf("WSDL", StringComparison.OrdinalIgnoreCase);
                    source = this.sourceBusTrans.Substring(0, idx) + "Schemas";
                }
                else
                {
                    //throw new Exception("");
                }
            }
            else
            {
                source = getSchemaCollectionFromBusTransaction();
            }

            if (source != this.schemaColPath)
                //Copy the schema collection from the source;
                CopyDirectory(source, this.schemaColPath);
        }

        private string getSchemaCollectionFromBusTransaction()
        {
            string schemaLocation = "";
            string filename = this.sourceBusTrans + removeSpace(this.repository.GetPackageByID(Int32.Parse(this.scope)).Name) + ".wsdl";
            XmlTextReader reader = new XmlTextReader(filename);
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element: // The node is an element.
                        if (reader.Name.Equals("xs:include"))
                        {
                            reader.MoveToAttribute("schemaLocation");
                            schemaLocation = reader.Value;
                            reader.Close();
                        }
                        break;
                }
            }
            reader.Close();

            string[] pathParts = this.sourceBusTrans.Split(Path.DirectorySeparatorChar);

            string[] pathParts2 = schemaLocation.Split("/".ToCharArray());
            int counter = pathParts.Length - 2;
            foreach (string x in pathParts2)
            {
                if (x.Equals(".."))
                {
                    counter--;
                }
                else
                    break;
            }

            string resultPath = "";
            for (int a = 0; a <= counter; a++)
            {
                resultPath += pathParts[a] + @"\";
            }

            return resultPath + "Schemas";
        }
        #endregion

        #region Common Method

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

        private string removeSpace(string name)
        {
            name = name.Replace(" ", "");
            return name;
        }

        private void setBPELPath()
        {
            this.wsdlPath = this.path + "WSDL" + @"\";
            this.schemaColPath = this.path + "Schemas" + @"\";
            this.bpelPath = this.path + "BPEL" + @"\";
            this.xsltPath = this.path + @"BPEL\Mappings" + @"\";

            //this.wsdlPath = this.path + "WSDL" + @"\";
            //this.schemaColPath = this.path + "Schemas" + @"\";
            //this.busTransPath = this.path + "WSDL" + @"\" + removeSpace(this.repository.GetPackageByID(Int32.Parse(this.scope)).Name) + ".wsdl";
            //this.transModulePath = this.path + @"WSDL\GIEMTransactionModuleMessage.wsdl";
        }

        private string getNamespaceFromWSDLFile(string path)
        {
            XmlTextReader reader = new XmlTextReader(path);
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
                throw new Exception("Failed getting namespace from file " + path + ". Error message: " + ex.Message);
            }
            finally
            {
                reader.Close();
            }
            return "";
        }

        private void getInitiatorResponderElement()
        {
            EA.Package pkg = this.repository.GetPackageByID(Int32.Parse(this.scope));

            //get element with stereotype <<BusinessTransaction>>
            foreach (EA.Element e in pkg.Elements)
            {
                if (e.Stereotype.Equals("BusinessTransaction", StringComparison.OrdinalIgnoreCase))
                {
                    this.busTransElement = e;
                    break;
                }
            }

            if (this.busTransElement == null)
            {
                throw new Exception("No element with stereotype <<BusinessTransaction>>. Therefore, there is no WSDL schema to generate.");
            }

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
                if (!CCTS.XMLTools.getElementTVValue("businessTransactionType", this.busTransElement).Equals("Notification", StringComparison.OrdinalIgnoreCase) ||
                    !CCTS.XMLTools.getElementTVValue("businessTransactionType", this.busTransElement).Equals("InformationDistribution", StringComparison.OrdinalIgnoreCase))
                {
                    string errorMsg = "<<BusinessTransactionSwimlane>> that contains <<RespondingBusinessActivity>> doesn't exist." +
                    ". \nCan't continue to generate WSDL.";
                    throw new Exception(errorMsg);
                }

            }
        }

        private void CopyDirectory(string source, string destination)
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

        private void generateArtefact()
        {
            getInitiatorResponderElement(); //get important element for the generator
            generateTransactionModule(); //generate transaction module WSDL
            generateBPEL(); //generate BPEL
        }

        public void resetBlnAnyLevel(bool blnAnyLevel)
        {
            this.blnAnyLevel = blnAnyLevel;
            this.chkBindingService.Checked = false;

            this.txtWSDLLocation.Text = "";
            this.txtSavingPath.Text = "";
            this.txtSchemaLocation.Text = "";
        }

        private void getCheckedOption()
        {
            if (this.chkUseAlias.Checked)
                this.blnAlias = true;
            else
                this.blnAlias = false;
        }
        #endregion

        
    }
}