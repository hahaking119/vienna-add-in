using System;
using System.Collections.Generic;
using EA;
using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddIn.Utils;

namespace VIENNAAddIn.menu
{
    ///<summary>
    /// The Menu Manager handles all menu-related EA-Add-In events. The main objective for creating this 
    /// class was to have a way to specify menu structures in a hierarchical and concise way while minimizing 
    /// code duplication.
    /// 
    /// <para>
    /// <b>Defining menu structures:</b>
    /// </para>
    /// <para>
    /// Menu structures are defined in a hierarchical syntax. For example, 
    /// the following code defines a "File" menu with four menu items and a separator 
    /// (the "&amp;" designates the item's hot key):
    /// <code>
    /// var menu = ("File" 
    ///             + "&amp;New".OnClick(CreateFile)
    ///             + "&amp;Open".OnClick(OpenFile)
    ///             + "&amp;Close".OnClick(CloseFile)
    ///             + MenuItem.Separator 
    ///             + "&amp;Exit".OnClick(Exit)
    ///            );
    /// </code>
    /// When clicked, each of the menu items invokes the delegate specified with <see cref="MenuStringExtensions.OnClick"/>. 
    /// For example:
    /// <code>
    /// public void OpenFile(AddInContext context)
    /// {
    ///   // display open file dialog
    /// }
    /// </code>
    /// </para>
    /// 
    /// <para>
    /// <b>Sub-menus</b> can be nested in the menu structure definition as follows:
    /// <code>
    /// var menu = ("File" 
    ///             + ("&amp;New"
    ///                + "&amp;File".OnClick(CreateFile)
    ///                + "&amp;Project".OnClick(CreateProject)
    ///               )
    ///             + "&amp;Open".OnClick(OpenFile)
    ///             + "&amp;Close".OnClick(CloseFile)
    ///             + MenuItem.Separator 
    ///             + "&amp;Exit".OnClick(Exit)
    ///            );
    /// </code>
    /// </para>
    /// 
    /// <para>
    /// The <b>enabled/disabled</b> state of a menu item is determined at runtime via the delegate specified with <see cref="MenuItem.Enabled"/>.
    /// For sub-menus, <see cref="MenuItem.Enabled"/> must be specified after the closing parenthesis. For example:
    /// <code>
    /// var menu = ("File" 
    ///             + ("&amp;New"
    ///                + "&amp;File".OnClick(CreateFile)
    ///                + "&amp;Project".OnClick(CreateProject)
    ///               ).Enabled(IfANewFileCanBeCreated)
    ///             + "&amp;Open".OnClick(OpenFile)
    ///             + "&amp;Close".OnClick(CloseFile).Enabled(IfAFileIsOpen)
    ///             + MenuItem.Separator 
    ///             + "&amp;Exit".OnClick(Exit)
    ///            );
    /// 
    /// ...
    /// 
    /// public bool IfAFileIsOpen(AddInContext context)
    /// {
    ///   return (openFile != null);
    /// }
    /// </code>
    /// </para>
    /// 
    /// <para>
    /// The <b>checked</b> state of a menu item is determined at runtime via the delegate specified with <see cref="MenuAction.Checked"/>.
    /// For example:
    /// <code>
    /// var menu = ("File" 
    ///             ...
    ///             + "Allow multiple open files".OnClick(ToggleMultipleOpenFilesAllowed).Checked(IfMultipleOpenFilesAllowed)
    ///             ...
    ///            );
    /// 
    /// ...
    /// 
    /// public void ToggleMultipleOpenFilesAllowed(AddInContext context)
    /// {
    ///   multipleOpenFilesAllowed = !multipleOpenFilesAllowed;
    /// }
    /// 
    /// public bool IfMultipleOpenFilesAllowed(AddInContext context)
    /// {
    ///   return multipleOpenFilesAllowed;
    /// }
    /// </code>
    /// </para>
    /// 
    /// <para>
    /// <b>
    /// Assigning menu structures to contexts:
    /// </b>
    /// </para>
    /// <para>
    /// We want to be able to display different menu structures in different contexts. Currently, we distinguish 
    /// contexts along the following dimensions:
    /// <list type="bullet">
    /// <item>
    /// <b>Menu Location</b><br/>
    /// EA provides the menu location as an argument to the relevant API functions. Possible values are "MainMenu", 
    /// "TreeView" and "Diagram", which are also represented in the <see cref="MenuLocation"/> enum.
    /// </item>
    /// <item>
    /// <b>Package stereotypes</b><br/>
    /// We also use the package stereotype of either the selected package or the containing package of the selected 
    /// element or diagram for context determination.
    /// </item>
    /// </list>
    /// </para>
    /// <para>
    /// Menu contexts are evaluated in the order they are defined. The menu assigned to the first matching context is displayed.<br/>
    /// </para>
    /// <para>
    /// Menu structures are assigned to contexts as follows:
    /// <code>
    /// var menuManager = new MenuManager();
    /// menuManager[MenuLocation.MainMenu] = ("My main menu" + ...);
    /// menuManager[MenuLocation.TreeView, "BDTLibrary"] = ("Context menu for BDT Libraries" + ...);
    /// menuManager[MenuLocation.TreeView, "BIELibrary"] = ("Context menu for BIE Libraries" + ...);
    /// menuManager[MenuLocation.TreeView] = ("Context menu if none of the above match" + ...);
    /// </code>
    /// </para>
    /// <para>
    /// Additionally, a context can be defined with an arbitrary Predicate&lt;<see cref="AddInContext"/>&gt;:
    /// <code>
    /// menuManager[MyOwnContext] = ("Context menu for my own context" + ...);
    /// ...
    /// public bool MyOwnContext(AddInContext context)
    /// {
    ///   return /* context matches my requirements */;
    /// }
    /// </code>
    /// </para>
    /// 
    /// <para>
    /// <b>Default settings:</b>
    /// </para>
    /// <para>
    /// The following default settings must be specified as MenuManager properties:
    /// <list type="bullet">
    /// <item><see cref="DefaultEnabled"/>: A predicate to determine the enabled/disabled state of items where <see cref="MenuItem.Enabled"/> is not explicitely set.</item>
    /// <item><see cref="DefaultChecked"/>: A predicate to determine the checked state of items where <see cref="MenuAction.Checked"/> is not explicitely set.</item>
    /// <item><see cref="DefaultMenuItems"/>: A string[] containing the menu items to be displayed when no context matches.</item>
    /// </list>
    /// Example:
    /// <code>
    /// var menuManager = new MenuManager
    ///                   {
    ///                     DefaultMenuItems = new[] {"My Add-In"},
    ///                     DefaultEnabled = Always,
    ///                     DefaultChecked = Never
    ///                   };
    /// 
    /// ...
    /// 
    /// public bool Always(AddInContext)
    /// {
    ///   return true;
    /// }
    /// 
    /// public bool Never(AddInContext)
    /// {
    ///   return false;
    /// }
    /// </code>
    /// </para>
    /// 
    /// <para>
    /// See <see cref="VIENNAAddIn"/> or MenuManagerTest for real-life menu definitions.
    /// </para>
    /// 
    /// <para>
    /// <b>Limitations:</b>
    /// </para>
    /// <para>
    /// Due to a bug in Enterprise Architect's EA_MenuClick event, the menu item is selected only based on the menu name and 
    /// menu item name (e.g. menu name: "File", menu item name: "Close"). The menu location is ignored, because EA does not 
    /// consistently provide the correct menu location.
    /// </para>
    ///</summary>
    public class MenuManager
    {
        private readonly Dictionary<string, Dictionary<string, Action<AddInContext>>> menuActions =
            new Dictionary<string, Dictionary<string, Action<AddInContext>>>();

