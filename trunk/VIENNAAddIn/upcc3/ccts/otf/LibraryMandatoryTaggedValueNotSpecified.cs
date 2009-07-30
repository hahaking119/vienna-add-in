using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts.otf
{
    internal class LibraryMandatoryTaggedValueNotSpecified : AbstractValidationIssue
    {
        private readonly string packageName;
        private TaggedValues TaggedValue { get; set; }

        public LibraryMandatoryTaggedValueNotSpecified(ItemId packageId, string packageName, TaggedValues taggedValue) : base(packageId, packageId)
        {
            this.packageName = packageName;
            TaggedValue = taggedValue;
        }

        public override string Message
        {
            get { return "Tagged value '" + TaggedValue + "' of library '" + packageName + "' must be specified."; }
        }
    }
}