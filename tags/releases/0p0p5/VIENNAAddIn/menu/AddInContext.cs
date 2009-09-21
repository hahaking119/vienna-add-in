using System;
using System.Windows.Forms;
using EA;
using VIENNAAddIn.upcc3.ccts.dra;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.menu
{
    ///<summary>
    /// Contains the current context of the Add-In. Note that only a subset of the properties is set, depending on which API method that created the context.
    ///</summary>
    public class AddInContext
    {
        public AddInContext(Repository eaRepository, string menuLocation)
        {
            EARepository = eaRepository;

            MenuLocation = (MenuLocation) Enum.Parse(typeof (MenuLocation), menuLocation);

            if (MenuLocation == MenuLocation.TreeView)
            {
                /// Workaround to fix problem in Enterprise Architect:
                /// The "EA_OnContextItemChanged" method is not invoked in case the user
                /// selects a model in the tree view which causes SelectedItem
                /// to contain an invalid value. Therefore we override the values of the variables whenever the 
                /// user selects a package in the tree view. 
                SelectedItem = EARepository.GetTreeSelectedObject();
            }
            else
            {
                // Cannot simply use EARepository.GetContextObject(), because this method always returns the TreeSelectedObject!
                // (which probably is a bug)
                object contextObject;
                EARepository.GetContextItem(out contextObject);
                SelectedItem = contextObject;
            }
        }

        ///<summary>
        /// The EA repository.
        ///</summary>
        public Repository EARepository { get; private set; }

        ///<summary>
        /// The current menu location.
        ///</summary>
        public MenuLocation MenuLocation { get; private set; }

        ///<summary>
        /// A CCRepository wrapped around the EA repository.
        ///</summary>
        public CCRepository CCRepository
        {
            get { return new CCRepository(EARepository); }
        }

        ///<summary>
        /// The currently selected item.
        ///</summary>
        public object SelectedItem { get; private set; }

        public bool SelectedItemIsLibraryOfType(string stereotype)
        {
            return (SelectedItem as Package).IsA(stereotype);
        }

        public bool SelectedItemIsABIE()
        {
            return (SelectedItem as Element).IsABIE();
        }

        public bool SelectedItemIsRootModel()
        {
            var selectedPackage = SelectedItem as Package;
            return selectedPackage != null && selectedPackage.ParentID == 0;
        }
    }
}