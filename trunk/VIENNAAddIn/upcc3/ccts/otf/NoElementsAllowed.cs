namespace VIENNAAddIn.upcc3.ccts.otf
{
    internal class NoElementsAllowed : AbstractValidationIssue
    {
        private readonly string packageName;

        public NoElementsAllowed(ItemId packageId, ItemId elementId, string packageName)
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