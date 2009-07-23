namespace VIENNAAddIn.upcc3.ccts.otf
{
    internal class InvalidSubPackageStereotype : PackageValidationIssue
    {
        private readonly string packageName;
        private readonly string subPackageName;

        public InvalidSubPackageStereotype(int packageId, int subPackageId, string packageName, string subPackageName) : base(packageId, subPackageId)
        {
            this.packageName = packageName;
            this.subPackageName = subPackageName;
        }

        public override string Message
        {
            get { return "Sub-package '" + subPackageName + "' of package '" + packageName + "' has an invalid stereotype."; }
        }
    }
}