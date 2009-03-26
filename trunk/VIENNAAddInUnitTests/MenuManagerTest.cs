using System;
using NUnit.Framework;
using VIENNAAddIn;
using VIENNAAddIn.Settings;

namespace VIENNAAddInUnitTests
{
    [TestFixture]
    public class MenuManagerTest
    {
        [Test]
        public void TestGetMenuItems()
        {
            var menuManager = new MenuManager(AddInSettings.AddInCaption);
            menuManager.SetMainMenu(
                new SubMenu("sub menu 1").SetItems(
                    new SubMenu("sub menu 1.1").SetItems(
                        new MenuAction("action 1.1.&1", new TestAction("action 1.1.1").Execute),
                        new MenuSeparator(),
                        new MenuAction("action 1.1.&2", new TestAction("action 1.1.2").Execute)
                        ),
                    new MenuAction("do something", new TestAction("do something").Execute)
                    ));
            Assert.AreEqual("-" + AddInSettings.AddInCaption, menuManager.GetMenuItems(null, "MainMenu", "")[0]);
            Assert.AreEqual(new[] { "-sub menu 1" }, menuManager.GetMenuItems(null, "MainMenu", "-" + AddInSettings.AddInCaption));
            Assert.AreEqual(new[] { "-sub menu 1.1", "do something" }, menuManager.GetMenuItems(null, "MainMenu", "-sub menu 1"));
            Assert.AreEqual(new[] { "action 1.1.&1", "-", "action 1.1.&2" }, menuManager.GetMenuItems(null, "MainMenu", "-sub menu 1.1"));
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