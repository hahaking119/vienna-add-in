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
    internal class PRIM : UpccClass, IPRIM
    {
        public PRIM(CCRepository repository, Element element) : base(repository, element, "PRIM")
        {
        }

        #region IPRIM Members

        public string Pattern
        {
            get {
                var tv = ElementExtensions.GetTaggedValue(element, (TaggedValues) TaggedValues.Pattern);
                return tv ?? string.Empty;
            }
        }

        public string FractionDigits
        {
            get {
                var tv = ElementExtensions.GetTaggedValue(element, (TaggedValues) TaggedValues.FractionDigits);
                return tv ?? string.Empty;
            }
        }

        public string Length
        {
            get {
                var tv = ElementExtensions.GetTaggedValue(element, (TaggedValues) TaggedValues.Length);
                return tv ?? string.Empty;
            }
        }

        public string MaxExclusive
        {
            get {
                var tv = ElementExtensions.GetTaggedValue(element, (TaggedValues) TaggedValues.MaxExclusive);
                return tv ?? string.Empty;
            }
        }

        public string MaxInclusive
        {
            get {
                var tv = ElementExtensions.GetTaggedValue(element, (TaggedValues) TaggedValues.MaxInclusive);
                return tv ?? string.Empty;
            }
        }

        public string MaxLength
        {
            get {
                var tv = ElementExtensions.GetTaggedValue(element, (TaggedValues) TaggedValues.MaxLength);
                return tv ?? string.Empty;
            }
        }

        public string MinExclusive
        {
            get {
                var tv = ElementExtensions.GetTaggedValue(element, (TaggedValues) TaggedValues.MinExclusive);
                return tv ?? string.Empty;
            }
        }

        public string MinInclusive
        {
            get {
                var tv = ElementExtensions.GetTaggedValue(element, (TaggedValues) TaggedValues.MinInclusive);
                return tv ?? string.Empty;
            }
        }

        public string MinLength
        {
            get {
                var tv = ElementExtensions.GetTaggedValue(element, (TaggedValues) TaggedValues.MinLength);
                return tv ?? string.Empty;
            }
        }

        public string TotalDigits
        {
            get {
                var tv = ElementExtensions.GetTaggedValue(element, (TaggedValues) TaggedValues.TotalDigits);
                return tv ?? string.Empty;
            }
        }

        public string WhiteSpace
        {
            get {
                var tv = ElementExtensions.GetTaggedValue(element, (TaggedValues) TaggedValues.WhiteSpace);
                return tv ?? string.Empty;
            }
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