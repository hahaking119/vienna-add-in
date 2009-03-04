using EA;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    internal class ENUM : UpccClass, IENUM
    {
        public ENUM(CCRepository repository, Element element) : base(repository, element, "ENUM")
        {
        }

        #region IENUM Members

        public string AgencyIdentifier
        {
            get { return element.GetTaggedValue(TaggedValues.AgencyIdentifier); }
        }

        public string AgencyName
        {
            get { return element.GetTaggedValue(TaggedValues.AgencyName); }
        }

        public string EnumerationURI
        {
            get { return element.GetTaggedValue(TaggedValues.EnumerationURI); }
        }

        #endregion
    }
}