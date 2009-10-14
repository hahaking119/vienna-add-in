using System;

namespace VIENNAAddIn.validator.upcc3.onTheFly
{
    public class ValidationIssueAddedEventArgs : EventArgs
    {
        public ValidationIssue Issue { get; private set; }

        public ValidationIssueAddedEventArgs(ValidationIssue issue)
        {
            Issue = issue;
        }
    }
}