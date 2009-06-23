using System;
using System.Collections.Generic;
using EA;
using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddIn.Utils;

namespace VIENNAAddIn.menu
{
    ///<summary>
    /// <para>
    /// The MenuManager handles all menu-related EA-Add-In events. The main objective for creating this 
    /// class was to have a way to specify menu structures in a hierarchical and concise way while minimizing 
    /// code duplication.
    /// </para>
    /// 
    /// <hr/>
    /// 
    /// <para>
    /// <b>Defining menu structures:</b>
    /// </para>
    /// 
    /// <para>
    /// A menu consists of <b>actions</b>, <b>sub-menus</b> and <b>separators</b>.
    /// </para>
    /// 
    /// <para>
    /// <b>Actions</b> can be defined with the <see cref="MenuStringExtensions.OnClick"/> string extension method 
    /// (or with <see cref="MenuAction"/>'s constructor). The following example defines an action
    /// with the name "Open":
    /// 
    /// <code>
    /// var action = "Open".OnClick(OpenFile)
    /// 
    /// ...
    /// 
    /// public void OpenFile(AddInContext context)
    /// {
    ///     // display open file dialog
    /// }
    /// </code>
    /// <br/>
    /// 
    /// As shown in the example, the argument of the OnClick() method is a method (or delegate) that has one argument of 
    /// type <see cref="AddInContext"/>.
    /// </para>
    /// 
    /// <para>
    /// At runtime, an action can be either <b>enabled or disabled</b>. This is determined via
    /// a delegate that is set with the <see cref="MenuAction.Enabled"/> method. In the following example, we make sure
    /// that the "File"/"Close" menu action is only enabled if a file is currently open:
    /// 
    /// <code>
    /// var action = "Close".OnClick(CloseFile).Enabled(IfAFileIsOpen)
    /// 
    /// ...
    /// 
    /// public bool IfAFileIsOpen(AddInContext context)
    /// {
    ///   return openFile != null;
    /// }
    /// </code>
    /// <br/>
    /// 
    /// As shown in the example, the argument of the Enabled() method is a method (or delegate) that has one argument of 
    /// type <see cref="AddInContext"/> and returns a <c>bool</c> (i.e. it is a predicate).
    /// </para>
    /// 
    /// <para>
    /// Note that it is possible to set a default delegate for Enabled(), which will be applied to all menu actions that
    /// where no specific delegate has been set (see default settings below).
    /// </para>
    /// 
    /// <para>
    /// Similar to the enabled/disabled state, an action can be either <b>checked or unchecked</b>. This is 
    /// determined via a delegate that is set with the <see cref="MenuAction.Checked"/> method, as can be seen in the following example:
    /// 
    /// <code>
    /// var action = "Allow multiple open files".OnClick(ToggleMultipleOpenFilesAllowed).Checked(IfMultipleOpenFilesAllowed)
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
    /// <br/>
    /// 
    /// As shown in the example, the argument of the Checked() method is a method (or delegate) that has one argument of 
    /// type <see cref="AddInContext"/> and returns a <c>bool</c> (i.e. it is a predicate).
    /// </para>
    /// 
    /// <para>
    /// Note that it is possible to set a default delegate for Checked(), which will be applied to all menu actions that
    /// where no specific delegate has been set (see default settings below).
    /// </para>
    /// 
    /// <para>
    /// <b>Sub-Menus</b> are defined using a special syntax (based on overloading the "+" operator). For example, let's define
    /// a "File" sub-menu:
    /// 
    /// <code>
    /// var menu = ("File" 
    ///             + "New".OnClick(CreateFile)
    ///             + "Open".OnClick(OpenFile)
    ///             + "Close".OnClick(CloseFile)
    ///             + "Exit".OnClick(Exit)
    ///            );
    /// </code>
    /// <br/>
    /// The first string becomes the sub-menu's name. Menu items are then added with the "+" operator. Note that the parenthesis
    /// could be omitted in this case.
    /// </para>
    /// 
    /// <para>
    /// Sub-menus can also be nested, as shown by the following example (in this case, the nested menu must be 
    /// enclosed in parenthesis):
    /// 
    /// <code>
    /// var menu = ("File" 
    ///             + ("New"
    ///                + "File".OnClick(CreateFile)
    ///                + "Project".OnClick(CreateProject)
    ///               )
    ///             + "Open".OnClick(OpenFile)
    ///             + "Close".OnClick(CloseFile)
    ///             + "Exit".OnClick(Exit)
    ///            );
    /// </code>
    /// <br/>
    /// </para>
    /// 
    /// <para>
    /// <b>Separators</b> are added using <see cref="MenuItem.Separator"/>.
    /// 
    /// <code>
    /// var menu = ("File" 
    ///             + "New".OnClick(CreateFile)
    ///             + "Open".OnClick(OpenFile)
    ///             + "Close".OnClick(CloseFile)
    ///             + MenuItem.Separator
    ///             + "Exit".OnClick(Exit)
    ///            );
    /// </code>
    /// <br/>
    /// </para>
    /// 
    /// <para>
    /// <b>Putting it all together:</b>
    /// </para>
    /// 
    /// <para>
    /// Here is an example of a complete "File" menu with actions, nested sub-menus and separators:
    /// 
    /// <code>
    /// var menu = ("&amp;File" 
    ///             + ("&amp;New"
    ///                + "&amp;File".OnClick(CreateFile)
    ///                + "&amp;Project".OnClick(CreateProject)
    ///               )
    ///             + "&amp;Open".OnClick(OpenFile)
    ///             + "&amp;Close".OnClick(CloseFile)
    ///             + MenuItem.Separator
    ///             + "Allow multiple open files".OnClick(ToggleMultipleOpenFilesAllowed).Checked(IfMultipleOpenFilesAllowed)
    ///             + MenuItem.Separator 
    ///             + "&amp;Exit".OnClick(Exit)
    ///            );
    /// </code>
    /// <br/>
    /// The resulting menu structure will look like this (arrows indicate a sub-menu relation):
    /// <code>
    /// File --> New ------------------------> File
    ///          Open                          Project
    ///          Close
    ///          ------
    ///          Allow multiple open files
    ///          ------
    ///          Exit
    /// </code>
    /// </para>
    /// 
    /// <hr/>
    /// 
    /// <para>
    /// <b>Assigning menu structures to contexts:</b>
    /// </para>
    /// 
    /// <para>
    /// The previous section described how menu structures can be defined. However, we want to be able to display 
    /// different menu structures in different contexts. Currently, we distinguish contexts along the following two 
    /// dimensions:
    /// 
    /// <list type="bullet">
    /// <item>
    /// <b>Menu Location</b><br/>
    /// EA provides the menu location as an argument to the relevant API functions.
    /// </item>
    /// <item>
    /// <b>Package stereotypes</b><br/>
    /// We also use the package stereotype of either the selected package or the containing package of the selected 
    /// element or diagram for context determination.
    /// </item>
    /// </list>
    /// 
    /// Menu contexts are defined using indexers ("[]") of the MenuManager class.
    /// </para>
    /// 
    /// <para>
    /// <b>Assigning a menu to a menu location:</b>
    /// </para>
    /// 
    /// <para>
    /// MenuManager defines an indexer to assign a menu structure to a menu location. For example:
    /// 
    /// <code>
    /// var menuManager = new MenuManager();
    /// menuManager[MenuLocation.MainMenu] = ("My main menu"
    ///                                       + "Do something".OnClick(DoSomething)
    ///                                       + "Do something else".OnClick(DoSomethingElse)
    ///                                      );
    /// </code>
    /// <br/>
    /// 
    /// This code assigns the "My main menu" menu to the "MainMenu" menu location. This means that this menu will be displayed as
    /// a sub-menu of the "Add-Ins" menu in EA's main menu bar.
    /// </para>
    /// <para>
    /// Possible menu locations are (see also <see cref="MenuLocation"/>):
    /// <list type="bullet">
    /// <item><b>MainMenu:</b> The menu is displayed as a sub-menu of the "Add-Ins" menu in EA's main menu bar.</item>
    /// <item><b>TreeView:</b> The menu is displayed as a sub-menu of the "Add-In" menu in the tree view's context menu.</item>
    /// <item><b>Diagram:</b> The menu is displayed as a sub-menu of the "Add-Ins" menu in a diagram's context menu.</item>
    /// </list>
    /// 
    /// Menu locations can be combined using the "|" operator:
    /// 
    /// <code>
    /// var menuManager = new MenuManager();
    /// menuManager[MenuLocation.TreeView | MenuLocation.Diagram] = ("My context menu"
    ///                                                              + "Do something".OnClick(DoSomething)
    ///                                                              + "Do something else".OnClick(DoSomethingElse)
    ///                                                             );
    /// </code>
    /// <br/>
    /// In this case, the defined menu will be shown in the context menu of both diagrams and the tree view.
    /// </para>
    /// 
    /// <para>
    /// <b>Assigning a menu to a package stereotype:</b>
    /// </para>
    /// 
    /// <para>
    /// In addition to the menu location, we also want to be able to display different kinds of context menus for
    /// different kinds of selected elements. Currently, we only distinguish package stereotypes. This means, if a package
    /// is selected, we look at its stereotype. If a diagram or package element is selected, we look at the stereotype of the
    /// package containing that diagram or element. The package stereotype can be specified as a second parameter to the indexer:
    /// 
    /// <code>
    /// var menuManager = new MenuManager();
    /// menuManager[MenuLocation.TreeView, "BDTLibrary"] = ("BDTLibrary context menu"
    ///                                                      + "Do something specific to BDT libraries".OnClick(DoSomeBDTStuff)
    ///                                                      + "Do something else".OnClick(DoSomethingElse)
    ///                                                     );
    /// </code>
    /// <br/>
    /// This menu will only be displayed in the context menu of the tree view and only if a BDTLibrary is currently selected (or a 
    /// diagram/element within a BDT library).
    /// </para>
    /// 
    /// <para>
    /// <b>Defining arbitrary menu contexts:</b>
    /// </para>
    /// 
    /// <para>
    /// A third indexer allows for the definition of arbitrary contexts, based on a predicate (i.e. a method that takes an 
    /// <see cref="AddInContext"/> argument and returns a bool):
    /// 
    /// <code>
    /// var menuManager = new MenuManager();
    /// menuManager[MySpecialContext] = ("Context menu for my special context"
    ///                                  + "Do something".OnClick(DoSomething)
    ///                                 );
    /// ...
    /// public bool MySpecialContext(AddInContext context)
    /// {
    ///   return /* context matches my requirements */;
    /// }
    /// </code>
    /// </para>
    /// 
    /// <para>
    /// <b>Evaluation of menu contexts:</b>
    /// </para>
    /// 
    /// <para>
    /// Menu contexts are evaluated in the order they are defined. The menu assigned to the first matching context is displayed.
    /// For example, consider the following definitions:
    /// 
    /// <code>
    /// var menuManager = new MenuManager();
    /// menuManager[MenuLocation.TreeView, "BDTLibrary"] = ("Context menu for BDT Libraries" + ...);
    /// menuManager[MenuLocation.TreeView] = ("Context menu if none of the above match" + ...);
    /// </code>
    /// <br/>
    /// In this case, if the user right-clicks on a BDTLibrary in the tree view, the first menu will be displayed. The second menu 
    /// will be displayed for other kinds of libraries. However, if the definition order is reversed, the special menu for BDTLibraries
    /// will never be displayed, because the more general context will always match first:
    /// 
    /// <code>
    /// var menuManager = new MenuManager();
    /// menuManager[MenuLocation.TreeView] = ("Context menu if none of the above match" + ...);
    /// menuManager[MenuLocation.TreeView, "BDTLibrary"] = ("Context menu for BDT Libraries" + ...);
    /// </code>
    /// <br/>
    /// </para>
    /// 
    /// <hr/>
    /// 
    /// <para>
    /// <b>Default settings:</b>
    /// </para>
    /// 
    /// <para>
    /// The following default settings must be specified as MenuManager properties:
    /// <list type="bullet">
    /// <item><see cref="DefaultEnabled"/>: A predicate to determine the enabled/disabled state of actions where <see cref="MenuAction.Enabled"/> is not explicitely set.</item>
    /// <item><see cref="DefaultChecked"/>: A predicate to determine the checked state of actions where <see cref="MenuAction.Checked"/> is not explicitely set.</item>
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
    /// public bool Always(AddInContext context)
    /// {
    ///   return true;
    /// }
    /// 
    /// public bool Never(AddInContext context)
    /// {
    ///   return false;
    /// }
    /// </code>
    /// </para>
    /// 
    /// <hr/>
    /// 
    /// <para>
    /// <b>Limitations:</b>
    /// </para>
    /// 
    /// <para>
    /// Due to a bug in Enterprise Architect's EA_MenuClick event, the menu item is selected only based on the menu name and 
    /// menu item name (e.g. menu name: "File", menu item name: "Close"). The menu location is ignored, because EA does not 
    /// consistently provide the correct menu location.
    /// <br/>
    /// Basically, this means that a combination of menu name and menu item should trigger the same action, no matter in which
    /// menu it is defined. Example:
    /// 
    /// <code>
    /// var menuManager = new MenuManager();
    /// menuManager[MenuLocation.MainMenu] = ("File"
    ///                                       + "Open".OnClick(OpenFile)
    ///                                      );
    /// menuManager[MenuLocation.TreeView] = ("File"
    ///                                       + "Open".OnClick(OpenFile)
    ///                                      );
    /// </code>
    /// <br/>
    /// In this case, both "File"/"Open" actions invoke the same delegate, no matter whether the user clicked on the main menu
    /// or tree view menu item. This is the required behaviour. If they invoke different delegates, there is no guarantee as to 
    /// which delegate will be invoked.
    /// </para>
    /// 
    /// <hr/>
    /// 
    /// <para>
    /// See <see cref="VIENNAAddIn"/> or MenuManagerTest for real-life menu definitions.
    /// </para>
    ///</summary>
    public class MenuManager
    {
        private readonly Dictionary<string, Dictionary<string, MenuAction>> menuActions =
            new Dictionary<string, Dictionary<string, MenuAction>>();

