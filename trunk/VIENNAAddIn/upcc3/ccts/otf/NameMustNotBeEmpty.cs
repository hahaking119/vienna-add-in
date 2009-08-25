using System.Collections.Generic;
using VIENNAAddIn.upcc3.ccts.otf.validators.constraints;

namespace VIENNAAddIn.upcc3.ccts.otf
{
    public class NameMustNotBeEmpty : SafeConstraint<IRepositoryItem>
    {
        protected override IEnumerable<ConstraintViolation> SafeCheck(IRepositoryItem item)
        {
            if (string.IsNullOrEmpty(item.Data.Name))
            {
                yield return new ConstraintViolation(item.Id, item.Id, "The name of library " + item.Id + " is not specified.");
            }
        }
    }
}