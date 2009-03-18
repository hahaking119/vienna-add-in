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