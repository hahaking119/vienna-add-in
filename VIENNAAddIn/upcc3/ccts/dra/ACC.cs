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
using CctsRepository.cc;
using EA;
using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddInUtils;
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
            return connector.IsASCC() && connector.GetAssociatingEnd(element.ElementID).Aggregation != (int) AggregationKind.None;
        }

        protected override IEnumerable<TaggedValueSpec> GetTaggedValueSpecs(ACCSpec spec)
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

        private static IEnumerable<TaggedValueSpec> GetBccTaggedValueSpecs(BCCSpec spec)
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

        private static IEnumerable<TaggedValueSpec> GetAsccTaggedValueSpecs(ASCCSpec spec)
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

        protected override IEnumerable<AttributeSpec> GetAttributeSpecs(ACCSpec spec)
        {
            var bccSpecs = spec.BCCs;
            if (bccSpecs != null)
            {
                var duplicateBccNames = GetDuplicates(bccSpecs.Select(bccSpec => bccSpec.Name));
                foreach (BCCSpec bccSpec in bccSpecs)
                {
                    var name = bccSpec.Name;
                    if (duplicateBccNames.Contains(name))
                    {
                        name = name + bccSpec.Type.Name;
                    }
                    yield return new AttributeSpec(Stereotype.BCC, name, bccSpec.Type.Name, bccSpec.Type.Id, bccSpec.LowerBound, bccSpec.UpperBound, GetBccTaggedValueSpecs(bccSpec));
                }
            }
        }

        private static HashSet<string> GetDuplicates(IEnumerable<string> strings)
        {
            var orderedStrings = new List<string>(strings.OrderBy(s => s));
            var duplicates = new HashSet<string>();
            for (int i = 0; i < orderedStrings.Count - 1; ++i)
            {
                if (orderedStrings[i] == orderedStrings[i + 1])
                {
                    duplicates.Add(orderedStrings[i]);
                }
            }
            return duplicates;
        }

        protected override IEnumerable<ConnectorSpec> GetConnectorSpecs(ACCSpec spec)
        {
            if (spec.IsEquivalentTo != null) yield return ConnectorSpec.CreateDependency(Stereotype.IsEquivalentTo, spec.IsEquivalentTo.Id, "1", "1");

            var asccSpecs = spec.ASCCs;
            if (asccSpecs != null)
            {
                var duplicateAsccNames = GetDuplicates(asccSpecs.Select(asccSpec => asccSpec.Name));
                foreach (ASCCSpec asccSpec in asccSpecs)
                {
                    var associatedACC = asccSpec.AssociatedACC;
                    if (associatedACC == null)
                    {
                        // TODO throw meaningful exception instead
                        continue;
                    }
                    var name = asccSpec.Name;
                    if (duplicateAsccNames.Contains(name))
                    {
                        name = name + associatedACC.Name;
                    }
                    yield return
                        ConnectorSpec.CreateAggregation(AggregationKind.Shared, Stereotype.ASCC, name,
                                                        asccSpec.AssociatedACC.Id, asccSpec.LowerBound, asccSpec.UpperBound, GetAsccTaggedValueSpecs(asccSpec));
                }
            }
        }
    }
}