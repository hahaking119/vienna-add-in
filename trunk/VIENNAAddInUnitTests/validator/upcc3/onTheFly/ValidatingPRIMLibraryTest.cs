using System.Linq;
using EA;
using Moq;
using NUnit.Framework;
using VIENNAAddIn;
using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddIn.validator.upcc3.onTheFly;
using VIENNAAddInUnitTests.TestRepository;
using Stereotype=VIENNAAddIn.upcc3.ccts.util.Stereotype;

namespace VIENNAAddInUnitTests.validator.upcc3.onTheFly
{
    [TestFixture]
    [Ignore]
    public class ValidatingPRIMLibraryTest
    {
        private static Mock<Element> MockPRIM(string name, int id)
        {
            return MockElement(Stereotype.PRIM, name, id);
        }

        private static Mock<Element> MockElement(string stereotype, string name, int id)
        {
            var primMock = new Mock<Element>();
            primMock.SetupGet(e => e.Stereotype).Returns(stereotype);
            primMock.SetupGet(e => e.Name).Returns(name);
            primMock.SetupGet(e => e.ElementID).Returns(id);
            primMock.SetupGet(e => e.ElementGUID).Returns(string.Empty + id);
            return primMock;
        }

        [Test]
        public void C514g_A_PRIMLibrary_shall_only_contain_PRIMs()
        {
            var validationContext = new Mock<IValidationContext>();

            var primLibrary = new ValidatingPRIMLibrary(null, validationContext.Object);

            Mock<Element> primMock = MockPRIM("String", 27);
            Mock<Element> nonPRIMMock = MockElement("foobar", "Not a PRIM", 28);

            primLibrary.ProcessElementCreated(primMock.Object);
            Assert.AreEqual(1, primLibrary.PRIMs.Where(prim => prim.Name == "String").Count());

            primLibrary.ProcessElementCreated(nonPRIMMock.Object);
            validationContext.Verify(context => context.AddValidationIssue(It.Is<InvalidElementStereotype>(issue => issue.GUID == "28")), Times.Exactly(1));
            Assert.AreEqual(1, primLibrary.PRIMs.Where(prim => prim.Name == "Not a PRIM").Count());
        }

        [Test]
        public void C514m_A_PRIMLibrary_must_not_contain_any_sub_packages()
        {
            var validationContext = new Mock<IValidationContext>();

            var primLibrary = new ValidatingPRIMLibrary(null, validationContext.Object);

            var packageMock = new Mock<Package>();
            packageMock.SetupGet(package => package.PackageGUID).Returns("21");

            primLibrary.ProcessCreatePackage(packageMock.Object);
            validationContext.Verify(context => context.AddValidationIssue(It.Is<NoSubpackagesAllowed>(issue => issue.GUID == "21")), Times.Exactly(1));
        }

        [Test]
        public void C544a_The_name_of_a_PRIM_shall_be_unique_for_a_given_PRIMLibrary()
        {
            var validationContext = new Mock<IValidationContext>();

            var primLibrary = new ValidatingPRIMLibrary(null, validationContext.Object);

            Mock<Element> primMock1 = MockPRIM("String", 27);
            Mock<Element> primMock2 = MockPRIM("String", 28);

            primLibrary.ProcessElementCreated(primMock1.Object);
            Assert.AreEqual(1, primLibrary.PRIMs.Where(prim => prim.Name == "String").Count());
            validationContext.Verify(context => context.AddValidationIssue(It.IsAny<DuplicateElementName>()), Times.Never());

            primLibrary.ProcessElementCreated(primMock2.Object);
            validationContext.Verify(context => context.AddValidationIssue(It.Is<DuplicateElementName>(issue => issue.GUID == "27")), Times.Exactly(1));
            validationContext.Verify(context => context.AddValidationIssue(It.Is<DuplicateElementName>(issue => issue.GUID == "28")), Times.Exactly(1));
            Assert.AreEqual(2, primLibrary.PRIMs.Where(prim => prim.Name == "String").Count());
        }

