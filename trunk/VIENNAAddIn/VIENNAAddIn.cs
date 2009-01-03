/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using EA;

using VIENNAAddIn.Exceptions;
using VIENNAAddIn.common;
using VIENNAAddIn.constants;
using VIENNAAddIn.ErrorReporter;
using VIENNAAddIn.validator;

using Microsoft.Win32;
using System.Threading;
using System.Runtime.InteropServices;
using VIENNAAddIn.Utils;
using VIENNAAddIn.Settings;
using VIENNAAddIn.workflow;

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
        string EA_Connect(EA.Repository repository);

        /// <summary>
        /// Disconnect
        /// </summary>
        void EA_Disconnect(EA.Repository repository);

        /// <summary>
        /// Open File
        /// </summary>
        /// <param name="repository"></param>
        void EA_FileOpen(EA.Repository repository);

        /// <summary>
        /// Get Menu-Items
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="menulocation"></param>
        /// <param name="menuname"></param>
        /// <returns></returns>
        string[] EA_GetMenuItems(EA.Repository repository, string menulocation, string menuname);


        object OnInitializeTechnologies(EA.Repository repository);

        
       /// <summary>
        /// Menu Click
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="menuname"></param>
        /// <param name="menuitem"></param>
        void EA_MenuClick(EA.Repository repository, string menulocation, string menuname, string menuitem);

        /// <summary>
        /// Menu State
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="menulocation"></param>
        /// <param name="menuname"></param>
        /// <param name="menuitem"></param>
        /// <param name="IsEnabled"></param>
        /// <param name="IsChecked"></param>
        void EA_GetMenuState(EA.Repository repository, string menulocation, string menuname, string menuitem, ref bool IsEnabled, ref bool IsChecked);

    }
    #endregion


    [Guid("ADFF62A3-BEB5-4f39-9F79-560989B6E48B"),
    ClassInterface(ClassInterfaceType.None),
    ComSourceInterfaces(typeof(VIENNAAddInEvents))]
    public class VIENNAAddIn : VIENNAAddInInterface
    {
        #region Variables
        public static EA.Repository repo;
        private bool isUMM2Model = false;

        //The XSD from CCTS Transformer (there must be at max one instance of the window)
        private WSDLGenerator.WSDLGenerator wsdlGenerator = null;
        private WSDLGenerator.TransactionModuleArtefact tmArtefact = null; //WSDL Generator
        private XBRLGenerator.XBRLLinkbase xbrlLinkbaseGenerator = null;
        private XBRLGenerator.XBRLGenerator xbrlGenerator = null;

        private CCTS.BusinessLibraryGenerator BLGenerator = null;
        private CCTS.BIEGenerator bieGenerator = null;
        private CCTS.QDTGenerator qdtGenerator = null;
        private CCTS.CDTGenerator cdtGenerator = null;
        private CCTS.CCGenerator ccGenerator = null;
        private CCTS.DOCGenerator xsdGenerator = null;
        private CCTS.ENUMGenerator enumGenerator = null;
        private CCTS.QDTWindow qdtWindow = null;
        private CCTS.CCWindow ccWindow = null;

        private ExportImport.ExportPackage exportFeature = null;
        private ExportImport.ImportPackage importFeature = null;

        private Setting.SynchTaggedValue synchTaggedValue = null;

        //UMM Validator window        
        private ValidatorForm validatorForm = null;

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
        public void EA_GetMenuState(EA.Repository repository, string menulocation, string menuname, string itemname, ref bool isEnabled, ref bool isChecked)
        {

            repo = repository;
            if (itemname == "&Set Model as UMM2 Model")
            {
                isChecked = isUMM2Model;
            }
            else if (!isUMM2Model)
            {
                isEnabled = false;
            }

        }

        /// <summary>
        /// Connect
        /// </summary>
        /// <param name="repository"></param>
        /// <returns></returns>
        public string EA_Connect(EA.Repository repository)
        {
            
            try
            {
                /* this method checks if all needed registry entries are present 
                 * at startup. if not, an exception will occur and the AddIn functionality
                // * will be disabled to avoid uncertain states of the AddIn or EA itself.*/
                WindowsRegistryLoader.checkRegistryEntries();

                repo = repository;

            }
            catch (Exception e)
            {
                String err = "An Error occured while checking if all needed registry values for the AddIn are present:\n " + e.Message + ".\n Please reinstall the AddIn";
                MessageBox.Show(err, "AddIn Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //logger.Error(err);

            }
            return null;

        }

        /// <summary>
        /// Disconnect
        /// </summary>
        public void EA_Disconnect(EA.Repository repository)
        {
            repository.CloseAddins();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }


        /// <summary>
        /// EA File Open
        /// </summary>
        /// <param name="repository"></param>
        public void EA_FileOpen(EA.Repository repository)
        {
            // check if the opened model has been marked as an UMM2 model before
            isUMM2Model = this.CheckIfModelIsUMM2Model(repository);
            repo = repository;
        }

        /// <summary>
        /// EA - get menu items
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="menulocation"></param>
        /// <param name="menuname"></param>
        /// <returns></returns>
        public string[] EA_GetMenuItems(EA.Repository repository, string menulocation, string menuname)
        {
            repo = repository;
            ArrayList menu = new ArrayList();
            if (menuname == String.Empty)
            { 
                menu.Add("-" + AddInSettings.getAddInCaption());

            }
            else if (menulocation == "MainMenu")
            {

                if (menuname == "-" + AddInSettings.getAddInCaption())
                {
                    menu.Add("&Set Model as UMM2 Model");
                    menu.Add("&Create initial UMM 2 model structure");
                    menu.Add("-");
                    menu.Add("&Validate All - CCTS");
                    menu.Add("&Validate All - UMM2");
                    menu.Add("-");
                    menu.Add("-Maintenance");
                    menu.Add("&Import CCTS Library");
                    menu.Add("&Options");
                    menu.Add("&About " + AddInSettings.getAddInCaption() + " Add-In");
                }
                else if (menuname == "-Maintenance")
                {
                    menu.Add("Synch tagged value");
                    menu.Add("&BPEL-XSLT Template Setting");
                }
            }
            else if (menulocation == "Diagram")
            {
                if (menuname == "-UMM2")
                {
                    return getDiagramMenu(repository);
                }

            }
            else if (menulocation == "TreeView")
            {
                if (menuname == "-" + AddInSettings.getAddInCaption())
                {
                    return getTreeViewMenu(repository);
                }
            }
            
            return (String[])menu.ToArray(typeof(System.String));
        }

        public object OnInitializeTechnologies(EA.Repository repository)
        {

            

            

           return loadMDGFile();
        }

        /// <summary>
        /// Get MDG file path from registry and read it
        /// </summary>
        /// <returns>MDG in string</returns>
        private string loadMDGFile()
        {
            TextReader reader = null;
            string mdgText  = "";
            
            //get location of MDG profile from registry
            string mdgPath = WindowsRegistryLoader.getMDGFile();

            //read MDG profile file
            reader = new StreamReader(mdgPath);
            mdgText = reader.ReadToEnd();
            reader.Close();
            
            return mdgText;
        }

        #endregion

        /// <summary>
        /// EA menu click
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="menuname"></param>
        /// <param name="menuitem"></param>
        public void EA_MenuClick(EA.Repository repository, string menulocation, string menuname, string menuitem)
        {
            EA.Diagram diagram = repository.GetCurrentDiagram();
            repo = repository;

            //This try/catch catches all Exceptions, which might possibly occur
            //during the execution of the plugin and shows the ErrorWindow to the user
            try
            {

                if (menuitem == "&Import CCTS Library")
                {
                    
                    Utility.importCCTSLibrary(repository, true);

                }
                else
                {
                    //showing the About-Window
                    if (menuitem == "&About " + AddInSettings.getAddInCaption() + " Add-In")
                    {
                        AboutWindow aboutWindow = new AboutWindow();
                        aboutWindow.Show(); 
                    }
                    else if (menuitem == "&BPEL-XSLT Template Setting")
                    {
                        WSDLGenerator.Setting.TemplateSetting setting = new WSDLGenerator.Setting.TemplateSetting();
                        setting.Show();
                    }
                    //Synch tagged value, this function need EA version 7.0.818
                    else if (menuitem == "Synch tagged value")
                    {
                        String scope = determineScope(repository, menulocation, true);

                        synchTaggedValue = new Setting.SynchTaggedValue(repository, scope);
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
                    else if (menuitem == "&Set Model as UMM2 Model")
                    {
                        try
                        {
                            if (isUMM2Model)
                            {
                                /* if the UnSet.. Method returns true, all UMM2 relevant things have been
                                             * successfuly removed from the model and thus it is not a UMM2 model any
                                             * longer (therefore the negation is set) */
                                isUMM2Model = !this.UnSetAsUMM2Model(repository);
                            }
                            else
                            {
                                /* if everything succeeds in the Set... Method, the model has been marked
                                             * as an UMM2 model */
                                isUMM2Model = this.SetAsUMM2Model(repository);
                            }
                        }
                        catch (System.Runtime.InteropServices.COMException comEx)
                        {
                            MessageBox.Show("Please open a model first", "AddIn Error");
                           
                        }
                    }
                    else if (menuitem == "&Create initial UMM 2 model structure")
                    {
                        InitialPackageStructureCreator creator = new InitialPackageStructureCreator(repository);
                        creator.Show();
                    }
                    else if (menuitem == "&Options")
                    {
                        OptionsForm optionsForm = new OptionsForm(repository);
                        optionsForm.ShowDialog();
                    }
                    //menu item validate has been chosen



                    //Invoke a validation of the whole UMM model
                    else if (menuitem == "&Validate All - UMM2")
                    {

                        String scope = "ROOT";

                        if (this.validatorForm == null || this.validatorForm.IsDisposed)
                        {
                            this.validatorForm = new ValidatorForm(repository, scope);
                            this.validatorForm.Show();
                        }
                        else
                        {
                            this.validatorForm.resetValidatorForm(scope);
                            this.validatorForm.Select();
                            this.validatorForm.Focus();
                            this.validatorForm.Show();

                        }
                    }

                    //A validation has been invoked from the menu entry in the treeview
                    else if (menuitem == "&Validate")
                    {
                                                
                        //First determine try to determine a UMM scope
                        String scope = "";
                        scope = this.determineValidationScope(repository, menulocation);

                        if (scope != "")
                        {

                            if (this.validatorForm == null || this.validatorForm.IsDisposed)
                            {
                                this.validatorForm = new ValidatorForm(repository, scope);
                                this.validatorForm.Show();
                            }
                            else
                            {
                                this.validatorForm.resetValidatorForm(scope);
                                this.validatorForm.Select();
                                this.validatorForm.Focus();
                                this.validatorForm.Show();

                            }

                        }

                        //TO DO - add additional routines here which i.e. try to determine
                        //a UPCC validation scope

                        if (scope == "")
                        {
                            MessageBox.Show("Unable to determine a validator for the chosen stereotype. Currently only UMM validation is supported. Make sure that the different top level packages are stereotyped correclty.");
                        }



                    }
                    else if (menuitem == "&Validate All - CCTS")
                    {
                        MessageBox.Show("Sorry this feature has not been activated.", "Info");
                    }

                    //        String scope = "";
                    //        if (menuitem == "Validate All - &CCTS")
                    //            scope = "ALL_CCTS";
                    //        else if (menuitem == "Validate All - &UMM2")
                    //            scope = "ALL_UMM2";
                    //        else
                    //            scope = determineScope(repository, menulocation, true);
                    //        if (this.CCTSvalidatorForm == null || this.CCTSvalidatorForm.IsDisposed)
                    //        {
                    //            this.CCTSvalidatorForm = new CCTS.Validator.CCTSValidator(repository, scope);
                    //            //this.CCTSvalidatorForm = new ValidatorForm(repository, scope);
                    //            this.CCTSvalidatorForm.Show();
                    //        }
                    //        else
                    //        {
                    //            this.CCTSvalidatorForm.resetValidatorForm(scope);
                    //            this.CCTSvalidatorForm.Select();
                    //            this.CCTSvalidatorForm.Focus();
                    //            this.CCTSvalidatorForm.Show();
                    //        }
                    //    }

                    //Generate &WSDL
                    else if (menuitem == "&Generate WSDL from Business Transaction")
                    {
                        String scope = determineScope(repository, menulocation, true);

                        //We already have a running instance
                        if (this.wsdlGenerator == null || this.wsdlGenerator.IsDisposed)
                        {
                            this.wsdlGenerator = new WSDLGenerator.WSDLGenerator(repository, scope, false);
                            this.wsdlGenerator.Show();
                        }
                        //No instance yet
                        else
                        {
                            this.wsdlGenerator.resetGenerator(scope);
                            this.wsdlGenerator.resetBlnAnyLevel(false);
                            this.wsdlGenerator.Select();
                            this.wsdlGenerator.Focus();
                            this.wsdlGenerator.Show();
                        }

                    }
                    //Generate &WSDL from BusinessChoreographyView
                    else if (menuitem == "&Generate all WSDL in BusinessChoreographyView")
                    {
                        String scope = determineScope(repository, menulocation, true);

                        //We already have a running instance
                        if (this.wsdlGenerator == null || this.wsdlGenerator.IsDisposed)
                        {
                            this.wsdlGenerator = new WSDLGenerator.WSDLGenerator(repository, scope, true);
                            this.wsdlGenerator.Show();
                        }
                        //No instance yet
                        else
                        {
                            this.wsdlGenerator.resetGenerator(scope);
                            this.wsdlGenerator.resetBlnAnyLevel(true);
                            this.wsdlGenerator.Select();
                            this.wsdlGenerator.Focus();
                            this.wsdlGenerator.Show();
                        }

                    }

                        //Generate BPEL from package with stereotype BusinessTransactionView 
                    else if (menuitem == "&Generate Transaction Module Artefacts")
                    {
                        String scope = determineScope(repository, menulocation, true);

                        //No instance yet
                        if (this.tmArtefact == null || this.tmArtefact.IsDisposed)
                        {
                            this.tmArtefact = new WSDLGenerator.TransactionModuleArtefact(repository, scope, false); //CCTS.WSDLGenerator.TransactionModuleGen(repository, scope, false);
                            this.tmArtefact.Show();
                        }
                        //We already have a running instance
                        else
                        {
                            this.tmArtefact.resetGenerator(scope);
                            this.tmArtefact.resetBlnAnyLevel(false);
                            this.tmArtefact.Select();
                            this.tmArtefact.Focus();
                            this.tmArtefact.Show();
                        }

                    }

                    //Generate BPEL from package with stereotype BusinessTransactionView 
                    else if (menuitem == "&Generate ALL Transaction Module Artefacts")
                    {
                        String scope = determineScope(repository, menulocation, true);

                        //No instance yet
                        if (this.tmArtefact == null || this.tmArtefact.IsDisposed)
                        {
                            this.tmArtefact = new WSDLGenerator.TransactionModuleArtefact(repository, scope, true);
                            this.tmArtefact.Show();
                        }
                        //We already have a running instance
                        else
                        {
                            this.tmArtefact.resetGenerator(scope);
                            this.tmArtefact.resetBlnAnyLevel(true);
                            this.tmArtefact.Select();
                            this.tmArtefact.Focus();
                            this.tmArtefact.Show();
                        }

                    }


                    //Generate &XBRL
                    else if (menuitem == "&Generate XBRL Linkbase file")
                    {
                        EA.Diagram d = null;
                        Object obj = null;
                        ObjectType o = repository.GetTreeSelectedItem(out obj);

                        d = (EA.Diagram)obj;
                        int diagramID = d.DiagramID;

                        if (this.xbrlLinkbaseGenerator == null || this.xbrlLinkbaseGenerator.IsDisposed)
                        {
                            this.xbrlLinkbaseGenerator = new XBRLGenerator.XBRLLinkbase(repository, diagramID);
                            this.xbrlLinkbaseGenerator.Show();
                        }
                        else
                        {
                            this.xbrlLinkbaseGenerator.resetGenerator(diagramID);
                            this.xbrlLinkbaseGenerator.Select();
                            this.xbrlLinkbaseGenerator.Focus();
                            this.xbrlLinkbaseGenerator.Show();
                        }

                    }

                        //Generate Linkbase XBRL
                    else if (menuitem == "&Generate XBRL")
                    {
                        String scope = determineScope(repository, menulocation, true);

                        //We already have a running instance
                        if (this.xbrlGenerator == null || this.xbrlGenerator.IsDisposed)
                        {
                            this.xbrlGenerator = new XBRLGenerator.XBRLGenerator(repository, scope, true);
                            this.xbrlGenerator.Show();
                        }
                        //No instance yet
                        else
                        {
                            this.xbrlGenerator.resetGenerator(scope);
                            this.xbrlGenerator.Select();
                            this.xbrlGenerator.Focus();
                            this.xbrlGenerator.Show();
                        }


                    }

                    //Generate XSD from CCTS has been chosen
                    else if (menuitem == "&Generate XSD")
                    {
                        String scope = determineScope(repository, menulocation, true);

                        //We already have a running instance
                        if (this.BLGenerator == null || this.BLGenerator.IsDisposed)
                        {
                            this.BLGenerator = new CCTS.BusinessLibraryGenerator(repository, scope, true);
                            this.BLGenerator.Show();
                        }
                        //No instance yet
                        else
                        {
                            this.BLGenerator.resetGenerator(scope);
                            this.BLGenerator.Select();
                            this.BLGenerator.Focus();
                            this.BLGenerator.Show();
                        }


                    }

                    //Generate XSD from DOC has been chosen
                    else if (menuitem == "&Generate XSD from DOC")
                    {
                        String scope = determineScope(repository, menulocation, true);

                        //We already have a running instance
                        if (this.xsdGenerator == null || this.xsdGenerator.IsDisposed)
                        {
                            this.xsdGenerator = new CCTS.DOCGenerator(repository, scope, true);
                            this.xsdGenerator.Show();
                        }
                        //No instance yet
                        else
                        {
                            this.xsdGenerator.resetGenerator(scope);
                            this.xsdGenerator.fillChoiceBox();
                            this.xsdGenerator.Select();
                            this.xsdGenerator.Focus();
                            this.xsdGenerator.Show();
                        }


                    }


                    //Enum Generator Form is Choosen

                    else if (menuitem == "&Generate XSD from ENUM")
                    {
                        String scope = determineScope(repository, menulocation, true);
                        //We already have a running instance
                        if (this.enumGenerator == null || this.enumGenerator.IsDisposed)
                        {
                            this.enumGenerator = new CCTS.ENUMGenerator(repository, scope, false);
                            this.enumGenerator.Show();
                        }
                        //No instance yet
                        else
                        {
                            this.enumGenerator.resetGenerator(scope);
                            this.enumGenerator.Select();
                            this.enumGenerator.Focus();
                            this.enumGenerator.Show();
                        }
                    }


                    //Generate XSD Schema from BIE
                    else if (menuitem == "&Generate XSD from BIE")
                    {
                        String scope = determineScope(repository, menulocation, true);
                        //We already have a running instance
                        if (this.bieGenerator == null || this.bieGenerator.IsDisposed)
                        {
                            this.bieGenerator = new CCTS.BIEGenerator(repository, scope, true);
                            this.bieGenerator.Show();
                        }
                        //No instance yet
                        else
                        {
                            this.bieGenerator.resetGenerator(scope);
                            this.bieGenerator.Select();
                            this.bieGenerator.Focus();
                            this.bieGenerator.Show();
                        }
                    }
                    //Generate XSD Schema from QDT
                    else if (menuitem == "&Generate XSD from QDT")
                    {
                        String scope = determineScope(repository, menulocation, true);
                        //We already have a running instance
                        if (this.qdtGenerator == null || this.qdtGenerator.IsDisposed)
                        {
                            this.qdtGenerator = new CCTS.QDTGenerator(repository, scope, true);
                            this.qdtGenerator.Show();
                        }
                        //No instance yet
                        else
                        {
                            this.qdtGenerator.resetGenerator(scope);
                            this.qdtGenerator.Select();
                            this.qdtGenerator.Focus();
                            this.qdtGenerator.Show();
                        }
                    }
                    //Generate XSD Schema from CDT
                    else if (menuitem == "&Generate XSD from CDT")
                    {
                        String scope = determineScope(repository, menulocation, true);
                        //We already have a running instance
                        if (this.cdtGenerator == null || this.cdtGenerator.IsDisposed)
                        {
                            this.cdtGenerator = new CCTS.CDTGenerator(repository, scope, false);
                            this.cdtGenerator.Show();
                        }
                        //No instance yet
                        else
                        {
                            this.cdtGenerator.resetGenerator(scope);
                            this.cdtGenerator.Select();
                            this.cdtGenerator.Focus();
                            this.cdtGenerator.Show();
                        }
                    }

                    else if (menuitem == "&Generate XSD from CC")
                    {
                        String scope = determineScope(repository, menulocation, true);
                        //We already have a running instance
                        if (this.ccGenerator == null || this.ccGenerator.IsDisposed)
                        {
                            this.ccGenerator = new CCTS.CCGenerator(repository, scope, false);
                            this.ccGenerator.Show();
                        }
                        //No instance yet
                        else
                        {
                            this.ccGenerator.resetGenerator(scope);
                            this.ccGenerator.Select();
                            this.ccGenerator.Focus();
                            this.ccGenerator.Show();
                        }
                    }
                    //Insert new Qualified Data Type
                    else if (menuitem == "&Create new Qualified Data Type")
                    {

                        String scope = determineScope(repository, menulocation, true);
                        if (CCTS.CC_Utils.checkPackageConsistencyForQDT(repository, scope))
                        {
                            if (this.qdtWindow == null || this.qdtWindow.IsDisposed)
                            {
                                qdtWindow = new CCTS.QDTWindow(repository, scope);
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

                        String scope = determineScope(repository, menulocation, true);
                        if (CCTS.CC_Utils.checkPackageConsistencyForCC(repository, scope))
                        {
                            if (this.ccWindow == null || this.ccWindow.IsDisposed)
                            {
                                ccWindow = new CCTS.CCWindow(repository, scope);
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

                    else if (menuitem == "&Export Package to CSV file")
                    {
                        string scope = determineScope(repository, menulocation, false);
                        if (this.exportFeature == null || this.exportFeature.IsDisposed)
                        {
                            this.exportFeature = new ExportImport.ExportPackage(repository, scope);
                            this.exportFeature.Show();
                        }
                        else
                        {
                            this.exportFeature.resetGenerator(scope);
                            this.exportFeature.Select();
                            this.exportFeature.Focus();
                            this.exportFeature.Show();
                        }
                    }
                    else if (menuitem == "&Import Package to CSV file")
                    {
                        string scope = determineScope(repository, menulocation, false);
                        if (this.importFeature == null || this.importFeature.IsDisposed)
                        {
                            this.importFeature = new ExportImport.ImportPackage(repository, scope);
                            this.importFeature.Show();
                        }
                        else
                        {
                            this.importFeature.resetGenerator(scope);
                            this.importFeature.Select();
                            this.importFeature.Focus();
                            this.importFeature.Show();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                //Show the ErrorWindow
                new ErrorReporter.ErrorReport(e.Message + "\n" + e.StackTrace);
            }
        }
        
        #endregion

        #region Private Method



        private string determineScope(EA.Repository repository, String menulocation, bool showErrors)
        {
            Object obj = null;
            EA.ObjectType otype = repository.GetTreeSelectedItem(out obj);
            string s = "";

            //Now otype could be determined - show an error
            if (!Enum.IsDefined(typeof(EA.ObjectType), otype))
            {
                if (showErrors)
                {
                    //String error = "In order to validate/Transform a package, you must click on a package or on an arbitrary element within a package.";
                    //MessageBox.Show(error, "Error");
                    //this.logger.Error(error);
                }
            }
            //The user clicked on a package - try to determine the stereotype
            else if (otype == EA.ObjectType.otPackage)
            {
                EA.Package p = (EA.Package)obj;
                s = "" + p.PackageID;
            }
            else if (otype == EA.ObjectType.otDiagram)
            {
                EA.Diagram dgr = (EA.Diagram)obj;
                s = dgr.PackageID.ToString();
            }
            else if (otype == EA.ObjectType.otElement)
            {
                EA.Element elm = (EA.Element)obj;
                s = elm.PackageID.ToString();
            }

            return s;
        }

        /// <summary>
        /// Get the AddIn Menu for the diagram context
        /// </summary>
        /// <param name="repository"></param>
        /// <returns></returns>
        private String[] getDiagramMenu(EA.Repository repository)
        {
            ArrayList UMM2DiagramMenu = new ArrayList();
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
            return (String[])UMM2DiagramMenu.ToArray(typeof(String));
        }

        /// <summary>
        /// Get the correct menu items for the AddIn menu in the treeview
        /// </summary>
        /// <param name="repository"></param>
        /// <returns></returns>
        private String[] getTreeViewMenu(EA.Repository repository)
        {
            ArrayList VIENNAAddInTreeViewMenu = new ArrayList();
            //When an element in the treeview is right clicked (in order to access the
            //AddIn menu), we must evaluate, if the clicked element is of type EA.Diagram
            // or of type EA.Element
            
            Object obj = null;
            EA.ObjectType otype = repository.GetTreeSelectedItem(out obj);
            bool isSpecificMenu = false;

            /* check if the selected model element is a Package */
            if (otype.Equals(EA.ObjectType.otPackage))
            {
                #region Object Type : Package
                EA.Package package = (EA.Package)obj;
                String stereotype = null;

                try
                {
                    stereotype = package.Element.Stereotype;
                }
                catch (Exception e) { }

                /* if the package is a BusinessRequirementsView or BusinessTransactionView, than enable the menu
                to add a subpackage */
                if (stereotype != null && ((stereotype.Equals(StereotypeOwnTaggedValues.BusinessRequirementsView.ToString()) || stereotype.Equals(UMM.bRequirementsV.ToString()) || 
                    stereotype.Equals(StereotypeOwnTaggedValues.BusinessTransactionView.ToString()) || stereotype.Equals(UMM.bTransactionV.ToString())))) 
                {
                    if (stereotype != null && (stereotype.Equals(StereotypeOwnTaggedValues.BusinessTransactionView.ToString()) || stereotype.Equals(UMM.bTransactionV.ToString())))
                    {
                        VIENNAAddInTreeViewMenu.Add("&Generate WSDL from Business Transaction");
                        VIENNAAddInTreeViewMenu.Add("&Generate Transaction Module Artefacts");
                    }
                }
                else if (stereotype != null && (stereotype.Equals(StereotypeOwnTaggedValues.BusinessChoreographyView.ToString()) || stereotype.Equals(UMM.bChoreographyV.ToString())))
                {
                    VIENNAAddInTreeViewMenu.Add("&Generate all WSDL in BusinessChoreographyView");
                    VIENNAAddInTreeViewMenu.Add("&Generate ALL Transaction Module Artefacts");
                }

                //If the package has stereotype "DOCLibrary" add a link for generating XSD Schema
                //from CCTS
                else if (stereotype != null && (stereotype.Equals(CCTS_Types.DOCLibrary.ToString())))
                {
                    isSpecificMenu = true;
                    VIENNAAddInTreeViewMenu.Add("&Generate XSD from DOC");
                }
                //If the package has stereotype "BIELibrary" add a link for generating XSD Schema
                //from BIE
                else if (stereotype != null && (stereotype.Equals(CCTS_Types.BIELibrary.ToString())))
                {
                    isSpecificMenu = true;
                    VIENNAAddInTreeViewMenu.Add("&Generate XSD from BIE");
                    VIENNAAddInTreeViewMenu.Add("&Create new Business Information Entity");
                }
                //If the package has stereotype "ENUMLibrary" add a link for generating XSD Schema
                //from ENUM
                else if (stereotype != null && (stereotype.Equals(CCTS_Types.ENUMLibrary.ToString())))
                {
                    isSpecificMenu = true;
                    VIENNAAddInTreeViewMenu.Add("&Generate XSD from ENUM");
                }
                //If the package has stereotype "QDTLibrary" add a link for generating XSD Schema
                //from QDT
                else if (stereotype != null && (stereotype.Equals(CCTS_Types.QDTLibrary.ToString())))
                {
                    isSpecificMenu = true;
                    VIENNAAddInTreeViewMenu.Add("&Generate XSD from QDT");
                    VIENNAAddInTreeViewMenu.Add("&Create new Qualified Data Type");
                }
                //If the package has stereotype "CDTLibrary" add a link for generating XSD Schema
                //from CDT
                else if (stereotype != null && (stereotype.Equals(CCTS_Types.CDTLibrary.ToString())))
                {
                    isSpecificMenu = true;
                    VIENNAAddInTreeViewMenu.Add("&Generate XSD from CDT");
                }
                //If the package has stereotype "CCLibrary" add a link for generating XSD Schema
                //from CC
                else if (stereotype != null && (stereotype.Equals(CCTS_Types.CCLibrary.ToString())))
                {
                    isSpecificMenu = true;
                    VIENNAAddInTreeViewMenu.Add("&Generate XSD from CC");
                }
                //We can export XSD and XBRL from bLibraries
                else if (stereotype != null && (stereotype.Equals(CCTS_Types.BusinessLibrary.ToString()) || stereotype.Equals(CCTS_Types.bLibrary.ToString())))
                {
                    VIENNAAddInTreeViewMenu.Add("&Generate XSD");
                    VIENNAAddInTreeViewMenu.Add("&Generate XBRL");
                }
                

                VIENNAAddInTreeViewMenu.Add("&Export Package to CSV file");
                VIENNAAddInTreeViewMenu.Add("&Import Package to CSV file");

                #endregion
            }
            else if (otype == EA.ObjectType.otDiagram)
            {
                #region Object Type : Diagram
                EA.Diagram selectedDiagram = (EA.Diagram)obj;
                
                //Get the diagram id in order to get the package, in which the diagram is located			
                EA.Element parentPackage = repository.GetPackageByID(selectedDiagram.PackageID).Element;
                
                
                //if the stereotype of the package is "DOCLibrary" add a link to generate xsd from
                //CCTS definition
                if (parentPackage.Stereotype.ToString().Equals(CCTS_Types.DOCLibrary.ToString()))
                {
                    isSpecificMenu = true;
                    VIENNAAddInTreeViewMenu.Add("&Generate XSD from DOC");
                    VIENNAAddInTreeViewMenu.Add("&Generate XBRL Linkbase file");
                }


                //if the stereotype of the package is "ENUMLibrary" add a link 
                else if (parentPackage.Stereotype.ToString().Equals(CCTS_Types.ENUMLibrary.ToString()))
                {
                    isSpecificMenu = true;
                    VIENNAAddInTreeViewMenu.Add("&Generate XSD from ENUM");
                }

                //if the steroetype of the package is "BIELibrary" add a link to generate xsd from 
                //BIE definition
                else if (parentPackage.Stereotype.ToString().Equals(CCTS_Types.BIELibrary.ToString()))
                {
                    isSpecificMenu = true;
                    VIENNAAddInTreeViewMenu.Add("&Generate XSD from BIE");
                    VIENNAAddInTreeViewMenu.Add("&Create new Business Information Entity");
                }
                //if the steroetype of the package is "QDTLibrary" add a link to generate xsd from 
                //QDT definition
                else if (parentPackage.Stereotype.ToString().Equals(CCTS_Types.QDTLibrary.ToString()))
                {
                    isSpecificMenu = true;
                    VIENNAAddInTreeViewMenu.Add("&Generate XSD from QDT");
                    VIENNAAddInTreeViewMenu.Add("&Create new Qualified Data Type");

                }
                //if the steroetype of the package is "CDTLibrary" add a link to generate xsd from 
                //CDT definition
                else if (parentPackage.Stereotype.ToString().Equals(CCTS_Types.CDTLibrary.ToString()))
                {
                    isSpecificMenu = true;
                    VIENNAAddInTreeViewMenu.Add("&Generate XSD from CDT");
                }

                #endregion
            }
            else if (otype == EA.ObjectType.otElement)
            {
                #region Object Type : Element
                EA.Element element = (EA.Element)obj;
                String stereotype = element.Stereotype.ToString();
                EA.Element parentPackage = repository.GetPackageByID(element.PackageID).Element;
                
                //if the stereotype of the package is "DOCLibrary" add a link to generate xsd from
                //CCTS definition
                if (parentPackage.Stereotype.ToString().Equals(CCTS_Types.DOCLibrary.ToString()))
                {
                    isSpecificMenu = true;
                    VIENNAAddInTreeViewMenu.Add("&Generate XSD from DOC");
                }
                //if the steroetype of the package is "ENUMLibrary" add a link to generate xsd from 
                //ENUM definition
                else if (parentPackage.Stereotype.ToString().Equals(CCTS_Types.ENUMLibrary.ToString()))
                {
                    isSpecificMenu = true;
                    VIENNAAddInTreeViewMenu.Add("&Generate XSD from ENUM");
                }
                //if the steroetype of the package is "BIELibrary" add a link to generate xsd from 
                //BIE definition
                else if (parentPackage.Stereotype.ToString().Equals(CCTS_Types.BIELibrary.ToString()))
                {
                    isSpecificMenu = true;
                    VIENNAAddInTreeViewMenu.Add("&Generate XSD from BIE");
                    VIENNAAddInTreeViewMenu.Add("&Create new Business Information Entity");
                }
                //if the steroetype of the package is "QDTLibrary" add a link to generate xsd from 
                //QDT definition
                else if (parentPackage.Stereotype.ToString().Equals(CCTS_Types.QDTLibrary.ToString()))
                {
                    isSpecificMenu = true;
                    VIENNAAddInTreeViewMenu.Add("&Generate XSD from QDT");
                    VIENNAAddInTreeViewMenu.Add("&Create new Qualified Data Type");
                }
                //if the steroetype of the package is "CDTLibrary" add a link to generate xsd from 
                //CDT definition
                else if (parentPackage.Stereotype.ToString().Equals(CCTS_Types.CDTLibrary.ToString()))
                {
                    isSpecificMenu = true;
                    VIENNAAddInTreeViewMenu.Add("&Generate XSD from CDT");
                }

                #endregion
            }

            

            VIENNAAddInTreeViewMenu.Add("-");
            VIENNAAddInTreeViewMenu.Add("&Validate");

            //if (Utility.DEBUG)
            //    VIENNAAddInTreeViewMenu.Add("&Show PackageID");
            return (String[])VIENNAAddInTreeViewMenu.ToArray(typeof(System.String));

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
        /// <param name="menuname"></param>
        /// <returns></returns>
        private String determineValidationScope(EA.Repository repository, String menulocation)
        {
            String s = "";


            if (menulocation == "MainMenu")
            {
                s = "ROOT";
            }
            //If, in the TreeView, the User clicks on a package, the stereotype
            //of the package determines the scope
            //If the user clicks on a package without a stereotype no validation
            //is possible, because no scope can be determined								
            else if (menulocation == "TreeView")
            {


                //Get the element in the tree view which was clicked
                Object obj = null;
                EA.ObjectType otype = repository.GetTreeSelectedItem(out obj);

                //Now otype could be determined - show an error
                if (!Enum.IsDefined(typeof(EA.ObjectType), otype))
                {
                    //Should not occur
                    String error = "Unable to determine object type of element.";
                    MessageBox.Show(error, "Error");
                }
                //The user clicked on a package - try to determine the stereotype
                else if (otype == EA.ObjectType.otPackage)
                {
                    EA.Package p = (EA.Package)obj;
                    //If the package has no superpackage, it must be the very top package
                    //-> if the very top package is clicked, ALL will be validated
                    int pID = p.PackageID;
                    bool hasParent = false;
                    try
                    {
                        int dummy = p.ParentID;
                        if (dummy != 0)
                            hasParent = true;
                    }
                    catch (Exception e) { }

                    if (!hasParent)
                    {
                        s = "ROOT";
                    }
                    else
                    {
                        String stereotype = p.Element.Stereotype;
                        s = "" + p.PackageID;
                    }
                }
                //In the treeview apart from a package the user can click on 
                // an element, a diagram, an attribute or a method
                //All of these cases are handled here
                else
                {
                    int packageID = 0;

                    if (otype == EA.ObjectType.otElement)
                        packageID = ((EA.Element)obj).PackageID;
                    else if (otype == EA.ObjectType.otDiagram)
                        packageID = ((EA.Diagram)obj).PackageID;
                    else if (otype == EA.ObjectType.otAttribute)
                    {
                        EA.Attribute att = (EA.Attribute)obj;
                        //Get the element that this attribute is part of
                        EA.Element el = repository.GetElementByID(att.ParentID);
                        //Get the package, where this element is located in
                        packageID = el.PackageID;
                    }
                    else if (otype == EA.ObjectType.otMethod)
                    {
                        EA.Method meth = (EA.Method)obj;
                        //Get the the element, that this attribute is part of
                        EA.Element el = repository.GetElementByID(meth.ParentID);
                        //Get the package, where this element is located in
                        packageID = el.PackageID;
                    }
                    //Get the package					 
                    EA.Package p = repository.GetPackageByID(packageID);
                    String stereotype = Utility.getStereoTypeFromPackage(p);

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
                    Object obj = null;
                    EA.ObjectType o = repository.GetContextItem(out obj);
                    if (o == EA.ObjectType.otDiagram)
                        packageID = ((EA.Diagram)obj).PackageID;
                    else if (o == EA.ObjectType.otElement)
                        packageID = ((EA.Element)obj).PackageID;
                }
                catch (Exception ex) { }

                if (packageID != 0)
                {
                    //To which package does this diagram belong?
                    EA.Package p = repository.GetPackageByID(packageID);
                    String stereotype = Utility.getStereoTypeFromPackage(p);

                    s = "" + p.PackageID;

                }
            }

            return s;
        }






        /// <sUMM2ary>
        /// Checks if an opened model has been previously defined as an UMM2 
        /// model
        /// </sUMM2ary>
        /// <param name="repository">the model, which has been openend and will
        /// be checked now</param>
        /// <returns>true if the model has been marked as an UMM2 model, 
        /// false if not
        /// </returns>
        private bool CheckIfModelIsUMM2Model(EA.Repository repository)
        {
            foreach (EA.ProjectIssues issue in repository.Issues)
            {
                if (issue.Name.Equals("UMM2Model"))
                {
                    MessageBox.Show("Model is defined as an UMM2 Model", "AddIn");
                    
                    return true;
                }
            }
            
            return false;
        }

        /// <sUMM2ary>
        /// defines a normal EA model as an UMM2 model. An "Issue" is added to the repository object,
        /// which marks the model permanently until the user chooses to revert the setting. Also
        /// an MDG file is loaded, which contains the UMM2 Profile and the UMM2 Standard Transaction
        /// Patterns
        /// </sUMM2ary>
        /// <param name="repository">the model, which should be marked as UMM2 model</param>
        /// <returns>true if the model can be successfully marked and the relevant 
        /// MDG file (Profiles, Patterns) can be loaded successfully</returns>
        private bool SetAsUMM2Model(EA.Repository repository)
        {
            // set the issue to mark the model as UMM2Model
            EA.ProjectIssues pIssues = (EA.ProjectIssues)repository.Issues.AddNew("UMM2Model", "Issue");
            pIssues.Update();
            repository.Issues.Refresh();
            // display a message box after the setting
            String succMsg = "This Model is now defined as an UMM2 Model";

            /* load the MDG UMM2 technology file, which contains the patterns 
                         * and profiles */
            try
            {
                String mdgFile = WindowsRegistryLoader.getMDGFileGIEM();
                StreamReader sr = new StreamReader(mdgFile);
                String fileContent = sr.ReadToEnd();
                sr.Close();
                /* finally, after reading the file from the filesystem, load it into the model */
                repository.ImportTechnology(fileContent);
                  
                MessageBox.Show(succMsg, "AddIn");


            }
            catch (Exception e)
            {
                String err = "The following exception occured while loading the MDG File: " + e.Message;
                MessageBox.Show(err, "AddIn Error");

                this.UnSetAsUMM2Model(repository);
                return false;
            }
            return true;

        }

        /// <sUMM2ary>
        /// Unmark an EA Model, which has previously defined as an UMM2 Model. This operation
        /// also unloads the MDG technology file, which contains the UMM2 Profile and the UMM2
        /// Standard transaction patterns
        /// </sUMM2ary>
        /// <param name="repository">the model which shouldnt be marked as UMM2 Model any longer</param>
        /// <returns>true, if the model can be successfully unmarked</returns>
        private bool UnSetAsUMM2Model(EA.Repository repository)
        {
            /* iterate over all issues to find the one, which defines the model 
             * as an UMM2 model */
            EA.Collection pIssues = repository.Issues;
            for (short i = 0; i < pIssues.Count; i++)
            {
                EA.ProjectIssues pIssue = (EA.ProjectIssues)pIssues.GetAt(i);
                if (pIssue.Name.Equals("UMM2Model"))
                {
                    pIssues.DeleteAt(i, true);
                    String unSetMsg = "Model is not defined as an UMM2 Model any longer";
                    MessageBox.Show(unSetMsg, "AddIn");
                    
                    break;
                }
            }
            /* unload mdg UMM2 technology */
            bool mdg_unloaded = repository.DeleteTechnology("UMM2FoundV2");
            // leave a msg if there is a problem with unloading the MDG file
            if (!mdg_unloaded)
            {
                String err = "The MDG Technology File, which contains the UMM2 Profile and some Patterns could not be unloaded";
                MessageBox.Show(err, "AddIn Error");
            }
            else
            {
                
            }
            return true;
        }
        #endregion

    }
}
