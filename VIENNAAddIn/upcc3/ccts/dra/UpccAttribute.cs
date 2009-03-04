using System.Collections.Generic;
using EA;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    internal class UpccAttribute<TContainer> where TContainer : ICCTSElement
    {
        protected Attribute attribute;
        protected CCRepository repository;

        public UpccAttribute(CCRepository repository, Attribute attribute, TContainer container)
        {
            this.repository = repository;
            this.attribute = attribute;
            this.Container = container;
        }

        public int Id
        {
            get { return attribute.AttributeID; }
        }

        public string Name
        {
            get { return attribute.Name; }
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

        public IEnumerable<string> BusinessTerms
        {
            get { return attribute.GetTaggedValues(TaggedValues.BusinessTerm); }
        }

        public IBusinessLibrary Library
        {
            get { return Container.Library; }
        }

        public IEnumerable<string> UsageRules
        {
            get { return attribute.GetTaggedValues(TaggedValues.UsageRule); }
        }

        public string SequencingKey
        {
            get { return attribute.GetTaggedValue(TaggedValues.SequencingKey); }
        }

        public TContainer Container { get; private set; }
    }
}