using System.Collections.Generic;
using NUnit.Framework;
using VIENNAAddIn;
using VIENNAAddIn.upcc3.ccts;
using VIENNAAddIn.upcc3.ccts.otf;
using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddInUnitTests.TestRepository;

namespace VIENNAAddInUnitTests.upcc3.ccts.otf
{
    [TestFixture]
    public class Given_a_new_CCRepository
    {
        [Test]
        public void When_it_is_created_Then_it_should_load_all_EA_repository_contents_and_build_an_internal_model()
        {
            var eaRepository = new EARepository();
            eaRepository.AddModel("Model", model =>
                                           {
                                               model.AddPackage("Package 1", package_1 =>
                                                                             {
                                                                                 package_1.Element.Stereotype = Stereotype.BLibrary;
                                                                                 package_1.AddPackage("Package 1.1", package_1_1 => { package_1_1.Element.Stereotype = Stereotype.BLibrary; });
                                                                                 package_1.AddPackage("Package 1.2", package_1_2 => { package_1_2.Element.Stereotype = "Foo bar 1"; });
                                                                             });
                                               model.AddPackage("Package 2", package_2 => { package_2.Element.Stereotype = "Foo bar 2"; });
                                               model.AddPackage("Package 3", package_3 =>
                                                                             {
                                                                                 package_3.Element.Stereotype = Stereotype.BInformationV;
                                                                                 package_3.AddPackage("Package 3.1", package_3_1 =>
                                                                                                                     {
                                                                                                                         package_3_1.Element.Stereotype = Stereotype.BLibrary;
                                                                                                                         package_3_1.AddPackage("Package 3.1.1", package_3_1_1 => { package_3_1_1.Element.Stereotype = Stereotype.BLibrary; });
                                                                                                                         package_3_1.AddPackage("Package 3.1.2", package_3_1_2 => { package_3_1_2.Element.Stereotype = "Foo bar 3"; });
                                                                                                                     });
                                                                             });
                                           });
            var ccRepository = new ValidatingCCRepository(eaRepository);
            var bLibraries = new List<IBLibrary>(ccRepository.Libraries<IBLibrary>());
            Assert.AreEqual(4, bLibraries.Count);
            Assert.AreEqual("Package 1", bLibraries[0].Name);
            Assert.AreEqual("Package 1.1", bLibraries[1].Name);
            Assert.AreEqual("Package 3.1", bLibraries[2].Name);
            Assert.AreEqual("Package 3.1.1", bLibraries[3].Name);
        }
    }

    [TestFixture]
    public class Given_a_bLibrary
    {
        [Test]
        public void When_it_contains_elements_Then_report_an_error_for_each_element()
        {
            var eaRepository = new EARepository();
            int element_1_1_ID = 0;
            int element_1_2_ID = 0;
            eaRepository.AddModel("Model", m => m.AddPackage("Package 1", package_1 =>
                                                                          {
                                                                              package_1.Element.Stereotype = Stereotype.BLibrary;
                                                                              package_1.AddPackage("Package 1.1", package_1_1 => package_1_1.Element.Stereotype = Stereotype.BLibrary);
                                                                              element_1_1_ID = package_1.AddClass("Element 1.1").ElementID;
                                                                              element_1_2_ID = package_1.AddClass("Element 1.2").ElementID;
                                                                          }));
            var ccRepository = new ValidatingCCRepository(eaRepository);
            ccRepository.ValidateAll();
            var bLibraries = new List<IBLibrary>(ccRepository.Libraries<IBLibrary>());
            Assert.AreEqual(2, bLibraries.Count);
            Assert.AreEqual("Package 1", bLibraries[0].Name);
            Assert.AreEqual("Package 1.1", bLibraries[1].Name);
            Assert.AreEqual(2, ccRepository.ValidationIssues.Count);
            Assert.AreEqual(element_1_1_ID, ccRepository.ValidationIssues[0].ItemId);
            Assert.AreEqual(element_1_2_ID, ccRepository.ValidationIssues[1].ItemId);
        }

