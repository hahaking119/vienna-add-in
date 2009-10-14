// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
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
            get { return GetTaggedValue(TaggedValues.pattern); }
        }

        public string FractionDigits
        {
            get { return GetTaggedValue(TaggedValues.fractionDigits); }
        }

        public string Length
        {
            get { return GetTaggedValue(TaggedValues.length); }
        }

        public string MaxExclusive
        {
            get { return GetTaggedValue(TaggedValues.maxExclusive); }
        }

        public string MaxInclusive
        {
            get { return GetTaggedValue(TaggedValues.maxInclusive); }
        }

        public string MaxLength
        {
            get { return GetTaggedValue(TaggedValues.maxLength); }
        }

        public string MinExclusive
        {
            get { return GetTaggedValue(TaggedValues.minExclusive); }
        }

        public string MinInclusive
        {
            get { return GetTaggedValue(TaggedValues.minInclusive); }
        }

        public string MinLength
        {
            get { return GetTaggedValue(TaggedValues.minLength); }
        }

        public string TotalDigits
        {
            get { return GetTaggedValue(TaggedValues.totalDigits); }
        }

        public string WhiteSpace
        {
            get { return GetTaggedValue(TaggedValues.whiteSpace); }
        }

        public string ApplyTo
        {
            get { return GetTaggedValue(TaggedValues.applyTo); }
        }

        public ICDT CDT
        {
            get { return repository.GetCDT(connector.SupplierID); }
        }

        #endregion

        private string GetTaggedValue(TaggedValues key)
        {
            return EAConnectorExtensions.GetTaggedValue(connector, key.ToString()) ?? string.Empty;
        }
    }
}