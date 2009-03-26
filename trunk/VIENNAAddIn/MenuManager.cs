using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using EA;
using VIENNAAddIn.constants;

namespace VIENNAAddIn
{
    ///<summary>
    ///</summary>
    public class MenuManager
    {
//        private static readonly SubMenu emptyMenu = new SubMenu();

        private readonly string[] topLevelMenu;
        private readonly string topLevelMenuName;

        private SubMenu mainMenu;
        private SubMenu diagramMenu;
        private Dictionary<string, SubMenu> treeViewPackageMenus;

        ///<summary>
        ///</summary>
        ///<param name="topLevelMenuName"></param>
        public MenuManager(string topLevelMenuName)
        {
            this.topLevelMenuName = topLevelMenuName;
            topLevelMenu = new[] {"-" + topLevelMenuName};
        }

        ///<summary>
        ///</summary>
        ///<param name="context"></param>
        ///<param name="menuLocation"></param>
        ///<param name="menuName"></param>
        ///<returns></returns>
        public string[] GetMenuItems(AddInContext context, string menuLocation, string menuName)
        {
            if (menuName == String.Empty)
            {
                return topLevelMenu;
            }
            return null; //GetSubMenu(menuLocation, menuName).GetMenuItems();
        }

        ///<summary>
        ///</summary>
        ///<param name="items"></param>
        public void SetMainMenu(params MenuItem[] items)
        {
            mainMenu = new SubMenu(topLevelMenuName, items);
        }

        ///<summary>
        ///</summary>
        ///<param name="items"></param>
        public void SetDiagramMenu(params MenuItem[] items)
        {
            diagramMenu = new SubMenu(topLevelMenuName, items);
        }

        ///<summary>
        ///</summary>
        ///<param name="items"></param>
        public void SetTreeViewPackageMenu(string stereotype, params MenuItem[] items)
        {
            treeViewPackageMenus[stereotype] = new SubMenu(topLevelMenuName, items);
        }

        ///<summary>
        ///</summary>
        ///<param name="items"></param>
        public void SetTreeViewPackageMenu(string[] stereotypes, params MenuItem[] items)
        {
            foreach (var stereotype in stereotypes)
            {
                SetTreeViewPackageMenu(stereotype, items);
            }
        }

        ///<summary>
        ///</summary>
        ///<param name="context"></param>
        ///<param name="menuLocation"></param>
        ///<param name="menuName"></param>
        ///<param name="menuItem"></param>
        ///<exception cref="ArgumentException"></exception>
        public void MenuClick(AddInContext context, string menuLocation, string menuName, string menuItem)
        {
            MenuAction action = null; //GetSubMenu(menuLocation, menuName).FindAction(menuItem);
            if (action == null)
            {
                throw new ArgumentException(string.Format("menu item not found: {0}/{1}/{2}", menuLocation, menuName,
                                                          menuItem));
            }
            action.Execute(context);
        }

        private SubMenu GetSubMenu(AddInContext context, string menuLocation, string menuName)
        {
            if (!menuName.StartsWith("-"))
            {
                throw new ArgumentException("sub-menu name does not start with '-': " + menuName);
            }
            SubMenu menu;
            switch (menuLocation)
            {
                case "MainMenu":
                    menu = mainMenu;
                    break;
                case "TreeView":
                    Object obj;
                    ObjectType otype = context.Repository.GetTreeSelectedItem(out obj);

                    if (otype.Equals(ObjectType.otPackage))
                    {
                        var package = (Package) obj;
                        String stereotype = package.Element.Stereotype;
                        menu = treeViewPackageMenus[stereotype];
//                        if (menu == null)
//                        {
//                            menu = emptyMenu;
//                        }
                    }
                    break;
                case "Diagram":
                    menu = diagramMenu;
                    break;
                default:
                    throw new ArgumentException("unknown menu location: " + menuLocation);
            }
            string subMenuName = menuName.Substring(1);
            SubMenu subMenu = null;// menu.FindSubMenu(subMenuName);
            if (subMenu == null)
            {
                throw new ArgumentException(string.Format("sub-menu not found: {0}/{1}", menuLocation, menuName));
            }
            return subMenu;
        }
    }

    ///<summary>
    ///</summary>
    public class SubMenu : MenuItem
    {
        private readonly List<MenuItem> items;

        ///<summary>
        ///</summary>
        ///<param name="name"></param>
        public SubMenu(string name) : this(name, new MenuItem[0])
        {
        }

        ///<summary>
        ///</summary>
        ///<param name="name"></param>
        ///<param name="items"></param>
        public SubMenu(string name, MenuItem[] items) : base(name)
        {
            this.items = new List<MenuItem>(items);
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
            SubMenu result = null;
            foreach (MenuItem item in items)
            {
                if (item is SubMenu)
                {
                    result = ((SubMenu) item).FindSubMenu(name);
                    if (result != null)
                    {
                        break;
                    }
                }
            }
            return result;
        }

        ///<summary>
        ///</summary>
        ///<param name="name"></param>
        ///<returns></returns>
        public MenuAction FindAction(string name)
        {
            foreach (MenuItem item in items)
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
            var menuItems = new List<string>(items.Count);
            foreach (MenuItem item in items)
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
    }

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

    ///<summary>
    ///</summary>
    public class MenuSeparator : MenuItem
    {
        ///<summary>
        ///</summary>
        public MenuSeparator() : base("-")
        {
        }
    }

    ///<summary>
    ///</summary>
    public abstract class MenuItem
    {
        protected MenuItem(string name)
        {
            Name = name;
        }

        ///<summary>
        ///</summary>
        public string Name { get; private set; }
    }

    ///<summary>
    ///</summary>
    public class AddInContext
    {
        ///<summary>
        ///</summary>
        public Repository Repository { get; set; }
    }
}