using System.Threading;
using System.Windows;
using NUnit.Framework;
using VIENNAAddIn.upcc3.ccts.dra;
using VIENNAAddIn.upcc3.Wizards.dev;
using VIENNAAddInUnitTests.upcc3.Wizards.dev.TestRepository;

namespace VIENNAAddInUnitTests.upcc3.Wizards.dev
{
    [TestFixture]
    public class ABIEModelerFormTest
    {
        [Test]
        [Ignore]
        public void ShouldOpenAndPopulateXamlForm()
        {
            var t = new Thread(() => new Application().Run(new ABIEEditor(new CCRepository(new EARepositoryABIEEditor()))));
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();
        }
    }
}