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
using VIENNAAddIn.upcc3.ccts.dra;
using VIENNAAddIn.upcc3.Wizards;
using VIENNAAddIn.Utils;
using VIENNAAddIn.validator;
using VIENNAAddIn.workflow;
using VIENNAAddIn.WSDLGenerator;
using VIENNAAddIn.WSDLGenerator.Setting;
using VIENNAAddIn.XBRLGenerator;
using Attribute=EA.Attribute;

namespace VIENNAAddIn
{
    ///<summary>
    ///</summary>
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

        private XBRLGeneratorForm XbrlGeneratorForm;
        private DOCGenerator xsdGenerator;

        //Check Tagged Values window

        #endregion

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
            bool isUmm2Model = IsUmm2Model(repo);
            if (!isUmm2Model)
            {
                isEnabled = false;
            }
            if (itemname == "&Set Model as UMM2/UPCC3 Model")
            {
                isChecked = isUmm2Model;
            }

            //Overridden for debug purposes
            isEnabled = true;
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
            return LoadMDGFile();
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
                                break;
                            case "&Create initial UMM 2 model structure":
                                InitialPackageStructureCreator.ShowForm(repo);
                                break;
                            case "&Options":
                                OptionsForm.ShowForm(repo);
                                break;
                            case "&Validate All - UMM2":
                                ValidatorForm.ShowForm(repo, "ROOT_UMM");
                                break;
                            case "&Validate":
                                {
                                    //First determine try to determine a UMM scope
                                    string scope = determineValidationScope(repo, menulocation);
                                    if (scope == "")
                                    {
                                        //TO DO - add additional routines here which i.e. try to determine
                                        //a UPCC validation scope

                                        MessageBox.Show(
                                            "Unable to determine a validator for the selected diagram, element or package.");
                                    }
                                    else
                                    {
                                        ValidatorForm.ShowForm(repo, scope);
                                    }
                                }
                                break;
                            case "&Validate All - UPCC3":
                                ValidatorForm.ShowForm(repo, "ROOT_UPCC");
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
                                {
                                    Object obj;
                                    repo.GetTreeSelectedItem(out obj);
                                    int diagramID = ((Diagram) obj).DiagramID;
                                    XBRLLinkbase.ShowForm(repo, diagramID);
                                }
                                break;
                            case "&Generate XBRL":
                                ShowXBRLGeneratorForm(repo.DetermineScope());
                                break;
                            case "&Generate XSD":
                                ShowBLGenerator(repo.DetermineScope());
                                break;
                            case "&Generate XSD from DOC":
                                ShowDOCGenerator(repo.DetermineScope());
                                break;
                            case "&Generate XSD from ENUM":
                                ShowENUMGenerator(repo.DetermineScope());
                                break;
                            case "&Generate XSD from BIE":
                                ShowBIEGenerator(repo.DetermineScope());
                                break;
                            case "&Generate XSD from QDT":
                                ShowQDTGenerator(repo.DetermineScope());
                                break;
                            case "&Generate XSD from CDT":
                                ShowCDTGenerator(repo.DetermineScope());
                                break;
                            case "&Generate XSD from CC":
                                ShowCCGenerator(repo.DetermineScope());
                                break;
                            case "&Create new Qualified Data Type":
                                {
                                    String scope = repo.DetermineScope();
                                    if (CC_Utils.checkPackageConsistencyForQDT(repo, scope))
                                    {
                                        ShowQDTWindow(scope);
                                    }
                                }
                                break;
                            case "&Create new Business Information Entity":
                                {
                                    String scope = repo.DetermineScope();
                                    if (CC_Utils.checkPackageConsistencyForCC(repo, scope))
                                    {
                                        ShowCCWindow(scope);
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
                                {
//                                    GeneratorWizardForm GeneratorWizard = new GeneratorWizardForm { Repository = repository };
                                    new GeneratorWizardForm(new CCRepository(repository)).Show();
                                }
                                break;
                            case "&Export Package to CSV file":
                                ShowExportPackage(repo.DetermineScope());
                                break;
                            case "&Import Package to CSV file":
                                ShowImportPackage(repo.DetermineScope());
                                break;
                        }
                }
            }
            catch (Exception e)
            {
                new ErrorReport(e.Message + "\n" + e.StackTrace, repo.LibraryVersion);
            }
        }

        private void ShowImportPackage(string scope)
        {
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

        private void ShowExportPackage(string scope)
        {
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

        private void ShowCCWindow(string scope)
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

        private void ShowQDTWindow(string scope)
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

        private void ShowCCGenerator(string scope)
        {
            if (ccGenerator == null || ccGenerator.IsDisposed)
            {
                ccGenerator = new CCGenerator(repo, scope, false);
                ccGenerator.Show();
            }
            else
            {
                ccGenerator.resetGenerator(scope);
                ccGenerator.Select();
                ccGenerator.Focus();
                ccGenerator.Show();
            }
        }

        private void ShowCDTGenerator(string scope)
        {
            if (cdtGenerator == null || cdtGenerator.IsDisposed)
            {
                cdtGenerator = new CDTGenerator(repo, scope, false);
                cdtGenerator.Show();
            }
            else
            {
                cdtGenerator.resetGenerator(scope);
                cdtGenerator.Select();
                cdtGenerator.Focus();
                cdtGenerator.Show();
            }
        }

        private void ShowQDTGenerator(string scope)
        {
            if (qdtGenerator == null || qdtGenerator.IsDisposed)
            {
                qdtGenerator = new QDTGenerator(repo, scope, true);
                qdtGenerator.Show();
            }
            else
            {
                qdtGenerator.resetGenerator(scope);
                qdtGenerator.Select();
                qdtGenerator.Focus();
                qdtGenerator.Show();
            }
        }

        private void ShowBIEGenerator(string scope)
        {
            if (bieGenerator == null || bieGenerator.IsDisposed)
            {
                bieGenerator = new BIEGenerator(repo, scope, true);
                bieGenerator.Show();
            }
            else
            {
                bieGenerator.resetGenerator(scope);
                bieGenerator.Select();
                bieGenerator.Focus();
                bieGenerator.Show();
            }
        }

        private void ShowENUMGenerator(string scope)
        {
            if (enumGenerator == null || enumGenerator.IsDisposed)
            {
                enumGenerator = new ENUMGenerator(repo, scope, false);
                enumGenerator.Show();
            }
            else
            {
                enumGenerator.resetGenerator(scope);
                enumGenerator.Select();
                enumGenerator.Focus();
                enumGenerator.Show();
            }
        }

        private void ShowDOCGenerator(string scope)
        {
            if (xsdGenerator == null || xsdGenerator.IsDisposed)
            {
                xsdGenerator = new DOCGenerator(repo, scope, true);
                xsdGenerator.Show();
            }
            else
            {
                xsdGenerator.resetGenerator(scope);
                xsdGenerator.fillChoiceBox();
                xsdGenerator.Select();
                xsdGenerator.Focus();
                xsdGenerator.Show();
            }
        }

        private void ShowBLGenerator(string scope)
        {
            if (BLGenerator == null || BLGenerator.IsDisposed)
            {
                BLGenerator = new BusinessLibraryGenerator(repo, scope, true);
                BLGenerator.Show();
            }
            else
            {
                BLGenerator.resetGenerator(scope);
                BLGenerator.Select();
                BLGenerator.Focus();
                BLGenerator.Show();
            }
        }

        private void ShowXBRLGeneratorForm(string scope)
        {
            if (XbrlGeneratorForm == null || XbrlGeneratorForm.IsDisposed)
            {
                XbrlGeneratorForm = new XBRLGeneratorForm(repo, scope, true);
                XbrlGeneratorForm.Show();
            }
            else
            {
                XbrlGeneratorForm.resetGenerator(scope);
                XbrlGeneratorForm.Select();
                XbrlGeneratorForm.Focus();
                XbrlGeneratorForm.Show();
            }
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
        private static String determineValidationScope(Repository repository, String menulocation)
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
                        int rootModelPackageID = ((Package) (repository.Models.GetAt(0))).PackageID;
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
            if (repository == null)
            {
                return false;
            }
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
                MessageBox.Show("The following exception occured while loading the MDG File: " + e.Message,
                                "AddIn Error");
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
                MessageBox.Show(
                    "The MDG Technology File, which contains the UMM2 Profile and some Patterns could not be unloaded",
                    "AddIn Error");
            }
        }

        #endregion
    }

    internal static class EARepositoryExtensions
    {
        internal static string DetermineScope(this Repository repository)
        {
            Object obj;
            switch (repository.GetTreeSelectedItem(out obj))
            {
                case ObjectType.otPackage:
                    return ((Package)obj).PackageID.ToString();
                case ObjectType.otDiagram:
                    return ((Diagram)obj).PackageID.ToString();
                case ObjectType.otElement:
                    return ((Element)obj).PackageID.ToString();
                default:
                    return "";
            }
        }
    }
}