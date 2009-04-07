using System;
using System.Collections.Generic;

namespace VIENNAAddIn.menu
{
    ///<summary>
    ///</summary>
    public class SubMenu : MenuItem
    {
        private readonly SubMenu chained;
        private readonly List<MenuItem> items = new List<MenuItem>();

        ///<summary>
        ///</summary>
        ///<param name="name"></param>
        public SubMenu(string name) : this(name, null)
        {
        }

        public SubMenu(string name, SubMenu chained):base(name)
        {
            this.chained = chained;
        }

        private IEnumerable<MenuItem> Items
        {
            get
            {
                foreach (var item in items)
                {
                    yield return item;
                }
                if (chained != null)
                {
                    foreach (var item in chained.items)
                    {
                        yield return item;
                    }
                }
            }
        }

        ///<summary>
        ///</summary>
        ///<param name="items"></param>
        ///<returns></returns>
        public MenuItem SetItems(params MenuItem[] items)
        {
            this.items.Clear();
            this.items.AddRange(items);
            return this;
        }

        ///<summary>
        ///</summary>
        ///<param name="name"></param>
        ///<returns></returns>
        public SubMenu FindSubMenu(string name)
        {
            if (Name == name)
            {
                return this;
            }
            foreach (MenuItem item in Items)
            {
                if (item is SubMenu)
                {
                    SubMenu result = ((SubMenu) item).FindSubMenu(name);
                    if (result != EmptyMenu)
                    {
                        return result;
                    }
                }
            }
            return EmptyMenu;
        }

        ///<summary>
        ///</summary>
        ///<param name="name"></param>
        ///<returns></returns>
        public MenuAction FindAction(string name)
        {
            foreach (MenuItem item in Items)
            {
                if (item is MenuAction)
                {
                    var action = (MenuAction) item;
                    if (action.Name == name)
                    {
                        return action;
                    }
                }
            }
            return null;
        }

        ///<summary>
        ///</summary>
        ///<returns></returns>
        public string[] GetMenuItems()
        {
            var menuItems = new List<string>();
            foreach (MenuItem item in Items)
            {
                if (item is SubMenu)
                {
                    menuItems.Add("-" + item.Name);
                }
                else
                {
                    menuItems.Add(item.Name);
                }
            }
            return menuItems.ToArray();
        }

        internal static readonly SubMenu EmptyMenu = new SubMenu(String.Empty);
    }
}