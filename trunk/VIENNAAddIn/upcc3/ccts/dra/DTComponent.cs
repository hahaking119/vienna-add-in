using System;
using System.Collections.Generic;
using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddIn.upcc3.XSDGenerator.Generator;
using Attribute=EA.Attribute;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    internal class DTComponent : IDTComponent
    {
        private readonly Attribute attribute;
        private readonly DTComponentType componentType;
        private readonly IDT dt;

        public DTComponent(Attribute attribute, DTComponentType componentType, IDT dt)
        {
            this.attribute = attribute;
            this.componentType = componentType;
            this.dt = dt;
        }

        #region IDTComponent Members

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

        public IEnumerable<string> BusinessTerms
        {
            get { return attribute.GetTaggedValues(TaggedValues.BusinessTerm); }
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