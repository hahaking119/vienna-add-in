/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;
using System.Diagnostics;
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
using VIENNAAddIn.validator.upcc3.onTheFly;
using VIENNAAddIn.workflow;
using MenuItem=VIENNAAddIn.menu.MenuItem;
using Stereotype=VIENNAAddIn.upcc3.ccts.util.Stereotype;
using Timer=System.Timers.Timer;

namespace VIENNAAddIn
{
    ///<summary>
    ///</summary>
    [Guid("ADFF62A3-BEB5-4f39-9F79-560989B6E48B"),
     ClassInterface(ClassInterfaceType.None),
     ComSourceInterfaces(typeof (VIENNAAddInEvents))]
    public class VIENNAAddIn : VIENNAAddInInterface
    {
        private static readonly AddInContext context = new AddInContext();
        private static Repository Repo;
        private readonly MenuManager menuManager;
        private OnTheFlyValidator onTheFlyValidator;

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

            MenuAction createUPCCStructure =
                "&Create initial UPCC3 model structure".OnClick(UpccModelWizardForm.ShowForm);
            MenuAction createABIE = "Create new &ABIE".OnClick(ABIEWizardForm.ShowABIEWizard);
            MenuAction createBDT = "Create new BD&T".OnClick(BDTWizardForm.ShowBDTWizard);
            MenuItem modifyABIE = "&Modify ABIE".OnClick(ABIEWizardForm.ShowModifyABIEWizard).Enabled(IfABIEIsSelected);
            MenuAction validate = "&Validate".OnClick(ValidatorForm.ShowValidator);
            MenuItem _____ = MenuItem.Separator;

