using Castle.Core.Interceptor;
using NUnit.Framework;
using VIENNAAddIn.upcc3.ccts;
using VIENNAAddIn.Utils;
using VIENNAAddInUnitTests.TestRepository;

namespace VIENNAAddInUnitTests.Utils
{
    [TestFixture]
    public class RepositoryCopierTest
    {
        [Test]
        public void TestCopyRepository()
        {
            var original = new EARepository1();
//            using (var copy = new TemporaryFileBasedRepository(new EmptyEARepository()))
//            {
            var copy = new EmptyEARepository();
                RepositoryCopier.CopyRepository(original, copy);
                EAAssert.RepositoriesAreEqual(original, copy, Path.EmptyPath);
//            }
        }
    }
}