using System;
using System.Collections.Generic;
using EA;

namespace VIENNAAddIn.validator.upcc3.onTheFly
{
    public class DuplicateElementName : ValidationIssue
    {
        public DuplicateElementName(string guid) : base(guid)
        {
        }

        public override IEnumerable<QuickFix> QuickFixes
        {
            get { return new QuickFix[] { new RenameElementQuickFix() }; }
        }

        public override object GetItem(Repository repository)
        {
            return repository.GetElementByGuid(GUID);
        }
    }
}