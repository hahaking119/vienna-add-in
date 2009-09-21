using NUnit.Framework;
using VIENNAAddIn.menu;
using VIENNAAddIn.upcc3.Wizards;
using VIENNAAddInUnitTests.TestRepository;

namespace VIENNAAddInUnitTests.upcc3.Wizards.util
{
    [TestFixture]
    public class StandardLibraryImporterFormTest
    {
        [Test]
        [Ignore]
        public void ShouldLaunchAndPopulateStandardLibraryImporterForm()
        {
            AddInContext context = new AddInContext(new EARepository(), MenuLocation.MainMenu.ToString());

            StandardLibraryImporterForm.ShowForm(context);    
        }        
    }
}