// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System;
using System.Linq;
using EA;
using VIENNAAddIn.upcc3.ccts.util;
using Stereotype=VIENNAAddIn.upcc3.ccts.util.Stereotype;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    internal class PRIM : UpccClass<PRIMSpec>, IPRIM
    {
        public PRIM(CCRepository repository, Element element) : base(repository, element, Stereotype.PRIM)
        {
        }

        #region IPRIM Members

        public override string DictionaryEntryName
        {
            get
            {
                string value = base.DictionaryEntryName;
                if (string.IsNullOrEmpty(value))
                {
                    value = Name;
                }
                return value;
            }
        }

        protected override bool DeleteConnectorOnUpdate(Connector connector)
        {
            return connector.IsIsEquivalentTo();
        }

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