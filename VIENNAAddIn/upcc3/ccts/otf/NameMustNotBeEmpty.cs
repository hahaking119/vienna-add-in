using System.Collections.Generic;
using VIENNAAddIn.upcc3.ccts.otf.validators.constraints;

namespace VIENNAAddIn.upcc3.ccts.otf
{
    public class NameMustNotBeEmpty : SafeConstraint<RepositoryItem>
    {
        protected override IEnumerable<ConstraintViolation> SafeCheck(RepositoryItem item)
        {
            if (string.IsNullOrEmpty(item.Name))
            {
                yield return new ConstraintViolation(item.Id, item.Id, "The name of library " + item.Id + " is not specified.");
            }
        }
    }
}