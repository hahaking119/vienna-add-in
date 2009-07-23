using EA;

namespace VIENNAAddIn.upcc3.ccts.otf
{
    internal abstract class PackageValidationIssue : AbstractValidationIssue
    {
        protected PackageValidationIssue(int validatedItemId, int itemId) : base(validatedItemId, itemId)
        {
        }

        public override object ResolveItem(Repository repository)
        {
            return repository.GetPackageByID(ItemId);
        }
    }
}