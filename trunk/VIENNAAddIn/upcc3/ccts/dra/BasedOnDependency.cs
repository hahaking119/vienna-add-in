using EA;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    internal class BasedOnDependency : IBasedOnDependency
    {
        private readonly Connector connector;
        private readonly CCRepository repository;

        public BasedOnDependency(CCRepository repository, Connector connector)
        {
            this.repository = repository;
            this.connector = connector;
        }

        #region IBasedOnDependency Members

        public string Pattern
        {
            get { return connector.GetTaggedValue(TaggedValues.Pattern); }
        }

        public string FractionDigits
        {
            get { return connector.GetTaggedValue(TaggedValues.FractionDigits); }
        }

        public string Length
        {
            get { return connector.GetTaggedValue(TaggedValues.Length); }
        }

        public string MaxExclusive
        {
            get { return connector.GetTaggedValue(TaggedValues.MaxExclusive); }
        }

        public string MaxInclusive
        {
            get { return connector.GetTaggedValue(TaggedValues.MaxInclusive); }
        }

        public string MaxLength
        {
            get { return connector.GetTaggedValue(TaggedValues.MaxLength); }
        }

        public string MinExclusive
        {
            get { return connector.GetTaggedValue(TaggedValues.MinExclusive); }
        }

        public string MinInclusive
        {
            get { return connector.GetTaggedValue(TaggedValues.MinInclusive); }
        }

        public string MinLength
        {
            get { return connector.GetTaggedValue(TaggedValues.MinLength); }
        }

        public string TotalDigits
        {
            get { return connector.GetTaggedValue(TaggedValues.TotalDigits); }
        }

        public string WhiteSpace
        {
            get { return connector.GetTaggedValue(TaggedValues.WhiteSpace); }
        }

        public string ApplyTo
        {
            get { return connector.GetTaggedValue(TaggedValues.ApplyTo); }
        }

        public ICDT CDT
        {
            get { return repository.GetCDT(connector.SupplierID); }
        }

        #endregion
    }
}