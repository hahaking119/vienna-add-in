namespace VIENNAAddIn.upcc3.ccts.otf
{
    internal class InvalidSubPackageStereotype : AbstractValidationIssue
    {
        private readonly string packageName;
        private readonly string subPackageName;

        public InvalidSubPackageStereotype(ItemId packageId, ItemId subPackageId, string packageName, string subPackageName) : base(packageId, subPackageId)
        {
            this.packageName = packageName;
            this.subPackageName = subPackageName;
        }

        public override string Message
        {
            get { return "Sub-package '" + subPackageName + "' of package '" + packageName + "' has an invalid stereotype."; }
        }
    }

    internal class InvalidElementStereotype : AbstractValidationIssue
    {
        private readonly string packageName;
        private readonly string elementName;

        public InvalidElementStereotype(ItemId packageId, ItemId elementId, string packageName, string elementName) : base(packageId, elementId)
        {
            this.packageName = packageName;
            this.elementName = elementName;
        }

        public override string Message
        {
            get { return "Element '" + elementName + "' of package '" + packageName + "' has an invalid stereotype."; }
        }
    }
}