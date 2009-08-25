using System.Collections.Generic;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts.otf.validators.constraints
{
    public class ElementMustHaveStereotype:SafeConstraint<IRepositoryItem>
    {
        private readonly string stereotype;

        public ElementMustHaveStereotype(string stereotype)
        {
            this.stereotype = stereotype;
        }

        protected override IEnumerable<ConstraintViolation> SafeCheck(IRepositoryItem item)
        {
            foreach (RepositoryItem child in item.Children)
            {
                if (child.Id.Type == ItemId.ItemType.Element)
                {
                    if (child.Data.Stereotype != Stereotype.PRIM)
                    {
                        yield return new ConstraintViolation(item.Id, child.Id, "Elements of " + item.Data.Name + " must have stereotype " + stereotype + ".");
                    }
                }
            }
        }
    }
}