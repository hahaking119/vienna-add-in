using System.Collections.Generic;

namespace UpccModel
{
    public class UpccTaggedValues
    {
        public readonly MetaTaggedValue BaseUrn = new MetaTaggedValue("baseURN");

        public readonly MetaTaggedValue BusinessTerm = new MetaTaggedValue("businessTerm")
                                                       {
                                                           Cardinality = Cardinality.Many,
                                                       };

        public readonly MetaTaggedValue Copyright = new MetaTaggedValue("copyright")
                                                    {
                                                        Cardinality = Cardinality.Many,
                                                    };

        public readonly MetaTaggedValue Definition = new MetaTaggedValue("definition");

        public readonly MetaTaggedValue DictionaryEntryName = new MetaTaggedValue("dictionaryEntryName");

        public readonly MetaTaggedValue FractionDigits = new MetaTaggedValue("fractionDigits");

        public readonly MetaTaggedValue LanguageCode = new MetaTaggedValue("languageCode")
                                                            {
                                                                Cardinality = Cardinality.ZeroOrOne,
                                                            };

        public readonly MetaTaggedValue Length = new MetaTaggedValue("length");

        public readonly MetaTaggedValue MaxExclusive = new MetaTaggedValue("maxExclusive");

        public readonly MetaTaggedValue MaxInclusive = new MetaTaggedValue("maxInclusive");

        public readonly MetaTaggedValue MaxLength = new MetaTaggedValue("maxLength");

        public readonly MetaTaggedValue MinExclusive = new MetaTaggedValue("minExclusive");

        public readonly MetaTaggedValue MinInclusive = new MetaTaggedValue("minInclusive");

        public readonly MetaTaggedValue MinLength = new MetaTaggedValue("minLength");

        public readonly MetaTaggedValue NamespacePrefix = new MetaTaggedValue("namespacePrefix")
                                                          {
                                                              Cardinality = Cardinality.ZeroOrOne,
                                                          };

        public readonly MetaTaggedValue Owner = new MetaTaggedValue("owner")
                                                {
                                                    Cardinality = Cardinality.Many,
                                                };

        public readonly MetaTaggedValue Pattern = new MetaTaggedValue("pattern");

        public readonly MetaTaggedValue Reference = new MetaTaggedValue("reference")
                                                    {
                                                        Cardinality = Cardinality.Many,
                                                    };

        public readonly MetaTaggedValue SequencingKey = new MetaTaggedValue("sequencingKey")
                                                        {
                                                            Cardinality = Cardinality.ZeroOrOne,
                                                        };

        public readonly MetaTaggedValue Status = new MetaTaggedValue("status")
                                                 {
                                                     Cardinality = Cardinality.ZeroOrOne,
                                                 };

        public readonly MetaTaggedValue TotalDigits = new MetaTaggedValue("totalDigits");

        public readonly MetaTaggedValue UniqueIdentifier = new MetaTaggedValue("uniqueIdentifier")
                                                            {
                                                                Cardinality = Cardinality.ZeroOrOne,
                                                            };

        public readonly MetaTaggedValue UsageRule = new MetaTaggedValue("usageRule")
                                                    {
                                                        Cardinality = Cardinality.Many,
                                                    };

        public readonly MetaTaggedValue VersionIdentifier = new MetaTaggedValue("versionIdentifier")
                                                            {
                                                                Cardinality = Cardinality.ZeroOrOne,
                                                            };

        public readonly MetaTaggedValue WhiteSpace = new MetaTaggedValue("whiteSpace");

        public IEnumerable<MetaTaggedValue> All
        {
            get
            {
                foreach (var field in GetType().GetFields())
                {
                    yield return (MetaTaggedValue) field.GetValue(this);
                }
            }
        }
    }
}