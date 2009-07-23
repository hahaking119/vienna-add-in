namespace VIENNAAddIn.upcc3.ccts.otf
{
    internal class NoElementsAllowed : ElementValidationIssue
    {
        private readonly string packageName;

        public NoElementsAllowed(int packageId, int elementId, string packageName)
            : base(packageId, elementId)
        {
            this.packageName = packageName;
        }

        public override string Message
        {
            get { return "No elements are allowed in library '" + packageName + "'."; }
        }
    }
}