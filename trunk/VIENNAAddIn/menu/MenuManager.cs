using System;
using System.Collections.Generic;
using EA;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.menu
{
    ///<summary>
    ///</summary>
    public class MenuManager
    {
        private readonly Dictionary<string, Dictionary<string, MenuState>> menuStates =
            new Dictionary<string, Dictionary<string, MenuState>>();

        private readonly Dictionary<string, Dictionary<string, Action<AddInContext>>> menuActions =
            new Dictionary<string, Dictionary<string, Action<AddInContext>>>();

        private readonly List<MenuItems> menuItems = new List<MenuItems>();

        public string[] DefaultMenuItems { get; set; }
        public Predicate<AddInContext> DefaultEnabled { get; set; }
        public Predicate<AddInContext> DefaultChecked { get; set; }

        ///<summary>
        ///</summary>
        ///<param name="context"></param>
        ///<param name="isEnabled"></param>
        ///<param name="isChecked"></param>
        public void GetMenuState(AddInContext context, ref bool isEnabled, ref bool isChecked)
        {
            Dictionary<string, MenuState> menu;
            if (menuStates.TryGetValue(context.MenuName, out menu))
            {
                MenuState menuState;
                if (menu.TryGetValue(context.MenuItem, out menuState))
                {
                    isEnabled = menuState.IsEnabled == null ? DefaultEnabled(context) : menuState.IsEnabled(context);
                    isChecked = menuState.IsChecked == null ? DefaultChecked(context) : menuState.IsChecked(context);
                    return;
                }
            }
            isEnabled = false;
            isChecked = false;
        }

        private void ProcessMenuItem(Predicate<AddInContext> predicate, string menuName, MenuItem menuItem)
        {
            var menuState = new MenuState {IsEnabled = menuItem.IsEnabled};
            if (menuItem is SubMenu)
            {
                var subMenu = (SubMenu) menuItem;
                menuState.IsChecked = Never;
                var items = new List<string>();
                foreach (MenuItem item in subMenu.Items)
                {
                    items.Add(item.Name);
                    ProcessMenuItem(predicate, menuItem.Name, item);
                }
                menuItems.Add(new MenuItems(MenuNameIs(menuItem.Name).And(predicate), items));
            }
            else if (menuItem is MenuAction)
            {
                var menuAction = (MenuAction) menuItem;
                menuState.IsChecked = menuAction.IsChecked;
                AddMenuAction(menuName, menuItem.Name, menuAction.Execute);
            }
            AddMenuState(menuName, menuItem.Name, menuState);
        }

        private static Predicate<AddInContext> MenuNameIs(string menuName)
        {
            return context => (menuName??string.Empty) == context.MenuName;
        }

        ///<summary>
        ///</summary>
        ///<param name="context"></param>
        ///<returns></returns>
        public static bool Never(AddInContext context)
        {
            return false;
        }

        ///<summary>
        ///</summary>
        ///<param name="context"></param>
        ///<returns></returns>
        public string[] GetMenuItems(AddInContext context)
        {
            foreach (var items in menuItems)
            {
                if (items.Matches(context))
                {
                    return items.Items;
                }
            }
            return DefaultMenuItems;
        }

        ///<summary>
        ///</summary>
        ///<param name="context"></param>
        ///<exception cref="ArgumentException"></exception>
        public void MenuClick(AddInContext context)
        {
            Dictionary<string, Action<AddInContext>> menu;
            if (menuActions.TryGetValue(context.MenuName, out menu))
            {
                Action<AddInContext> action;
                if (menu.TryGetValue(context.MenuItem, out action))
                {
                    action(context);
                }
            }
        }

        private void AddMenuAction(string menuName, string menuItem, Action<AddInContext> action)
        {
            menuActions.GetAndCreate(menuName)[menuItem] = action;
        }

        private void AddMenuState(string menuName, string menuItem, MenuState menuState)
        {
            menuStates.GetAndCreate(menuName)[menuItem] = menuState;
        }

        ///<summary>
        ///</summary>
        ///<param name="predicate"></param>
        public MenuItem this[Predicate<AddInContext> predicate]
        {
            set
            {
                menuItems.Add(new MenuItems(MenuNameIs(null).And(predicate), new List<string> {value.Name}));
                ProcessMenuItem(predicate, string.Empty, value);
            }
        }

        ///<summary>
        ///</summary>
        ///<param name="menuLocation"></param>
        ///<param name="packageStereotype"></param>
        public MenuItem this[MenuLocation menuLocation, string packageStereotype]
        {
            set
            {
                this[context =>
                {
                    if ((menuLocation & context.MenuLocation) == context.MenuLocation)
                    {
                        ObjectType otype = context.SelectedItemObjectType;

                        if (otype.Equals(ObjectType.otPackage))
                        {
                            var obj = context.Repository.GetPackageByGuid(context.SelectedItemGUID);
                            return obj.HasStereotype(packageStereotype);
                        }
                        if (otype.Equals(ObjectType.otElement))
                        {
                            var obj = context.Repository.GetElementByGuid(context.SelectedItemGUID);
                            return context.Repository.GetPackageByID(obj.PackageID).HasStereotype(packageStereotype);
                        }
                        if (otype.Equals(ObjectType.otDiagram))
                        {
                            var obj = (Diagram) context.Repository.GetDiagramByGuid(context.SelectedItemGUID);
                            return context.Repository.GetPackageByID(obj.PackageID).HasStereotype(packageStereotype);
                        }
                    }
                    return false;
                }] = value;
            }
        }

        ///<summary>
        ///</summary>
        ///<param name="menuLocation"></param>
        public MenuItem this[MenuLocation menuLocation]
        {
            set
            {
                this[context => (menuLocation & context.MenuLocation) == context.MenuLocation] = value;
            }
        }
    }

    class MenuItems
    {
        public Predicate<AddInContext> Matches { get; private set; }

        public MenuItems(Predicate<AddInContext> predicate, List<string> items)
        {
            Matches = predicate;
            Items = items.ToArray();
        }

        public string[] Items { get; private set; }
    }

    class MenuState
    {
        public Predicate<AddInContext> IsEnabled { get; set; }

        public Predicate<AddInContext> IsChecked { get; set; }
    }

}