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
using CctsRepository;
using EA;
using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddInUtils;
using Stereotype=CctsRepository.Stereotype;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    public class ABIE : UpccClass<ABIESpec>, IABIE
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
                Connector connector = Connectors.FirstOrDefault(EAConnectorExtensions.IsBasedOn);
                return connector != null ? repository.GetACC(connector.SupplierID) : null;
            }
        }

        public IABIE IsEquivalentTo
        {
            get
            {
                Connector connector = Connectors.FirstOrDefault(EAConnectorExtensions.IsIsEquivalentTo);
                return connector != null ? repository.GetABIE(connector.SupplierID) : null;
            }
        }

        #endregion

        protected override bool DeleteConnectorOnUpdate(Connector connector)
        {
            return connector.IsBasedOn() || connector.IsIsEquivalentTo() || IsASBIE(connector);
        }

        private bool IsASBIE(Connector connector)
        {
            if (connector.IsASBIE())
            {
                if (connector.ClientID == element.ElementID)
                {
                    return connector.ClientEnd.Aggregation != (int) EAAggregationKind.None;
                }
                if (connector.SupplierID == element.ElementID)
                {
                    return connector.SupplierEnd.Aggregation != (int) EAAggregationKind.None;
                }
            }
            return false;
        }

        protected override IEnumerable<TaggedValueSpec> GetTaggedValueSpecs(ABIESpec spec)
        {
            return new List<TaggedValueSpec>
                   {
                       new TaggedValueSpec(TaggedValues.businessTerm, spec.BusinessTerms),
                       new TaggedValueSpec(TaggedValues.definition, spec.Definition),
                       new TaggedValueSpec(TaggedValues.dictionaryEntryName, spec.DictionaryEntryName),
                       new TaggedValueSpec(TaggedValues.languageCode, spec.LanguageCode),
                       new TaggedValueSpec(TaggedValues.uniqueIdentifier, spec.UniqueIdentifier),
                       new TaggedValueSpec(TaggedValues.usageRule, spec.UsageRules),
                       new TaggedValueSpec(TaggedValues.versionIdentifier, spec.VersionIdentifier),
                   };
        }

        private static IEnumerable<TaggedValueSpec> GetBbieTaggedValueSpecs(BBIESpec spec)
        {
            return new List<TaggedValueSpec>
                   {
                       new TaggedValueSpec(TaggedValues.businessTerm, spec.BusinessTerms),
                       new TaggedValueSpec(TaggedValues.definition, spec.Definition),
                       new TaggedValueSpec(TaggedValues.dictionaryEntryName, spec.DictionaryEntryName),
                       new TaggedValueSpec(TaggedValues.languageCode, spec.LanguageCode),
                       new TaggedValueSpec(TaggedValues.sequencingKey, spec.SequencingKey),
                       new TaggedValueSpec(TaggedValues.uniqueIdentifier, spec.UniqueIdentifier),
                       new TaggedValueSpec(TaggedValues.usageRule, spec.UsageRules),
                       new TaggedValueSpec(TaggedValues.versionIdentifier, spec.VersionIdentifier),
                   };
        }

        private static IEnumerable<TaggedValueSpec> GetAsbieTaggedValueSpecs(ASBIESpec spec)
        {
            return new List<TaggedValueSpec>
                   {
                       new TaggedValueSpec(TaggedValues.businessTerm, spec.BusinessTerms),
                       new TaggedValueSpec(TaggedValues.definition, spec.Definition),
                       new TaggedValueSpec(TaggedValues.dictionaryEntryName, spec.DictionaryEntryName),
                       new TaggedValueSpec(TaggedValues.languageCode, spec.LanguageCode),
                       new TaggedValueSpec(TaggedValues.sequencingKey, spec.SequencingKey),
                       new TaggedValueSpec(TaggedValues.uniqueIdentifier, spec.UniqueIdentifier),
                       new TaggedValueSpec(TaggedValues.usageRule, spec.UsageRules),
                       new TaggedValueSpec(TaggedValues.versionIdentifier, spec.VersionIdentifier),
                   };
        }

        protected override IEnumerable<AttributeSpec> GetAttributeSpecs(ABIESpec spec)
        {
            var bbieSpecs = spec.BBIEs;
            if (bbieSpecs != null)
            {
                foreach (BBIESpec bbieSpec in bbieSpecs)
                {
                    yield return new AttributeSpec(Stereotype.BBIE, bbieSpec.Name, bbieSpec.Type.Name, bbieSpec.Type.Id, bbieSpec.LowerBound, bbieSpec.UpperBound, GetBbieTaggedValueSpecs(bbieSpec));
                }
            }
        }

        protected override IEnumerable<ConnectorSpec> GetConnectorSpecs(ABIESpec spec)
        {
            if (spec.IsEquivalentTo != null) yield return ConnectorSpec.CreateDependency(Stereotype.IsEquivalentTo, spec.IsEquivalentTo.Id, "1", "1");
            if (spec.BasedOn != null) yield return ConnectorSpec.CreateDependency(Stereotype.BasedOn, spec.BasedOn.Id, "1", "1");

            var asbieSpecs = spec.ASBIEs;
            if (asbieSpecs != null)
            {
                foreach (ASBIESpec asbieSpec in asbieSpecs)
                {
                    yield return ConnectorSpec.CreateAggregation(asbieSpec.AggregationKind, Stereotype.ASBIE, asbieSpec.Name, asbieSpec.AssociatedABIEId, asbieSpec.LowerBound, asbieSpec.UpperBound, GetAsbieTaggedValueSpecs(asbieSpec));
                }
            }
        }
    }
}