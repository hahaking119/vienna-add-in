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
using VIENNAAddIn.upcc3.export.cctsndr;
using Attribute=EA.Attribute;
using Stereotype=VIENNAAddIn.upcc3.ccts.util.Stereotype;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    internal class ENUM : UpccClass<ENUMSpec>, IENUM
    {
        public ENUM(CCRepository repository, Element element) : base(repository, element, Stereotype.ENUM)
        {
        }

        #region IENUM Members

        public override string DictionaryEntryName
        {
            get
            {
                string value = base.DictionaryEntryName;
                if (string.IsNullOrEmpty(value))
                {
                    value = Name;
                }
                return value;
            }
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
                return connector != null ? repository.GetENUM(connector.SupplierID) : null;
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

        #endregion

        protected override bool DeleteConnectorOnUpdate(Connector connector)
        {
            return connector.IsIsEquivalentTo();
        }

        protected override IEnumerable<TaggedValueSpec> GetTaggedValueSpecs(ENUMSpec spec)
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

        protected override IEnumerable<AttributeSpec> GetAttributeSpecs(ENUMSpec spec)
        {
            var codelistEntrySpecs = spec.CodelistEntries;
            if (codelistEntrySpecs != null)
            {
                foreach (var codelistEntrySpec in codelistEntrySpecs)
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

        protected override IEnumerable<ConnectorSpec> GetConnectorSpecs(ENUMSpec spec)
        {
            if (spec.IsEquivalentTo != null) yield return ConnectorSpec.CreateDependency(Stereotype.IsEquivalentTo, spec.IsEquivalentTo.Id, "1", "1");
        }
    }
}