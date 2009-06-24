using System.Collections.Generic;
using EA;
using Moq;
using NUnit.Framework;
using VIENNAAddIn.menu;
using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddInUnitTests.TestRepository;
using Stereotype=VIENNAAddIn.upcc3.ccts.util.Stereotype;

namespace VIENNAAddInUnitTests.menu
{
    [TestFixture]
    public class MenuManagerTest
    {
        private static AddInContext CreatePackageContext(string stereotype, string menuName, string menuItem)
        {
            Mock<Package> packageMock = CreatePackageMock(stereotype);
            var repositoryMock = new Mock<Repository>();
            repositoryMock.Setup(r => r.GetContextItemType()).Returns(ObjectType.otPackage);
            repositoryMock.Setup(r => r.GetContextObject()).Returns(packageMock.Object);
            return new AddInContext(repositoryMock.Object, MenuLocation.TreeView.ToString(), menuName, menuItem);
        }

        private static AddInContext CreateElementContext(string menuName)
        {
            var elementMock = new Mock<Element>();
            var repositoryMock = new Mock<Repository>();
            repositoryMock.Setup(r => r.GetContextItemType()).Returns(ObjectType.otElement);
            repositoryMock.Setup(r => r.GetContextObject()).Returns(elementMock.Object);
            return new AddInContext(repositoryMock.Object, MenuLocation.TreeView.ToString(), menuName, null);
        }

        private static Mock<Package> CreatePackageMock(string stereotype)
        {
            var packageElementMock = new Mock<Element>();
            packageElementMock.Setup(e => e.Stereotype).Returns(stereotype);
            var packageMock = new Mock<Package>();
            packageMock.Setup(p => p.Element).Returns(packageElementMock.Object);
            return packageMock;
        }

        private static AddInContext CreateDiagramContext(string menuName)
        {
            var diagramMock = new Mock<Diagram>();
            var repositoryMock = new Mock<Repository>();
            repositoryMock.Setup(r => r.GetContextItemType()).Returns(ObjectType.otDiagram);
            repositoryMock.Setup(r => r.GetContextObject()).Returns(diagramMock.Object);
            return new AddInContext(repositoryMock.Object, MenuLocation.TreeView.ToString(), menuName, null);
        }

        private static void DoNothing(AddInContext context)
        {
        }

        private static void AssertMenuState(TestPredicate testPredicate, MenuManager menuManager, string menuName, string menuItem)
        {
            var context = CreateMainMenuContext(menuName, menuItem);
            bool isEnabled = false;
            bool isChecked = false;

            testPredicate.Enable();
            testPredicate.Check();
            menuManager.GetMenuState(context, ref isEnabled, ref isChecked);
            Assert.IsTrue(isEnabled);
            Assert.IsTrue(isChecked);

            testPredicate.Disable();
            testPredicate.Uncheck();
            menuManager.GetMenuState(context, ref isEnabled, ref isChecked);
            Assert.IsFalse(isEnabled);
            Assert.IsFalse(isChecked);
        }

        private static AddInContext CreateMainMenuContext(string menuName, string menuItem)
        {
            return new AddInContext(null, MenuLocation.MainMenu.ToString(), menuName, menuItem);
        }

        private static bool ContextIsBDTLibrary(AddInContext context)
        {
            return context.MenuLocation.IsContextMenu() && context.IsLibraryOfType(Stereotype.BDTLibrary);
        }

        [Test]
        public void TestContextMenuClick()
        {
            var action = new AssertableMenuAction();
            var menuManager = new MenuManager();
            menuManager[ContextIsBDTLibrary] =
                ("VIENNAAddIn"
                 + "Validate BDT Library".OnClick(action.Execute)
                 + "Create new BDT".OnClick(action.Execute)
                );
            menuManager.GetMenuItems(CreatePackageContext(Stereotype.BDTLibrary, null, null));
            menuManager.MenuClick(CreatePackageContext(Stereotype.BDTLibrary, "-VIENNAAddIn", "Validate BDT Library"));
            menuManager.MenuClick(CreatePackageContext(Stereotype.BDTLibrary, "-VIENNAAddIn", "Create new BDT"));
            Assert.AreEqual(new[] {"Validate BDT Library", "Create new BDT"},
                            action.ExecutedActions);
        }

