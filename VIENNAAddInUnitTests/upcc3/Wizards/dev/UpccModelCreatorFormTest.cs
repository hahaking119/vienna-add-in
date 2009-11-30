using System.Threading;
using System.Windows;
using NUnit.Framework;
using VIENNAAddIn.menu;
using VIENNAAddIn.upcc3.ccts.dra;
using VIENNAAddIn.upcc3.Wizards.dev;
using VIENNAAddInUnitTests.upcc3.Wizards.TestRepository;

namespace VIENNAAddInUnitTests.upcc3.Wizards.dev
{
    [TestFixture]
    public class UpccModelCreatorFormTest
    {
        [Test]
        [Ignore]
        public void ShouldLaunchAndPopulateUpccModelCreatorForm()
        {
            AddInContext context = new AddInContext(new EARepositoryModelCreator(), MenuLocation.MainMenu.ToString());

            var t = new Thread(() => new Application().Run(new UpccModelCreator(context)));
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();
        }
    }
}