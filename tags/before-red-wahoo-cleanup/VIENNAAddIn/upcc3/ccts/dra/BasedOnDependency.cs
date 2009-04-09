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
            get { return GetTaggedValue(TaggedValues.Pattern); }
        }

        public string FractionDigits
        {
            get { return GetTaggedValue(TaggedValues.FractionDigits); }
        }

        public string Length
        {
            get { return GetTaggedValue(TaggedValues.Length); }
        }

        public string MaxExclusive
        {
            get { return GetTaggedValue(TaggedValues.MaxExclusive); }
        }

        public string MaxInclusive
        {
            get { return GetTaggedValue(TaggedValues.MaxInclusive); }
        }

        public string MaxLength
        {
            get { return GetTaggedValue(TaggedValues.MaxLength); }
        }

        public string MinExclusive
        {
            get { return GetTaggedValue(TaggedValues.MinExclusive); }
        }

        public string MinInclusive
        {
            get { return GetTaggedValue(TaggedValues.MinInclusive); }
        }

        public string MinLength
        {
            get { return GetTaggedValue(TaggedValues.MinLength); }
        }

        public string TotalDigits
        {
            get { return GetTaggedValue(TaggedValues.TotalDigits); }
        }

        public string WhiteSpace
        {
            get { return GetTaggedValue(TaggedValues.WhiteSpace); }
        }

        public string ApplyTo
        {
            get { return GetTaggedValue(TaggedValues.ApplyTo); }
        }

        public ICDT CDT
        {
            get { return repository.GetCDT(connector.SupplierID); }
        }

        #endregion

        private string GetTaggedValue(TaggedValues key)
        {
            return connector.GetTaggedValue(key) ?? string.Empty;
        }
    }
}