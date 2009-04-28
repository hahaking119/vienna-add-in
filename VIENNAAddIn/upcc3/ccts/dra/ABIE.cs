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
    internal class ABIE : UpccClass, IABIE, IUpdateable<ABIESpec>
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
            get { return element.GetTaggedValues(TaggedValues.UsageRule); }
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

        #region IUpdateable<ABIESpec> Members

        public void Update(ABIESpec spec)
        {
            Update((CCTSElementSpec) spec);

            for (var i = (short) (element.Connectors.Count - 1); i >= 0; i--)
            {
                element.Connectors.Delete(i);
            }
            element.Connectors.Refresh();

            if (spec.BasedOn != null)
            {
                element.AddDependency(Stereotype.BasedOn, spec.BasedOn.Id, "1", "1");
            }

            if (spec.IsEquivalentTo != null)
            {
                element.AddDependency(Stereotype.IsEquivalentTo, spec.IsEquivalentTo.Id, "1", "1");
            }

            if (spec.ASBIEs != null)
            {
                foreach (ASBIESpec asbie in spec.ASBIEs)
                {
                    element.AddAggregation(AggregationKind.Composite, Stereotype.ASBIE, asbie.Name,
                                           asbie.AssociatedABIEId, asbie.LowerBound, asbie.UpperBound);
                }
            }

            element.Connectors.Refresh();

            for (var i = (short) (element.Attributes.Count - 1); i >= 0; i--)
            {
                element.Attributes.Delete(i);
            }
            element.Attributes.Refresh();

            if (spec.BBIEs != null)
            {
                foreach (BBIESpec bbie in spec.BBIEs)
                {
                    element.AddAttribute(Stereotype.BBIE, bbie.Name, bbie.Type.Name, bbie.Type.Id, bbie.LowerBound,
                                         bbie.UpperBound,
                                         bbie.GetTaggedValues());
                }
            }
            element.Attributes.Refresh();

            element.Update();
            element.Refresh();
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