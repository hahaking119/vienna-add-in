using System;
using System.Collections.Generic;
using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddIn.upcc3.XSDGenerator.Generator;
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
            get { return attribute.GetTaggedValue(TaggedValues.Definition); }
        }

        public string DictionaryEntryName
        {
            get { return attribute.GetTaggedValue(TaggedValues.DictionaryEntryName); }
        }

        public string LanguageCode
        {
            get { return attribute.GetTaggedValue(TaggedValues.LanguageCode); }
        }

        public string UniqueIdentifier
        {
            get { return attribute.GetTaggedValue(TaggedValues.UniqueIdentifier); }
        }

        public string VersionIdentifier
        {
            get { return attribute.GetTaggedValue(TaggedValues.VersionIdentifier); }
        }

        public bool ModificationAllowedIndicator
        {
            get
            {
                string value = attribute.GetTaggedValue(TaggedValues.ModificationAllowedIndicator).DefaultTo("true");
                return "true" == value.ToLower();
            }
        }

        public IEnumerable<string> BusinessTerms
        {
            get { return attribute.GetTaggedValues(TaggedValues.BusinessTerm); }
        }

        public IBusinessLibrary Library
        {
            get { return DT.Library; }
        }

        public IEnumerable<string> UsageRules
        {
            get { return attribute.GetTaggedValues(TaggedValues.UsageRule); }
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
    }
}