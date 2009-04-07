using System;
using EA;
using Moq;
using NUnit.Framework;
using VIENNAAddIn.menu;
using VIENNAAddIn.Settings;

namespace VIENNAAddInUnitTests.menu
{
    [TestFixture]
    public class MenuManagerTest
    {
        private static AddInContext CreatePackageContext(string stereotype)
        {
            var packageMock = CreatePackageMock(stereotype);
            var repositoryMock = new Mock<Repository>();
            object selectedObject = packageMock.Object;
            repositoryMock.Setup(r => r.GetTreeSelectedItem(out selectedObject)).Returns(ObjectType.otPackage);

            return new AddInContext {Repository = repositoryMock.Object};
        }

        private static AddInContext CreateElementContext(string packageStereotype)
        {
            var elementMock = new Mock<Element>();

            var packageMock = CreatePackageMock(packageStereotype);

            var repositoryMock = new Mock<Repository>();
            object selectedObject = elementMock.Object;
            repositoryMock.Setup(r => r.GetTreeSelectedItem(out selectedObject)).Returns(ObjectType.otElement);
            repositoryMock.Setup(r => r.GetPackageByID(It.IsAny<int>())).Returns(packageMock.Object);

            return new AddInContext {Repository = repositoryMock.Object};
        }

        private static Mock<Package> CreatePackageMock(string stereotype)
        {
            var packageElementMock = new Mock<Element>();
            packageElementMock.Setup(e => e.Stereotype).Returns(stereotype);
            var packageMock = new Mock<Package>();
            packageMock.Setup(p => p.Element).Returns(packageElementMock.Object);
            return packageMock;
        }

        private static AddInContext CreateDiagramContext(string packageStereotype)
        {
            var diagramMock = new Mock<Diagram>();

            var packageMock = CreatePackageMock(packageStereotype);

            var repositoryMock = new Mock<Repository>();
            object selectedObject = diagramMock.Object;
            repositoryMock.Setup(r => r.GetTreeSelectedItem(out selectedObject)).Returns(ObjectType.otDiagram);
            repositoryMock.Setup(r => r.GetPackageByID(It.IsAny<int>())).Returns(packageMock.Object);

            return new AddInContext { Repository = repositoryMock.Object };
        }

        private static MenuAction TestMenuAction(string menuName)
        {
            return new MenuAction(menuName, new TestAction(menuName).Execute);
        }

