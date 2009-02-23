using System;
using System.Collections.Generic;
using VIENNAAddIn.upcc3.ccts.util;
using Attribute=EA.Attribute;
using VIENNAAddIn.upcc3.XSDGenerator.Generator;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    internal class DTComponent : IDTComponent
    {
        private readonly CCRepository repository;
        private readonly Attribute attribute;
        private readonly DTComponentType componentType;

        public DTComponent(CCRepository repository, Attribute attribute, DTComponentType componentType)
        {
            this.repository = repository;
            this.attribute = attribute;
            this.componentType = componentType;
        }

        public DTComponentType ComponentType
        {
            get { return componentType; }
        }

        public int Id
        {
            get { return attribute.AttributeID; }
        }

        public string Name
        {
            get { return attribute.Name; }
        }

        public string Type
        {
            get { return attribute.Type; }
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

        public IList<string> BusinessTerms
        {
            get { return attribute.CollectTaggedValues(TaggedValues.BusinessTerm); }
        }

        public IList<string> UsageRules
        {
            get { return attribute.CollectTaggedValues(TaggedValues.UsageRule); }
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
            get { return repository.GetDT(attribute.ParentID); }
        }

        public bool IsOptional()
        {
            int i;
            return Int32.TryParse(LowerBound, out i) && i == 0;
        }
    }
}