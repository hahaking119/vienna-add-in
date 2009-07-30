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

        [SetUp]
        public void Context()
        {
            validationService = new ValidationService();
            ItemId itemId = ItemId.ForElement(1);
            itemIssues = OneValidationIssue(itemId, itemId);
            itemMock = new Mock<IRepositoryItem>();
            itemMock.SetupGet(element => element.Id).Returns(itemId);
            itemMock.Setup(element => element.Validate()).Returns(itemIssues);
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
            itemMock.Verify(item => item.Validate(), Times.Exactly(2));
        }

        [Test]
        public void When_the_item_is_not_modified_Then_it_should_not_be_revalidated()
        {
            validationService.Validate();
            itemMock.Verify(item => item.Validate(), Times.Exactly(1));
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
            itemMock.Verify(item => item.Validate(), Times.Exactly(1));
        }

        private static IValidationIssue[] OneValidationIssue(ItemId validatedItemId, ItemId itemId)
        {
            return new IValidationIssue[] {new TestValidationIssue(validatedItemId, itemId)};
        }
    }
}