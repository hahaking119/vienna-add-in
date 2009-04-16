/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using EA;
using VIENNAAddIn.ErrorReporter;
using VIENNAAddIn.Exceptions;
using VIENNAAddIn.menu;
using VIENNAAddIn.Settings;
using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddIn.upcc3.Wizards;
using VIENNAAddIn.Utils;
using VIENNAAddIn.validator;
using VIENNAAddIn.workflow;
using MenuItem=VIENNAAddIn.menu.MenuItem;
using Stereotype=VIENNAAddIn.upcc3.ccts.util.Stereotype;

namespace VIENNAAddIn
{
    ///<summary>
    ///</summary>
    [Guid("ADFF62A3-BEB5-4f39-9F79-560989B6E48B"),
     ClassInterface(ClassInterfaceType.None),
     ComSourceInterfaces(typeof (VIENNAAddInEvents))]
    public class VIENNAAddIn : VIENNAAddInInterface
    {
        private static Repository Repo;
        private static string SelectedItemGUID;
        private static ObjectType SelectedItemObjectType;
        private readonly MenuManager menuManager;

        ///<summary>
        ///</summary>
        public VIENNAAddIn()
        {
            menuManager = new MenuManager
                          {
                              DefaultMenuItems = new[] {AddInSettings.AddInName},
                              DefaultEnabled = IfRepositoryIsUmm2Model,
                              DefaultChecked = Never
                          };

            var createABIE = "Create new &ABIE".OnClick(ShowABIEWizard);
            var createBDT = "Create new BD&T".OnClick(ShowBDTWizard);
            var modifyABIE = "&Modify ABIE".OnClick(ShowModifyABIEWizard).Enabled(ABIEIsSelected);
            var validate = "&Validate".OnClick(ShowValidator);
            var _____ = MenuItem.Separator;

            menuManager[MenuLocation.MainMenu] =
                (AddInSettings.AddInName
                 + "&Set Model as UMM2/UPCC3 Model".OnClick(ToggleUmm2ModelState).Checked(IfRepositoryIsUmm2Model).Enabled(Always)
                 + "&Create initial UPCC3 model structure".OnClick(ShowInitialUPCCPackageStructureCreator)
                 + "&Create initial UMM 2 model structure".OnClick(ShowInitialPackageStructureCreator)
                 + _____
                 + "&Validate All - UPCC3".OnClick(ShowUpccValidator)
                 + "&Validate All - UMM2".OnClick(ShowUmmValidator)
                 + _____
                 + ("Maintenance"
                    + "Synchronize &Tagged Values...".OnClick(ShowSynchStereotypes)
                   )
                 + ("Wizards"
                    + createABIE
                    + modifyABIE
                    + createBDT
                    + "Generate &XML Schema".OnClick(ShowGeneratorWizard)
                   )
                 + "&Options".OnClick(ShowOptions)
                 + ("&About " + AddInSettings.AddInName).OnClick(ShowAbout)
                );
            menuManager[MenuLocation.TreeView | MenuLocation.Diagram, Stereotype.BIELibrary] =
                (AddInSettings.AddInName
                 + createABIE
                 + modifyABIE
                 + _____
                 + validate
                );
            menuManager[MenuLocation.TreeView | MenuLocation.Diagram, Stereotype.BDTLibrary] =
                (AddInSettings.AddInName
                 + createBDT
                 + _____
                 + validate
                );
            menuManager[MenuLocation.TreeView | MenuLocation.Diagram] =
                (AddInSettings.AddInName
                 + validate
                );
        }

        private void ShowInitialUPCCPackageStructureCreator(AddInContext context)
        {
            new UpccModelWizardForm(context.Repository).Show();
        }

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
            if (Repo == null)
            {
                isEnabled = false;
                isChecked = false;
            }
            else
            {
                menuManager.GetMenuState(new AddInContext
                                         {
                                             Repository = Repo,
                                             MenuLocationString = menulocation,
                                             MenuName = menuname,
                                             MenuItem = itemname,
                                             SelectedItemGUID = SelectedItemGUID,
                                             SelectedItemObjectType = SelectedItemObjectType
                                         }, ref isEnabled, ref isChecked);
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
            Repo = repository;
            Repo.EnableCache = true;
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
            return menuManager.GetMenuItems(new AddInContext
                                            {
                                                Repository = Repo,
                                                MenuLocationString = menulocation,
                                                MenuName = menuname,
                                                SelectedItemGUID = SelectedItemGUID,
                                                SelectedItemObjectType = SelectedItemObjectType
                                            });
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
                menuManager.MenuClick(new AddInContext
                                      {
                                          Repository = Repo,
                                          MenuLocationString = menulocation,
                                          MenuName = menuname,
                                          MenuItem = menuitem,
                                          SelectedItemGUID = SelectedItemGUID,
                                          SelectedItemObjectType = SelectedItemObjectType
                                      });
            }
            catch (Exception e)
            {
                new ErrorReporterForm(e.Message + "\n" + e.StackTrace, Repo.LibraryVersion);
            }
        }

        ///<summary>
        ///</summary>
        ///<param name="repository"></param>
        ///<param name="GUID"></param>
        ///<param name="ot"></param>
        ///<returns></returns>
        public void EA_OnContextItemChanged(Repository repository, string GUID, ObjectType ot)
        {
            SelectedItemObjectType = ot;
            SelectedItemGUID = GUID;
        }

        #endregion

        private static void ShowValidator(AddInContext context)
        {
            ValidatorForm.ShowForm(context.Repository, null,
                                   context.MenuLocationString);
        }

        private static bool ABIEIsSelected(AddInContext context)
        {
            return context.SelectedItemObjectType == ObjectType.otElement &&
                   Repo.GetElementByGuid(context.SelectedItemGUID).IsABIE();
        }

        private static void ShowAbout(AddInContext context)
        {
            new AboutWindow().Show();
        }

        private static void ShowOptions(AddInContext context)
        {
            OptionsForm.ShowForm(context.Repository);
        }

        private static void ShowGeneratorWizard(AddInContext context)
        {
            new GeneratorWizardForm(context.CCRepository).Show();
        }

        private static void ShowBDTWizard(AddInContext context)
        {
            new BDTWizardForm(context.Repository).Show();
        }

        private static void ShowABIEWizard(AddInContext context)
        {
            new ABIEWizardForm(context.Repository).Show();
        }

        private static void ShowModifyABIEWizard(AddInContext context)
        {
            new ABIEWizardForm(context.Repository, context.Repository.GetElementByGuid(SelectedItemGUID)).Show();
        }

        private static void ShowSynchStereotypes(AddInContext context)
        {
            SynchStereotypesForm.ShowForm(context.Repository);
        }

        private static void ShowUmmValidator(AddInContext context)
        {
            ValidatorForm.ShowForm(context.Repository, "ROOT_UMM", context.MenuLocationString);
        }

        private static void ShowUpccValidator(AddInContext context)
        {
            ValidatorForm.ShowForm(context.Repository, "ROOT_UPCC", context.MenuLocationString);
        }

        private static void ShowInitialPackageStructureCreator(AddInContext context)
        {
            InitialPackageStructureCreator.ShowForm(context.Repository);
        }

        private static bool Always(AddInContext context)
        {
            return true;
        }

        private static bool Never(AddInContext context)
        {
            return false;
        }

        private static bool IfRepositoryIsUmm2Model(AddInContext context)
        {
            return context.Repository.IsUmm2Model();
        }

        private static void ToggleUmm2ModelState(AddInContext context)
        {
            context.Repository.ToggleUmm2ModelState();
        }
    }
}