using Moq;
using NUnit.Framework;
using VIENNAAddIn.upcc3.ccts.otf;

namespace VIENNAAddInUnitTests.upcc3.ccts.otf
{
    [TestFixture]
    public class Given_a_Validator
    {
        private ValidationService validationService;
        private Mock<IRepositoryItem> itemMock;
        private IValidationIssue[] itemIssues;
        private Mock<IConstraint> constraintMock;

        [SetUp]
        public void Context()
        {
            validationService = new ValidationService();
            ItemId itemId = ItemId.ForElement(1);
            itemIssues = new IValidationIssue[] {new TestValidationIssue(itemId, itemId)};
            itemMock = new Mock<IRepositoryItem>();
            itemMock.SetupGet(element => element.Id).Returns(itemId);
            constraintMock = new Mock<IConstraint>();
            constraintMock.Setup(c => c.Matches(It.IsAny<IRepositoryItem>())).Returns(true);
            constraintMock.Setup(c => c.Check(It.IsAny<IRepositoryItem>())).Returns(itemIssues);
            validationService.AddConstraint(constraintMock.Object);
        }

        [Test]
        public void When_an_item_is_validated_Then_the_validator_should_contain_the_items_validation_issues()
        {
            validationService.ItemCreatedOrModified(itemMock.Object);
            validationService.Validate();
            AssertCollections.AreEqual(itemIssues, validationService.ValidationIssues, issue => issue.Id);
        }

        [Test]
        public void When_an_item_is_created_Then_the_item_should_be_validated_exactly_once()
        {
            validationService.ItemCreatedOrModified(itemMock.Object);
            validationService.Validate();
            constraintMock.Verify(c => c.Check(It.IsAny<IRepositoryItem>()), Times.Exactly(1));
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