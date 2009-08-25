using System.Collections.Generic;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts.otf.validators.constraints
{
    public class ParentMustBeModelOrBInformationVOrBLibrary:SafeConstraint<RepositoryItem>
    {
        protected override IEnumerable<ConstraintViolation> SafeCheck(RepositoryItem item)
        {
            string parentStereotype = item.Parent.Stereotype;
            if (!(item.Parent.ParentId.IsNull || Stereotype.BInformationV == parentStereotype || Stereotype.bLibrary == parentStereotype))
            {
                yield return new ConstraintViolation(item.Id, item.Id, 
                                                     "The parent package of " + item.Name + 
                                                     " must be a model"+
                                                     " or have stereotype " + Stereotype.BInformationV +
                                                     " or have stereotype " + Stereotype.bLibrary + 
                                                     ".");
            }
        }
    }
}