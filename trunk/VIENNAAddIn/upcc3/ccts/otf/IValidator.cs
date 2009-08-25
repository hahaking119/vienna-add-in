using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts.otf
{
    public interface IValidator
    {
        bool Matches(object item);
        IEnumerable<ConstraintViolation> Validate(object item);
    }
}