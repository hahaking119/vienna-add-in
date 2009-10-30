using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts.otf.validators.constraints
{
    public class MustNotContainAnyPackages: SafeConstraint<RepositoryItem>
    {
        protected override IEnumerable<ConstraintViolation> SafeCheck(RepositoryItem item)
        {
            foreach (RepositoryItem child in item.Children)
            {
                if (child.Id.Type == ItemId.ItemType.Package)
                {
                    yield return new ConstraintViolation(item.Id, child.Id, item.Name + " must not contain any packages.");
                }
            }
        }
    }
}