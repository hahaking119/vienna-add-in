using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts.otf.validators.constraints
{
    public class ParentPackageMustHaveStereotype<T> : SafeConstraint<T> where T : RepositoryItem
    {
        private readonly string stereotype;

        public ParentPackageMustHaveStereotype(string stereotype)
        {
            this.stereotype = stereotype;
        }

        protected override IEnumerable<ConstraintViolation> SafeCheck(T item)
        {
            if (stereotype != item.Parent.Stereotype)
            {
                yield return new ConstraintViolation(item.Id, item.Id, "The parent package of " + item.Name + " must have stereotype " + stereotype + ", but has " + item.Parent.Stereotype + ".");
            }
        }
    }
}