        [Test]
        public void C544b_A_PRIM_can_only_be_equivalent_to_another_PRIM()
        {
            var eaRepository = new EmptyEARepository();
            Element primString = null;
            Element primZeichenkette = null;
            Element primCode = null;
            Element primDecimal = null;
            Element enumCountryCodes = null;
            eaRepository.AddModel("Model",
                                  model => model.AddPackage("bLibrary",
                                                            bLibrary =>
                                                            {
                                                                bLibrary.Element.Stereotype = Stereotype.bLibrary;
                                                                bLibrary.AddPackage("PRIMLibrary",
                                                                                    p =>
                                                                                    {
                                                                                        p.Element.Stereotype = Stereotype.PRIMLibrary;
                                                                                        primString = p.AddPRIM("String");
                                                                                        primZeichenkette = p.AddPRIM("Zeichenkette");
                                                                                        primCode = p.AddPRIM("Code");
                                                                                        primDecimal = p.AddPRIM("Decimal");
                                                                                    });
                                                                bLibrary.AddPackage("ENUMLibrary",
                                                                                    p =>
                                                                                    {
                                                                                        p.Element.Stereotype = Stereotype.ENUMLibrary;
                                                                                        enumCountryCodes = p.AddENUM("CountryCodes", primCode,
                                                                                                                     new[] {"Austria", "at", "status"},
                                                                                                                     new[] {"Germany", "de", "status"});
                                                                                    });
                                                            }));
            primZeichenkette.AddIsEquivalentToDependency(primString);
            primZeichenkette.AddIsEquivalentToDependency(enumCountryCodes);

            var validationContext = new Mock<IValidationContext>();

            var primLibrary = new ValidatingPRIMLibrary(eaRepository, validationContext.Object);

            primLibrary.ProcessElementCreated(primZeichenkette);
            Assert.AreEqual(1, primLibrary.PRIMs.Count());
            validationContext.Verify(context => context.AddValidationIssue(It.Is<InvalidSupplierForIsEquivalentTo>(issue =>
                                                                                                                   issue.GUID == primZeichenkette.ElementGUID &&
                                                                                                                   issue.SupplierStereotype == enumCountryCodes.Stereotype
                                                                               )), Times.Exactly(1));
        }

        [Test]
        public void C544b_A_PRIM_which_is_the_source_of_an_isEquivalentTo_dependency_must_not_be_the_target_of_an_isEquivalentTo_dependency()
        {
            var eaRepository = new EmptyEARepository();
            Element primString = null;
            Element primZeichenkette = null;
            Element primCode = null;
            Element primDecimal = null;
            Element enumCountryCodes = null;
            eaRepository.AddModel("Model",
                                  model => model.AddPackage("bLibrary",
                                                            bLibrary =>
                                                            {
                                                                bLibrary.Element.Stereotype = Stereotype.bLibrary;
                                                                bLibrary.AddPackage("PRIMLibrary",
                                                                                    p =>
                                                                                    {
                                                                                        p.Element.Stereotype = Stereotype.PRIMLibrary;
                                                                                        primString = p.AddPRIM("String");
                                                                                        primZeichenkette = p.AddPRIM("Zeichenkette");
                                                                                        primCode = p.AddPRIM("Code");
                                                                                        primDecimal = p.AddPRIM("Decimal");
                                                                                    });
                                                                bLibrary.AddPackage("ENUMLibrary",
                                                                                    p =>
                                                                                    {
                                                                                        p.Element.Stereotype = Stereotype.ENUMLibrary;
                                                                                        enumCountryCodes = p.AddENUM("CountryCodes", primCode,
                                                                                                                     new[] { "Austria", "at", "status" },
                                                                                                                     new[] { "Germany", "de", "status" });
                                                                                    });
                                                            }));
            primZeichenkette.AddIsEquivalentToDependency(primString);
            primCode.AddIsEquivalentToDependency(primZeichenkette);

            var validationContext = new Mock<IValidationContext>();

            var primLibrary = new ValidatingPRIMLibrary(eaRepository, validationContext.Object);

            primLibrary.ProcessElementCreated(primZeichenkette);
            Assert.AreEqual(1, primLibrary.PRIMs.Count());
            validationContext.Verify(context => context.AddValidationIssue(It.Is<ElementIsSourceAndTargetOfIsEquivalentTo>(issue => issue.GUID == primZeichenkette.ElementGUID)), Times.Exactly(1));
        }
    }
}