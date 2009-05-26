using System.Linq;
using EA;
using Moq;
using NUnit.Framework;
using VIENNAAddIn.validator.upcc3.onTheFly;
using Stereotype=VIENNAAddIn.upcc3.ccts.util.Stereotype;

namespace VIENNAAddInUnitTests.validator.upcc3.onTheFly
{
    [TestFixture]
    public class OnTheFlyValidatorTest
    {
        private static Mock<Element> MockPRIM(string name, string guid)
        {
            var primMock = new Mock<Element>();
            primMock.SetupGet(e => e.Stereotype).Returns(Stereotype.PRIM);
            primMock.SetupGet(e => e.Name).Returns(name);
            primMock.SetupGet(e => e.ElementGUID).Returns(guid);
            return primMock;
        }

        [Test]
        public void C514h_A_PRIMLibrary_shall_only_contain_PRIMs()
        {
            var validationContext = new Mock<IValidationContext>();

            var primLibrary = new ValidatingPRIMLibrary(validationContext.Object);

            Mock<Element> primMock = MockPRIM("String", "27");

            var nonPRIMMock = new Mock<Element>();
            nonPRIMMock.SetupGet(e => e.Stereotype).Returns("foobar");
            nonPRIMMock.SetupGet(e => e.Name).Returns("Not a PRIM");
            nonPRIMMock.SetupGet(e => e.ElementGUID).Returns("28");

            primLibrary.ProcessElementCreated(primMock.Object);
            Assert.AreEqual(1, primLibrary.PRIMs.Where(prim => prim.Name == "String").Count());

            primLibrary.ProcessElementCreated(nonPRIMMock.Object);
            validationContext.Verify(context => context.AddValidationIssue(It.Is<InvalidElementStereotype>(issue => issue.GUID == "28")), Times.Exactly(1));
            Assert.AreEqual(1, primLibrary.PRIMs.Where(prim => prim.Name == "Not a PRIM").Count());
        }

        [Test]
        public void C514n_A_PRIMLibrary_must_not_contain_any_sub_packages()
        {
            var validationContext = new Mock<IValidationContext>();

            var primLibrary = new ValidatingPRIMLibrary(validationContext.Object);

            var packageMock = new Mock<Package>();
            packageMock.SetupGet(package => package.PackageGUID).Returns("21");

            primLibrary.ProcessCreatePackage(packageMock.Object);
            validationContext.Verify(context => context.AddValidationIssue(It.Is<NoSubpackagesAllowed>(issue => issue.GUID == "21")), Times.Exactly(1));
        }

        [Test]
        public void C544a_The_name_of_a_PRIM_shall_be_unique_for_a_given_PRIMLibrary()
        {
            var validationContext = new Mock<IValidationContext>();

            var primLibrary = new ValidatingPRIMLibrary(validationContext.Object);

            Mock<Element> primMock1 = MockPRIM("String", "27");
            Mock<Element> primMock2 = MockPRIM("String", "28");

            primLibrary.ProcessElementCreated(primMock1.Object);
            Assert.AreEqual(1, primLibrary.PRIMs.Where(prim => prim.Name == "String").Count());
            validationContext.Verify(context => context.AddValidationIssue(It.IsAny<DuplicateElementName>()), Times.Never());

            primLibrary.ProcessElementCreated(primMock2.Object);
            validationContext.Verify(context => context.AddValidationIssue(It.Is<DuplicateElementName>(issue => issue.GUID == "27")), Times.Exactly(1));
            validationContext.Verify(context => context.AddValidationIssue(It.Is<DuplicateElementName>(issue => issue.GUID == "28")), Times.Exactly(1));
            Assert.AreEqual(2, primLibrary.PRIMs.Where(prim => prim.Name == "String").Count());
        }
    }
}