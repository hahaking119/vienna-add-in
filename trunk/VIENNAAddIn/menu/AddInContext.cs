using System;
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
            MenuLocation = (MenuLocation)Enum.Parse(typeof(MenuLocation), menuLocation);
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
        /// The type of the currently selected item.
        ///</summary>
        private ObjectType SelectedItemObjectType
        {
            get { return EARepository.GetContextItemType(); }
        }

        private object selectedItem;

        ///<summary>
        /// The currently selected item.
        ///</summary>
        public object SelectedItem
        {
            get
            {
                if (selectedItem == null)
                {
//                    MessageBox.Show("loading selected item");
                    if (MenuLocation == MenuLocation.TreeView && SelectedItemObjectType == ObjectType.otNone)
                    {
                        /// Workaround to fix problem in Enterprise Architect:
                        /// The "EA_OnContextItemChanged" method is not invoked in case the user
                        /// selects a model in the tree view which causes SelectedItem
                        /// to contain an invalid value. Therefore we override the values of the variables whenever the 
                        /// user selects a package in the tree view. 
                        selectedItem = EARepository.GetTreeSelectedObject();
                    }
                    else
                    {
                        selectedItem = EARepository.GetContextObject();
                    }
                }
                return selectedItem;
            }
        }

        public bool SelectedItemIsLibraryOfType(string stereotype)
        {
            return SelectedItem != null
                   && SelectedItemObjectType == ObjectType.otPackage
                   && ((Package) SelectedItem).HasStereotype(stereotype);
        }

        public bool SelectedItemIsABIE()
        {
            return SelectedItem != null
                   && SelectedItemObjectType == ObjectType.otElement
                   && ((Element) SelectedItem).IsABIE();
        }

        public bool SelectedItemIsRootModel()
        {
            return SelectedItem != null 
                   && SelectedItem is Package
                   && ((Package) SelectedItem).ParentID == 0;
        }
    }
}