using EA;

namespace VIENNAAddIn.validator.upcc3.onTheFly
{
    public interface OnTheFlyValidator
    {
        void ProcessItemModified(string guid, ObjectType objectType);
        void FocusIssueItem(int issueId);
        void ShowQuickFixes(int issueId);
        void ProcessElementCreated(int id);
    }
}