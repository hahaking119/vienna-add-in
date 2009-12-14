using System;
using System.Collections.Generic;
using CctsRepository;
using CctsRepository.BdtLibrary;
using CctsRepository.BLibrary;
using Moq;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddInUnitTests.upcc3.newrepo.upcc;
using VIENNAAddInUnitTests.upcc3.newrepo.upcc.uml;

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

    public class UmlPackageBuilder
    {
        private int id;
        private string stereotype;

        public UmlPackageBuilder WithId(int id)
        {
            this.id = id;
            return this;
        }

        public UmlPackageBuilder WithStereotype(string stereotype)
        {
            this.stereotype = stereotype;
            return this;
        }

        public IUmlPackage Build()
        {
            var mock = new Mock<IUmlPackage>();
            mock.SetupGet(p => p.Id).Returns(id);
            mock.SetupGet(p => p.Stereotype).Returns(stereotype);
            return mock.Object;
        }
    }

    internal class UmlRepositoryBuilder
    {
        private readonly List<UmlPackageBuilder> packageBuilders = new List<UmlPackageBuilder>();

        public UmlRepositoryBuilder With(UmlPackageBuilder packageBuilder)
        {
            packageBuilders.Add(packageBuilder);
            return this;
        }

        public IUmlRepository Build()
        {
            var packagesByStereotype = new Dictionary<string, List<IUmlPackage>>();
            foreach (UmlPackageBuilder packageBuilder in packageBuilders)
            {
                IUmlPackage package = packageBuilder.Build();
                if (packagesByStereotype.ContainsKey(package.Stereotype))
                {
                    packagesByStereotype[package.Stereotype].Add(package);
                }
                else
                {
                    packagesByStereotype[package.Stereotype] = new List<IUmlPackage>
                                                               {
                                                                   package
                                                               };
                }
            }
            var mock = new Mock<IUmlRepository>();
            foreach (var stereotype in packagesByStereotype.Keys)
            {
                string s = stereotype;
                mock.Setup(repo => repo.GetPackagesByStereotype(s)).Returns(packagesByStereotype[stereotype]);
            }
            return mock.Object;
        }
    }
}