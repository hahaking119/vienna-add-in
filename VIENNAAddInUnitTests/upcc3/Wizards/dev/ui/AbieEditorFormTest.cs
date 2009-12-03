using System.Threading;
using System.Windows;
using NUnit.Framework;
using VIENNAAddIn.upcc3.ccts.dra;
using VIENNAAddIn.upcc3.Wizards.dev.ui;
using VIENNAAddInUnitTests.upcc3.Wizards.dev.TestRepository;

namespace VIENNAAddInUnitTests.upcc3.Wizards.dev.ui
{
    [TestFixture]
    public class AbieEditorFormTest
    {
        [Test]
        [Ignore]
        public void ShouldLaunchAndPopulateAbieModelerForm()
        {
            var t = new Thread(() => new Application().Run(new AbieEditor(new CCRepository(new EARepositoryAbieEditor()))));
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();
        }
    }
}