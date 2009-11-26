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
using CctsRepository.EnumLibrary;
using EA;
using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddIn.upcc3.export.cctsndr;
using Attribute=EA.Attribute;
using Stereotype=VIENNAAddIn.upcc3.ccts.util.Stereotype;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    internal class ENUM : IENUM, IEquatable<ENUM>
    {
        private readonly Element element;
        private readonly CCRepository repository;

        public ENUM(CCRepository repository, Element element)
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

        #region IENUM Members

        public string DictionaryEntryName
        {
            get { return GetTaggedValue(TaggedValues.dictionaryEntryName).DefaultTo(Name); }
        }

        public string CodeListAgencyIdentifier
        {
            get { return GetTaggedValue(TaggedValues.codeListAgencyIdentifier); }
        }

        public string CodeListAgencyName
        {
            get { return GetTaggedValue(TaggedValues.codeListAgencyName); }
        }

        public string CodeListIdentifier
        {
            get { return GetTaggedValue(TaggedValues.codeListIdentifier); }
        }

        public string CodeListName
        {
            get { return GetTaggedValue(TaggedValues.codeListName); }
        }

        public string EnumerationURI
        {
            get { return GetTaggedValue(TaggedValues.enumerationURI); }
        }

        public bool ModificationAllowedIndicator
        {
            get { return "true" == GetTaggedValue(TaggedValues.modificationAllowedIndicator).DefaultTo("true").ToLower(); }
        }

        public string RestrictedPrimitive
        {
            get { return GetTaggedValue(TaggedValues.restrictedPrimitive); }
        }

        public string Status
        {
            get { return GetTaggedValue(TaggedValues.status); }
        }

        public IENUM IsEquivalentTo
        {
            get
            {
                Connector connector = Connectors.FirstOrDefault(EAConnectorExtensions.IsIsEquivalentTo);
                return connector != null ? repository.GetEnumById(connector.SupplierID) : null;
            }
        }

        public IEnumerable<ICodelistEntry> CodelistEntries
        {
            get
            {
                var values = new List<ICodelistEntry>();
                foreach (Attribute attribute in Attributes)
                {
                    if (attribute.Stereotype == Stereotype.CodelistEntry)
                    {
                        values.Add(new CodelistEntry(attribute));
                    }
                }
                return values;
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
        public IENUMLibrary Library
        {
            get { return repository.GetEnumLibraryById(element.PackageID); }
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

        #region IEquatable<ENUM> Members

        public bool Equals(ENUM other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.element.ElementID, element.ElementID);
        }

        #endregion

        private static IEnumerable<TaggedValueSpec> GetTaggedValueSpecs(ENUMSpec spec)
        {
            return new List<TaggedValueSpec>
                   {
                       new TaggedValueSpec(TaggedValues.dictionaryEntryName, spec.DictionaryEntryName),
                       new TaggedValueSpec(TaggedValues.definition, spec.Definition),
                       new TaggedValueSpec(TaggedValues.uniqueIdentifier, spec.UniqueIdentifier),
                       new TaggedValueSpec(TaggedValues.versionIdentifier, spec.VersionIdentifier),
                       new TaggedValueSpec(TaggedValues.languageCode, spec.LanguageCode),
                       new TaggedValueSpec(TaggedValues.businessTerm, spec.BusinessTerms),
                       new TaggedValueSpec(TaggedValues.codeListAgencyIdentifier, spec.CodeListAgencyIdentifier),
                       new TaggedValueSpec(TaggedValues.codeListAgencyName, spec.CodeListAgencyName),
                       new TaggedValueSpec(TaggedValues.codeListIdentifier, spec.CodeListIdentifier),
                       new TaggedValueSpec(TaggedValues.codeListName, spec.CodeListName),
                       new TaggedValueSpec(TaggedValues.enumerationURI, spec.EnumerationURI),
                       new TaggedValueSpec(TaggedValues.modificationAllowedIndicator, spec.ModificationAllowedIndicator),
                       new TaggedValueSpec(TaggedValues.restrictedPrimitive, spec.RestrictedPrimitive),
                       new TaggedValueSpec(TaggedValues.status, spec.Status),
                   };
        }

        private static IEnumerable<AttributeSpec> GetAttributeSpecs(ENUMSpec spec)
        {
            IEnumerable<CodelistEntrySpec> codelistEntrySpecs = spec.CodelistEntries;
            if (codelistEntrySpecs != null)
            {
                foreach (CodelistEntrySpec codelistEntrySpec in codelistEntrySpecs)
                {
                    yield return new AttributeSpec(Stereotype.CodelistEntry, codelistEntrySpec.Name, "string", 0, "1", "1", GetCodelistEntryTaggedValueSpecs(codelistEntrySpec));
                }
            }
        }

        private static IEnumerable<TaggedValueSpec> GetCodelistEntryTaggedValueSpecs(CodelistEntrySpec spec)
        {
            return new List<TaggedValueSpec>
                   {
                       new TaggedValueSpec(TaggedValues.codeName, spec.CodeName),
                       new TaggedValueSpec(TaggedValues.status, spec.Status),
                   };
        }

        private static IEnumerable<ConnectorSpec> GetConnectorSpecs(ENUMSpec spec)
        {
            if (spec.IsEquivalentTo != null) yield return ConnectorSpec.CreateDependency(Stereotype.IsEquivalentTo, spec.IsEquivalentTo.Id, "1", "1");
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj is ENUM) return Equals((ENUM) obj);
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

        public void Update(ENUMSpec spec)
        {
            element.Name = spec.Name;
            foreach (TaggedValueSpec taggedValueSpec in GetTaggedValueSpecs(spec))
            {
                element.SetTaggedValue(taggedValueSpec.Key, taggedValueSpec.Value);
            }

            for (var i = (short) (element.Connectors.Count - 1); i >= 0; i--)
            {
                if (((Connector) element.Connectors.GetAt(i)).IsIsEquivalentTo())
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