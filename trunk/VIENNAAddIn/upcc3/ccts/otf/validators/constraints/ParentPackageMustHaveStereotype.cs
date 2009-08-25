using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts.otf.validators.constraints
{
    public class ParentPackageMustHaveStereotype: SafeConstraint<IRepositoryItem>
    {
        private readonly string stereotype;

        public ParentPackageMustHaveStereotype(string stereotype)
        {
            this.stereotype = stereotype;
        }

        protected override IEnumerable<ConstraintViolation> SafeCheck(IRepositoryItem item)
        {
            if (stereotype != item.Parent.Data.Stereotype)
            {
                yield return new ConstraintViolation(item.Id, item.Id, "The parent package of " + item.Data.Name + " must have stereotype " + stereotype + ".");
            }
        }
    }
}