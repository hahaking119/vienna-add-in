using System;
using System.Collections.Generic;
using CctsRepository;
using CctsRepository.CdtLibrary;
using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddIn.upcc3.export.cctsndr;
using Attribute=EA.Attribute;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    internal class CDTContentComponent : ICdtCon
    {
        private readonly Attribute attribute;
        private readonly ICdt cdt;
        private readonly CCRepository repository;

        public CDTContentComponent(CCRepository repository, Attribute attribute, ICdt cdt)
        {
            this.repository = repository;
            this.attribute = attribute;
            this.cdt = cdt;
        }

        #region ICdtCon Members

        public string DictionaryEntryName
        {
            get
            {
                string value = GetTaggedValue(TaggedValues.dictionaryEntryName);
                if (string.IsNullOrEmpty(value))
                {
                    value = Cdt.Name + ". Content";
                }
                return value;
            }
        }

        public ICdt Cdt
        {
            get { return cdt; }
        }

        public int Id
        {
            get { return attribute.AttributeID; }
        }

        public string Name
        {
            get { return attribute.Name; }
        }

        public BasicType BasicType
        {
            get { return repository.GetBasicTypeById(attribute.ClassifierID); }
        }

        public string Definition
        {
            get { return GetTaggedValue(TaggedValues.definition); }
        }

        public string LanguageCode
        {
            get { return GetTaggedValue(TaggedValues.languageCode); }
        }

        public string UniqueIdentifier
        {
            get { return GetTaggedValue(TaggedValues.uniqueIdentifier); }
        }

        public string VersionIdentifier
        {
            get { return GetTaggedValue(TaggedValues.versionIdentifier); }
        }

        public bool ModificationAllowedIndicator
        {
            get { return "true" == GetTaggedValue(TaggedValues.modificationAllowedIndicator).DefaultTo("true").ToLower(); }
        }

        public IEnumerable<string> BusinessTerms
        {
            get { return attribute.GetTaggedValues(TaggedValues.businessTerm); }
        }

        public IEnumerable<string> UsageRules
        {
            get { return attribute.GetTaggedValues(TaggedValues.usageRule); }
        }

        public string UpperBound
        {
            get { return attribute.UpperBound; }
        }

        public string LowerBound
        {
            get { return attribute.LowerBound; }
        }

        public bool IsOptional()
        {
            int i;
            return Int32.TryParse(LowerBound, out i) && i == 0;
        }

        #endregion

        private string GetTaggedValue(TaggedValues key)
        {
            return attribute.GetTaggedValue(key) ?? string.Empty;
        }
    }
}