using System.Threading;
using System.Windows;
using NUnit.Framework;
using VIENNAAddIn.upcc3.ccts.dra;
using VIENNAAddIn.upcc3.Wizards.dev.ui;
using VIENNAAddInUnitTests.upcc3.Wizards.dev.TestRepository;

namespace VIENNAAddInUnitTests.upcc3.Wizards.dev.ui
{
    [TestFixture]
    public class BdtEditorFormTest
    {
        [Test]
        [Ignore]
        public void ShouldLaunchAndPopulateAbieModelerForm()
        {
            var t = new Thread(() => new Application().Run(new BdtEditor(new CCRepository(new EARepositoryBdtEditor()))));
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();
        }
    }
}