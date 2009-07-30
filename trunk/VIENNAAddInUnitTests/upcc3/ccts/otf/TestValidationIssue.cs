using VIENNAAddIn.upcc3.ccts.otf;

namespace VIENNAAddInUnitTests.upcc3.ccts.otf
{
    public class TestValidationIssue : AbstractValidationIssue
    {
        public TestValidationIssue(ItemId validatedItemId, ItemId itemId) : base(validatedItemId, itemId)
        {
        }

        public override string Message
        {
            get { return "test validation issue"; }
        }
    }
}