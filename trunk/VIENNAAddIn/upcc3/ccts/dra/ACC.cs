// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using CctsRepository;
using EA;
using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddInUtils;
using Stereotype=CctsRepository.Stereotype;

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

        protected override bool DeleteConnectorOnUpdate(Connector connector)
        {
            return connector.IsIsEquivalentTo() || IsASCC(connector);
        }

        ///<summary>
        ///</summary>
        public IEnumerable<string> UsageRules
        {
            get { return element.GetTaggedValues(TaggedValues.usageRule); }
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
                Connector connector = Connectors.FirstOrDefault(EAConnectorExtensions.IsIsEquivalentTo);
                return connector != null ? repository.GetACC(connector.SupplierID) : null;
            }
        }

        #endregion

        private bool IsASCC(Connector connector)
        {
            return connector.IsASCC() && connector.GetAssociatingEnd(element.ElementID).Aggregation != (int) EAAggregationKind.None;
        }
    }
}