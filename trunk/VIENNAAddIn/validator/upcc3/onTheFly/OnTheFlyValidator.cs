using EA;

namespace VIENNAAddIn.validator.upcc3.onTheFly
{
    internal interface OnTheFlyValidator
    {
        void ProcessItem(string guid, ObjectType objectType);
        void FocusIssueItem(int issueId);
        void ShowQuickFixes(int issueId);
    }
}