using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts.otf.validators.constraints
{
    public class MustNotContainAnyPackages: SafeConstraint<IRepositoryItem>
    {
        protected override IEnumerable<ConstraintViolation> SafeCheck(IRepositoryItem item)
        {
            foreach (RepositoryItem child in item.Children)
            {
                if (child.Id.Type == ItemId.ItemType.Package)
                {
                    yield return new ConstraintViolation(item.Id, child.Id, item.Data.Name + " must not contain any packages.");
                }
            }
        }
    }
}