        [Test]
        public void When_it_contains_invalid_subpackages_Then_report_an_error_for_each_case()
        {
            var eaRepository = new EARepository();
            int package_1_1_ID = 0;
            int package_1_3_ID = 0;
            eaRepository.AddModel("Model", m => m.AddPackage("Package 1", package_1 =>
                                                                          {
                                                                              package_1.Element.Stereotype = Stereotype.BLibrary;
                                                                              package_1_1_ID = package_1.AddPackage("Package 1.1", package_1_1 => package_1_1.Element.Stereotype = "Foo").PackageID;
                                                                              package_1.AddPackage("Package 1.2", package_1_2 => package_1_2.Element.Stereotype = Stereotype.BLibrary);
                                                                              package_1_3_ID = package_1.AddPackage("Package 1.3", package_1_3 => package_1_3.Element.Stereotype = "Bar").PackageID;
                                                                          }));
            var ccRepository = new ValidatingCCRepository(eaRepository);
            ccRepository.ValidateAll();
            var bLibraries = new List<IBLibrary>(ccRepository.Libraries<IBLibrary>());
            Assert.AreEqual(2, bLibraries.Count);
            Assert.AreEqual("Package 1", bLibraries[0].Name);
            Assert.AreEqual("Package 1.2", bLibraries[1].Name);
            Assert.AreEqual(2, ccRepository.ValidationIssues.Count);
            Assert.AreEqual(package_1_1_ID, ccRepository.ValidationIssues[0].ItemId);
            Assert.AreEqual(package_1_3_ID, ccRepository.ValidationIssues[1].ItemId);
        }

        [Test]
        public void When_its_parent_is_a_bInformationV_Then_do_not_report_an_error()
        {
            var eaRepository = new EARepository();
            eaRepository.AddModel("Model", m => m.AddPackage("Package 1", package_1 =>
                                                                          {
                                                                              package_1.Element.Stereotype = Stereotype.BInformationV;
                                                                              package_1.AddPackage("Package 1.1", package_1_1 => { package_1_1.Element.Stereotype = Stereotype.BLibrary; });
                                                                          }));
            var ccRepository = new ValidatingCCRepository(eaRepository);
            ccRepository.ValidateAll();
            var bLibraries = new List<IBLibrary>(ccRepository.Libraries<IBLibrary>());
            Assert.AreEqual(1, bLibraries.Count);
            Assert.AreEqual("Package 1.1", bLibraries[0].Name);
            Assert.AreEqual(0, ccRepository.ValidationIssues.Count);
        }

        [Test]
        public void When_its_parent_is_a_bLibrary_Then_do_not_report_an_error()
        {
            var eaRepository = new EARepository();
            eaRepository.AddModel("Model", m => m.AddPackage("Package 1", package_1 =>
                                                                          {
                                                                              package_1.Element.Stereotype = Stereotype.BLibrary;
                                                                              package_1.AddPackage("Package 1.1", package_1_1 => { package_1_1.Element.Stereotype = Stereotype.BLibrary; });
                                                                          }));
            var ccRepository = new ValidatingCCRepository(eaRepository);
            ccRepository.ValidateAll();
            var bLibraries = new List<IBLibrary>(ccRepository.Libraries<IBLibrary>());
            Assert.AreEqual(2, bLibraries.Count);
            Assert.AreEqual("Package 1", bLibraries[0].Name);
            Assert.AreEqual("Package 1.1", bLibraries[1].Name);
            Assert.AreEqual(0, ccRepository.ValidationIssues.Count);
        }

        [Test]
        public void When_its_parent_is_a_Model_Then_do_not_report_an_error()
        {
            var eaRepository = new EARepository();
            eaRepository.AddModel("Model", m => m.AddPackage("Package 1", package_1 => { package_1.Element.Stereotype = Stereotype.BLibrary; }));
            var ccRepository = new ValidatingCCRepository(eaRepository);
            ccRepository.ValidateAll();
            var bLibraries = new List<IBLibrary>(ccRepository.Libraries<IBLibrary>());
            Assert.AreEqual(1, bLibraries.Count);
            Assert.AreEqual("Package 1", bLibraries[0].Name);
            Assert.AreEqual(0, ccRepository.ValidationIssues.Count);
        }

        [Test]
        public void When_its_parent_is_neither_a_Model_nor_a_bInformationV_nor_a_bLibrary_Then_do_report_an_error()
        {
            var eaRepository = new EARepository();
            eaRepository.AddModel("Model", m => m.AddPackage("Package 1", package_1 =>
                                                                          {
                                                                              package_1.Element.Stereotype = "Foo bar";
                                                                              package_1.AddPackage("Package 1.1", package_1_1 => { package_1_1.Element.Stereotype = Stereotype.BLibrary; });
                                                                          }));
            var ccRepository = new ValidatingCCRepository(eaRepository);
            ccRepository.ValidateAll();
            var bLibraries = new List<IBLibrary>(ccRepository.Libraries<IBLibrary>());
            Assert.AreEqual(1, bLibraries.Count);
            Assert.AreEqual("Package 1.1", bLibraries[0].Name);
            Assert.AreEqual(1, ccRepository.ValidationIssues.Count);
            Assert.AreEqual(bLibraries[0].Id, ccRepository.ValidationIssues[0].ItemId);
        }
    }
}