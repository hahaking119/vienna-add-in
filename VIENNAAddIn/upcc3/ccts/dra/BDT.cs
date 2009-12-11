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
using CctsRepository.BdtLibrary;
using CctsRepository.CdtLibrary;
using EA;
using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddInUtils;
using Attribute=EA.Attribute;
using Stereotype=VIENNAAddIn.upcc3.ccts.util.Stereotype;
using VIENNAAddIn.upcc3.export.cctsndr;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    public class BDT : IBdt, IEquatable<BDT>
    {
        private readonly Element element;
        private readonly CCRepository repository;

        public BDT(CCRepository repository, Element element)
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

        #region IBdt Members

        public IBdt IsEquivalentTo
        {
            get
            {
                Connector connector = Connectors.FirstOrDefault(EAConnectorExtensions.IsIsEquivalentTo);
                return connector != null ? repository.GetBdtById(connector.SupplierID) : null;
            }
        }

        ///<summary>
        ///</summary>
        public ICdt BasedOn
        {
            get
            {
                Connector connector = Connectors.FirstOrDefault(EAConnectorExtensions.IsBasedOn);
                return connector != null ? repository.GetCdtById(connector.SupplierID) : null;
            }
        }

        public string DictionaryEntryName
        {
            get { return GetTaggedValue(TaggedValues.dictionaryEntryName).DefaultTo(Name + ". Type"); }
        }

        public IEnumerable<string> UsageRules
        {
            get { return element.GetTaggedValues(TaggedValues.usageRule); }
        }

        public IEnumerable<IBdtSup> Sups
        {
            get
            {
                return
                    Attributes.Where(EAAttributeExtensions.IsSUP).Convert(a => (IBdtSup) new BDTSupplementaryComponent(repository, a, this));
            }
        }

        public IBdtSup CreateBdtSup(BdtSupSpec specification)
        {
            throw new NotImplementedException();
        }

        public IBdtSup UpdateBdtSup(IBdtSup bdtSup, BdtSupSpec specification)
        {
            throw new NotImplementedException();
        }

        public void RemoveBdtSup(IBdtSup bdtSup)
        {
            throw new NotImplementedException();
        }

        public IBdtCon Con
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
        public IBdtLibrary BdtLibrary
        {
            get { return repository.GetBdtLibraryById(element.PackageID); }
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

        ///<summary>
        ///</summary>
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

        #region IEquatable<BDT> Members

        public bool Equals(BDT other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.element.ElementID, element.ElementID);
        }

        #endregion

        private static IEnumerable<TaggedValueSpec> GetTaggedValueSpecs(BdtSpec spec)
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

        private static IEnumerable<TaggedValueSpec> GetCONTaggedValueSpecs(BdtConSpec spec)
        {
            return new List<TaggedValueSpec>
                   {
                       new TaggedValueSpec(TaggedValues.businessTerm, spec.BusinessTerms),
                       new TaggedValueSpec(TaggedValues.definition, spec.Definition),
                       new TaggedValueSpec(TaggedValues.enumeration, spec.Enumeration),
                       new TaggedValueSpec(TaggedValues.dictionaryEntryName, spec.DictionaryEntryName),
                       new TaggedValueSpec(TaggedValues.languageCode, spec.LanguageCode),
                       new TaggedValueSpec(TaggedValues.uniqueIdentifier, spec.UniqueIdentifier),
                       new TaggedValueSpec(TaggedValues.usageRule, spec.UsageRules),
                       new TaggedValueSpec(TaggedValues.versionIdentifier, spec.VersionIdentifier),
                       new TaggedValueSpec(TaggedValues.modificationAllowedIndicator, spec.ModificationAllowedIndicator),
                       new TaggedValueSpec(TaggedValues.pattern, spec.Pattern),
                       new TaggedValueSpec(TaggedValues.fractionDigits, spec.FractionDigits),
                       new TaggedValueSpec(TaggedValues.maximumExclusive, spec.MaximumExclusive),
                       new TaggedValueSpec(TaggedValues.maximumInclusive, spec.MaximumInclusive),
                       new TaggedValueSpec(TaggedValues.maximumLength, spec.MaximumLength),
                       new TaggedValueSpec(TaggedValues.minimumExclusive, spec.MinimumExclusive),
                       new TaggedValueSpec(TaggedValues.minimumInclusive, spec.MinimumInclusive),
                       new TaggedValueSpec(TaggedValues.minimumLength, spec.MinimumLength),
                       new TaggedValueSpec(TaggedValues.totalDigits, spec.TotalDigits),
                   };
        }

        private static IEnumerable<TaggedValueSpec> GetSUPTaggedValueSpecs(BdtSupSpec spec)
        {
            return new List<TaggedValueSpec>
                   {
                       new TaggedValueSpec(TaggedValues.businessTerm, spec.BusinessTerms),
                       new TaggedValueSpec(TaggedValues.definition, spec.Definition),
                       new TaggedValueSpec(TaggedValues.dictionaryEntryName, spec.DictionaryEntryName),
                       new TaggedValueSpec(TaggedValues.enumeration, spec.Enumeration),
                       new TaggedValueSpec(TaggedValues.languageCode, spec.LanguageCode),
                       new TaggedValueSpec(TaggedValues.uniqueIdentifier, spec.UniqueIdentifier),
                       new TaggedValueSpec(TaggedValues.usageRule, spec.UsageRules),
                       new TaggedValueSpec(TaggedValues.versionIdentifier, spec.VersionIdentifier),
                       new TaggedValueSpec(TaggedValues.modificationAllowedIndicator, spec.ModificationAllowedIndicator),
                       new TaggedValueSpec(TaggedValues.pattern, spec.Pattern),
                       new TaggedValueSpec(TaggedValues.fractionDigits, spec.FractionDigits),
                       new TaggedValueSpec(TaggedValues.maximumExclusive, spec.MaximumExclusive),
                       new TaggedValueSpec(TaggedValues.maximumInclusive, spec.MaximumInclusive),
                       new TaggedValueSpec(TaggedValues.maximumLength, spec.MaximumLength),
                       new TaggedValueSpec(TaggedValues.minimumExclusive, spec.MinimumExclusive),
                       new TaggedValueSpec(TaggedValues.minimumInclusive, spec.MinimumInclusive),
                       new TaggedValueSpec(TaggedValues.minimumLength, spec.MinimumLength),
                       new TaggedValueSpec(TaggedValues.totalDigits, spec.TotalDigits),
                   };
        }

        private static IEnumerable<AttributeSpec> GetAttributeSpecs(BdtSpec spec)
        {
            BdtConSpec conSpec = spec.Con;
            if (conSpec != null)
            {
                // TODO throw exception if null
                yield return new AttributeSpec(Stereotype.CON, "Content", conSpec.BasicType.Name, conSpec.BasicType.Id, conSpec.LowerBound, conSpec.UpperBound, GetCONTaggedValueSpecs(conSpec), spec.Name + ". Content");
            }
            if (spec.Sups != null)
            {
                foreach (BdtSupSpec supSpec in spec.Sups)
                {
                    yield return new AttributeSpec(Stereotype.SUP, supSpec.Name, supSpec.BasicType.Name, supSpec.BasicType.Id, supSpec.LowerBound, supSpec.UpperBound, GetSUPTaggedValueSpecs(supSpec), spec.Name + ". " + supSpec.Name + ". " + supSpec.BasicType.Name);
                }
            }
        }

        private static IEnumerable<ConnectorSpec> GetConnectorSpecs(BdtSpec spec)
        {
            if (spec.IsEquivalentTo != null) yield return ConnectorSpec.CreateDependency(Stereotype.isEquivalentTo, spec.IsEquivalentTo.Id, "1", "1");
            if (spec.BasedOn != null) yield return ConnectorSpec.CreateDependency(Stereotype.basedOn, spec.BasedOn.Id, "1", "1");
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj is BDT) return Equals((BDT) obj);
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

        public void Update(BdtSpec spec)
        {
            element.Name = spec.Name;
            foreach (TaggedValueSpec taggedValueSpec in GetTaggedValueSpecs(spec))
            {
                switch (taggedValueSpec.Key)
                {
                    case TaggedValues.uniqueIdentifier:
                        element.SetOrGenerateTaggedValue(taggedValueSpec, element.ElementGUID);
                        break;
                    case TaggedValues.dictionaryEntryName:
                        element.SetOrGenerateTaggedValue(taggedValueSpec, element.Name + ". Type");
                        break;
                    default:
                        element.SetTaggedValue(taggedValueSpec.Key, taggedValueSpec.Value);
                        break;
                }
            }

            for (var i = (short) (element.Connectors.Count - 1); i >= 0; i--)
            {
                if (((Connector) element.Connectors.GetAt(i)).IsBasedOn())
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