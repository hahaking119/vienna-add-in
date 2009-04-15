using System;
using EA;
using VIENNAAddIn.upcc3.ccts.dra;

namespace VIENNAAddIn.menu
{
    ///<summary>
    ///</summary>
    public class AddInContext
    {
        ///<summary>
        ///</summary>
        public Repository Repository { get; set; }

        public MenuLocation MenuLocation { get; set; }

        public string MenuLocationString
        {
            get { return MenuLocation.ToString(); }
            set { MenuLocation = (MenuLocation) Enum.Parse(typeof (MenuLocation), value); }
        }

        public CCRepository CCRepository
        {
            get { return new CCRepository(Repository); }
        }

        private string menuName;
        public string MenuName
        {
            get { return menuName??string.Empty; }
            set { menuName = value; }
        }

        public string MenuItem { get; set; }

        public ObjectType SelectedItemObjectType { get; set; }

        public string SelectedItemGUID { get; set; }
    }
}