        private readonly List<MenuItems> menuItems = new List<MenuItems>();

        private readonly Dictionary<string, Dictionary<string, MenuState>> menuStates =
            new Dictionary<string, Dictionary<string, MenuState>>();

        #region Default settings

        ///<summary>
        /// The default menu items to be displayed if no menu is defined for the current context.
        ///</summary>
        public string[] DefaultMenuItems { get; set; }

        ///<summary>
        /// The default predicate to determine the enabled/disabled state of menu items that have no explicit predicate set.
        ///</summary>
        public Predicate<AddInContext> DefaultEnabled { get; set; }

        ///<summary>
        /// The default predicate to determine the checked state of menu items that have no explicit predicate set.
        ///</summary>
        public Predicate<AddInContext> DefaultChecked { get; set; }

        #endregion

        #region Indexers for defining menu contexts

        ///<summary>
        /// Use this indexer to assign a menu to a context matched by an arbitrary predicate.
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
        /// Use this indexer to assign a menu to a context matching the given <see cref="MenuLocation"/> and package stereotype.
        ///</summary>
        ///<param name="menuLocation">One or more menu locations (joined with the | operator)</param>
        ///<param name="packageStereotype">The stereotype of the selected package or the package containing the selected element or diagram.</param>
        public MenuItem this[MenuLocation menuLocation, string packageStereotype]
        {
            set
            {
                Predicate<AddInContext> predicate = context =>
                                                        {
                                                            if ((menuLocation & context.MenuLocation) ==
                                                                context.MenuLocation)
                                                            {
                                                                ObjectType otype = context.SelectedItemObjectType;

                                                                if (otype.Equals(ObjectType.otPackage))
                                                                {
                                                                    Package obj =
                                                                        context.Repository.GetPackageByGuid(
                                                                            context.SelectedItemGUID);
                                                                    return obj.HasStereotype(packageStereotype);
                                                                }
                                                                if (otype.Equals(ObjectType.otElement))
                                                                {
                                                                    Element obj =
                                                                        context.Repository.GetElementByGuid(
                                                                            context.SelectedItemGUID);
                                                                    return
                                                                        context.Repository.GetPackageByID(obj.PackageID)
                                                                            .HasStereotype(packageStereotype);
                                                                }
                                                                if (otype.Equals(ObjectType.otDiagram))
                                                                {
                                                                    var obj =
                                                                        (Diagram)
                                                                        context.Repository.GetDiagramByGuid(
                                                                            context.SelectedItemGUID);
                                                                    return
                                                                        context.Repository.GetPackageByID(obj.PackageID)
                                                                            .HasStereotype(packageStereotype);
                                                                }
                                                            }
                                                            return false;
                                                        };
                this[predicate] = value;
            }
        }

