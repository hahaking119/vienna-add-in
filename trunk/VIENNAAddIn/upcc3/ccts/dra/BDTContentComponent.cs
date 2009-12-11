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
using CctsRepository;
using CctsRepository.BdtLibrary;
using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddIn.upcc3.export.cctsndr;
using VIENNAAddInUtils;
using Attribute=EA.Attribute;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    internal class BDTContentComponent : IBdtCon
    {
        private readonly Attribute attribute;
        private readonly IBdt bdt;
        private readonly CCRepository repository;

        public BDTContentComponent(CCRepository repository, Attribute attribute, IBdt bdt)
        {
            this.repository = repository;
            this.attribute = attribute;
            this.bdt = bdt;
        }

        #region IBdtCon Members

        public string DictionaryEntryName
        {
            get
            {
                string value = GetTaggedValue(TaggedValues.dictionaryEntryName);
                if (string.IsNullOrEmpty(value))
                {
                    value = Bdt.Name + ". Content";
                }
                return value;
            }
        }

        public string Enumeration
        {
            get { return GetTaggedValue(TaggedValues.enumeration); }
        }

        public IBdt Bdt
        {
            get { return bdt; }
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

        public string Pattern
        {
            get { return GetTaggedValue(TaggedValues.pattern); }
        }

        public string FractionDigits
        {
            get { return GetTaggedValue(TaggedValues.fractionDigits); }
        }

        public string MaximumExclusive
        {
            get { return GetTaggedValue(TaggedValues.maximumExclusive); }
        }

        public string MaximumInclusive
        {
            get { return GetTaggedValue(TaggedValues.maximumInclusive); }
        }

        public string MaximumLength
        {
            get { return GetTaggedValue(TaggedValues.maximumLength); }
        }

        public string MinimumExclusive
        {
            get { return GetTaggedValue(TaggedValues.minimumExclusive); }
        }

        public string MinimumInclusive
        {
            get { return GetTaggedValue(TaggedValues.minimumInclusive); }
        }

        public string MinimumLength
        {
            get { return GetTaggedValue(TaggedValues.minimumLength); }
        }

        public string TotalDigits
        {
            get { return GetTaggedValue(TaggedValues.totalDigits); }
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

        protected string GetTaggedValue(TaggedValues key)
        {
            return attribute.GetTaggedValue(key) ?? string.Empty;
        }
    }
}