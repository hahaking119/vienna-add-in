using System.Linq;
using Moq;
using NUnit.Framework;
using VIENNAAddIn.upcc3.ccts.otf;

namespace VIENNAAddInUnitTests.upcc3.ccts.otf
{
    [TestFixture]
    public class Given_a_Validator_with_one_validated_item
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
            itemIssues = OneValidationIssue(itemId, itemId);

            itemMock = new Mock<IRepositoryItem>();
            itemMock.SetupGet(element => element.Id).Returns(itemId);

            constraintMock = new Mock<IConstraint>();
            constraintMock.Setup(c => c.Matches(It.IsAny<IRepositoryItem>())).Returns(true);
            constraintMock.Setup(c => c.Check(It.IsAny<IRepositoryItem>())).Returns(itemIssues);
            validationService.AddConstraint(constraintMock.Object);

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
            constraintMock.Verify(c => c.Check(It.IsAny<IRepositoryItem>()), Times.Exactly(2));
        }

        [Test]
        public void When_the_item_is_not_modified_Then_it_should_not_be_revalidated()
        {
            validationService.Validate();
            constraintMock.Verify(c => c.Check(It.IsAny<IRepositoryItem>()), Times.Exactly(1));
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
            constraintMock.Verify(c => c.Check(It.IsAny<IRepositoryItem>()), Times.Exactly(1));
        }

        private static IValidationIssue[] OneValidationIssue(ItemId validatedItemId, ItemId itemId)
        {
            return new IValidationIssue[] {new TestValidationIssue(validatedItemId, itemId)};
        }
    }
}