using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts.otf.validators.constraints
{
    public class PackageNameMustNotBeEmpty : SafeConstraint<RepositoryItem>
    {
        protected override IEnumerable<ConstraintViolation> SafeCheck(RepositoryItem item)
        {
            if (string.IsNullOrEmpty(item.Name))
            {
                yield return new ConstraintViolation(item.Id, item.Id, "The name of package " + item.Id + " is not specified.");
            }
        }
    }
}