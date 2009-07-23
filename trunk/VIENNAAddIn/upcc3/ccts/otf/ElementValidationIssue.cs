using EA;

namespace VIENNAAddIn.upcc3.ccts.otf
{
    internal abstract class ElementValidationIssue : AbstractValidationIssue
    {
        protected ElementValidationIssue(int validatedItemId, int itemId)
            : base(validatedItemId, itemId)
        {
        }

        public override object ResolveItem(Repository repository)
        {
            return repository.GetElementByID(ItemId);
        }
    }
}