using System.Collections.Generic;
using EA;

namespace VIENNAAddIn.validator.upcc3.onTheFly
{
    internal class InvalidStereotype : ValidationIssue
    {
        private readonly string expectedStereotype;

        public InvalidStereotype(Repository repository, string guid, ObjectType objectType, string expectedStereotype)
            : base(repository, guid, objectType)
        {
            this.expectedStereotype = expectedStereotype;
        }

        public override string Message
        {
            get
            {
                switch (objectType)
                {
                    case ObjectType.otElement:
                        {
                            Element element = repository.GetElementByGuid(guid);
                            return "Element " + element.Name + " has an invalid stereotype: <<" + element.Stereotype +
                                   ">>. Expected stereotype: <<" + expectedStereotype + ">>.";
                        }
                }
                return "ERROR: Unknown object type: " + objectType;
            }
        }

        public override IEnumerable<QuickFix> QuickFixes
        {
            get { return new QuickFix[] {new SetStereotypeQuickFix(expectedStereotype)}; }
        }
    }
}