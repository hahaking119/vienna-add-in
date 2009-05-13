using EA;
using NUnit.Framework;
using VIENNAAddIn;
using VIENNAAddIn.upcc3.ccts;
using VIENNAAddInUnitTests.TestRepository;

namespace VIENNAAddInUnitTests
{
    [TestFixture]
    public class EARepositoryExtensionTest
    {
        private readonly Repository repository = new MyEARepository();

        private void AssertPackagePath(params string[] pathElements)
        {
            var path = new Path(pathElements);
            var package = repository.Resolve<Package>(path);
            Assert.IsNotNull(package, "package path could not be resolved {0}", path);
            Assert.AreEqual(pathElements[pathElements.Length - 1], package.Name, "wrong package resolved");
        }

        private void AssertElementPath(params string[] pathElements)
        {
            var path = new Path(pathElements);
            var package = repository.Resolve<Element>(path);
            Assert.IsNotNull(package, "element path could not be resolved {0}", path);
            Assert.AreEqual(pathElements[pathElements.Length - 1], package.Name, "wrong element resolved");
        }

        [Test]
        public void EmptyPathShouldResolveToNull()
        {
            Assert.IsNull(repository.Resolve<object>(new Path()));
        }

        [Test]
        public void InvalidTypeShouldResolveToNull()
        {
            Assert.IsNull(repository.Resolve<Package>((Path) "m1"/"m1p1"/"m1p1e1"));
            Assert.IsNull(repository.Resolve<Element>((Path) "m1"/"m1p1"/"m1p1p1"));
        }

        [Test]
        public void ObjectShouldResolveToCorrectType()
        {
            Assert.IsNotNull(repository.Resolve<object>((Path) "m1"/"m1p1"/"m1p1e1") as Element);
            Assert.IsNotNull(repository.Resolve<object>((Path) "m1"/"m1p1"/"m1p1p1") as Package);
        }

        [Test]
        public void ResolvePathWorksWithElements()
        {
            AssertElementPath("m1", "m1p1", "m1p1e1");
        }

        [Test]
        public void ResolvePathWorksWithMultipleModels()
        {
            AssertPackagePath("m2", "m2p1");
        }

        [Test]
        public void ResolvePathWorksWithNestedPackages()
        {
            AssertPackagePath("m1", "m1p1", "m1p1p1");
        }

        [Test]
        public void ResolvePathWorksWithPackages()
        {
            AssertPackagePath("m1", "m1p1");
        }
    }

    internal class MyEARepository : EARepository
    {
        public MyEARepository()
        {
            SetContent(
                Package("m1", "")
                    .Packages(
                    Package("m1p1", "")
                        .Packages(
                        Package("m1p1p1", "")
                        )
                        .Elements(
                        Element("m1p1e1", "")
                        )
                    ),
                Package("m2", "")
                    .Packages(
                    Package("m2p1", "")
                    )
                );
        }
    }
}