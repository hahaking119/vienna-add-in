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

        #region implement VIENNAAddInInterface

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
                    menu.Add("Create new &ABIE");
                    menu.Add("Create new BD&T");
                    menu.Add("Generate &XML Schema");
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
                new ErrorReport(e.Message + "\n" + e.StackTrace, repo.LibraryVersion);
            }
        }

        #endregion

        #region Private Method

        /// <summary>
        /// Get the AddIn Menu for the diagram context
        /// </summary>
        /// <returns></returns>
        private static string[] getDiagramMenu()
        {
            var UMM2DiagramMenu = new ArrayList();
            try
            {
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

            Object obj;
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
                    VIENNAAddInTreeViewMenu.Add("Create new &ABIE");
                }
                    //If the package has stereotype "ENUMLibrary" add a link for generating XSD Schema
                    //from ENUM
                else if (stereotype != null && (stereotype.Equals(CCTS_Types.ENUMLibrary.ToString())))
                {
                    VIENNAAddInTreeViewMenu.Add("&Generate XSD from ENUM");
                }
                else if (stereotype != null && (stereotype.Equals(CCTS_Types.BDTLibrary.ToString())))
                {
                    VIENNAAddInTreeViewMenu.Add("Create new BD&T");
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
                    VIENNAAddInTreeViewMenu.Add("Create new &ABIE");
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
                    VIENNAAddInTreeViewMenu.Add("Create new BD&T");
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
                    VIENNAAddInTreeViewMenu.Add("Create new &ABIE");
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
                    VIENNAAddInTreeViewMenu.Add("Create new BD&T");
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

        #endregion
    }
}