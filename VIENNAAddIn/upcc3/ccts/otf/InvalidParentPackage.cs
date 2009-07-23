using System;

namespace VIENNAAddIn.upcc3.ccts.otf
{
    internal class InvalidParentPackage : PackageValidationIssue
    {
        private readonly string packageName;

        public InvalidParentPackage(int packageId, string packageName) : base(packageId, packageId)
        {
            this.packageName = packageName;
        }

        public override string Message
        {
            get { return "The parent package of '" + packageName + "' is not valid."; }
        }
    }
}