using System.Threading;
using System.Windows;
using NUnit.Framework;
using VIENNAAddIn.menu;
using VIENNAAddIn.upcc3.Wizards;
using VIENNAAddIn.upcc3.Wizards.dev;
using VIENNAAddInUnitTests.upcc3.Wizards.dev.TestRepository;
using VIENNAAddInUnitTests.upcc3.Wizards.TestRepository;

namespace VIENNAAddInUnitTests.upcc3.Wizards.dev
{
    [TestFixture]
    public class ABIEModelerFormTest
    {
        [Test]
        [Ignore]
        public void ShouldOpenAndPopulateXamlForm()
        {
            var t = new Thread(() => new Application().Run(new ABIEModeler(new EARepositoryModelCreator())));
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();
        }
        [Test]
        [Ignore]
        public void ShouldLaunchAndPopulateABIEModellerForm()
        {
            AddInContext context = new AddInContext(new EARepositoryCCCache(), MenuLocation.MainMenu.ToString());

            ABIEModelerForm.ShowForm(context);            
        }        
    }
}