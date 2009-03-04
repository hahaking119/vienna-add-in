using System.Collections.Generic;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts
{
    public class CCTSElementSpec
    {
        private readonly Dictionary<TaggedValues, string> taggedValues = new Dictionary<TaggedValues, string>();

        public string Name { get; set; }

        public string DictionaryEntryName
        {
            get { return GetTaggedValue(TaggedValues.DictionaryEntryName); }
            set { SetTaggedValue(TaggedValues.DictionaryEntryName, value); }
        }

        public string Definition
        {
            get { return GetTaggedValue(TaggedValues.Definition); }
            set { SetTaggedValue(TaggedValues.Definition, value); }
        }
        public string UniqueIdentifier
        {
            get { return GetTaggedValue(TaggedValues.UniqueIdentifier); }
            set { SetTaggedValue(TaggedValues.UniqueIdentifier, value); }
        }

        public string VersionIdentifier
        {
            get { return GetTaggedValue(TaggedValues.VersionIdentifier); }
            set { SetTaggedValue(TaggedValues.VersionIdentifier, value); }
        }
        public string LanguageCode
        {
            get { return GetTaggedValue(TaggedValues.LanguageCode); }
            set { SetTaggedValue(TaggedValues.LanguageCode, value); }
        }

        public IEnumerable<string> BusinessTerms
        {
            get; set;
        }

        public IEnumerable<TaggedValueSpec> GetTaggedValues()
        {
            foreach (var pair in taggedValues)
            {
                yield return new TaggedValueSpec(pair.Key.AsString(), pair.Value);
            }
        }

        protected void SetTaggedValue(TaggedValues key, string value)
        {
            taggedValues[key] = value;
        }

        protected string GetTaggedValue(TaggedValues key)
        {
            return taggedValues[key];
        }
    }
}