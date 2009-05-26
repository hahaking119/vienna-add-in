using System.Collections.Generic;
using EA;

namespace VIENNAAddIn.validator.upcc3.onTheFly
{
    public class InvalidElementStereotype : ValidationIssue
    {
        private readonly string expectedStereotype;

        public InvalidElementStereotype(string guid, string expectedStereotype)
            : base(guid)
        {
            this.expectedStereotype = expectedStereotype;
        }

        public override IEnumerable<QuickFix> QuickFixes
        {
            get { return new QuickFix[] {new SetElementStereotypeQuickFix(expectedStereotype)}; }
        }

        public override object GetItem(Repository repository)
        {
            return repository.GetElementByGuid(GUID);
        }
    }
}