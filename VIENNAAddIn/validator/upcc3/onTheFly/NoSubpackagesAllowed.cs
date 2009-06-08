using System.Collections.Generic;
using EA;

namespace VIENNAAddIn.validator.upcc3.onTheFly
{
    public class NoSubpackagesAllowed : ValidationIssue
    {
        public NoSubpackagesAllowed(string guid)
            : base(guid)
        {
        }

        public override IEnumerable<QuickFix> QuickFixes
        {
            get { return new QuickFix[] {new DeletePackage(GUID)}; }
        }

        public override object GetItem(Repository repository)
        {
            return repository.GetPackageByGuid(GUID);
        }
    }
}