/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;
using System.Collections;
using System.Diagnostics;
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
using VIENNAAddIn.upcc3.ccts.dra;
using VIENNAAddIn.upcc3.Wizards;
using VIENNAAddIn.Utils;
using VIENNAAddIn.validator;
using VIENNAAddIn.workflow;
using VIENNAAddIn.WSDLGenerator;
using VIENNAAddIn.WSDLGenerator.Setting;
using VIENNAAddIn.XBRLGenerator;

namespace VIENNAAddIn
{
    ///<summary>
    ///</summary>
    [Guid("ADFF62A3-BEB5-4f39-9F79-560989B6E48B"),
     ClassInterface(ClassInterfaceType.None),
     ComSourceInterfaces(typeof (VIENNAAddInEvents))]
    public class VIENNAAddIn : VIENNAAddInInterface
    {
        private static Repository repo;

        #region VIENNAAddInInterface Members

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
            if (repo == null)
            {
                isEnabled = false;
                isChecked = false;
            }
            else
            {
                bool isUmm2Model = repo.IsUmm2Model();
                if (itemname == "&Set Model as UMM2/UPCC3 Model")
                {
                    isEnabled = true;
                    isChecked = isUmm2Model;
                }
                else
                {
                    isEnabled = isUmm2Model;
                    isChecked = false;
                }
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
            if (menuname == string.Empty)
            {
                return new[] {"-" + AddInSettings.AddInCaption};
            }
            switch (menulocation)
            {
                case "MainMenu":
                    return GetMainMenu(menuname);
                case "Diagram":
                    return GetDiagramMenu();
                case "TreeView":
                    return GetTreeViewMenu(repo);
            }
            return null;
        }

