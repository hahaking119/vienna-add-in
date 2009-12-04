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
using CctsRepository.CdtLibrary;
using EA;
using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddIn.upcc3.export.cctsndr;
using VIENNAAddInUtils;
using Attribute=EA.Attribute;
using Stereotype=VIENNAAddIn.upcc3.ccts.util.Stereotype;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    public class CDT : ICdt, IEquatable<CDT>
    {
        private readonly Element element;
        private readonly CCRepository repository;

        public CDT(CCRepository repository, Element element)
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

        #region ICdt Members

        public ICdt IsEquivalentTo
        {
            get
            {
                Connector connector = Connectors.FirstOrDefault(EAConnectorExtensions.IsIsEquivalentTo);
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

        public IEnumerable<ICdtSup> Sups
        {
            get
            {
                return
                    Attributes.Where(EAAttributeExtensions.IsSUP).Convert(a => (ICdtSup) new CDTSupplementaryComponent(repository, a, this));
            }
        }

        public ICdtCon Con
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
        public ICdtLibrary CdtLibrary
        {
            get { return repository.GetCdtLibraryById(element.PackageID); }
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

        #region IEquatable<CDT> Members

        public bool Equals(CDT other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.element.ElementID, element.ElementID);
        }

        #endregion

        private static IEnumerable<TaggedValueSpec> GetTaggedValueSpecs(CdtSpec spec)
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

        private static IEnumerable<TaggedValueSpec> GetCONTaggedValueSpecs(CdtConSpec spec)
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

        private static IEnumerable<TaggedValueSpec> GetSUPTaggedValueSpecs(CdtSupSpec spec)
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

        private static IEnumerable<AttributeSpec> GetAttributeSpecs(CdtSpec spec)
        {
            CdtConSpec conSpec = spec.CON;
            if (conSpec != null)
            {
                // TODO throw exception if null
                yield return new AttributeSpec(Stereotype.CON, "Content", conSpec.BasicType.Name, conSpec.BasicType.Id, conSpec.LowerBound, conSpec.UpperBound, GetCONTaggedValueSpecs(conSpec));
            }
            List<CdtSupSpec> supSpecs = spec.SUPs;
            if (supSpecs != null)
            {
                foreach (CdtSupSpec supSpec in supSpecs)
                {
                    yield return new AttributeSpec(Stereotype.SUP, supSpec.Name, supSpec.BasicType.Name, supSpec.BasicType.Id, supSpec.LowerBound, supSpec.UpperBound, GetSUPTaggedValueSpecs(supSpec));
                }
            }
        }

        private static IEnumerable<ConnectorSpec> GetConnectorSpecs(CdtSpec spec)
        {
            if (spec.IsEquivalentTo != null) yield return ConnectorSpec.CreateDependency(Stereotype.isEquivalentTo, spec.IsEquivalentTo.Id, "1", "1");
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj is CDT) return Equals((CDT) obj);
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

        public void Update(CdtSpec spec)
        {
            element.Name = spec.Name;
            foreach (TaggedValueSpec taggedValueSpec in GetTaggedValueSpecs(spec))
            {
                element.SetTaggedValue(taggedValueSpec.Key, taggedValueSpec.Value);
            }

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