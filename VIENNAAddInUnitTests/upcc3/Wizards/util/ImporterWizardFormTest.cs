using System.Threading;
using System.Windows;
using NUnit.Framework;
using VIENNAAddIn.upcc3.Wizards;
using VIENNAAddInUnitTests.upcc3.Wizards.TestRepository;

namespace VIENNAAddInUnitTests.upcc3.Wizards.util
{
    [TestFixture]
    public class ImporterWizardFormTest
    {
        [Test]
        [Ignore]
        public void ShouldOpenAndPopulateForm()
        {
            var t = new Thread(() => new Application().Run(new ImporterWizard(new EARepositoryModelCreator())));
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();
        }
    }
}