using NUnit.Framework;
using VIENNAAddIn.menu;
using VIENNAAddIn.upcc3.Wizards.dev;
using VIENNAAddInUnitTests.upcc3.Wizards.dev.TestRepository;

namespace VIENNAAddInUnitTests.upcc3.Wizards.dev
{
    [TestFixture]
    public class ABIEModelerFormTest
    {
        [Test]
        [Ignore]
        public void ShouldLaunchAndPopulateABIEModellerForm()
        {
            AddInContext context = new AddInContext(new EARepositoryCCCache(), MenuLocation.MainMenu.ToString());

            ABIEModelerForm.ShowForm(context);            
        }        
    }
}