        private readonly List<MenuItems> menuItems = new List<MenuItems>();

        private readonly MenuAction defaultAction = string.Empty.OnClick(DoNothing).Checked(Never).Enabled(Always);

        private static void DoNothing(AddInContext context)
        {
            // do nothing
        }

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
                menuItems.Add(new MenuItems(MenuNameIs(null).And(predicate), new List<string>
                                                                             {
                                                                                 value.Name
                                                                             }));
                ProcessMenuItem(predicate, string.Empty, value);
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
            var menuAction = GetMenuAction(context);
            isEnabled = menuAction.IsEnabled == null ? DefaultEnabled(context) : menuAction.IsEnabled(context);
            isChecked = menuAction.IsChecked == null ? DefaultChecked(context) : menuAction.IsChecked(context);
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
            GetMenuAction(context).Execute(context);
        }

        private MenuAction GetMenuAction(AddInContext context)
        {
            Dictionary<string, MenuAction> menu;
            if (menuActions.TryGetValue(context.MenuName, out menu))
            {
                MenuAction action;
                if (menu.TryGetValue(context.MenuItem, out action))
                {
                    return action;
                }
            }
            return defaultAction;
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
            if (menuItem is SubMenu)
            {
                var subMenu = (SubMenu) menuItem;
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
                menuActions.GetAndCreate(menuName)[menuItem.Name] = (MenuAction) menuItem;
            }
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
        private static bool Always(AddInContext context)
        {
            return true;
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