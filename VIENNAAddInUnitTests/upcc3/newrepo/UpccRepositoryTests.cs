using System.Collections.Generic;
using CctsRepository;
using CctsRepository.BdtLibrary;
using CctsRepository.BLibrary;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddIn.upcc3.repo;
using VIENNAAddIn.upcc3.uml;

namespace VIENNAAddInUnitTests.upcc3.newrepo
{
    [TestFixture]
    public class UpccRepositoryTests
    {
        private static UmlPackageBuilder AUmlPackage
        {
            get { return new UmlPackageBuilder(); }
        }

        private static UmlRepositoryBuilder AUmlRepository
        {
            get { return new UmlRepositoryBuilder(); }
        }

        [Test]
        public void ShouldLoadBdtLibrariesFromUmlRepository()
        {
            IUmlRepository mockUmlRepository =
                AUmlRepository
                    .With(AUmlPackage.WithStereotype(Stereotype.BDTLibrary))
                    .With(AUmlPackage.WithStereotype(Stereotype.BDTLibrary))
                    .With(AUmlPackage.WithStereotype("other than BDTLibrary"))
                    .Build();
            ICctsRepository cctsRepository = new UpccRepository(mockUmlRepository);
            var libraries = new List<IBdtLibrary>(cctsRepository.GetBdtLibraries());
            Assert.That(libraries.Count, Is.EqualTo(2));
        }

        [Test]
        [Ignore("not yet implemented")]
        public void ShouldLoadBieLibrariesFromUmlRepository()
        {
        }

        [Test]
        public void ShouldLoadBLibrariesFromUmlRepository()
        {
            IUmlRepository mockUmlRepository =
                AUmlRepository
                    .With(AUmlPackage.WithStereotype(Stereotype.bLibrary))
                    .With(AUmlPackage.WithStereotype(Stereotype.bLibrary))
                    .With(AUmlPackage.WithStereotype("other than bLibrary"))
                    .Build();
            ICctsRepository cctsRepository = new UpccRepository(mockUmlRepository);
            var libraries = new List<IBLibrary>(cctsRepository.GetBLibraries());
            Assert.That(libraries.Count, Is.EqualTo(2));
        }

        [Test]
        [Ignore("not yet implemented")]
        public void ShouldLoadCcLibrariesFromUmlRepository()
        {
        }

        [Test]
        [Ignore("not yet implemented")]
        public void ShouldLoadCdtLibrariesFromUmlRepository()
        {
        }

        [Test]
        [Ignore("not yet implemented")]
        public void ShouldLoadDocLibrariesFromUmlRepository()
        {
        }

        [Test]
        [Ignore("not yet implemented")]
        public void ShouldLoadEnumLibrariesFromUmlRepository()
        {
        }

        [Test]
        [Ignore("not yet implemented")]
        public void ShouldLoadPrimLibrariesFromUmlRepository()
        {
        }
    }
}