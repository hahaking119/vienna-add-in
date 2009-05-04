using System;
using EA;
using VIENNAAddIn.upcc3.ccts.dra;

namespace VIENNAAddIn.menu
{
    ///<summary>
    /// Contains the current context of the Add-In. Note that only a subset of the properties is set, depending on which API method that created the context.
    ///</summary>
    public class AddInContext
    {
        ///<summary>
        /// The EA repository.
        ///</summary>
        public Repository Repository { get; set; }

        ///<summary>
        /// The current menu location.
        ///</summary>
        public MenuLocation MenuLocation { get; set; }

        ///<summary>
        /// The current menu location as a string.
        ///</summary>
        public string MenuLocationString
        {
            get { return MenuLocation.ToString(); }
            set { MenuLocation = (MenuLocation) Enum.Parse(typeof (MenuLocation), value); }
        }

        ///<summary>
        /// A CCRepository wrapped around the EA repository.
        ///</summary>
        public CCRepository CCRepository
        {
            get { return new CCRepository(Repository); }
        }

        private string menuName;
        ///<summary>
        /// The current menu name.
        ///</summary>
        public string MenuName
        {
            get { return menuName??string.Empty; }
            set { menuName = value; }
        }

        ///<summary>
        /// The current menu item.
        ///</summary>
        public string MenuItem { get; set; }

        ///<summary>
        /// The type of the currently selected item.
        ///</summary>
        public ObjectType SelectedItemObjectType { get; set; }

        ///<summary>
        /// The GUID of the currently selected item.
        ///</summary>
        public string SelectedItemGUID { get; set; }
    }
}