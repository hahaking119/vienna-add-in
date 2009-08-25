using System.Collections.Generic;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts.otf.validators.constraints
{
    public class SubPackagesMustBeBusinessLibraries:SafeConstraint<IRepositoryItem>
    {
        protected override IEnumerable<ConstraintViolation> SafeCheck(IRepositoryItem item)
        {
            foreach (RepositoryItem child in item.Children)
            {
                if (child.Id.Type == ItemId.ItemType.Package && !(Stereotype.IsBusinessLibraryStereotype(child.Data.Stereotype)))
                {
                    yield return new ConstraintViolation(item.Id, child.Id, "Sub-packages of " + item.Data.Name + " must have a UPCC business library stereotype.");
                }
            }
        }
    }
}