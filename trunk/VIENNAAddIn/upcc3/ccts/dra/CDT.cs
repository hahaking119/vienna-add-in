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
using EA;
using CctsRepository;
using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddInUtils;
using Attribute=EA.Attribute;
using Stereotype=VIENNAAddIn.upcc3.ccts.util.Stereotype;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    public class CDT : UpccClass<CDTSpec> , ICDT
    {
        public CDT(CCRepository repository, Element element) : base(repository, element, Stereotype.CDT)
        {
        }

        #region ICDT Members

        public ICDT IsEquivalentTo
        {
            get
            {
                Connector connector = Connectors.FirstOrDefault(EAConnectorExtensions.IsIsEquivalentTo);
                return connector != null ? repository.GetCDT(connector.SupplierID) : null;
            }
        }

        public override string DictionaryEntryName
        {
            get
            {
                string value = base.DictionaryEntryName;
                if (string.IsNullOrEmpty(value))
                {
                    return Name + ". Type";
                }
                return value;
            }
        }

        public IEnumerable<string> UsageRules
        {
            get { return element.GetTaggedValues(TaggedValues.usageRule); }
        }

        public IEnumerable<ICDTSupplementaryComponent> SUPs
        {
            get
            {
                return
                    Attributes.Where(EAAttributeExtensions.IsSUP).Convert(a => (ICDTSupplementaryComponent) new CDTSupplementaryComponent(repository, a, this));
            }
        }

        public ICDTContentComponent CON
        {
            get
            {
                Attribute conAttribute = Attributes.FirstOrDefault(EAAttributeExtensions.IsCON);
                if (conAttribute == null)
                {
                    throw new Exception("data type contains no attribute with stereotype <<CON>>");
                }
                return new CDTContentComponent(repository, conAttribute, this);
            }
        }

        #endregion

        protected override bool DeleteConnectorOnUpdate(Connector connector)
        {
            return false;
        }

        protected override IEnumerable<TaggedValueSpec> GetTaggedValueSpecs(CDTSpec spec)
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

        private static IEnumerable<TaggedValueSpec> GetCONTaggedValueSpecs(CDTContentComponentSpec spec)
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
                       new TaggedValueSpec(TaggedValues.modificationAllowedIndicator, spec.ModificationAllowedIndicator),
                   };
        }

        private static IEnumerable<TaggedValueSpec> GetSUPTaggedValueSpecs(CDTSupplementaryComponentSpec spec)
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
                       new TaggedValueSpec(TaggedValues.modificationAllowedIndicator, spec.ModificationAllowedIndicator),
                   };
        }

        protected override IEnumerable<AttributeSpec> GetAttributeSpecs(CDTSpec spec)
        {
            var conSpec = spec.CON;
            if (conSpec != null)
            {
                // TODO throw exception if null
                yield return new AttributeSpec(Stereotype.CON, "Content", conSpec.BasicType.Name, conSpec.BasicType.Id, conSpec.LowerBound, conSpec.UpperBound, GetCONTaggedValueSpecs(conSpec));
            }
            var supSpecs = spec.SUPs;
            if (supSpecs != null)
            {
                foreach (var supSpec in supSpecs)
                {
                    yield return new AttributeSpec(Stereotype.SUP, supSpec.Name, supSpec.BasicType.Name, supSpec.BasicType.Id, supSpec.LowerBound, supSpec.UpperBound, GetSUPTaggedValueSpecs(supSpec));
                }
            }
        }

        protected override IEnumerable<ConnectorSpec> GetConnectorSpecs(CDTSpec spec)
        {
            if (spec.IsEquivalentTo != null) yield return ConnectorSpec.CreateDependency(Stereotype.IsEquivalentTo, spec.IsEquivalentTo.Id, "1", "1");
        }
    }
}