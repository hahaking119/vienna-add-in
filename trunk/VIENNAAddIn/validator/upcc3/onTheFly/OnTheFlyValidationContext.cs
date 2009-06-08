using System;
using System.Collections.Generic;

namespace VIENNAAddIn.validator.upcc3.onTheFly
{
    public class OnTheFlyValidationContext : IValidationContext
    {
        private readonly Dictionary<int, ValidationIssue> validationIssues = new Dictionary<int, ValidationIssue>();

        public event EventHandler<ValidationIssueAddedEventArgs> ValidationIssueAdded;

        public void AddValidationIssue(ValidationIssue validationIssue)
        {
            validationIssues[validationIssue.Id] = validationIssue;
            ValidationIssueAdded(this, new ValidationIssueAddedEventArgs(validationIssue));
        }

        public ValidationIssue GetIssue(int issueId)
        {
            ValidationIssue issue;
            if (validationIssues.TryGetValue(issueId, out issue))
            {
                return issue;
            }
            return null;
        }
    }
}