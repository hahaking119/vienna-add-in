using System.Runtime.InteropServices;
using EA;

namespace VIENNAAddIn
{
    ///<summary>
    ///</summary>
    [Guid("AC600C85-5BFE-45d5-9D5C-EEE1B5BE852B")]
    public interface VIENNAAddInInterface
    {
        /// <summary>
        /// Connect
        /// </summary>
        /// <param name="repository"></param>
        /// <returns></returns>
        string EA_Connect(Repository repository);

        /// <summary>
        /// Disconnect
        /// </summary>
        void EA_Disconnect(Repository repository);

        /// <summary>
        /// Open File
        /// </summary>
        /// <param name="repository"></param>
        void EA_FileOpen(Repository repository);

        /// <summary>
        /// Get Menu-Items
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="menulocation"></param>
        /// <param name="menuname"></param>
        /// <returns></returns>
        string[] EA_GetMenuItems(Repository repository, string menulocation, string menuname);

        ///<summary>
        ///</summary>
        ///<param name="repository"></param>
        ///<returns></returns>
        object OnInitializeTechnologies(Repository repository);

        /// <summary>
        /// Menu Click
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="menulocation"></param>
        /// <param name="menuname"></param>
        /// <param name="menuitem"></param>
        void EA_MenuClick(Repository repository, string menulocation, string menuname, string menuitem);

        /// <summary>
        /// Menu State
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="menulocation"></param>
        /// <param name="menuname"></param>
        /// <param name="menuitem"></param>
        /// <param name="IsEnabled"></param>
        /// <param name="IsChecked"></param>
        void EA_GetMenuState(Repository repository, string menulocation, string menuname, string menuitem,
                             ref bool IsEnabled, ref bool IsChecked);

//        bool EA_OnNotifyContextItemModified(EA.Repository repository, string GUID, EA.ObjectType ot);
        void EA_OnContextItemChanged(EA.Repository repository, string GUID, EA.ObjectType ot);
//        bool EA_OnContextItemDoubleClicked(EA.Repository repository, string GUID, EA.ObjectType ot);

    }
}