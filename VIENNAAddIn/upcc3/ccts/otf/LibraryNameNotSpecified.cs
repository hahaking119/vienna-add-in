namespace VIENNAAddIn.upcc3.ccts.otf
{
    internal class LibraryNameNotSpecified : PackageValidationIssue
    {
        public LibraryNameNotSpecified(int packageId) : base(packageId, packageId)
        {
        }

        public override string Message
        {
            get { return "The name of library " + ItemId + " is not specified."; }
        }
    }
}