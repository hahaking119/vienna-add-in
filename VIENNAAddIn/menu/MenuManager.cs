using System;
using System.Collections.Generic;
using EA;

namespace VIENNAAddIn.menu
{
    ///<summary>
    ///</summary>
    public class MenuManager
    {
        private readonly Dictionary<string, SubMenu> diagramMenus = new Dictionary<string, SubMenu>();
        private readonly Dictionary<string, SubMenu> elementMenus = new Dictionary<string, SubMenu>();
        private readonly Dictionary<string, SubMenu> packageMenus = new Dictionary<string, SubMenu>();
        private readonly string[] topLevelMenu;
        private readonly string topLevelMenuName;

        ///<summary>
        ///</summary>
        ///<param name="topLevelMenuName"></param>
        public MenuManager(string topLevelMenuName)
        {
            this.topLevelMenuName = topLevelMenuName;
            topLevelMenu = new[] {"-" + topLevelMenuName};
            MainMenu = new SubMenu(topLevelMenuName);
            AllPackages = new SubMenu(topLevelMenuName);
            AllElements = new SubMenu(topLevelMenuName);
            AllDiagrams = new SubMenu(topLevelMenuName);
        }

        ///<summary>
        ///</summary>
        public SubMenu MainMenu { get; private set; }

        ///<summary>
        ///</summary>
        public SubMenu AllPackages { get; private set; }

        ///<summary>
        ///</summary>
        public SubMenu AllElements { get; private set; }

        ///<summary>
        ///</summary>
        public SubMenu AllDiagrams { get; private set; }

        ///<summary>
        ///</summary>
        ///<param name="context"></param>
        ///<param name="menuLocation"></param>
        ///<param name="menuName"></param>
        ///<returns></returns>
        public string[] GetMenuItems(AddInContext context, string menuLocation, string menuName)
        {
            return menuName == String.Empty ? topLevelMenu : GetSubMenu(context, menuLocation, menuName).GetMenuItems();
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
            MenuAction action = GetSubMenu(context, menuLocation, menuName).FindAction(menuItem);
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
            string subMenuName = menuName.Substring(1);
            SubMenu menu = SubMenu.EmptyMenu;
            switch (menuLocation)
            {
                case "MainMenu":
                    menu = MainMenu;
                    break;
                case "TreeView":
                    Object obj;
                    ObjectType otype = context.Repository.GetTreeSelectedItem(out obj);

                    if (otype.Equals(ObjectType.otPackage))
                    {
                        String stereotype = ((Package) obj).Element.Stereotype;
                        menu = packageMenus.ContainsKey(stereotype) ? packageMenus[stereotype] : AllPackages;
                    }
                    else if (otype.Equals(ObjectType.otElement))
                    {
                        var stereotype = context.Repository.GetPackageByID(((Element) obj).PackageID).Element.Stereotype;
                        menu = elementMenus.ContainsKey(stereotype) ? elementMenus[stereotype] : AllElements;
                    }
                    else if (otype.Equals(ObjectType.otDiagram))
                    {
                        var stereotype = context.Repository.GetPackageByID(((Diagram)obj).PackageID).Element.Stereotype;
                        menu = diagramMenus.ContainsKey(stereotype) ? diagramMenus[stereotype] : AllDiagrams;
                    }

                    break;
                case "Diagram":
                    menu = SubMenu.EmptyMenu;
                    break;
                default:
                    throw new ArgumentException("unknown menu location: " + menuLocation);
            }
            SubMenu subMenu = menu.FindSubMenu(subMenuName);
            if (subMenu == null)
            {
                throw new ArgumentException(string.Format("sub-menu not found: {0}/{1}", menuLocation, menuName));
            }
            return subMenu;
        }

        ///<summary>
        ///</summary>
        ///<param name="stereotypes"></param>
        ///<returns></returns>
        public StereotypeMenuBuilder ForPackagesWithStereotypes(params string[] stereotypes)
        {
            var builder = new StereotypeMenuBuilder();
            foreach (string stereotype in stereotypes)
            {
                builder.AddMenu(packageMenus[stereotype] = new SubMenu(topLevelMenuName, AllPackages));
            }
            return builder;
        }

        ///<summary>
        ///</summary>
        ///<param name="stereotypes"></param>
        ///<returns></returns>
        public StereotypeMenuBuilder ForElementsWithPackageStereotypes(params string[] stereotypes)
        {
            var builder = new StereotypeMenuBuilder();
            foreach (string stereotype in stereotypes)
            {
                builder.AddMenu(elementMenus[stereotype] = new SubMenu(topLevelMenuName, AllElements));
            }
            return builder;
        }

        ///<summary>
        ///</summary>
        ///<param name="stereotypes"></param>
        ///<returns></returns>
        public StereotypeMenuBuilder ForDiagramsWithPackageStereotypes(params string[] stereotypes)
        {
            var builder = new StereotypeMenuBuilder();
            foreach (string stereotype in stereotypes)
            {
                builder.AddMenu(diagramMenus[stereotype] = new SubMenu(topLevelMenuName, AllDiagrams));
            }
            return builder;
        }
    }

    ///<summary>
    ///</summary>
    public class StereotypeMenuBuilder
    {
        private readonly List<SubMenu> menus = new List<SubMenu>();

        ///<summary>
        ///</summary>
        ///<param name="menu"></param>
        public void AddMenu(SubMenu menu)
        {
            menus.Add(menu);
        }

        ///<summary>
        ///</summary>
        ///<param name="items"></param>
        public void SetItems(params MenuItem[] items)
        {
            foreach (SubMenu menu in menus)
            {
                menu.SetItems(items);
            }
        }
    }
}