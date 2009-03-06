using System.Linq;
using EA;
using VIENNAAddIn.upcc3.ccts.util;
using Stereotype=VIENNAAddIn.upcc3.ccts.util.Stereotype;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    internal class PRIM : UpccClass, IPRIM
    {
        public PRIM(CCRepository repository, Element element) : base(repository, element, "PRIM")
        {
        }

        #region IPRIM Members

        public string Pattern
        {
            get { return element.GetTaggedValue(TaggedValues.Pattern); }
        }

        public string FractionDigits
        {
            get { return element.GetTaggedValue(TaggedValues.FractionDigits); }
        }

        public string Length
        {
            get { return element.GetTaggedValue(TaggedValues.Length); }
        }

        public string MaxExclusive
        {
            get { return element.GetTaggedValue(TaggedValues.MaxExclusive); }
        }

        public string MaxInclusive
        {
            get { return element.GetTaggedValue(TaggedValues.MaxInclusive); }
        }

        public string MaxLength
        {
            get { return element.GetTaggedValue(TaggedValues.MaxLength); }
        }

        public string MinExclusive
        {
            get { return element.GetTaggedValue(TaggedValues.MinExclusive); }
        }

        public string MinInclusive
        {
            get { return element.GetTaggedValue(TaggedValues.MinInclusive); }
        }

        public string MinLength
        {
            get { return element.GetTaggedValue(TaggedValues.MinLength); }
        }

        public string TotalDigits
        {
            get { return element.GetTaggedValue(TaggedValues.TotalDigits); }
        }

        public string WhiteSpace
        {
            get { return element.GetTaggedValue(TaggedValues.WhiteSpace); }
        }

        public IPRIM IsEquivalentTo
        {
            get
            {
                Connector connector = Connectors.FirstOrDefault(Stereotype.IsIsEquivalentTo);
                return connector != null ? repository.GetPRIM(connector.SupplierID) : null;
            }
        }

        #endregion
    }
}