        public object OnInitializeTechnologies(Repository repository)
        {
            return AddInSettings.LoadMDGFile();
        }

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
                    if (menuitem == "&About " + AddInSettings.AddInCaption + " Add-In")
                    {
                        new AboutWindow().Show();
                    }
                    else
                        switch (menuitem)
                        {
                            case "&BPEL-XSLT Template Setting":
                                TemplateSetting.ShowForm();
                                break;
                            case "Synchronize &Tagged Values...":
                                SynchStereotypesForm.ShowForm(repo);
                                break;
                            case "Synch tagged value":
                                SynchTaggedValue.ShowForm(repo);
                                break;
                            case "&Set Model as UMM2/UPCC3 Model":
                                repo.ToggleUmm2ModelState();
                                break;
                            case "&Create initial UMM 2 model structure":
                                InitialPackageStructureCreator.ShowForm(repo);
                                break;
                            case "&Options":
                                OptionsForm.ShowForm(repo);
                                break;
                            case "&Validate All - UMM2":
                                ValidatorForm.ShowForm(repo, "ROOT_UMM", menulocation);
                                break;
                            case "&Validate":
                                ValidatorForm.ShowForm(repo, null, menulocation);
                                break;
                            case "&Validate All - UPCC3":
                                ValidatorForm.ShowForm(repo, "ROOT_UPCC", menulocation);
                                break;
                            case "&Generate WSDL from Business Transaction":
                                WSDLGenerator.WSDLGenerator.ShowForm(repo, false);
                                break;
                            case "&Generate all WSDL in BusinessChoreographyView":
                                WSDLGenerator.WSDLGenerator.ShowForm(repo, true);
                                break;
                            case "&Generate Transaction Module Artefacts":
                                TransactionModuleArtefact.ShowForm(repo, false);
                                break;
                            case "&Generate ALL Transaction Module Artefacts":
                                TransactionModuleArtefact.ShowForm(repo, true);
                                break;
                            case "&Generate XBRL Linkbase file":
                                XBRLLinkbase.ShowForm(repo);
                                break;
                            case "&Generate XBRL":
                                XBRLGeneratorForm.ShowForm(repo);
                                break;
                            case "&Generate XSD":
                                BusinessLibraryGenerator.ShowForm(repo);
                                break;
                            case "&Generate XSD from DOC":
                                DOCGenerator.ShowForm(repo);
                                break;
                            case "&Generate XSD from ENUM":
                                ENUMGenerator.ShowForm(repo);
                                break;
                            case "&Generate XSD from BIE":
                                BIEGenerator.ShowForm(repo);
                                break;
                            case "&Generate XSD from QDT":
                                QDTGenerator.ShowForm(repo);
                                break;
                            case "&Generate XSD from CDT":
                                CDTGenerator.ShowForm(repo);
                                break;
                            case "&Generate XSD from CC":
                                CCGenerator.ShowForm(repo);
                                break;
                            case "&Create new Qualified Data Type":
                                {
                                    if (CC_Utils.checkPackageConsistencyForQDT(repo))
                                    {
                                        QDTWindow.ShowForm(repo);
                                    }
                                }
                                break;
                            case "&Create new Business Information Entity":
                                {
                                    if (CC_Utils.checkPackageConsistencyForCC(repo))
                                    {
                                        CCWindow.ShowForm(repo);
                                    }
                                }
                                break;
                            case "Create new &ABIE":
                                new ABIEWizardForm(repo).Show();
                                break;
                            case "Create new BD&T":
                                new BDTWizardForm(repo).Show();
                                break;
                            case "Generate &XML Schema":
                                new GeneratorWizardForm(new CCRepository(repository)).Show();
                                break;
                            case "&Export Package to CSV file":
                                ExportPackage.ShowForm(repo);
                                break;
                            case "&Import Package to CSV file":
                                ImportPackage.ShowForm(repo);
                                break;
                        }
                }
            }
            catch (Exception e)
            {
                new ErrorReporterForm(e.Message + "\n" + e.StackTrace, repo.LibraryVersion);
            }
        }

        #endregion

        private static string[] GetMainMenu(string menuname)
        {
            var menu = new ArrayList();
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
                menu.Add("Create new &ABIE");
                menu.Add("Create new BD&T");
                menu.Add("Generate &XML Schema");
            }
            return (String[]) menu.ToArray(typeof (String));
        }

        /// <summary>
        /// Get the AddIn Menu for the diagram context
        /// </summary>
        /// <returns></returns>
        private static string[] GetDiagramMenu()
        {
            return new [] {"&Validate"};
        }

        /// <summary>
        /// Get the correct menu items for the AddIn menu in the treeview
        /// </summary>
        /// <param name="repository"></param>
        /// <returns></returns>
        private static String[] GetTreeViewMenu(Repository repository)
        {
            var VIENNAAddInTreeViewMenu = new ArrayList();

            Object obj;
            ObjectType otype = repository.GetTreeSelectedItem(out obj);

            switch (otype)
            {
                case ObjectType.otPackage:
                    GetTreeViewPackageMenu(obj, VIENNAAddInTreeViewMenu);
                    break;
                case ObjectType.otDiagram:
                    GetTreeViewDiagramMenu(repository, obj, VIENNAAddInTreeViewMenu);
                    break;
                case ObjectType.otElement:
                    GetTreeViewElementMenu(repository, obj, VIENNAAddInTreeViewMenu);
                    break;
            }

            VIENNAAddInTreeViewMenu.Add("-");
            VIENNAAddInTreeViewMenu.Add("&Validate");

            return (String[]) VIENNAAddInTreeViewMenu.ToArray(typeof (String));
        }

        private static void GetTreeViewElementMenu(Repository repository, object obj, ArrayList VIENNAAddInTreeViewMenu)
        {
            var stereotype = repository.GetPackageByID(((Element) obj).PackageID).Element.Stereotype;
            if (stereotype.Equals(CCTS_Types.DOCLibrary.ToString()))
            {
                VIENNAAddInTreeViewMenu.Add("&Generate XSD from DOC");
            }
            else if (stereotype.Equals(CCTS_Types.ENUMLibrary.ToString()))
            {
                VIENNAAddInTreeViewMenu.Add("&Generate XSD from ENUM");
            }
            else if (stereotype.Equals(CCTS_Types.BIELibrary.ToString()))
            {
                VIENNAAddInTreeViewMenu.Add("&Generate XSD from BIE");
                VIENNAAddInTreeViewMenu.Add("&Create new Business Information Entity");
                VIENNAAddInTreeViewMenu.Add("Create new &ABIE");
            }
            else if (stereotype.Equals(CCTS_Types.QDTLibrary.ToString()))
            {
                VIENNAAddInTreeViewMenu.Add("&Generate XSD from QDT");
                VIENNAAddInTreeViewMenu.Add("&Create new Qualified Data Type");
            }
            else if (stereotype.Equals(CCTS_Types.BDTLibrary.ToString()))
            {
                VIENNAAddInTreeViewMenu.Add("Create new BD&T");
            }
            else if (stereotype.Equals(CCTS_Types.CDTLibrary.ToString()))
            {
                VIENNAAddInTreeViewMenu.Add("&Generate XSD from CDT");
            }
        }

        private static void GetTreeViewDiagramMenu(Repository repository, object obj, ArrayList VIENNAAddInTreeViewMenu)
        {
            var stereotype = repository.GetPackageByID(((Diagram) obj).PackageID).Element.Stereotype;
            if (stereotype.Equals(CCTS_Types.DOCLibrary.ToString()))
            {
                VIENNAAddInTreeViewMenu.Add("&Generate XSD from DOC");
                VIENNAAddInTreeViewMenu.Add("&Generate XBRL Linkbase file");
            }
            else if (stereotype.Equals(CCTS_Types.ENUMLibrary.ToString()))
            {
                VIENNAAddInTreeViewMenu.Add("&Generate XSD from ENUM");
            }
            else if (stereotype.Equals(CCTS_Types.BIELibrary.ToString()))
            {
                VIENNAAddInTreeViewMenu.Add("&Generate XSD from BIE");
                VIENNAAddInTreeViewMenu.Add("&Create new Business Information Entity");
                VIENNAAddInTreeViewMenu.Add("Create new &ABIE");
            }
            else if (stereotype.Equals(CCTS_Types.QDTLibrary.ToString()))
            {
                VIENNAAddInTreeViewMenu.Add("&Generate XSD from QDT");
                VIENNAAddInTreeViewMenu.Add("&Create new Qualified Data Type");
            }
            else if (stereotype.Equals(CCTS_Types.BDTLibrary.ToString()))
            {
                VIENNAAddInTreeViewMenu.Add("Create new BD&T");
            }
            else if (stereotype.Equals(CCTS_Types.CDTLibrary.ToString()))
            {
                VIENNAAddInTreeViewMenu.Add("&Generate XSD from CDT");
            }
        }

        private static void GetTreeViewPackageMenu(object obj, ArrayList VIENNAAddInTreeViewMenu)
        {
            string stereotype = ((Package) obj).Element.Stereotype;
            if (stereotype != null)
            {
                if (stereotype.Equals(StereotypeOwnTaggedValues.BusinessTransactionView.ToString()) ||
                    stereotype.Equals(UMM.bTransactionV.ToString()))
                {
                    VIENNAAddInTreeViewMenu.Add("&Generate WSDL from Business Transaction");
                    VIENNAAddInTreeViewMenu.Add("&Generate Transaction Module Artefacts");
                }
                else if ((stereotype.Equals(StereotypeOwnTaggedValues.BusinessChoreographyView.ToString()) ||
                          stereotype.Equals(UMM.bChoreographyV.ToString())))
                {
                    VIENNAAddInTreeViewMenu.Add("&Generate all WSDL in BusinessChoreographyView");
                    VIENNAAddInTreeViewMenu.Add("&Generate ALL Transaction Module Artefacts");
                }
                else if ((stereotype.Equals(CCTS_Types.DOCLibrary.ToString())))
                {
                    VIENNAAddInTreeViewMenu.Add("&Generate XSD from DOC");
                }
                else if ((stereotype.Equals(CCTS_Types.BIELibrary.ToString())))
                {
                    VIENNAAddInTreeViewMenu.Add("&Generate XSD from BIE");
                    VIENNAAddInTreeViewMenu.Add("&Create new Business Information Entity");
                    VIENNAAddInTreeViewMenu.Add("Create new &ABIE");
                }
                else if ((stereotype.Equals(CCTS_Types.ENUMLibrary.ToString())))
                {
                    VIENNAAddInTreeViewMenu.Add("&Generate XSD from ENUM");
                }
                else if ((stereotype.Equals(CCTS_Types.BDTLibrary.ToString())))
                {
                    VIENNAAddInTreeViewMenu.Add("Create new BD&T");
                }
                else if ((stereotype.Equals(CCTS_Types.QDTLibrary.ToString())))
                {
                    VIENNAAddInTreeViewMenu.Add("&Generate XSD from QDT");
                    VIENNAAddInTreeViewMenu.Add("&Create new Qualified Data Type");
                }
                else if ((stereotype.Equals(CCTS_Types.CDTLibrary.ToString())))
                {
                    VIENNAAddInTreeViewMenu.Add("&Generate XSD from CDT");
                }
                else if ((stereotype.Equals(CCTS_Types.CCLibrary.ToString())))
                {
                    VIENNAAddInTreeViewMenu.Add("&Generate XSD from CC");
                }
                else if ((stereotype.Equals(CCTS_Types.BusinessLibrary.ToString()) ||
                          stereotype.Equals(CCTS_Types.bLibrary.ToString())))
                {
                    VIENNAAddInTreeViewMenu.Add("&Generate XSD");
                    VIENNAAddInTreeViewMenu.Add("&Generate XBRL");
                }
            }

            VIENNAAddInTreeViewMenu.Add("&Export Package to CSV file");
            VIENNAAddInTreeViewMenu.Add("&Import Package to CSV file");
        }
    }
}