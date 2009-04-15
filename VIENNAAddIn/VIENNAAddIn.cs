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
using VIENNAAddIn.common;
using VIENNAAddIn.constants;
using VIENNAAddIn.ErrorReporter;
using VIENNAAddIn.Exceptions;
using VIENNAAddIn.Settings;
using VIENNAAddIn.upcc3.ccts.dra;
using VIENNAAddIn.upcc3.Wizards;
using VIENNAAddIn.Utils;
using VIENNAAddIn.validator;
using VIENNAAddIn.workflow;

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
        private static ObjectType selectedOT;
        private static string selectedGUID;

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

                if(itemname == "&Modify ABIE")
                {
                    if(selectedOT == EA.ObjectType.otElement)
                    {
                        isEnabled = repo.GetElementByGuid(selectedGUID).Stereotype.ToString().Equals("ABIE");
                    }
                    else
                    {
                        isEnabled = false;
                    }
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
                    if (menuitem == "&About " + AddInSettings.AddInCaption + " Add-In")
                    {
                        new AboutWindow().Show();
                    }
                    else
                        switch (menuitem)
                        {
                            case "Synchronize &Tagged Values...":
                                SynchStereotypesForm.ShowForm(repo);
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
                            case "Create new &ABIE":
                                new ABIEWizardForm(repo).Show();
                                break;
                            case "&Modify ABIE":
                                new ABIEWizardForm(repo, repo.GetElementByGuid(selectedGUID)).Show();
                                break;
                            case "Create new BD&T":
                                new BDTWizardForm(repo).Show();
                                break;
                            case "Generate &XML Schema":
                                new GeneratorWizardForm(new CCRepository(repository)).Show();
                                break;
                        }
            }
            catch (Exception e)
            {
                new ErrorReporterForm(e.Message + "\n" + e.StackTrace, repo.LibraryVersion);
            }
        }

        ///<summary>
        ///</summary>
        ///<param name="repository"></param>
        ///<param name="GUID"></param>
        ///<param name="ot"></param>
        ///<returns></returns>
        public void EA_OnContextItemChanged(EA.Repository repository, string GUID, EA.ObjectType ot)
        {
            selectedOT = ot;
            selectedGUID = GUID;
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
                menu.Add("&Options");
                menu.Add("&About " + AddInSettings.AddInCaption + " Add-In");
            }
            else if (menuname == "-Maintenance")
            {
                menu.Add("Synchronize &Tagged Values...");
            }
            else if (menuname == "-Wizards")
            {
                menu.Add("Create new &ABIE");
                menu.Add("&Modify ABIE");
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
            return new[] {"&Validate"};
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
            switch (repository.GetTreeSelectedItem(out obj))
            {
                case ObjectType.otPackage:
                    GetTreeViewPackageMenu((Package) obj, VIENNAAddInTreeViewMenu);
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
            if (stereotype.Equals(CCTS_Types.BIELibrary.ToString()))
            {
                VIENNAAddInTreeViewMenu.Add("Create new &ABIE");
            }
            else if (stereotype.Equals(CCTS_Types.BDTLibrary.ToString()))
            {
                VIENNAAddInTreeViewMenu.Add("Create new BD&T");
            }
        }

        private static void GetTreeViewDiagramMenu(Repository repository, object obj, ArrayList VIENNAAddInTreeViewMenu)
        {
            var stereotype = repository.GetPackageByID(((Diagram) obj).PackageID).Element.Stereotype;
            if (stereotype.Equals(CCTS_Types.BIELibrary.ToString()))
            {
                VIENNAAddInTreeViewMenu.Add("Create new &ABIE");
            }
            else if (stereotype.Equals(CCTS_Types.BDTLibrary.ToString()))
            {
                VIENNAAddInTreeViewMenu.Add("Create new BD&T");
            }
        }

        private static void GetTreeViewPackageMenu(Package package, ArrayList VIENNAAddInTreeViewMenu)
        {
            if (package.Element != null)
            {
                string stereotype = package.Element.Stereotype;
                if (stereotype != null)
                {
                    if ((stereotype.Equals(CCTS_Types.BIELibrary.ToString())))
                    {
                        VIENNAAddInTreeViewMenu.Add("Create new &ABIE");
                    }
                    else if ((stereotype.Equals(CCTS_Types.BDTLibrary.ToString())))
                    {
                        VIENNAAddInTreeViewMenu.Add("Create new BD&T");
                    }
                }
            }
        }
    }
}