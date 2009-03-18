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
    internal class ENUM : UpccClass, IENUM
    {
        public ENUM(CCRepository repository, Element element) : base(repository, element, "ENUM")
        {
        }

        #region IENUM Members

        public string AgencyIdentifier
        {
            get {
                var tv = ElementExtensions.GetTaggedValue(element, (TaggedValues) TaggedValues.AgencyIdentifier);
                return tv ?? string.Empty;
            }
        }

        public string AgencyName
        {
            get {
                var tv = ElementExtensions.GetTaggedValue(element, (TaggedValues) TaggedValues.AgencyName);
                return tv ?? string.Empty;
            }
        }

        public string EnumerationURI
        {
            get {
                var tv = ElementExtensions.GetTaggedValue(element, (TaggedValues) TaggedValues.EnumerationURI);
                return tv ?? string.Empty;
            }
        }

        public IENUM IsEquivalentTo
        {
            get
            {
                Connector connector = Connectors.FirstOrDefault(Stereotype.IsIsEquivalentTo);
                return connector != null ? repository.GetENUM(connector.SupplierID) : null;
            }
        }

        #endregion
    }
}