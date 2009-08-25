using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using VIENNAAddIn.upcc3.ccts.otf;
using System.Linq;

namespace VIENNAAddInUnitTests.upcc3.ccts.otf
{
    [TestFixture]
    public class ValidationServiceTests
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
            itemConstraintViolations = new[] {new ConstraintViolation(itemId, itemId, "A constraint has been violated.")};
            itemMock = new Mock<IRepositoryItem>();
            itemMock.SetupGet(element => element.Id).Returns(itemId);
            validatorMock = new Mock<IValidator>();
            validatorMock.Setup(c => c.Matches(It.IsAny<IRepositoryItem>())).Returns(true);
            validatorMock.Setup(c => c.Validate(It.IsAny<IRepositoryItem>())).Returns(itemConstraintViolations);
            validationService.AddValidator(validatorMock.Object);
        }

        [Test]
        public void When_an_item_is_validated_Then_the_validator_should_contain_the_items_validation_issues()
        {
            validationService.ItemCreatedOrModified(itemMock.Object);
            validationService.Validate();
            var constraintViolations = new List<ConstraintViolation>(from issue in validationService.ValidationIssues select issue.ConstraintViolation);
            Assert.That(constraintViolations, Is.EquivalentTo(itemConstraintViolations));
        }

        [Test]
        public void When_an_item_is_created_Then_the_item_should_be_validated_exactly_once()
        {
            validationService.ItemCreatedOrModified(itemMock.Object);
            validationService.Validate();
            validatorMock.Verify(c => c.Validate(It.IsAny<IRepositoryItem>()), Times.Exactly(1));
        }

        [Test]
        public void When_validation_issues_are_added_Then_an_event_should_be_generated()
        {
            var eventCount = 0;
            validationService.ValidationIssuesUpdated += issues => eventCount++;
            Assert.AreEqual(0, eventCount);
            validationService.ItemCreatedOrModified(itemMock.Object);
            validationService.Validate();
            Assert.AreEqual(1, eventCount);
        }

        [Test]
        public void When_validation_issues_are_removed_Then_an_event_should_be_generated()
        {
            var eventCount = 0;
            validationService.ValidationIssuesUpdated += issues => eventCount++;
            validationService.ItemCreatedOrModified(itemMock.Object);
            validationService.Validate();
            Assert.AreEqual(1, eventCount);
            validationService.ItemDeleted(itemMock.Object);
            Assert.AreEqual(2, eventCount);
        }
    }
}