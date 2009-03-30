/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using EA;
using VIENNAAddIn.CCTS;
using VIENNAAddIn.common;
using VIENNAAddIn.constants;
using VIENNAAddIn.ErrorReporter;
using VIENNAAddIn.Exceptions;
using VIENNAAddIn.ExportImport;
using VIENNAAddIn.Setting;
using VIENNAAddIn.Settings;
using VIENNAAddIn.Utils;
using VIENNAAddIn.validator;
using VIENNAAddIn.workflow;
using VIENNAAddIn.WSDLGenerator;
using VIENNAAddIn.WSDLGenerator.Setting;
using VIENNAAddIn.XBRLGenerator;
using VIENNAAddIn.upcc3.Wizards;
using Attribute=EA.Attribute;

namespace VIENNAAddIn
{

    #region Interface

    [Guid("CF25D0B4-8D4E-419e-A4B1-AB4C9D90D62E"),
     InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface VIENNAAddInEvents
    {
    }

    [Guid("AC600C85-5BFE-45d5-9D5C-EEE1B5BE852B")]
    public interface VIENNAAddInInterface
    {
        /// <summary>
        /// Connect
        /// </summary>
        /// <param name="repository"></param>
        /// <returns></returns>
        string EA_Connect(Repository repository);

        /// <summary>
        /// Disconnect
        /// </summary>
        void EA_Disconnect(Repository repository);

        /// <summary>
        /// Open File
        /// </summary>
        /// <param name="repository"></param>
        void EA_FileOpen(Repository repository);

        /// <summary>
        /// Get Menu-Items
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="menulocation"></param>
        /// <param name="menuname"></param>
        /// <returns></returns>
        string[] EA_GetMenuItems(Repository repository, string menulocation, string menuname);

        object OnInitializeTechnologies(Repository repository);

        /// <summary>
        /// Menu Click
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="menulocation"></param>
        /// <param name="menuname"></param>
        /// <param name="menuitem"></param>
        void EA_MenuClick(Repository repository, string menulocation, string menuname, string menuitem);

        /// <summary>
        /// Menu State
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="menulocation"></param>
        /// <param name="menuname"></param>
        /// <param name="menuitem"></param>
        /// <param name="IsEnabled"></param>
        /// <param name="IsChecked"></param>
        void EA_GetMenuState(Repository repository, string menulocation, string menuname, string menuitem,
                             ref bool IsEnabled, ref bool IsChecked);
    }

    #endregion

    [Guid("ADFF62A3-BEB5-4f39-9F79-560989B6E48B"),
     ClassInterface(ClassInterfaceType.None),
     ComSourceInterfaces(typeof (VIENNAAddInEvents))]
    public class VIENNAAddIn : VIENNAAddInInterface
    {
        #region Variables

        private static Repository repo;
        private BIEGenerator bieGenerator;
        private BusinessLibraryGenerator BLGenerator;
        private CCGenerator ccGenerator;
        private CCWindow ccWindow;
        private CDTGenerator cdtGenerator;
        private ENUMGenerator enumGenerator;

        private ExportPackage exportFeature;
        private ImportPackage importFeature;
        private QDTGenerator qdtGenerator;
        private QDTWindow qdtWindow;

        private SynchTaggedValue synchTaggedValue;
        private TransactionModuleArtefact tmArtefact; //WSDL Generator

        //UMM Validator window        
        private ValidatorForm validatorForm;
        private WSDLGenerator.WSDLGenerator wsdlGenerator;
        private XBRLGenerator.XBRLGenerator xbrlGenerator;
        private XBRLLinkbase xbrlLinkbaseGenerator;
        private DOCGenerator xsdGenerator;

        //Check Tagged Values window
        private SynchStereotypesForm synchStereotypesForm;

        #endregion

        #region implement VIENNAAddInInterface

        #region Non-Menu method

        /// <summary>
        /// Get menu state
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="menulocation"></param>
        /// <param name="menuname"></param>
        /// <param name="itemname"></param>
        /// <param name="isEnabled"></param>
        /// <param name="isChecked"></param>
        public void EA_GetMenuState(Repository repository, string menulocation, string menuname, string itemname,
                                    ref bool isEnabled, ref bool isChecked)
        {
            var isUmm2Model = IsUmm2Model(repo);
            if (!isUmm2Model)
            {
                isEnabled = false;
            }
            if (itemname == "&Set Model as UMM2/UPCC3 Model")
            {
                isChecked = isUmm2Model;
            }
        }

        /// <summary>
        /// Connect
        /// </summary>
        /// <param name="repository"></param>
        /// <returns></returns>
        public string EA_Connect(Repository repository)
        {
            try
            {
                AddInSettings.LoadRegistryEntries();
            }
            catch (RegistryAccessException e)
            {
                String err = string.Format("Error loading settings from registry:\n{0}.\n Please reinstall the AddIn.",
                                           e.Message);
                MessageBox.Show(err, "AddIn Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return null;
        }

        /// <summary>
        /// Disconnect
        /// </summary>
        public void EA_Disconnect(Repository repository)
        {
            repository.CloseAddins();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        /// <summary>
        /// EA File Open
        /// </summary>
        /// <param name="repository"></param>
        public void EA_FileOpen(Repository repository)
        {
            repo = repository;
            repo.EnableCache = true;
        }

        /// <summary>
        /// EA - get menu items
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="menulocation"></param>
        /// <param name="menuname"></param>
        /// <returns></returns>
        public string[] EA_GetMenuItems(Repository repository, string menulocation, string menuname)
        {
            var menu = new ArrayList();
            if (menuname == String.Empty)
            {
                menu.Add("-" + AddInSettings.AddInCaption);
            }
            else if (menulocation == "MainMenu")
            {
                if (menuname == "-" + AddInSettings.AddInCaption)
                {
                    menu.Add("&Set Model as UMM2/UPCC3 Model");
                    menu.Add("&Create initial UMM 2 model structure");
                    menu.Add("-");
                    menu.Add("&Validate All - UPCC3");
                    menu.Add("&Validate All - UMM2");
                    menu.Add("-");
                    menu.Add("-Maintenance");
                    menu.Add("-Wizards");
                    menu.Add("&Import CCTS Library");
                    menu.Add("&Options");
                    menu.Add("&About " + AddInSettings.AddInCaption + " Add-In");
                }
                else if (menuname == "-Maintenance")
                {
                    menu.Add("Synch tagged value");
                    menu.Add("Synchronize &Tagged Values...");
                    menu.Add("&BPEL-XSLT Template Setting");
                }
                else if (menuname == "-Wizards")
                {
                    menu.Add("Create new &ABIE using the Wizard");
                    menu.Add("Create new BD&T using the Wizard");
                }
            }
            else if (menulocation == "Diagram")
            {
                if (menuname == "-UMM2")
                {
                    return getDiagramMenu();
                }
            }
            else if (menulocation == "TreeView")
            {
                if (menuname == "-" + AddInSettings.AddInCaption)
                {
                    return getTreeViewMenu(repo);
                }
            }

            return (String[]) menu.ToArray(typeof (String));
        }

        public object OnInitializeTechnologies(Repository repository)
        {
            return LoadMDGFile();
        }

        /// <summary>
        /// Get MDG file path from registry and read it
        /// </summary>
        /// <returns>MDG in string</returns>
        private static string LoadMDGFile()
        {
            using (TextReader reader = new StreamReader(AddInSettings.MDGFilePath))
            {
                return reader.ReadToEnd();
            }
        }

        #endregion

        /// <summary>
        /// EA menu click
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="menulocation"></param>
        /// <param name="menuname"></param>
        /// <param name="menuitem"></param>
        public void EA_MenuClick(Repository repository, string menulocation, string menuname, string menuitem)
        {
            //This try/catch catches all Exceptions, which might possibly occur
            //during the execution of the plugin and shows the ErrorWindow to the user
            try
            {
                if (menuitem == "&Import CCTS Library")
                {
                    Utility.importCCTSLibrary(repo, true);
                }
                else
                {
                    //showing the About-Window
                    if (menuitem == "&About " + AddInSettings.AddInCaption + " Add-In")
                    {
                        var aboutWindow = new AboutWindow();
                        aboutWindow.Show();
                    }
                    else if (menuitem == "&BPEL-XSLT Template Setting")
                    {
                        var setting = new TemplateSetting();
                        setting.Show();
                    }
                    else if (menuitem == "Synchronize &Tagged Values...")
                    {
                        if (synchStereotypesForm == null || synchStereotypesForm.IsDisposed)
                        {
                            synchStereotypesForm = new SynchStereotypesForm(repo);
                            synchStereotypesForm.Show();
                        }
                        else
                        {
                            synchStereotypesForm.resetSynchStereotypesForm();
                            synchStereotypesForm.Select();
                            synchStereotypesForm.Focus();
                            synchStereotypesForm.Show();
                        }
                    }
                    //Synch tagged value, this function need EA version 7.0.818
                    else if (menuitem == "Synch tagged value")
                    {
                        String scope = determineScope(repo, true);

                        synchTaggedValue = new SynchTaggedValue(repo, scope);
                        synchTaggedValue.Show();

                        //foreach (object enumStereotype in VIENNAAddIn_BIV)
                        //{
                        //    string tempStereotype = enumStereotype.ToString();
                        //    string abc = repository.CustomCommand("Repository", "SynchProfile", "Profile=BIV;Stereotype=" + tempStereotype + ";");
                        //}
                        //foreach (object enumStereotype in VIENNAAddIn_BRV)
                        //{
                        //    string tempStereotype = enumStereotype.ToString();
                        //    string abc = repository.CustomCommand("Repository", "SynchProfile", "Profile=BRV;Stereotype=" + tempStereotype + ";");
                        //}
                        //foreach (object enumStereotype in VIENNAAddIn_BCV)
                        //{
                        //    string tempStereotype = enumStereotype.ToString();
                        //    string abc = repository.CustomCommand("Repository", "SynchProfile", "Profile=BCV;Stereotype=" + tempStereotype + ";");
                        //}
                        //try
                        //{

                        //}
                        //catch (Exception e)
                        //{
                        //    Console.WriteLine(e.Message);
                        //}
                    }

                        /* defines an EA model as an UMM2 Model */
                    else if (menuitem == "&Set Model as UMM2/UPCC3 Model")
                    {
                        try
                        {
                            if (IsUmm2Model(repo))
                            {
                                UnSetAsUMM2Model(repo);
                            }
                            else
                            {
                                SetAsUMM2Model(repo);
                            }
                        }
                        catch (COMException)
                        {
                            MessageBox.Show("Please open a model first", "AddIn Error");
                        }
                    }
                    else if (menuitem == "&Create initial UMM 2 model structure")
                    {
                        var creator = new InitialPackageStructureCreator(repo);
                        creator.Show();
                    }
                    else if (menuitem == "&Options")
                    {
                        var optionsForm = new OptionsForm(repo);
                        optionsForm.ShowDialog();
                    }
                        //menu item validate has been chosen



                        //Invoke a validation of the whole UMM model
                    else if (menuitem == "&Validate All - UMM2")
                    {
                        String scope = "ROOT_UMM";

                        if (validatorForm == null || validatorForm.IsDisposed)
                        {
                            validatorForm = new ValidatorForm(repo, scope);
                            validatorForm.Show();
                        }
                        else
                        {
                            validatorForm.resetValidatorForm(scope);
                            validatorForm.Select();
                            validatorForm.Focus();
                            validatorForm.Show();
                        }
                    }

                        //A validation has been invoked from the menu entry in the treeview
                    else if (menuitem == "&Validate")
                    {
                        //First determine try to determine a UMM scope
                        string scope = determineValidationScope(repo, menulocation);

                        if (scope != "")
                        {
                            if (validatorForm == null || validatorForm.IsDisposed)
                            {
                                validatorForm = new ValidatorForm(repo, scope);
                                validatorForm.Show();
                            }
                            else
                            {
                                validatorForm.resetValidatorForm(scope);
                                validatorForm.Select();
                                validatorForm.Focus();
                                validatorForm.Show();
                            }
                        }

                        //TO DO - add additional routines here which i.e. try to determine
                        //a UPCC validation scope

                        if (scope == "")
                        {
                            MessageBox.Show(
                                "Unable to determine a validator for the selected diagram, element or package.");
                        }
                    }
                    else if (menuitem == "&Validate All - UPCC3")
                    {
                        String scope = "ROOT_UPCC";

                        if (validatorForm == null || validatorForm.IsDisposed)
                        {
                            validatorForm = new ValidatorForm(repo, scope);
                            validatorForm.Show();
                        }
                        else
                        {
                            validatorForm.resetValidatorForm(scope);
                            validatorForm.Select();
                            validatorForm.Focus();
                            validatorForm.Show();
                        }
                    }
                    else if (menuitem == "&Generate WSDL from Business Transaction")
                    {
                        String scope = determineScope(repo, true);

                        //We already have a running instance
                        if (wsdlGenerator == null || wsdlGenerator.IsDisposed)
                        {
                            wsdlGenerator = new WSDLGenerator.WSDLGenerator(repo, scope, false);
                            wsdlGenerator.Show();
                        }
                            //No instance yet
                        else
                        {
                            wsdlGenerator.resetGenerator(scope);
                            wsdlGenerator.resetBlnAnyLevel(false);
                            wsdlGenerator.Select();
                            wsdlGenerator.Focus();
                            wsdlGenerator.Show();
                        }
                    }
                        //Generate &WSDL from BusinessChoreographyView
                    else if (menuitem == "&Generate all WSDL in BusinessChoreographyView")
                    {
                        String scope = determineScope(repo, true);

                        //We already have a running instance
                        if (wsdlGenerator == null || wsdlGenerator.IsDisposed)
                        {
                            wsdlGenerator = new WSDLGenerator.WSDLGenerator(repo, scope, true);
                            wsdlGenerator.Show();
                        }
                            //No instance yet
                        else
                        {
                            wsdlGenerator.resetGenerator(scope);
                            wsdlGenerator.resetBlnAnyLevel(true);
                            wsdlGenerator.Select();
                            wsdlGenerator.Focus();
                            wsdlGenerator.Show();
                        }
                    }

                        //Generate BPEL from package with stereotype BusinessTransactionView 
                    else if (menuitem == "&Generate Transaction Module Artefacts")
                    {
                        String scope = determineScope(repo, true);

                        //No instance yet
                        if (tmArtefact == null || tmArtefact.IsDisposed)
                        {
                            tmArtefact = new TransactionModuleArtefact(repo, scope, false);
                            //CCTS.WSDLGenerator.TransactionModuleGen(repository, scope, false);
                            tmArtefact.Show();
                        }
                            //We already have a running instance
                        else
                        {
                            tmArtefact.resetGenerator(scope);
                            tmArtefact.resetBlnAnyLevel(false);
                            tmArtefact.Select();
                            tmArtefact.Focus();
                            tmArtefact.Show();
                        }
                    }

                        //Generate BPEL from package with stereotype BusinessTransactionView 
                    else if (menuitem == "&Generate ALL Transaction Module Artefacts")
                    {
                        String scope = determineScope(repo, true);

                        //No instance yet
                        if (tmArtefact == null || tmArtefact.IsDisposed)
                        {
                            tmArtefact = new TransactionModuleArtefact(repo, scope, true);
                            tmArtefact.Show();
                        }
                            //We already have a running instance
                        else
                        {
                            tmArtefact.resetGenerator(scope);
                            tmArtefact.resetBlnAnyLevel(true);
                            tmArtefact.Select();
                            tmArtefact.Focus();
                            tmArtefact.Show();
                        }
                    }


                        //Generate &XBRL
                    else if (menuitem == "&Generate XBRL Linkbase file")
                    {
                        Diagram d;
                        Object obj;
                        ObjectType o = repo.GetTreeSelectedItem(out obj);

                        d = (Diagram) obj;
                        int diagramID = d.DiagramID;

                        if (xbrlLinkbaseGenerator == null || xbrlLinkbaseGenerator.IsDisposed)
                        {
                            xbrlLinkbaseGenerator = new XBRLLinkbase(repo, diagramID);
                            xbrlLinkbaseGenerator.Show();
                        }
                        else
                        {
                            xbrlLinkbaseGenerator.resetGenerator(diagramID);
                            xbrlLinkbaseGenerator.Select();
                            xbrlLinkbaseGenerator.Focus();
                            xbrlLinkbaseGenerator.Show();
                        }
                    }

                        //Generate Linkbase XBRL
                    else if (menuitem == "&Generate XBRL")
                    {
                        String scope = determineScope(repo, true);

                        //We already have a running instance
                        if (xbrlGenerator == null || xbrlGenerator.IsDisposed)
                        {
                            xbrlGenerator = new XBRLGenerator.XBRLGenerator(repo, scope, true);
                            xbrlGenerator.Show();
                        }
                            //No instance yet
                        else
                        {
                            xbrlGenerator.resetGenerator(scope);
                            xbrlGenerator.Select();
                            xbrlGenerator.Focus();
                            xbrlGenerator.Show();
                        }
                    }

                        //Generate XSD from CCTS has been chosen
                    else if (menuitem == "&Generate XSD")
                    {
                        String scope = determineScope(repo, true);

                        //We already have a running instance
                        if (BLGenerator == null || BLGenerator.IsDisposed)
                        {
                            BLGenerator = new BusinessLibraryGenerator(repo, scope, true);
                            BLGenerator.Show();
                        }
                            //No instance yet
                        else
                        {
                            BLGenerator.resetGenerator(scope);
                            BLGenerator.Select();
                            BLGenerator.Focus();
                            BLGenerator.Show();
                        }
                    }

                        //Generate XSD from DOC has been chosen
                    else if (menuitem == "&Generate XSD from DOC")
                    {
                        String scope = determineScope(repo, true);

                        //We already have a running instance
                        if (xsdGenerator == null || xsdGenerator.IsDisposed)
                        {
                            xsdGenerator = new DOCGenerator(repo, scope, true);
                            xsdGenerator.Show();
                        }
                            //No instance yet
                        else
                        {
                            xsdGenerator.resetGenerator(scope);
                            xsdGenerator.fillChoiceBox();
                            xsdGenerator.Select();
                            xsdGenerator.Focus();
                            xsdGenerator.Show();
                        }
                    }


                        //Enum Generator Form is Choosen

                    else if (menuitem == "&Generate XSD from ENUM")
                    {
                        String scope = determineScope(repo, true);
                        //We already have a running instance
                        if (enumGenerator == null || enumGenerator.IsDisposed)
                        {
                            enumGenerator = new ENUMGenerator(repo, scope, false);
                            enumGenerator.Show();
                        }
                            //No instance yet
                        else
                        {
                            enumGenerator.resetGenerator(scope);
                            enumGenerator.Select();
                            enumGenerator.Focus();
                            enumGenerator.Show();
                        }
                    }


                        //Generate XSD Schema from BIE
                    else if (menuitem == "&Generate XSD from BIE")
                    {
                        String scope = determineScope(repo, true);
                        //We already have a running instance
                        if (bieGenerator == null || bieGenerator.IsDisposed)
                        {
                            bieGenerator = new BIEGenerator(repo, scope, true);
                            bieGenerator.Show();
                        }
                            //No instance yet
                        else
                        {
                            bieGenerator.resetGenerator(scope);
                            bieGenerator.Select();
                            bieGenerator.Focus();
                            bieGenerator.Show();
                        }
                    }
                        //Generate XSD Schema from QDT
                    else if (menuitem == "&Generate XSD from QDT")
                    {
                        String scope = determineScope(repo, true);
                        //We already have a running instance
                        if (qdtGenerator == null || qdtGenerator.IsDisposed)
                        {
                            qdtGenerator = new QDTGenerator(repo, scope, true);
                            qdtGenerator.Show();
                        }
                            //No instance yet
                        else
                        {
                            qdtGenerator.resetGenerator(scope);
                            qdtGenerator.Select();
                            qdtGenerator.Focus();
                            qdtGenerator.Show();
                        }
                    }
                        //Generate XSD Schema from CDT
                    else if (menuitem == "&Generate XSD from CDT")
                    {
                        String scope = determineScope(repo, true);
                        //We already have a running instance
                        if (cdtGenerator == null || cdtGenerator.IsDisposed)
                        {
                            cdtGenerator = new CDTGenerator(repo, scope, false);
                            cdtGenerator.Show();
                        }
                            //No instance yet
                        else
                        {
                            cdtGenerator.resetGenerator(scope);
                            cdtGenerator.Select();
                            cdtGenerator.Focus();
                            cdtGenerator.Show();
                        }
                    }

                    else if (menuitem == "&Generate XSD from CC")
                    {
                        String scope = determineScope(repo, true);
                        //We already have a running instance
                        if (ccGenerator == null || ccGenerator.IsDisposed)
                        {
                            ccGenerator = new CCGenerator(repo, scope, false);
                            ccGenerator.Show();
                        }
                            //No instance yet
                        else
                        {
                            ccGenerator.resetGenerator(scope);
                            ccGenerator.Select();
                            ccGenerator.Focus();
                            ccGenerator.Show();
                        }
                    }
                        //Insert new Qualified Data Type
                    else if (menuitem == "&Create new Qualified Data Type")
                    {
                        String scope = determineScope(repo, true);
                        if (CC_Utils.checkPackageConsistencyForQDT(repo, scope))
                        {
                            if (qdtWindow == null || qdtWindow.IsDisposed)
                            {
                                qdtWindow = new QDTWindow(repo, scope);
                                qdtWindow.Show();
                            }
                            else
                            {
                                qdtWindow.resetWindow(scope);
                                qdtWindow.Select();
                                qdtWindow.Focus();
                                qdtWindow.Show();
                            }
                        }
                    }
                        //Insert Business Information Entity
                    else if (menuitem == "&Create new Business Information Entity")
                    {
                        String scope = determineScope(repo, true);
                        if (CC_Utils.checkPackageConsistencyForCC(repo, scope))
                        {
                            if (ccWindow == null || ccWindow.IsDisposed)
                            {
                                ccWindow = new CCWindow(repo, scope);
                                ccWindow.Show();
                            }
                            else
                            {
                                ccWindow.resetWindow(scope);
                                ccWindow.Select();
                                ccWindow.Focus();
                                ccWindow.Show();
                            }
                        }
                    }
                    else if (menuitem == "Create new &ABIE using the Wizard")
                    {
                        ABIEWizardForm ABIEWizard = new ABIEWizardForm(repo);
                        ABIEWizard.Show();                         
                    }
                    else if (menuitem == "Create new BD&T using the Wizard")
                    {
                        BDTWizardForm BDTWizard = new BDTWizardForm(repo);
                        BDTWizard.Show();                        
                    }
                    else if (menuitem == "&Export Package to CSV file")
                    {
                        string scope = determineScope(repo, false);
                        if (exportFeature == null || exportFeature.IsDisposed)
                        {
                            exportFeature = new ExportPackage(repo, scope);
                            exportFeature.Show();
                        }
                        else
                        {
                            exportFeature.resetGenerator(scope);
                            exportFeature.Select();
                            exportFeature.Focus();
                            exportFeature.Show();
                        }
                    }
                    else if (menuitem == "&Import Package to CSV file")
                    {
                        string scope = determineScope(repo, false);
                        if (importFeature == null || importFeature.IsDisposed)
                        {
                            importFeature = new ImportPackage(repo, scope);
                            importFeature.Show();
                        }
                        else
                        {
                            importFeature.resetGenerator(scope);
                            importFeature.Select();
                            importFeature.Focus();
                            importFeature.Show();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                //Show the ErrorWindow
                new ErrorReport(e.Message + "\n" + e.StackTrace, repo.LibraryVersion);
            }
        }

        #endregion

        #region Private Method

        private static string determineScope(Repository repository, bool showErrors)
        {
            Object obj = null;
            ObjectType otype = repository.GetTreeSelectedItem(out obj);
            string s = "";

            //Now otype could be determined - show an error
            if (!Enum.IsDefined(typeof (ObjectType), otype))
            {
                if (showErrors)
                {
                    //String error = "In order to validate/Transform a package, you must click on a package or on an arbitrary element within a package.";
                    //MessageBox.Show(error, "Error");
                    //this.logger.Error(error);
                }
            }
                //The user clicked on a package - try to determine the stereotype
            else if (otype == ObjectType.otPackage)
            {
                var p = (Package) obj;
                s = "" + p.PackageID;
            }
            else if (otype == ObjectType.otDiagram)
            {
                var dgr = (Diagram) obj;
                s = dgr.PackageID.ToString();
            }
            else if (otype == ObjectType.otElement)
            {
                var elm = (Element) obj;
                s = elm.PackageID.ToString();
            }

            return s;
        }

        /// <summary>
        /// Get the AddIn Menu for the diagram context
        /// </summary>
        /// <returns></returns>
        private static string[] getDiagramMenu()
        {
            var UMM2DiagramMenu = new ArrayList();
            try
            {
                #region old code

                //Object obj = null;
                //EA.ObjectType oType = repository.GetContextItem(out obj);
                //if (oType == EA.ObjectType.otConnector)
                //{
                //    EA.Connector con = (EA.Connector)obj;
                //    if (con.Type.Equals("StateFlow") || con.Type.Equals("Transition") || con.Type.Equals("ControlFlow"))
                //    {
                //        UMM2DiagramMenu.Add("&Set guard condition");
                //        /* return here if the element is a connector, because a connector shouldnt
                //         * have the "default" menu entries like Edit Worksheet or Validate */
                //        return (String[])UMM2DiagramMenu.ToArray(typeof(String));
                //    }
                //}
                //else 
                //    if (oType == EA.ObjectType.otDiagram)
                //{
                //    /* retrieve the current diagram to display the relevant the "set as stereotype" 
                //             * menu entries */
                //    EA.Diagram currentDiagram = (EA.Diagram)obj;
                //    /* retrieve the parent package */
                //    EA.Element parentPackage = repository.GetPackageByID(currentDiagram.PackageID).Element;
                //    /* activitygraphs contained in a BusinessChoreographyView must be 
                //             * BusinessCollaborations only */
                //    if (parentPackage.Stereotype.Equals(UMM2_Stereotype.BusinessChoreographyView.ToString()))
                //    {
                //        UMM2DiagramMenu.Add("&Stereotype as BusinessCollaborationProtocol");
                //        if (currentDiagram.Stereotype.Equals(UMM2_Stereotype.BusinessCollaborationProtocol.ToString()))
                //        {
                //            UMM2DiagramMenu.Add("&Associate with BusinessCollaborationUseCase");
                //            UMM2DiagramMenu.Add("-");
                //            UMM2DiagramMenu.Add("&Transform BusinessCollaborationProtocol to a BPSS");
                //        }
                //    }
                //    else if (parentPackage.Stereotype.ToString().Equals(UMM2_Stereotype.BusinessInteractionView.ToString()))
                //    {
                //        UMM2DiagramMenu.Add("&Stereotype as BusinessTransaction");
                //        if (currentDiagram.Stereotype.ToString().Equals(UMM2_Stereotype.BusinessTransaction.ToString()))
                //        {
                //            UMM2DiagramMenu.Add("&Associate with BusinessTransactionUseCase");
                //        }
                //    }
                //    //If the diagram is located in the DOCLibrary package, then a menu for
                //    //inserting an ABIE must be shown
                //    else if (parentPackage.Stereotype.ToString().Equals(CCTS_Types.DOCLibrary.ToString()))
                //    {
                //        //UMM2DiagramMenu.Add("&Insert Business Information Entity");
                //    }
                //}
                //else if (oType == EA.ObjectType.otElement)
                //{
                //    EA.Element e = (EA.Element)obj;
                //    if (e != null)
                //    {
                //        String stereotype = e.Stereotype.ToString();
                //        //If a BusinessCollaborationUseCase is selected, then
                //        //Associate BusinessCollaborationProtocol must be shown
                //        if (stereotype.Equals(UMM2_Stereotype.BusinessCollaborationUseCase.ToString()))
                //        {
                //            UMM2DiagramMenu.Add("&Associate with BusinessCollaborationProtocol");
                //        }
                //        //If a BusinessTransactionUseCase is selected, then
                //        //Associate AcitivityDiagramm must be shown
                //        else if (stereotype.Equals(UMM2_Stereotype.BusinessTransactionUseCase.ToString()))
                //        {
                //            UMM2DiagramMenu.Add("&Associate with BusinessTransaction");
                //        }
                //        /* if it is a BusinessTransactionActivity, the menu entry to assign a BT should appear */
                //        else if (stereotype.Equals(UMM2_Stereotype.BusinessTransactionActivity.ToString()))
                //        {
                //            UMM2DiagramMenu.Add("&Set refining BusinessTransaction");
                //            try
                //            {
                //                EA.Diagram diagram = (EA.Diagram)Utility.getAssociatedElement(e, "bta", repository);
                //                if (diagram.Stereotype.Equals(UMM2_Stereotype.BusinessTransaction.ToString()))
                //                {
                //                    UMM2DiagramMenu.Add("&Open refining BusinessTransaction");
                //                }
                //            }
                //            catch { }
                //        }
                //        //if the user clicks on a SharedBusinessEntityState or an InternalBusinessEntityState
                //        //there should be the possiblity to set an InstanceClassifier
                //        else if (stereotype.Equals(UMM2_Stereotype.SharedBusinessEntityState.ToString()) ||
                //            stereotype.Equals(UMM2_Stereotype.InternalBusinessEntityState.ToString()))
                //        {
                //            UMM2DiagramMenu.Add("&Set Instance Classifier");
                //        }
                //    }
                //}
                //else 
                //if (oType.Equals(EA.ObjectType.otPackage))
                //{
                //    EA.Package package = (EA.Package)obj;
                //    String stereotype = null;
                //    try
                //    {
                //        stereotype = package.Element.Stereotype;
                //    }
                //    catch (Exception e) { }

                //    /* if the package is a BusinessRequirementsView or BusinessTransactionView, than enable the menu
                //    to add a subpackage */
                //    if (stereotype != null && (stereotype.Equals(UMM2_Stereotype.BusinessRequirementsView.ToString()) || stereotype.Equals(UMM2_Stereotype.BusinessTransactionView.ToString())))
                //    {
                //        UMM2DiagramMenu.Add("&Add SubView");
                //    }
                //    else if (stereotype != null && stereotype.Equals(UMM2_Stereotype.BusinessChoreographyView.ToString()))
                //    {
                //        UMM2DiagramMenu.Add("&Add BusinessChoreography and BusinessCollaborationProtocol");
                //    }
                //    else if (stereotype != null && stereotype.Equals(UMM2_Stereotype.BusinessInteractionView.ToString()))
                //    {
                //        UMM2DiagramMenu.Add("&Add BusinessInteraction and BusinessTransaction");
                //    }
                //}
                //UMM2DiagramMenu.Add("-");

                #endregion

                UMM2DiagramMenu.Add("&Validate");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An Error occured while creating the Diagram context Menu: " + ex.Message, "Error");
            }
            return (String[]) UMM2DiagramMenu.ToArray(typeof (String));
        }

        /// <summary>
        /// Get the correct menu items for the AddIn menu in the treeview
        /// </summary>
        /// <param name="repository"></param>
        /// <returns></returns>
        private static String[] getTreeViewMenu(Repository repository)
        {
            var VIENNAAddInTreeViewMenu = new ArrayList();
            //When an element in the treeview is right clicked (in order to access the
            //AddIn menu), we must evaluate, if the clicked element is of type EA.Diagram
            // or of type EA.Element

            Object obj = null;
            ObjectType otype = repository.GetTreeSelectedItem(out obj);

            /* check if the selected model element is a Package */
            if (otype.Equals(ObjectType.otPackage))
            {
                #region Object Type : Package

                var package = (Package) obj;
                String stereotype = null;

                try
                {
                    stereotype = package.Element.Stereotype;
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Error while checking Package (" + e.Message + ")");
                }

                /* if the package is a BusinessRequirementsView or BusinessTransactionView, than enable the menu
                to add a subpackage */
                if (stereotype != null &&
                    ((stereotype.Equals(StereotypeOwnTaggedValues.BusinessRequirementsView.ToString()) ||
                      stereotype.Equals(UMM.bRequirementsV.ToString()) ||
                      stereotype.Equals(StereotypeOwnTaggedValues.BusinessTransactionView.ToString()) ||
                      stereotype.Equals(UMM.bTransactionV.ToString()))))
                {
                    if ((stereotype.Equals(StereotypeOwnTaggedValues.BusinessTransactionView.ToString()) ||
                         stereotype.Equals(UMM.bTransactionV.ToString())))
                    {
                        VIENNAAddInTreeViewMenu.Add("&Generate WSDL from Business Transaction");
                        VIENNAAddInTreeViewMenu.Add("&Generate Transaction Module Artefacts");
                    }
                }
                else if (stereotype != null &&
                         (stereotype.Equals(StereotypeOwnTaggedValues.BusinessChoreographyView.ToString()) ||
                          stereotype.Equals(UMM.bChoreographyV.ToString())))
                {
                    VIENNAAddInTreeViewMenu.Add("&Generate all WSDL in BusinessChoreographyView");
                    VIENNAAddInTreeViewMenu.Add("&Generate ALL Transaction Module Artefacts");
                }

                    //If the package has stereotype "DOCLibrary" add a link for generating XSD Schema
                    //from CCTS
                else if (stereotype != null && (stereotype.Equals(CCTS_Types.DOCLibrary.ToString())))
                {
                    VIENNAAddInTreeViewMenu.Add("&Generate XSD from DOC");
                }
                    //If the package has stereotype "BIELibrary" add a link for generating XSD Schema
                    //from BIE
                else if (stereotype != null && (stereotype.Equals(CCTS_Types.BIELibrary.ToString())))
                {
                    VIENNAAddInTreeViewMenu.Add("&Generate XSD from BIE");
                    VIENNAAddInTreeViewMenu.Add("&Create new Business Information Entity");
                    VIENNAAddInTreeViewMenu.Add("Create new &ABIE using the Wizard");
                }
                    //If the package has stereotype "ENUMLibrary" add a link for generating XSD Schema
                    //from ENUM
                else if (stereotype != null && (stereotype.Equals(CCTS_Types.ENUMLibrary.ToString())))
                {
                    VIENNAAddInTreeViewMenu.Add("&Generate XSD from ENUM");
                }
                else if (stereotype != null && (stereotype.Equals(CCTS_Types.BDTLibrary.ToString())))
                {
                    VIENNAAddInTreeViewMenu.Add("Create new BD&T using the Wizard");
                }
                //If the package has stereotype "QDTLibrary" add a link for generating XSD Schema
                    //from QDT
                else if (stereotype != null && (stereotype.Equals(CCTS_Types.QDTLibrary.ToString())))
                {
                    VIENNAAddInTreeViewMenu.Add("&Generate XSD from QDT");
                    VIENNAAddInTreeViewMenu.Add("&Create new Qualified Data Type");
                }
                    //If the package has stereotype "CDTLibrary" add a link for generating XSD Schema
                    //from CDT
                else if (stereotype != null && (stereotype.Equals(CCTS_Types.CDTLibrary.ToString())))
                {
                    VIENNAAddInTreeViewMenu.Add("&Generate XSD from CDT");
                }
                    //If the package has stereotype "CCLibrary" add a link for generating XSD Schema
                    //from CC
                else if (stereotype != null && (stereotype.Equals(CCTS_Types.CCLibrary.ToString())))
                {
                    VIENNAAddInTreeViewMenu.Add("&Generate XSD from CC");
                }
                    //We can export XSD and XBRL from bLibraries
                else if (stereotype != null &&
                         (stereotype.Equals(CCTS_Types.BusinessLibrary.ToString()) ||
                          stereotype.Equals(CCTS_Types.bLibrary.ToString())))
                {
                    VIENNAAddInTreeViewMenu.Add("&Generate XSD");
                    VIENNAAddInTreeViewMenu.Add("&Generate XBRL");
                }

                VIENNAAddInTreeViewMenu.Add("&Export Package to CSV file");
                VIENNAAddInTreeViewMenu.Add("&Import Package to CSV file");

                #endregion
            }
            else if (otype == ObjectType.otDiagram)
            {
                #region Object Type : Diagram

                var selectedDiagram = (Diagram) obj;

                //Get the diagram id in order to get the package, in which the diagram is located			
                Element parentPackage = repository.GetPackageByID(selectedDiagram.PackageID).Element;

                //if the stereotype of the package is "DOCLibrary" add a link to generate xsd from
                //CCTS definition
                if (parentPackage.Stereotype.Equals(CCTS_Types.DOCLibrary.ToString()))
                {
                    VIENNAAddInTreeViewMenu.Add("&Generate XSD from DOC");
                    VIENNAAddInTreeViewMenu.Add("&Generate XBRL Linkbase file");
                }


                    //if the stereotype of the package is "ENUMLibrary" add a link 
                else if (parentPackage.Stereotype.Equals(CCTS_Types.ENUMLibrary.ToString()))
                {
                    VIENNAAddInTreeViewMenu.Add("&Generate XSD from ENUM");
                }

                    //if the steroetype of the package is "BIELibrary" add a link to generate xsd from 
                    //BIE definition
                else if (parentPackage.Stereotype.Equals(CCTS_Types.BIELibrary.ToString()))
                {
                    VIENNAAddInTreeViewMenu.Add("&Generate XSD from BIE");
                    VIENNAAddInTreeViewMenu.Add("&Create new Business Information Entity");
                    VIENNAAddInTreeViewMenu.Add("Create new &ABIE using the Wizard");
                }
                    //if the steroetype of the package is "QDTLibrary" add a link to generate xsd from 
                    //QDT definition
                else if (parentPackage.Stereotype.Equals(CCTS_Types.QDTLibrary.ToString()))
                {
                    VIENNAAddInTreeViewMenu.Add("&Generate XSD from QDT");
                    VIENNAAddInTreeViewMenu.Add("&Create new Qualified Data Type");
                }
                else if (parentPackage.Stereotype.Equals(CCTS_Types.BDTLibrary.ToString()))
                {
                    VIENNAAddInTreeViewMenu.Add("Create new BD&T using the Wizard");
                }
                    //if the steroetype of the package is "CDTLibrary" add a link to generate xsd from 
                    //CDT definition
                else if (parentPackage.Stereotype.Equals(CCTS_Types.CDTLibrary.ToString()))
                {
                    VIENNAAddInTreeViewMenu.Add("&Generate XSD from CDT");
                }

                #endregion
            }
            else if (otype == ObjectType.otElement)
            {
                #region Object Type : Element

                var element = (Element) obj;
                Element parentPackage = repository.GetPackageByID(element.PackageID).Element;

                //if the stereotype of the package is "DOCLibrary" add a link to generate xsd from
                //CCTS definition
                if (parentPackage.Stereotype.Equals(CCTS_Types.DOCLibrary.ToString()))
                {
                    VIENNAAddInTreeViewMenu.Add("&Generate XSD from DOC");
                }
                    //if the steroetype of the package is "ENUMLibrary" add a link to generate xsd from 
                    //ENUM definition
                else if (parentPackage.Stereotype.Equals(CCTS_Types.ENUMLibrary.ToString()))
                {
                    VIENNAAddInTreeViewMenu.Add("&Generate XSD from ENUM");
                }
                    //if the steroetype of the package is "BIELibrary" add a link to generate xsd from 
                    //BIE definition
                else if (parentPackage.Stereotype.Equals(CCTS_Types.BIELibrary.ToString()))
                {
                    VIENNAAddInTreeViewMenu.Add("&Generate XSD from BIE");
                    VIENNAAddInTreeViewMenu.Add("&Create new Business Information Entity");
                    VIENNAAddInTreeViewMenu.Add("Create new &ABIE using the Wizard");
                }
                    //if the steroetype of the package is "QDTLibrary" add a link to generate xsd from 
                    //QDT definition
                else if (parentPackage.Stereotype.Equals(CCTS_Types.QDTLibrary.ToString()))
                {
                    VIENNAAddInTreeViewMenu.Add("&Generate XSD from QDT");
                    VIENNAAddInTreeViewMenu.Add("&Create new Qualified Data Type");
                }
                else if (parentPackage.Stereotype.Equals(CCTS_Types.BDTLibrary.ToString()))
                {
                    VIENNAAddInTreeViewMenu.Add("Create new BD&T using the Wizard");
                }
                    //if the steroetype of the package is "CDTLibrary" add a link to generate xsd from 
                    //CDT definition
                else if (parentPackage.Stereotype.Equals(CCTS_Types.CDTLibrary.ToString()))
                {
                    VIENNAAddInTreeViewMenu.Add("&Generate XSD from CDT");
                }

                #endregion
            }

            VIENNAAddInTreeViewMenu.Add("-");
            VIENNAAddInTreeViewMenu.Add("&Validate");

            //if (Utility.DEBUG)
            //    VIENNAAddInTreeViewMenu.Add("&Show PackageID");
            return (String[]) VIENNAAddInTreeViewMenu.ToArray(typeof (String));
        }

        /// <summary>
        /// Depending on the area, where the user clicks "Validate", a different scope of the model
        /// is validated.
        /// For instance, a validation can be restricted to a certain view, or a certain diagram
        /// 
        /// 
        /// Comment by pl:
        /// Clearly the method could have been optimized in terms of eliminating re-occuring code.
        /// However re-occuring code was left for the sake of lucidity.
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="menulocation"></param>
        /// <returns></returns>
        private String determineValidationScope(Repository repository, String menulocation)
        {
            String s = "";
							
            if (menulocation == "TreeView")
            {
                //Get the element in the tree view which was clicked
                Object obj;
                ObjectType otype = repository.GetTreeSelectedItem(out obj);

                //Now otype could be determined - show an error
                if (!Enum.IsDefined(typeof (ObjectType), otype))
                {
                    //Should not occur
                    const string error = "Unable to determine object type of element.";
                    MessageBox.Show(error, "Error");
                }
                    //The user clicked on a package - try to determine the stereotype
                else if (otype == ObjectType.otPackage)
                {
                    var p = (Package) obj;
                    //If the package has no superpackage, it must be the very top package
                    //-> if the very top package is clicked, ALL will be validated
                    bool hasParent = false;
                    try
                    {
                        int dummy = p.ParentID;
                        if (dummy != 0)
                            hasParent = true;
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("Error checking for superpackage (" + e.Message + ")");
                    }

                    if (!hasParent)
                    {
                        EA.Collection rootPackages = ((EA.Package)(repository.Models.GetAt(0))).Packages;
                        int rootModelPackageID = ((EA.Package)(repository.Models.GetAt(0))).PackageID;
                        s = "" + rootModelPackageID;
                    }
                    else
                    {
                        s = "" + p.PackageID;
                    }
                }
                    //In the treeview apart from a package the user can click on 
                    // an element, a diagram, an attribute or a method
                    //All of these cases are handled here
                else
                {
                    int packageID = 0;

                    if (otype == ObjectType.otElement)
                        packageID = ((Element) obj).PackageID;
                    else if (otype == ObjectType.otDiagram)
                        packageID = ((Diagram) obj).PackageID;
                    else if (otype == ObjectType.otAttribute)
                    {
                        var att = (Attribute) obj;
                        //Get the element that this attribute is part of
                        Element el = repository.GetElementByID(att.ParentID);
                        //Get the package, where this element is located in
                        packageID = el.PackageID;
                    }
                    else if (otype == ObjectType.otMethod)
                    {
                        var meth = (Method) obj;
                        //Get the the element, that this attribute is part of
                        Element el = repository.GetElementByID(meth.ParentID);
                        //Get the package, where this element is located in
                        packageID = el.PackageID;
                    }
                    //Get the package					 
                    Package p = repository.GetPackageByID(packageID);

                    s = "" + p.PackageID;
                }
            }
                //If the users clicks into a diagram we must determine to which package
                //the diagram belongs
            else if (menulocation == "Diagram")
            {
                int packageID = 0;
                try
                {
                    Object obj;
                    ObjectType o = repository.GetContextItem(out obj);
                    if (o == ObjectType.otDiagram)
                        packageID = ((Diagram) obj).PackageID;
                    else if (o == ObjectType.otElement)
                        packageID = ((Element) obj).PackageID;
                }
                catch (Exception e)
                {
                    Debug.Write("Exception while determining Menulocation (" + e.Message + ")");
                }

                if (packageID != 0)
                {
                    //To which package does this diagram belong?
                    Package p = repository.GetPackageByID(packageID);

                    s = "" + p.PackageID;
                }
            }

            return s;
        }

        private static bool IsUmm2Model(Repository repository)
        {
            foreach (ProjectIssues issue in repository.Issues)
            {
                if (issue.Name.Equals("UMM2Model"))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// defines a normal EA model as an UMM2 model. An "Issue" is added to the repository object,
        /// which marks the model permanently until the user chooses to revert the setting. Also
        /// an MDG file is loaded, which contains the UMM2 Profile and the UMM2 Standard Transaction
        /// Patterns
        /// </summary>
        /// <param name="repository">the model, which should be marked as UMM2 model</param>
        /// <returns>true if the model can be successfully marked and the relevant 
        /// MDG file (Profiles, Patterns) can be loaded successfully</returns>
        private static void SetAsUMM2Model(Repository repository)
        {
            var pIssues = (ProjectIssues) repository.Issues.AddNew("UMM2Model", "Issue");
            pIssues.Update();
            repository.Issues.Refresh();

            try
            {
                repository.ImportTechnology(LoadMDGFile());
                MessageBox.Show("This Model is now defined as an UMM2/UPCC3 Model", "AddIn");
            }
            catch (Exception e)
            {
                MessageBox.Show("The following exception occured while loading the MDG File: " + e.Message, "AddIn Error");
                UnSetAsUMM2Model(repository);
            }
        }

        /// <summary>
        /// Unmark an EA Model, which has previously defined as an UMM2 Model. This operation
        /// also unloads the MDG technology file, which contains the UMM2 Profile and the UMM2
        /// Standard transaction patterns
        /// </summary>
        /// <param name="repository">the model which shouldnt be marked as UMM2 Model any longer</param>
        /// <returns>true, if the model can be successfully unmarked</returns>
        private static void UnSetAsUMM2Model(Repository repository)
        {
            Collection pIssues = repository.Issues;
            for (short i = 0; i < pIssues.Count; i++)
            {
                var pIssue = (ProjectIssues) pIssues.GetAt(i);
                if (pIssue.Name.Equals("UMM2Model"))
                {
                    pIssues.DeleteAt(i, true);
                    MessageBox.Show("Model is not defined as an UMM2/UPCC3 Model any longer", "AddIn");
                    break;
                }
            }
            if (!repository.DeleteTechnology("UMM2FoundV2"))
            {
                MessageBox.Show("The MDG Technology File, which contains the UMM2 Profile and some Patterns could not be unloaded", "AddIn Error");
            }
        }

        #endregion
    }
}