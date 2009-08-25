using System.Linq;
using Moq;
using NUnit.Framework;
using VIENNAAddIn.upcc3.ccts.otf;

namespace VIENNAAddInUnitTests.upcc3.ccts.otf
{
    [TestFixture]
    public class ValidationServiceWithOneValidatedItemTests
    {
        private ValidationService validationService;
        private Mock<IRepositoryItem> itemMock;
        private ConstraintViolation[] itemConstraintViolations;
        private Mock<IValidator> validatorMock;

        [SetUp]
        public void Context()
        {
            validationService = new ValidationService();
            ItemId itemId = ItemId.ForElement(1);
            itemConstraintViolations = new[] { new ConstraintViolation(itemId, itemId, "A constraint has been violated.") };

            itemMock = new Mock<IRepositoryItem>();
            itemMock.SetupGet(element => element.Id).Returns(itemId);

            validatorMock = new Mock<IValidator>();
            validatorMock.Setup(c => c.Matches(It.IsAny<IRepositoryItem>())).Returns(true);
            validatorMock.Setup(c => c.Validate(It.IsAny<IRepositoryItem>())).Returns(itemConstraintViolations);
            validationService.AddValidator(validatorMock.Object);

            validationService.ItemCreatedOrModified(itemMock.Object);
            validationService.Validate();
        }

        [Test]
        public void When_the_item_is_modified_Then_its_validation_issues_should_be_removed()
        {
            validationService.ItemCreatedOrModified(itemMock.Object);
            Assert.AreEqual(0, validationService.ValidationIssues.Count());
        }

        [Test]
        public void When_the_item_is_modified_Then_it_should_be_revalidated()
        {
            validationService.ItemCreatedOrModified(itemMock.Object);
            validationService.Validate();
            Assert.AreEqual(1, validationService.ValidationIssues.Count());
            validatorMock.Verify(c => c.Validate(It.IsAny<IRepositoryItem>()), Times.Exactly(2));
        }

        [Test]
        public void When_the_item_is_not_modified_Then_it_should_not_be_revalidated()
        {
            validationService.Validate();
            validatorMock.Verify(c => c.Validate(It.IsAny<IRepositoryItem>()), Times.Exactly(1));
        }

        [Test]
        public void When_the_item_is_deleted_Then_its_validation_issues_should_be_removed()
        {
            validationService.ItemDeleted(itemMock.Object);
            Assert.AreEqual(0, validationService.ValidationIssues.Count());
        }

        [Test]
        public void When_the_item_is_modified_and_then_deleted_Then_it_should_not_be_validated()
        {
            validationService.ItemCreatedOrModified(itemMock.Object);
            validationService.ItemDeleted(itemMock.Object);
            validationService.Validate();
            validatorMock.Verify(c => c.Validate(It.IsAny<IRepositoryItem>()), Times.Exactly(1));
        }
    }
}