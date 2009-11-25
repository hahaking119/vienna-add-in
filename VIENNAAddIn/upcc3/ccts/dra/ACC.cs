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
using CctsRepository.CcLibrary;
using EA;
using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddIn.upcc3.export.cctsndr;
using VIENNAAddInUtils;
using Attribute=EA.Attribute;
using Stereotype=VIENNAAddIn.upcc3.ccts.util.Stereotype;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    ///<summary>
    ///</summary>
    public class ACC : IACC, IEquatable<ACC>
    {
        private readonly Element element;
        private readonly CCRepository repository;

        ///<summary>
        ///</summary>
        ///<param name="repository"></param>
        ///<param name="element"></param>
        public ACC(CCRepository repository, Element element)
        {
            this.repository = repository;
            this.element = element;
        }

        private IEnumerable<Attribute> Attributes
        {
            get { return element.Attributes.AsEnumerable<Attribute>(); }
        }

        private IEnumerable<Connector> Connectors
        {
            get { return element.Connectors.AsEnumerable<Connector>(); }
        }

        #region IACC Members

        public string DictionaryEntryName
        {
            get { return GetTaggedValue(TaggedValues.dictionaryEntryName).DefaultTo(Name + ". Details"); }
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

        ///<summary>
        ///</summary>
        public int Id
        {
            get { return element.ElementID; }
        }

        ///<summary>
        ///</summary>
        public string Name
        {
            get { return element.Name; }
        }

        ///<summary>
        ///</summary>
        public IBusinessLibrary Library
        {
            get { return repository.GetLibrary(element.PackageID); }
        }

        ///<summary>
        ///</summary>
        public IEnumerable<string> BusinessTerms
        {
            get { return element.GetTaggedValues(TaggedValues.businessTerm); }
        }

        ///<summary>
        ///</summary>
        public string Definition
        {
            get { return GetTaggedValue(TaggedValues.definition); }
        }

        public string UniqueIdentifier
        {
            get { return GetTaggedValue(TaggedValues.uniqueIdentifier); }
        }

        ///<summary>
        ///</summary>
        public string VersionIdentifier
        {
            get { return GetTaggedValue(TaggedValues.versionIdentifier); }
        }

        ///<summary>
        ///</summary>
        public string LanguageCode
        {
            get { return GetTaggedValue(TaggedValues.languageCode); }
        }

        #endregion

        #region IEquatable<ACC> Members

        public bool Equals(ACC other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.element.ElementID, element.ElementID);
        }

        #endregion

        private bool DeleteConnectorOnUpdate(Connector connector)
        {
            return connector.IsIsEquivalentTo() || IsASCC(connector);
        }

        private bool IsASCC(Connector connector)
        {
            return connector.IsASCC() && connector.GetAssociatingEnd(element.ElementID).Aggregation != (int) EaAggregationKind.None;
        }

        private static IEnumerable<TaggedValueSpec> GetTaggedValueSpecs(ACCSpec spec)
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

        private static IEnumerable<AttributeSpec> GetAttributeSpecs(ACCSpec spec)
        {
            IEnumerable<BCCSpec> bccSpecs = spec.BCCs;
            if (bccSpecs != null)
            {
                HashSet<string> duplicateBccNames = GetDuplicates(bccSpecs.Select(bccSpec => bccSpec.Name));
                foreach (BCCSpec bccSpec in bccSpecs)
                {
                    string name = bccSpec.Name;
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

        private static IEnumerable<ConnectorSpec> GetConnectorSpecs(ACCSpec spec)
        {
            if (spec.IsEquivalentTo != null) yield return ConnectorSpec.CreateDependency(Stereotype.IsEquivalentTo, spec.IsEquivalentTo.Id, "1", "1");

            IEnumerable<ASCCSpec> asccSpecs = spec.ASCCs;
            if (asccSpecs != null)
            {
                HashSet<string> duplicateAsccNames = GetDuplicates(asccSpecs.Select(asccSpec => asccSpec.Name));
                foreach (ASCCSpec asccSpec in asccSpecs)
                {
                    IACC associatedACC = asccSpec.AssociatedACC;
                    if (associatedACC == null)
                    {
                        // TODO throw meaningful exception instead
                        continue;
                    }
                    string name = asccSpec.Name;
                    if (duplicateAsccNames.Contains(name))
                    {
                        name = name + associatedACC.Name;
                    }
                    yield return
                        ConnectorSpec.CreateAggregation(EaAggregationKind.Shared, Stereotype.ASCC, name,
                                                        asccSpec.AssociatedACC.Id, asccSpec.LowerBound, asccSpec.UpperBound, GetAsccTaggedValueSpecs(asccSpec));
                }
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj is ACC) return Equals((ACC) obj);
            return false;
        }

        public override int GetHashCode()
        {
            return (element != null ? element.GetHashCode() : 0);
        }

        private string GetTaggedValue(TaggedValues key)
        {
            return element.GetTaggedValue(key) ?? string.Empty;
        }

        public void Update(ACCSpec spec)
        {
            element.Name = spec.Name;
            foreach (TaggedValueSpec taggedValueSpec in GetTaggedValueSpecs(spec))
            {
                element.SetTaggedValue(taggedValueSpec.Key, taggedValueSpec.Value);
            }

            for (var i = (short) (element.Connectors.Count - 1); i >= 0; i--)
            {
                if (DeleteConnectorOnUpdate((Connector) element.Connectors.GetAt(i)))
                {
                    element.Connectors.Delete(i);
                }
            }
            element.Connectors.Refresh();
            foreach (ConnectorSpec connector in GetConnectorSpecs(spec))
            {
                element.AddConnector(connector);
            }
            element.Connectors.Refresh();

            for (var i = (short) (element.Attributes.Count - 1); i >= 0; i--)
            {
                element.Attributes.Delete(i);
            }
            element.Attributes.Refresh();
            foreach (AttributeSpec attribute in GetAttributeSpecs(spec))
            {
                element.AddAttribute(attribute);
            }
            element.Attributes.Refresh();

            element.Update();
            element.Refresh();
        }
    }
}