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

        public MenuItem Enabled(Predicate<AddInContext> isEnabled)
        {
            IsEnabled = isEnabled;
            return this;
        }

        public Predicate<AddInContext> IsEnabled { get; private set; }
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


    }
}