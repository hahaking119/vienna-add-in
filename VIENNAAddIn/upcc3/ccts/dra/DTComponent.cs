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
using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddIn.upcc3.export.cctsndr;
using Attribute=EA.Attribute;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    internal abstract class DTComponent : IDTComponent
    {
        private readonly Attribute attribute;
        private readonly IDT dt;
        private readonly CCRepository repository;

        protected DTComponent(CCRepository repository, Attribute attribute, IDT dt)
        {
            this.repository = repository;
            this.attribute = attribute;
            this.dt = dt;
        }

        #region IDTComponent Members

        public int Id
        {
            get { return attribute.AttributeID; }
        }

        public string GUID
        {
            get { return attribute.AttributeGUID; }
        }

        public string Name
        {
            get { return attribute.Name; }
        }

        public IBasicType BasicType
        {
            get { return repository.GetIType(attribute.ClassifierID); }
        }

        public string Definition
        {
            get { return GetTaggedValue(TaggedValues.definition); }
        }

        public virtual string DictionaryEntryName
        {
            get { return GetTaggedValue(TaggedValues.dictionaryEntryName); }
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

        public string Length
        {
            get { return GetTaggedValue(TaggedValues.length); }
        }

        public string MaxExclusive
        {
            get { return GetTaggedValue(TaggedValues.maxExclusive); }
        }

        public string MaxInclusive
        {
            get { return GetTaggedValue(TaggedValues.maxInclusive); }
        }

        public string MaxLength
        {
            get { return GetTaggedValue(TaggedValues.maxLength); }
        }

        public string MinExclusive
        {
            get { return GetTaggedValue(TaggedValues.minExclusive); }
        }

        public string MinInclusive
        {
            get { return GetTaggedValue(TaggedValues.minInclusive); }
        }

        public string MinLength
        {
            get { return GetTaggedValue(TaggedValues.minLength); }
        }

        public string TotalDigits
        {
            get { return GetTaggedValue(TaggedValues.totalDigits); }
        }

        public string WhiteSpace
        {
            get { return GetTaggedValue(TaggedValues.whiteSpace); }
        }

        public IBusinessLibrary Library
        {
            get { return DT.Library; }
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

        public IDT DT
        {
            get { return dt; }
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