        [Test]
        public void TestGetContextMenuItems()
        {
            var menuManager = new MenuManager
                              {
                                  DefaultMenuItems = new[] {"default"}
                              };
            menuManager[ContextIsBDTLibrary] =
                ("VIENNAAddIn"
                 + "Validate BDT Library".OnClick(DoNothing)
                 + "Create new BDT".OnClick(DoNothing)
                );
            Assert.AreEqual(new[] {"default"}, menuManager.GetMenuItems(CreateMainMenuContext(null, null)));
            Assert.AreEqual(new[] {"-VIENNAAddIn"}, menuManager.GetMenuItems(CreatePackageContext(Stereotype.BDTLibrary, null, null)));
            Assert.AreEqual(new[] {"Validate BDT Library", "Create new BDT"}, menuManager.GetMenuItems(CreatePackageContext(Stereotype.BDTLibrary, "-VIENNAAddIn", null)));
            Assert.AreEqual(new[] {"default"}, menuManager.GetMenuItems(CreateElementContext(null)));
            Assert.AreEqual(new[] {"default"}, menuManager.GetMenuItems(CreateDiagramContext(null)));
        }

        [Test]
        public void TestGetMainMenuItems()
        {
            var menuManager = new MenuManager
                              {
                                  DefaultMenuItems = new[] {"default"}
                              };
            menuManager[MenuLocation.MainMenu] = "menu"
                                                 + "action1".OnClick(DoNothing)
                                                 + "action2".OnClick(DoNothing)
                                                 + MenuItem.Separator
                                                 + ("sub-menu"
                                                    + "sub-menu-action1".OnClick(DoNothing)
                                                    + "sub-menu-action2".OnClick(DoNothing)
                                                   )
                                                 + "action3".OnClick(DoNothing);
            Assert.AreEqual(new[] {"default"}, menuManager.GetMenuItems(CreateMainMenuContext("unknown menu name", null)));
            Assert.AreEqual(new[] {"-menu"},
                            menuManager.GetMenuItems(CreateMainMenuContext(null, null)));
            Assert.AreEqual(new[] {"-menu"},
                            menuManager.GetMenuItems(CreateMainMenuContext(string.Empty, null)));
            Assert.AreEqual(new[] {"action1", "action2", "-", "-sub-menu", "action3"},
                            menuManager.GetMenuItems(CreateMainMenuContext("-menu", null)));
            Assert.AreEqual(new[] {"sub-menu-action1", "sub-menu-action2"},
                            menuManager.GetMenuItems(CreateMainMenuContext("-sub-menu", null)));
        }

        [Test]
        public void TestGetMainMenuState()
        {
            var testPredicate = new TestPredicate();
            var menuManager = new MenuManager();
            menuManager[MenuLocation.MainMenu] = ("menu"
                                                  + "action1".OnClick(DoNothing).Checked(testPredicate.IsChecked).Enabled(testPredicate.IsEnabled)
                                                  + "action2".OnClick(DoNothing).Checked(testPredicate.IsChecked).Enabled(testPredicate.IsEnabled)
                                                  + MenuItem.Separator
                                                  + ("sub-menu"
                                                     + "sub-menu-action1".OnClick(DoNothing).Checked(testPredicate.IsChecked).Enabled(testPredicate.IsEnabled)
                                                     + "sub-menu-action2".OnClick(DoNothing).Checked(testPredicate.IsChecked).Enabled(testPredicate.IsEnabled)
                                                    )
                                                  + "action3".OnClick(DoNothing).Checked(testPredicate.IsChecked).Enabled(testPredicate.IsEnabled)
                                                 );
            menuManager.GetMenuItems(CreateMainMenuContext(null, null));
            AssertMenuState(testPredicate, menuManager, "-menu", "action1");
            AssertMenuState(testPredicate, menuManager, "-menu", "action2");
            AssertMenuState(testPredicate, menuManager, "-menu", "action3");
            AssertMenuState(testPredicate, menuManager, "-sub-menu", "sub-menu-action1");
            AssertMenuState(testPredicate, menuManager, "-sub-menu", "sub-menu-action2");
        }

