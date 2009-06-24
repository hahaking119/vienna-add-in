using System;
using System.Collections.Generic;

namespace VIENNAAddIn.menu
{
    ///<summary>
    /// <para>
    /// The MenuManager handles all menu-related EA-Add-In events. The main objective for creating this 
    /// class was to have a way to specify menu structures in a hierarchical and concise way while minimizing 
    /// code duplication. Also, the EA Menu API is a bit complicated (and perhaps even a bit buggy), so that
    /// it is not exactly trivial to handle the events correctly.
    /// </para>
    /// 
    /// <hr/>
    /// 
    /// <para>
    /// <b>The EA Menu API:</b>
    /// </para>
    /// 
    /// <para>
    /// This section briefly describes the EA Menu API so that the knowledge does not get lost. The API consist of 
    /// three events, that invoke the following functions:
    /// </para>
    /// <code>
    /// string[] EA_GetMenuItems(Repository repository, string menuLocation, string menuName);
    /// void     EA_MenuClick   (Repository repository, string menuLocation, string menuName, string menuItem);
    /// void     EA_GetMenuState(Repository repository, string menuLocation, string menuName, string menuItem, ref bool IsEnabled, ref bool IsChecked);
    /// </code>
    /// 
    /// <para>
    /// The first argument always contains a reference to the EA repository.
    /// </para>
    /// <para>
    /// The second argument specifies the menu location, which is one of {"MainMenu", "TreeView", "Diagram"} (see <see cref="MenuLocation"/>).
    /// Now this part seems to be a bit buggy, since the menu location is only correct for the top level element of a menu tree, but this is handled
    /// by the implementation of the MenuManager.
    /// </para>
    /// <para>
    /// The remaining arguments have different meanings for the three events and will be explained later.
    /// </para>
    /// <para>
    /// So the basic question is: How does EA use these events to work with add-in menus? I will try to explain this by listing the steps for displaying
    /// a menu and (possibly) clicking one of the menu items. The steps are always the same and the MenuManager handles the events according to these steps.
    /// 
    /// <list type="number">
    /// <item>
    /// When the user clicks on the "Add-Ins" menu item in the main menu bar or right-clicks a repository element (such as a package or class) elsewhere 
    /// in the user interface, EA invokes EA_GetMenuItems with the correct menu location and an empty menuname. The function must return the top-level menu items
    /// of the add-in (in our case, a single item "VIENNAAddIn").
    /// </item>
    /// <item>
    /// <para>
    /// Then EA invokes the function again for each sub-menu, providing the sub-menu name as an argument 
    /// and expecting the items of the sub-menu as return value. As noted above, the menu location for these invocations is not correct (it seems to always be "Diagram", 
    /// even if the correct location is "MainMenu" or "TreeView"). In this way, EA constructs the entire menu tree at the time the user clicks on the main menu or 
    /// repository element (i.e. it does not wait to build sub-menus only when the user points to them).
    /// </para>
    /// <para>
    /// At some point during this process, EA also invokes EA_GetMenuState for each menu item in order to determine whether the item is enabled and/or checked. Note that
    /// sub-menus cannot be disabled or checked (only click-able items). I have not bothered to find out the exact times when this function is invoked.
    /// </para>
    /// </item>
    /// <item>
    /// If the user dismisses the menu (by clicking elsewhere), no further events are generated.
    /// </item>
    /// <item>
    /// If the user clicks on a menu item (other than a sub-menu), EA invokes EA_MenuClick. Again, the menu location is only correct for the top-level items. The menu name is 
    /// either empty or the name of the sub-menu wherein the menu item resides. The menu item is the name of the clicked item. Note that this implies that the combination of
    /// sub-menu name and menu item name must be unique within a given menu hierarchy, because it is the only information we have to decide which action to execute.
    /// </item>
    /// <item>
    /// The whole process begins again when the user clicks again on the main menu or right-clicks a repository element.
    /// </item>
    /// </list>
    /// </para>
    /// 
    /// <para>
    /// <b>Consequences for the implementation:</b> This behavior has the following consequences for the implementation of the menu manager:
    /// <list type="bullet">
    /// <item>
    /// The first (top-level) invocation of EA_GetMenuItems must be used to select the appropriate menu tree. This selection can depend on the menu location and on arbitrary predicates that
    /// can be defined or information in the repository (e.g. we can define a special menu for right-click context menus for BIE libraries in the tree view). We can recognize that
    /// an invocation is the first invocation, by checking whether the menu name is empty.
    /// </item>
    /// <item>
    /// All other invocations of the menu API operate on the menu tree selected in that first invocation. There is no way around this, since the menu location is not correct for these
    /// subsequent invocations. Also we can improve performance be evaluating the predicates only once for an entire menu tree.
    /// </item>
    /// <item>
    /// The selected menu tree is valid until the next top-level invocation of EA_GetMenuItems.
    /// </item>
    /// </list>
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
        private readonly MenuAction defaultAction = string.Empty.OnClick(DoNothing).Checked(Never).Enabled(Always);

        private readonly Dictionary<Predicate<AddInContext>, MenuItem> menus = new Dictionary<Predicate<AddInContext>, MenuItem>();

        private MenuItem activeMenu;

        private static void DoNothing(AddInContext context)
        {
            // do nothing
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
            set { menus[predicate] = value; }
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
            MenuAction menuAction = GetMenuAction(context);
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
            if (IsInitialInvocation(context))
            {
                activeMenu = null;
                foreach (var menu in menus)
                {
                    Predicate<AddInContext> menuMatches = menu.Key;
                    if (menuMatches(context))
                    {
                        activeMenu = menu.Value;
                        return new[] {activeMenu.Name};
                    }
                }
            }
            if (activeMenu == null)
            {
                return DefaultMenuItems;
            }
            return activeMenu.GetMenuItems(context) ?? DefaultMenuItems;
        }

        private static bool IsInitialInvocation(AddInContext context)
        {
            return string.IsNullOrEmpty(context.MenuName);
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
            if (activeMenu == null)
            {
                return defaultAction;
            }
            return activeMenu.GetMenuAction(context) ?? defaultAction;
        }

        #endregion
    }
}