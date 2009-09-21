using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts.otf
{
    public interface IConstraint
    {
        IEnumerable<ConstraintViolation> Check(object item);
    }
}