using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts.otf.validators.constraints
{
    public class NameMustNotBeEmpty<T> : SafeConstraint<T> where T:RepositoryItem
    {
        protected override IEnumerable<ConstraintViolation> SafeCheck(T item)
        {
            if (string.IsNullOrEmpty(item.Name))
            {
                yield return new ConstraintViolation(item.Id, item.Id, "The name of " + item.Id + " is not specified.");
            }
        }
    }
}