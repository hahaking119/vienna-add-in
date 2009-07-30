namespace VIENNAAddIn.upcc3.ccts.otf
{
    internal class InvalidParentPackage : AbstractValidationIssue
    {
        private readonly string packageName;

        public InvalidParentPackage(ItemId packageId, string packageName) : base(packageId, packageId)
        {
            this.packageName = packageName;
        }

        public override string Message
        {
            get { return "The parent package of '" + packageName + "' is not valid."; }
        }
    }
}