using System;

namespace VIENNAAddIn.menu
{
    ///<summary>
    ///</summary>
    public class MenuAction : MenuItem
    {
        ///<summary>
        ///</summary>
        ///<param name="name"></param>
        ///<param name="action"></param>
        public MenuAction(string name, Action<AddInContext> action) : base(name)
        {
            Execute = action;
        }

        ///<summary>
        ///</summary>
        public Action<AddInContext> Execute { get; private set; }
    }
}