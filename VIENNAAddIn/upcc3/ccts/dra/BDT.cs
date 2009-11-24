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
using CctsRepository.bdt;
using CctsRepository.cdt;
using EA;
using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddInUtils;
using Attribute=EA.Attribute;
using Stereotype=VIENNAAddIn.upcc3.ccts.util.Stereotype;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    ///<summary>
    ///</summary>
    public class BDT : UpccClass<BDTSpec>, IBDT
    {
        ///<summary>
        ///</summary>
        ///<param name="repository"></param>
        ///<param name="element"></param>
        public BDT(CCRepository repository, Element element) : base(repository, element, "BDT")
        {
        }

        #region IBDT Members

        public IBDT IsEquivalentTo
        {
            get
            {
                Connector connector = Connectors.FirstOrDefault(EAConnectorExtensions.IsIsEquivalentTo);
                return connector != null ? repository.GetBDT(connector.SupplierID) : null;
            }
        }

        ///<summary>
        ///</summary>
        public ICDT BasedOn
        {
            get
            {
                Connector connector = Connectors.FirstOrDefault(EAConnectorExtensions.IsBasedOn);
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

        public IEnumerable<IBDTSupplementaryComponent> SUPs
        {
            get
            {
                return
                    Attributes.Where(EAAttributeExtensions.IsSUP).Convert(a => (IBDTSupplementaryComponent) new BDTSupplementaryComponent(repository, a, this));
            }
        }

        public IBDTContentComponent CON
        {
            get
            {
                Attribute conAttribute = Attributes.FirstOrDefault(EAAttributeExtensions.IsCON);
                if (conAttribute == null)
                {
                    throw new Exception("data type contains no attribute with stereotype <<CON>>");
                }
                return new BDTContentComponent(repository, conAttribute, this);
            }
        }

        #endregion

        protected override bool DeleteConnectorOnUpdate(Connector connector)
        {
            return connector.IsBasedOn();
        }

        protected override IEnumerable<TaggedValueSpec> GetTaggedValueSpecs(BDTSpec spec)
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

        private static IEnumerable<TaggedValueSpec> GetCONTaggedValueSpecs(BDTContentComponentSpec spec)
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
                       new TaggedValueSpec(TaggedValues.pattern, spec.Pattern),
                       new TaggedValueSpec(TaggedValues.fractionDigits, spec.FractionDigits),
                       new TaggedValueSpec(TaggedValues.length, spec.Length),
                       new TaggedValueSpec(TaggedValues.maxExclusive, spec.MaxExclusive),
                       new TaggedValueSpec(TaggedValues.maxInclusive, spec.MaxInclusive),
                       new TaggedValueSpec(TaggedValues.maxLength, spec.MaxLength),
                       new TaggedValueSpec(TaggedValues.minExclusive, spec.MinExclusive),
                       new TaggedValueSpec(TaggedValues.minInclusive, spec.MinInclusive),
                       new TaggedValueSpec(TaggedValues.minLength, spec.MinLength),
                       new TaggedValueSpec(TaggedValues.totalDigits, spec.TotalDigits),
                       new TaggedValueSpec(TaggedValues.whiteSpace, spec.WhiteSpace),
                   };
        }

        private static IEnumerable<TaggedValueSpec> GetSUPTaggedValueSpecs(BDTSupplementaryComponentSpec spec)
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
                       new TaggedValueSpec(TaggedValues.pattern, spec.Pattern),
                       new TaggedValueSpec(TaggedValues.fractionDigits, spec.FractionDigits),
                       new TaggedValueSpec(TaggedValues.length, spec.Length),
                       new TaggedValueSpec(TaggedValues.maxExclusive, spec.MaxExclusive),
                       new TaggedValueSpec(TaggedValues.maxInclusive, spec.MaxInclusive),
                       new TaggedValueSpec(TaggedValues.maxLength, spec.MaxLength),
                       new TaggedValueSpec(TaggedValues.minExclusive, spec.MinExclusive),
                       new TaggedValueSpec(TaggedValues.minInclusive, spec.MinInclusive),
                       new TaggedValueSpec(TaggedValues.minLength, spec.MinLength),
                       new TaggedValueSpec(TaggedValues.totalDigits, spec.TotalDigits),
                       new TaggedValueSpec(TaggedValues.whiteSpace, spec.WhiteSpace),
                   };
        }

        protected override IEnumerable<AttributeSpec> GetAttributeSpecs(BDTSpec spec)
        {
            BDTContentComponentSpec conSpec = spec.CON;
            if (conSpec != null)
            {
                // TODO throw exception if null
                yield return new AttributeSpec(Stereotype.CON, "Content", conSpec.BasicType.Name, conSpec.BasicType.Id, conSpec.LowerBound, conSpec.UpperBound, GetCONTaggedValueSpecs(conSpec));
            }
            List<BDTSupplementaryComponentSpec> supSpecs = spec.SUPs;
            if (supSpecs != null)
            {
                foreach (BDTSupplementaryComponentSpec supSpec in supSpecs)
                {
                    yield return new AttributeSpec(Stereotype.SUP, supSpec.Name, supSpec.BasicType.Name, supSpec.BasicType.Id, supSpec.LowerBound, supSpec.UpperBound, GetSUPTaggedValueSpecs(supSpec));
                }
            }
        }

        protected override IEnumerable<ConnectorSpec> GetConnectorSpecs(BDTSpec spec)
        {
            if (spec.IsEquivalentTo != null) yield return ConnectorSpec.CreateDependency(Stereotype.IsEquivalentTo, spec.IsEquivalentTo.Id, "1", "1");
            if (spec.BasedOn != null) yield return ConnectorSpec.CreateDependency(Stereotype.BasedOn, spec.BasedOn.Id, "1", "1");
        }
    }
}