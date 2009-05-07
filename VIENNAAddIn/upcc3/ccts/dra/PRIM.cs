// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
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

        public IPRIM IsEquivalentTo
        {
            get
            {
                Connector connector = Connectors.FirstOrDefault(Stereotype.IsIsEquivalentTo);
                return connector != null ? repository.GetPRIM(connector.SupplierID) : null;
            }
        }

        #endregion

        protected override void AddConnectors(PRIMSpec spec)
        {
            if (spec.IsEquivalentTo != null)
            {
                element.AddDependency(Stereotype.IsEquivalentTo, spec.IsEquivalentTo.Id, "1", "1");
            }
        }
    }
}