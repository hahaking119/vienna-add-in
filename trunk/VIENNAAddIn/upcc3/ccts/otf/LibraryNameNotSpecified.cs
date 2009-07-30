namespace VIENNAAddIn.upcc3.ccts.otf
{
    internal class LibraryNameNotSpecified : AbstractValidationIssue
    {
        public LibraryNameNotSpecified(ItemId packageId) : base(packageId, packageId)
        {
        }

        public override string Message
        {
            get { return "The name of library " + ItemId + " is not specified."; }
        }
    }
}