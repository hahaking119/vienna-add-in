using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using VIENNAAddIn;
using VIENNAAddIn.upcc3.ccts;
using VIENNAAddIn.upcc3.ccts.otf;
using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddInUnitTests.TestRepository;

namespace VIENNAAddInUnitTests.upcc3.ccts.otf
{
    [TestFixture]
    public class Given_a_new_CCRepository : CCRepositoryTest
    {
        private ValidatingCCRepository ccRepository;

        [SetUp]
        public void Context()
        {
            var eaRepository = new EARepository();
            eaRepository.AddModel("Model", model =>
            {
                EAPackageExtensions.AddPackage(model, "Package 1", package_1 =>
                {
                    package_1.Element.Stereotype = Stereotype.BLibrary;
                    EAPackageExtensions.AddPackage(package_1, "Package 1.1", package_1_1 =>
                    {
                        package_1_1.Element.Stereotype = Stereotype.BLibrary;
                        EAPackageExtensions.AddPackage(package_1_1, "Package 1.1.1", p => { p.Element.Stereotype = Stereotype.PRIMLibrary; });
                        EAPackageExtensions.AddPackage(package_1_1, "Package 1.1.2", p => { p.Element.Stereotype = Stereotype.ENUMLibrary; });
                        EAPackageExtensions.AddPackage(package_1_1, "Package 1.1.3", p => { p.Element.Stereotype = Stereotype.CDTLibrary; });
                        EAPackageExtensions.AddPackage(package_1_1, "Package 1.1.4", p => { p.Element.Stereotype = Stereotype.CCLibrary; });
                        EAPackageExtensions.AddPackage(package_1_1, "Package 1.1.5", p => { p.Element.Stereotype = Stereotype.BDTLibrary; });
                        EAPackageExtensions.AddPackage(package_1_1, "Package 1.1.6", p => { p.Element.Stereotype = Stereotype.BIELibrary; });
                        EAPackageExtensions.AddPackage(package_1_1, "Package 1.1.7", p => { p.Element.Stereotype = Stereotype.DOCLibrary; });
                    });
                    EAPackageExtensions.AddPackage(package_1, "Package 1.2", package_1_2 => { package_1_2.Element.Stereotype = "Foo bar 1"; });
                });
                EAPackageExtensions.AddPackage(model, "Package 2", package_2 => { package_2.Element.Stereotype = "Foo bar 2"; });
                EAPackageExtensions.AddPackage(model, "Package 3", package_3 =>
                {
                    package_3.Element.Stereotype = Stereotype.BInformationV;
                    EAPackageExtensions.AddPackage(package_3, "Package 3.1", package_3_1 =>
                    {
                        package_3_1.Element.Stereotype = Stereotype.BLibrary;
                        EAPackageExtensions.AddPackage(package_3_1, "Package 3.1.1", package_3_1_1 => { package_3_1_1.Element.Stereotype = Stereotype.BLibrary; });
                        EAPackageExtensions.AddPackage(package_3_1, "Package 3.1.2", package_3_1_2 => { package_3_1_2.Element.Stereotype = "Foo bar 3"; });
                    });
                });
            });
            ccRepository = new ValidatingCCRepository(eaRepository);
        }

        [Test]
        public void When_it_is_created_Then_it_should_load_all_EA_repository_contents_and_build_an_internal_model()
        {
            ccRepository.LoadRepositoryContent();
            WaitUntilRepositoryIsReady();
            VerifyLibraries<IBLibrary>(ccRepository, "Package 1", "Package 1.1", "Package 3.1", "Package 3.1.1");
            VerifyLibraries<IPRIMLibrary>(ccRepository, "Package 1.1.1");
            VerifyLibraries<IENUMLibrary>(ccRepository, "Package 1.1.2");
            VerifyLibraries<ICDTLibrary>(ccRepository, "Package 1.1.3");
            VerifyLibraries<ICCLibrary>(ccRepository, "Package 1.1.4");
            VerifyLibraries<IBDTLibrary>(ccRepository, "Package 1.1.5");
            VerifyLibraries<IBIELibrary>(ccRepository, "Package 1.1.6");
            VerifyLibraries<IDOCLibrary>(ccRepository, "Package 1.1.7");
        }

        private void WaitUntilRepositoryIsReady()
        {
        }

        private static void VerifyLibraries<TLibrary>(ValidatingCCRepository ccRepository, params string[] expectedLibraryNames) where TLibrary : IBusinessLibrary
        {
            var libraries = new List<TLibrary>(ccRepository.Libraries<TLibrary>());
            Assert.AreEqual(expectedLibraryNames.Count(), libraries.Count);
            int i = 0;
            foreach (string libraryName in expectedLibraryNames)
            {
                Assert.AreEqual(libraryName, libraries[i++].Name);
            }
        }
    }
}