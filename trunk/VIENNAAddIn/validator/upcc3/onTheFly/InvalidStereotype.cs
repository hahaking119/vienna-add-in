using System.Collections.Generic;
using EA;

namespace VIENNAAddIn.validator.upcc3.onTheFly
{
    public class InvalidStereotype : ValidationIssue
    {
        private readonly string expectedStereotype;

        public InvalidStereotype(string guid, string expectedStereotype)
            : base(guid)
        {
            this.expectedStereotype = expectedStereotype;
        }

        public override IEnumerable<QuickFix> QuickFixes
        {
            get { return new QuickFix[] {new SetStereotypeQuickFix(expectedStereotype)}; }
        }
    }
}