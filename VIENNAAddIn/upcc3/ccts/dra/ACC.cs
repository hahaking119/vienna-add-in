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
    ///<summary>
    ///</summary>
    public class ACC : UpccClass<ACCSpec>, IACC
    {
        ///<summary>
        ///</summary>
        ///<param name="repository"></param>
        ///<param name="element"></param>
        public ACC(CCRepository repository, Element element) : base(repository, element, Stereotype.ACC)
        {
        }

        #region IACC Members

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

        ///<summary>
        ///</summary>
        public IEnumerable<string> UsageRules
        {
            get { return element.GetTaggedValues(TaggedValues.UsageRule); }
        }

        ///<summary>
        ///</summary>
        public IEnumerable<IBCC> BCCs
        {
            get { return Attributes.Convert(a => (IBCC) new BCC(repository, a, this)); }
        }

        ///<summary>
        ///</summary>
        public IEnumerable<IASCC> ASCCs
        {
            get { return Connectors.Where(IsASCC).Convert(c => (IASCC) new ASCC(repository, c, this)); }
        }

        ///<summary>
        ///</summary>
        public IACC IsEquivalentTo
        {
            get
            {
                Connector connector = Connectors.FirstOrDefault(Stereotype.IsIsEquivalentTo);
                return connector != null ? repository.GetACC(connector.SupplierID) : null;
            }
        }

        #endregion

        private bool IsASCC(Connector connector)
        {
            if (connector.IsASCC())
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

        protected override void AddConnectors(ACCSpec spec)
        {
            if (spec.IsEquivalentTo != null)
            {
                element.AddDependency(Stereotype.IsEquivalentTo, spec.IsEquivalentTo.Id, "1", "1");
            }

            if (spec.ASCCs != null)
            {
                foreach (ASCCSpec ascc in spec.ASCCs)
                {
                    element.AddAggregation(AggregationKind.Shared, Stereotype.ASCC, ascc.Name,
                                           ascc.AssociatedACCId, ascc.LowerBound, ascc.UpperBound);
                }
            }
        }

        protected override void AddAttributes(ACCSpec spec)
        {
            if (spec.BCCs != null)
            {
                foreach (BCCSpec bcc in spec.BCCs)
                {
                    element.AddAttribute(Stereotype.BCC, bcc.Name, bcc.Type.Name, bcc.Type.Id, bcc.LowerBound,
                                         bcc.UpperBound, bcc.GetTaggedValues());
                }
            }
        }
    }
}