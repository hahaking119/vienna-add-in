using System;
using System.Collections.Generic;

namespace VIENNAAddIn.menu
{
    ///<summary>
    ///</summary>
    public class MenuItem
    {
        public readonly static MenuItem Separator = new MenuItem("-");
        public virtual string Name { get; private set; }

        public MenuItem(string name)
        {
            Name = name;
        }

        public static SubMenu operator +(string lhs, MenuItem rhs)
        {
            var menu = new SubMenu(lhs);
            return menu.AddItem(rhs);
        }

        public static List<MenuItem> operator +(MenuItem lhs, MenuItem rhs)
        {
            return new List<MenuItem> {lhs, rhs};
        }

        public static List<MenuItem> operator +(List<MenuItem> lhs, MenuItem rhs)
        {
            return new List<MenuItem>(lhs) {rhs};
        }

        public virtual string[] GetMenuItems(AddInContext context)
        {
            return null;
        }

        public virtual MenuAction GetMenuAction(AddInContext context)
        {
            return null;
        }
    }

    public class SubMenu:MenuItem
    {
        public SubMenu(string name) : base(name)
        {
        }

        public override string Name
        {
            get { return "-" + base.Name; }
        }

        private string[] menuItems;
        private List<MenuItem> items = new List<MenuItem>();

        public List<MenuItem> Items
        {
            get { return items; }
        }

        public SubMenu AddItem(MenuItem item)
        {
            items.Add(item);
            return this;
        }

        public static SubMenu operator +(SubMenu lhs, MenuItem rhs)
        {
            return lhs.AddItem(rhs);
        }

        public override string[] GetMenuItems(AddInContext context)
        {
            if (Name.Equals(context.MenuName))
            {
                if (menuItems == null)
                {
                    menuItems = items.ConvertAll(item => item.Name).ToArray();
                }
                return menuItems;
            }
            foreach (MenuItem item in items)
            {
                var subMenuItems = item.GetMenuItems(context);
                if (subMenuItems != null)
                {
                    return subMenuItems;
                }
            }
            return null;
        }

        public override MenuAction GetMenuAction(AddInContext context)
        {
            if (Name.Equals(context.MenuName))
            {
                foreach (MenuItem item in items)
                {
                    if (item is MenuAction && item.Name.Equals(context.MenuItem))
                    {
                        return (MenuAction) item;
                    }
                }
            }
            foreach (MenuItem item in items)
            {
                if (item is SubMenu)
                {
                    var menuAction = item.GetMenuAction(context);
                    if (menuAction != null)
                    {
                        return menuAction;
                    }
                }
            }
            return null;
        }
    }
}