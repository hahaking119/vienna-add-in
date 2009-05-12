// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System.Collections.Generic;
using System.Linq;
using EA;
using VIENNAAddIn.upcc3.ccts.util;
using Stereotype=VIENNAAddIn.upcc3.ccts.util.Stereotype;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    internal class ABIE : UpccClass<ABIESpec>, IABIE
    {
        public ABIE(CCRepository repository, Element element) : base(repository, element, Stereotype.ABIE)
        {
        }

        #region IABIE Members

        public override string DictionaryEntryName
        {
            get
            {
                string value = base.DictionaryEntryName;
                if (string.IsNullOrEmpty(value))
                {
                    value = Name + ". Details";
                }
                return value;
            }
        }

        public IEnumerable<string> UsageRules
        {
            get { return element.GetTaggedValues(TaggedValues.usageRule); }
        }

        public IEnumerable<IBBIE> BBIEs
        {
            get
            {
                return
                    Attributes.Convert(a => (IBBIE) new BBIE(repository, a, this));
            }
        }

        public IEnumerable<IASBIE> ASBIEs
        {
            get
            {
                return
                    Connectors.Where(IsASBIE).Convert(c => (IASBIE) new ASBIE(repository, c, this));
            }
        }

        public IACC BasedOn
        {
            get
            {
                Connector connector = Connectors.FirstOrDefault(Stereotype.IsBasedOn);
                return connector != null ? repository.GetACC(connector.SupplierID) : null;
            }
        }

        public IABIE IsEquivalentTo
        {
            get
            {
                Connector connector = Connectors.FirstOrDefault(Stereotype.IsIsEquivalentTo);
                return connector != null ? repository.GetABIE(connector.SupplierID) : null;
            }
        }

        #endregion

        private bool IsASBIE(Connector connector)
        {
            if (connector.IsASBIE())
            {
                if (connector.ClientID == element.ElementID)
                {
                    return connector.ClientEnd.Aggregation != (int) AggregationKind.None;
                }
                if (connector.SupplierID == element.ElementID)
                {
                    return connector.SupplierEnd.Aggregation != (int) AggregationKind.None;
                }
            }
            return false;
        }
    }
}