        ///<summary>
        /// Use this indexer to assign a menu to a context matching the given <see cref="MenuLocation"/>.
        ///</summary>
        ///<param name="menuLocation">One or more menu locations (joined with the | operator)</param>
        public MenuItem this[MenuLocation menuLocation]
        {
            set { this[context => (menuLocation & context.MenuLocation) == context.MenuLocation] = value; }
        }

        #endregion

        #region EA Menu API

        ///<summary>
        /// Should be called from EA_GetMenuState.
        ///</summary>
        ///<param name="context">The current context.</param>
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

        ///<summary>
        /// Should be called from EA_GetMenuItems.
        ///</summary>
        ///<param name="context"></param>
        ///<returns></returns>
        public string[] GetMenuItems(AddInContext context)
        {
            foreach (MenuItems items in menuItems)
            {
                if (items.Matches(context))
                {
                    return items.Items;
                }
            }
            return DefaultMenuItems;
        }

        ///<summary>
        /// Should be called from EA_MenuClick.
        ///</summary>
        ///<param name="context"></param>
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

        #endregion

        /// <summary>
        /// Recursively adds the given menuItem and its children to the various lookup structures of the menu manager.
        /// </summary>
        /// <param name="predicate">A predicate to determine in which context the menuItem should be displayed.</param>
        /// <param name="menuName">The name of the parent menu.</param>
        /// <param name="menuItem">The menu item to process.</param>
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
                menuActions.GetAndCreate(menuName)[menuItem.Name] = menuAction.Execute;
            }
            menuStates.GetAndCreate(menuName)[menuItem.Name] = menuState;
        }

        /// <summary>
        /// Returns a predicate matching contexts where <see cref="AddInContext.MenuName"/> equals the given menu name.
        /// </summary>
        /// <param name="menuName"></param>
        /// <returns></returns>
        private static Predicate<AddInContext> MenuNameIs(string menuName)
        {
            return context => (menuName ?? string.Empty) == context.MenuName;
        }

        ///<summary>
        /// Returns false.
        ///</summary>
        ///<param name="context"></param>
        ///<returns></returns>
        private static bool Never(AddInContext context)
        {
            return false;
        }
    }

    /// <summary>
    /// Simple container for a context matching predicate and a list of menu items.
    /// </summary>
    internal class MenuItems
    {
        public MenuItems(Predicate<AddInContext> predicate, List<string> items)
        {
            Matches = predicate;
            Items = items.ToArray();
        }

        public Predicate<AddInContext> Matches { get; private set; }

        public string[] Items { get; private set; }
    }

    /// <summary>
    /// Simple container for enabled/disabled and checked predicates for a menu item.
    /// </summary>
    internal class MenuState
    {
        public Predicate<AddInContext> IsEnabled { get; set; }
        public Predicate<AddInContext> IsChecked { get; set; }
    }
}