            menuManager[MenuLocation.MainMenu] =
                (AddInSettings.AddInName
                 +
                 "&Set Model as UMM2/UPCC3 Model".OnClick(ToggleUmm2ModelState).Checked(IfRepositoryIsUmm2Model).Enabled
                     (Always)
                 + createUPCCStructure
                 + "&Create initial UMM 2 model structure".OnClick(InitialPackageStructureCreator.ShowForm)
                 + _____
                 + "&Validate All - UPCC3".OnClick(ValidatorForm.ShowUpccValidator)
                 + "&Validate All - UMM2".OnClick(ValidatorForm.ShowUmmValidator)
                 + _____
                 + ("Maintenance"
                    + "Synchronize &Tagged Values...".OnClick(SynchStereotypesForm.ShowForm)
                   )
                 + ("Wizards"
                    + createABIE
                    + modifyABIE
                    + createBDT
                    + "Generate &XML Schema".OnClick(GeneratorWizardForm.ShowGeneratorWizard)
                    + "&Import XML Schemas".OnClick(ImporterWizardForm.ShowImporterWizard)
                   )
                 + "&Options".OnClick(OptionsForm.ShowForm)
                 + ("&About " + AddInSettings.AddInName).OnClick(AboutWindow.ShowForm)
                );
            menuManager[ContextIsBLibrary] =
                (AddInSettings.AddInName
                 + "Import Standard CC Libraries".OnClick(ImportStandardCcLibraries)
                );
            menuManager[ContextIsBIELibrary] =
                (AddInSettings.AddInName
                 + createABIE
                 + validate
                );
            menuManager[ContextIsABIE] =
                (AddInSettings.AddInName
                 + modifyABIE
                );
            menuManager[ContextIsBDTLibrary] =
                (AddInSettings.AddInName
                 + createBDT
                 + validate
                );
            menuManager[IsRootModel] =
                (AddInSettings.AddInName
                 + createUPCCStructure
                );
            menuManager[MenuLocation.TreeView | MenuLocation.Diagram] =
                (AddInSettings.AddInName
                 + validate
                );
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
                context.Repository = repository;
                context.MenuLocationString = menulocation;
                context.MenuName = menuname;
                context.MenuItem = itemname;
                OverrideSelectedItemIfPackageSelectedInTreeView(repository);
                menuManager.GetMenuState(context, ref isEnabled, ref isChecked);
            }
        }

        ///<summary>
        ///</summary>
        ///<param name="repository"></param>
        ///<param name="GUID"></param>
        ///<param name="ot"></param>
        ///<returns></returns>
        public bool EA_OnNotifyContextItemModified(Repository repository, string GUID, ObjectType ot)
        {
//            onTheFlyValidator.ProcessItemModified(GUID, ot);
            return true;
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
            repository.CreateOutputTab(AddInSettings.AddInName);
            onTheFlyValidator = new OnTheFlyValidatorImpl(repository);
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
            try
            {
            context.Repository = repository;
            context.MenuLocationString = menulocation;
            context.MenuName = menuname;
            OverrideSelectedItemIfPackageSelectedInTreeView(repository);
            return menuManager.GetMenuItems(context);
            }
            catch (Exception e)
            {
                new ErrorReporterForm(e.Message + "\n" + e.StackTrace, Repo.LibraryVersion);
                if (menulocation == AddInSettings.AddInName)
                {
                    return new string[0];
                }
                return new[] {AddInSettings.AddInName};
            }
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
            try
            {
                context.Repository = repository;
                context.MenuLocationString = menulocation;
                context.MenuName = menuname;
                context.MenuItem = menuitem;
                OverrideSelectedItemIfPackageSelectedInTreeView(repository);
                menuManager.MenuClick(context);
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
            object contextItem;
            context.SelectedItemObjectType = repository.GetContextItem(out contextItem);
            context.SelectedItem = contextItem;
        }

        ///<summary>
        ///</summary>
        ///<param name="repository"></param>
        ///<param name="tabName"></param>
        ///<param name="text"></param>
        ///<param name="id"></param>
        public void EA_OnOutputItemClicked(Repository repository, string tabName, string text, int id)
        {
            if (tabName == AddInSettings.AddInName)
            {
                onTheFlyValidator.FocusIssueItem(id);
            }
        }

        ///<summary>
        ///</summary>
        ///<param name="repository"></param>
        ///<param name="tabName"></param>
        ///<param name="text"></param>
        ///<param name="id"></param>
        public void EA_OnOutputItemDoubleClicked(Repository repository, string tabName, string text, int id)
        {
            if (tabName == AddInSettings.AddInName)
            {
                onTheFlyValidator.ShowQuickFixes(id);
            }
        }

        ///<summary>
        ///</summary>
        ///<param name="repository"></param>
        ///<param name="info"></param>
        ///<returns></returns>
        public bool EA_OnPostNewElement(Repository repository, EventProperties info)
        {
            foreach (EventProperty eventProperty in info)
            {
                if (eventProperty.Name == "ElementID")
                {
                    int elementId = int.Parse(eventProperty.Value.ToString());
                    onTheFlyValidator.ProcessElementCreated(elementId);
                    return false;
                }
            }
            return false;
        }

        #endregion

        #region AddInContext Predicates

        private static bool ContextIsABIE(AddInContext context)
        {
            return context.MenuLocation.IsContextMenu()
                   && context.SelectedItem != null
                   && context.SelectedItemObjectType == ObjectType.otElement
                   && ((Element) context.SelectedItem).IsABIE();
        }

        private static bool ContextIsLibrary(AddInContext context, string stereotype)
        {
            return context.MenuLocation.IsContextMenu()
                   && context.SelectedItem != null
                   && context.SelectedItemObjectType == ObjectType.otPackage
                   && ((Package) context.SelectedItem).HasStereotype(stereotype);
        }

        private static bool ContextIsBLibrary(AddInContext context)
        {
            return ContextIsLibrary(context, Stereotype.BLibrary);
        }

        private static bool ContextIsBIELibrary(AddInContext context)
        {
            return ContextIsLibrary(context, Stereotype.BIELibrary);
        }

        private static bool ContextIsBDTLibrary(AddInContext context)
        {
            return ContextIsLibrary(context, Stereotype.BDTLibrary);
        }

        private static bool IsRootModel(AddInContext context)
        {
            return context.SelectedItem != null && context.SelectedItemObjectType == ObjectType.otPackage && ((Package) context.SelectedItem).IsModel;
        }

        private static bool IfABIEIsSelected(AddInContext context)
        {
            return context.SelectedItemObjectType == ObjectType.otElement && ((Element) context.SelectedItem).IsABIE();
        }

        private static bool IfRepositoryIsUmm2Model(AddInContext context)
        {
            return context.Repository.IsUmm2Model();
        }

        private static bool Always(AddInContext context)
        {
            return true;
        }

        private static bool Never(AddInContext context)
        {
            return false;
        }

        #endregion

        /// <summary>
        /// Workaround to fix problem in Enterprise Architect:
        /// The "EA_OnContextItemChanged" method is not invoked in case the user
        /// selects a model in the tree view which causes SelectedItemXXX members of the AddInContext
        /// to contain invalid values. Therefore we override the values of the variables whenever the 
        /// user selects a package in the tree view. 
        /// </summary>
        /// <param name="repository"></param>
        private static void OverrideSelectedItemIfPackageSelectedInTreeView(Repository repository)
        {
            if (context.MenuLocation == MenuLocation.TreeView)
            {
                var treeSelectedItemType = repository.GetTreeSelectedItemType();
                if (treeSelectedItemType == ObjectType.otPackage)
                {
                    context.SelectedItem = repository.GetTreeSelectedObject();
                    context.SelectedItemObjectType = treeSelectedItemType;
                }
            }
        }

        private static void ToggleUmm2ModelState(AddInContext context)
        {
            context.Repository.ToggleUmm2ModelState();
        }

        private static void ImportStandardCcLibraries(AddInContext context)
        {
            context.Repository.ImportStandardCcLibraries();
        }
    }
}