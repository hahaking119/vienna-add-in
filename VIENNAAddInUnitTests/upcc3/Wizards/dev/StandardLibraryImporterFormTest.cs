using System.Threading;
using System.Windows;
using NUnit.Framework;
using VIENNAAddIn.upcc3.Wizards.dev;
using VIENNAAddInUnitTests.upcc3.Wizards.TestRepository;

namespace VIENNAAddInUnitTests.upcc3.Wizards.dev
{
    [TestFixture]
    public class StandardLibraryImporterFormTest
    {
        [Test]
        [Ignore]
        public void ShouldLaunchAndPopulateStandardLibraryImporterForm()
        {
            var t = new Thread(() => new Application().Run(new StandardLibraryImporter(new EARepositoryLibraryImporter())));
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();
        }
    }
}