        [Test]
        public void TestMainMenuClick()
        {
            var action = new AssertableMenuAction();
            var menuManager = new MenuManager();
            menuManager[MenuLocation.MainMenu] = "menu"
                                                 + "action1".OnClick(action.Execute)
                                                 + "action2".OnClick(action.Execute)
                                                 + MenuItem.Separator
                                                 + ("sub-menu"
                                                    + "sub-menu-action1".OnClick(action.Execute)
                                                    + "sub-menu-action2".OnClick(action.Execute)
                                                   )
                                                 + "action3".OnClick(action.Execute);
            menuManager.GetMenuItems(CreateMainMenuContext(null, null));
            menuManager.MenuClick(CreateMainMenuContext("-menu", "action1"));
            menuManager.MenuClick(CreateMainMenuContext("-menu", "action2"));
            menuManager.MenuClick(CreateMainMenuContext("-sub-menu", "sub-menu-action1"));
            menuManager.MenuClick(CreateMainMenuContext("-sub-menu", "sub-menu-action2"));
            menuManager.MenuClick(CreateMainMenuContext("-menu", "action3"));
            Assert.AreEqual(new[] { "action1", "action2", "sub-menu-action1", "sub-menu-action2", "action3" }, action.ExecutedActions);
        }

        [Test]
        public void TestNestedSubMenu()
        {
            SubMenu nestedMenu = ("menu"
                                  + "action1".OnClick(DoNothing)
                                  + "action2".OnClick(DoNothing)
                                  + MenuItem.Separator
                                  + ("sub-menu"
                                     + "sub-menu-action1".OnClick(DoNothing)
                                     + "sub-menu-action2".OnClick(DoNothing)
                                    )
                                  + "action3".OnClick(DoNothing)
                                 );
            Assert.AreEqual("-menu", nestedMenu.Name);
            Assert.AreEqual(5, nestedMenu.Items.Count);
            Assert.AreEqual("action1", nestedMenu.Items[0].Name);
            Assert.AreEqual("action2", nestedMenu.Items[1].Name);
            Assert.AreEqual("-", nestedMenu.Items[2].Name);
            Assert.AreEqual("action3", nestedMenu.Items[4].Name);
            Assert.IsTrue(nestedMenu.Items[3] is SubMenu);
            var subMenu = (SubMenu) nestedMenu.Items[3];
            Assert.AreEqual("-sub-menu", subMenu.Name);
            Assert.AreEqual(2, subMenu.Items.Count);
            Assert.AreEqual("sub-menu-action1", subMenu.Items[0].Name);
            Assert.AreEqual("sub-menu-action2", subMenu.Items[1].Name);
        }

        [Test]
        public void TestSimpleSubMenu()
        {
            SubMenu simpleSubMenu = "subMenu"
                                    + "action1".OnClick(DoNothing)
                                    + "action2".OnClick(DoNothing)
                                    + MenuItem.Separator
                                    + "action3".OnClick(DoNothing);
            Assert.AreEqual("-subMenu", simpleSubMenu.Name);
            Assert.AreEqual(4, simpleSubMenu.Items.Count);
            Assert.AreEqual("action1", simpleSubMenu.Items[0].Name);
            Assert.AreEqual("action2", simpleSubMenu.Items[1].Name);
            Assert.AreEqual("-", simpleSubMenu.Items[2].Name);
            Assert.AreEqual("action3", simpleSubMenu.Items[3].Name);
        }

        [Test]
        public void TestSingleActionMenu()
        {
            MenuAction singleActionMenu = "action".OnClick(DoNothing);
            Assert.AreEqual("action", singleActionMenu.Name);
        }
    }

    public class TestPredicate
    {
        private bool isChecked;
        private bool isEnabled;

        public bool IsEnabled(AddInContext obj)
        {
            return isEnabled;
        }

        public void Enable()
        {
            isEnabled = true;
        }

        public void Disable()
        {
            isEnabled = false;
        }

        public bool IsChecked(AddInContext obj)
        {
            return isChecked;
        }

        public void Check()
        {
            isChecked = true;
        }

        public void Uncheck()
        {
            isChecked = false;
        }
    }

    public class AssertableMenuAction
    {
        private readonly List<string> executedActions = new List<string>();

        public string[] ExecutedActions
        {
            get { return executedActions.ToArray(); }
        }

        public void Execute(AddInContext context)
        {
            executedActions.Add(context.MenuItem);
        }
    }
}