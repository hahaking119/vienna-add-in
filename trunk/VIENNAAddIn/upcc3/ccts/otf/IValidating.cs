using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts.otf
{
    public interface IValidating
    {
        IEnumerable<IValidationIssue> ValidationIssues { get; }
        bool NeedsValidation { get; }
        IEnumerable<IValidationIssue> ValidateAll();
        IEnumerable<IValidationIssue> Validate();
    }
}