        [Test]
        public void TestGetMenuItems()
        {
            const string stereotype1 = "stereotype1";
            const string stereotype2 = "stereotype2";
            const string stereotype3 = "stereotype3";
            const string stereotype4 = "stereotype4";

            var menuManager = new MenuManager(AddInSettings.AddInCaption);
            menuManager
                .MainMenu.SetItems(
                new SubMenu("main_menu_1").SetItems(
                    new SubMenu("main_menu_1.1").SetItems(
                        TestMenuAction("main_menu_1.1_action_1"),
                        new MenuSeparator(),
                        TestMenuAction("main_menu_1.1_action_2")
                        ),
                    TestMenuAction("main_menu_1_action_1")
                    ));
            menuManager
                .ForPackagesWithStereotypes(
                stereotype1,
                stereotype2
                )
                .SetItems(
                TestMenuAction("tree_menu_package_1_2_action_1"),
                TestMenuAction("tree_menu_package_1_2_action_2")
                );
            menuManager
                .AllPackages.SetItems(
                TestMenuAction("tree_menu_package_all_action_1"),
                TestMenuAction("tree_menu_package_all_action_2")
                );
            menuManager
                .ForPackagesWithStereotypes(
                stereotype3
                )
                .SetItems(
                TestMenuAction("tree_menu_package_3_action_1"),
                new SubMenu("tree_menu_package_3_menu_1").SetItems(
                    TestMenuAction("tree_menu_package_3_menu_1_action_1"),
                    new MenuSeparator(),
                    TestMenuAction("tree_menu_package_3_menu_1_action_2"),
                    TestMenuAction("tree_menu_package_3_menu_1_action_3")
                    ),
                TestMenuAction("tree_menu_package_3_action_2")
                );
            menuManager
                .ForElementsWithPackageStereotypes(
                stereotype1,
                stereotype2
                )
                .SetItems(
                TestMenuAction("tree_menu_element_1_2_action_1"),
                TestMenuAction("tree_menu_element_1_2_action_2")
                );
            menuManager
                .AllElements.SetItems(
                TestMenuAction("tree_menu_element_all_action_1"),
                TestMenuAction("tree_menu_element_all_action_2")
                );
            menuManager
                .ForElementsWithPackageStereotypes(
                stereotype3
                )
                .SetItems(
                TestMenuAction("tree_menu_element_3_action_1"),
                new SubMenu("tree_menu_element_3_menu_1").SetItems(
                    TestMenuAction("tree_menu_element_3_menu_1_action_1"),
                    new MenuSeparator(),
                    TestMenuAction("tree_menu_element_3_menu_1_action_2"),
                    TestMenuAction("tree_menu_element_3_menu_1_action_3")
                    ),
                TestMenuAction("tree_menu_element_3_action_2")
                );
            menuManager
                .ForDiagramsWithPackageStereotypes(
                stereotype1,
                stereotype2
                )
                .SetItems(
                TestMenuAction("tree_menu_diagram_1_2_action_1"),
                TestMenuAction("tree_menu_diagram_1_2_action_2")
                );
            menuManager
                .AllDiagrams.SetItems(
                TestMenuAction("tree_menu_diagram_all_action_1"),
                TestMenuAction("tree_menu_diagram_all_action_2")
                );
            menuManager
                .ForDiagramsWithPackageStereotypes(
                stereotype3
                )
                .SetItems(
                TestMenuAction("tree_menu_diagram_3_action_1"),
                new SubMenu("tree_menu_diagram_3_menu_1").SetItems(
                    TestMenuAction("tree_menu_diagram_3_menu_1_action_1"),
                    new MenuSeparator(),
                    TestMenuAction("tree_menu_diagram_3_menu_1_action_2"),
                    TestMenuAction("tree_menu_diagram_3_menu_1_action_3")
                    ),
                TestMenuAction("tree_menu_diagram_3_action_2")
                );

            Assert.AreEqual("-" + AddInSettings.AddInCaption, menuManager.GetMenuItems(null, "MainMenu", "")[0]);
            Assert.AreEqual(new[] {"-main_menu_1"},
                            menuManager.GetMenuItems(null, "MainMenu", "-" + AddInSettings.AddInCaption));
            Assert.AreEqual(new[] {"-main_menu_1.1", "main_menu_1_action_1"},
                            menuManager.GetMenuItems(null, "MainMenu", "-main_menu_1"));
            Assert.AreEqual(new[] {"main_menu_1.1_action_1", "-", "main_menu_1.1_action_2"},
                            menuManager.GetMenuItems(null, "MainMenu", "-main_menu_1.1"));

            Assert.AreEqual("-" + AddInSettings.AddInCaption,
                            menuManager.GetMenuItems(
                                CreatePackageContext(stereotype1),
                                "TreeView", "")[0]);
            Assert.AreEqual(
                new[]
                {
                    "tree_menu_package_1_2_action_1", "tree_menu_package_1_2_action_2",
                    "tree_menu_package_all_action_1", "tree_menu_package_all_action_2"
                },
                menuManager.GetMenuItems(CreatePackageContext(stereotype1), "TreeView", "-" + AddInSettings.AddInCaption));
            Assert.AreEqual(new[]
                            {
                                "tree_menu_package_1_2_action_1",
                                "tree_menu_package_1_2_action_2",
                                "tree_menu_package_all_action_1",
                                "tree_menu_package_all_action_2"
                            },
                            menuManager.GetMenuItems(
                                CreatePackageContext(stereotype2),
                                "TreeView", "-" + AddInSettings.AddInCaption));
            Assert.AreEqual(new[]
                            {
                                "tree_menu_package_3_action_1",
                                "-tree_menu_package_3_menu_1",
                                "tree_menu_package_3_action_2",
                                "tree_menu_package_all_action_1",
                                "tree_menu_package_all_action_2"
                            },
                            menuManager.GetMenuItems(
                                CreatePackageContext(stereotype3),
                                "TreeView", "-" + AddInSettings.AddInCaption));
            Assert.AreEqual(new[]
                            {
                                "tree_menu_package_3_menu_1_action_1",
                                "-",
                                "tree_menu_package_3_menu_1_action_2",
                                "tree_menu_package_3_menu_1_action_3",
                            },
                            menuManager.GetMenuItems(
                                CreatePackageContext(stereotype3),
                                "TreeView", "-tree_menu_package_3_menu_1"));
            Assert.AreEqual(new[]
                            {
                                "tree_menu_package_all_action_1",
                                "tree_menu_package_all_action_2"
                            },
                            menuManager.GetMenuItems(
                                CreatePackageContext(stereotype4),
                                "TreeView", "-" + AddInSettings.AddInCaption));

            Assert.AreEqual("-" + AddInSettings.AddInCaption,
                    menuManager.GetMenuItems(
                        CreateElementContext(stereotype1),
                        "TreeView", "")[0]);
            Assert.AreEqual(
                new[]
                {
                    "tree_menu_element_1_2_action_1", "tree_menu_element_1_2_action_2",
                    "tree_menu_element_all_action_1", "tree_menu_element_all_action_2"
                },
                menuManager.GetMenuItems(CreateElementContext(stereotype1), "TreeView", "-" + AddInSettings.AddInCaption));
            Assert.AreEqual(new[]
                            {
                                "tree_menu_element_1_2_action_1",
                                "tree_menu_element_1_2_action_2",
                                "tree_menu_element_all_action_1",
                                "tree_menu_element_all_action_2"
                            },
                            menuManager.GetMenuItems(
                                CreateElementContext(stereotype2),
                                "TreeView", "-" + AddInSettings.AddInCaption));
            Assert.AreEqual(new[]
                            {
                                "tree_menu_element_3_action_1",
                                "-tree_menu_element_3_menu_1",
                                "tree_menu_element_3_action_2",
                                "tree_menu_element_all_action_1",
                                "tree_menu_element_all_action_2"
                            },
                            menuManager.GetMenuItems(
                                CreateElementContext(stereotype3),
                                "TreeView", "-" + AddInSettings.AddInCaption));
            Assert.AreEqual(new[]
                            {
                                "tree_menu_element_3_menu_1_action_1",
                                "-",
                                "tree_menu_element_3_menu_1_action_2",
                                "tree_menu_element_3_menu_1_action_3",
                            },
                            menuManager.GetMenuItems(
                                CreateElementContext(stereotype3),
                                "TreeView", "-tree_menu_element_3_menu_1"));
            Assert.AreEqual(new[]
                            {
                                "tree_menu_element_all_action_1",
                                "tree_menu_element_all_action_2"
                            },
                            menuManager.GetMenuItems(
                                CreateElementContext(stereotype4),
                                "TreeView", "-" + AddInSettings.AddInCaption));

            Assert.AreEqual("-" + AddInSettings.AddInCaption,
                    menuManager.GetMenuItems(
                        CreateDiagramContext(stereotype1),
                        "TreeView", "")[0]);
            Assert.AreEqual(
                new[]
                {
                    "tree_menu_diagram_1_2_action_1", "tree_menu_diagram_1_2_action_2",
                    "tree_menu_diagram_all_action_1", "tree_menu_diagram_all_action_2"
                },
                menuManager.GetMenuItems(CreateDiagramContext(stereotype1), "TreeView", "-" + AddInSettings.AddInCaption));
            Assert.AreEqual(new[]
                            {
                                "tree_menu_diagram_1_2_action_1",
                                "tree_menu_diagram_1_2_action_2",
                                "tree_menu_diagram_all_action_1",
                                "tree_menu_diagram_all_action_2"
                            },
                            menuManager.GetMenuItems(
                                CreateDiagramContext(stereotype2),
                                "TreeView", "-" + AddInSettings.AddInCaption));
            Assert.AreEqual(new[]
                            {
                                "tree_menu_diagram_3_action_1",
                                "-tree_menu_diagram_3_menu_1",
                                "tree_menu_diagram_3_action_2",
                                "tree_menu_diagram_all_action_1",
                                "tree_menu_diagram_all_action_2"
                            },
                            menuManager.GetMenuItems(
                                CreateDiagramContext(stereotype3),
                                "TreeView", "-" + AddInSettings.AddInCaption));
            Assert.AreEqual(new[]
                            {
                                "tree_menu_diagram_3_menu_1_action_1",
                                "-",
                                "tree_menu_diagram_3_menu_1_action_2",
                                "tree_menu_diagram_3_menu_1_action_3",
                            },
                            menuManager.GetMenuItems(
                                CreateDiagramContext(stereotype3),
                                "TreeView", "-tree_menu_diagram_3_menu_1"));
            Assert.AreEqual(new[]
                            {
                                "tree_menu_diagram_all_action_1",
                                "tree_menu_diagram_all_action_2"
                            },
                            menuManager.GetMenuItems(
                                CreateDiagramContext(stereotype4),
                                "TreeView", "-" + AddInSettings.AddInCaption));
        }
    }

    public class TestAction
    {
        private readonly string action;

        public TestAction(string action)
        {
            this.action = action;
        }

        public void Execute(AddInContext context)
        {
            Console.WriteLine("executing action {0}", action);
        }
    }
}