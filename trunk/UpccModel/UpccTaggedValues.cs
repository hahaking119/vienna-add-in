using System.Collections.Generic;
using System.Reflection;

namespace Upcc
{
    public class UpccTaggedValues
    {
        public readonly MetaTaggedValue BaseUrn = new MetaTaggedValue("baseURN");

        public readonly MetaTaggedValue BusinessTerm = new MetaTaggedValue("businessTerm")
                                                       {
                                                           Cardinality = Cardinality.Many,
                                                       };

        public readonly MetaTaggedValue CodeListAgencyIdentifier = new MetaTaggedValue("codeListAgencyIdentifier")
                                                                   {
                                                                       Cardinality = Cardinality.ZeroOrOne,
                                                                   };

        public readonly MetaTaggedValue CodeListAgencyName = new MetaTaggedValue("codeListAgencyName")
                                                             {
                                                                 Cardinality = Cardinality.ZeroOrOne,
                                                             };

        public readonly MetaTaggedValue CodeListIdentifier = new MetaTaggedValue("codeListIdentifier")
                                                             {
                                                                 Cardinality = Cardinality.ZeroOrOne,
                                                             };

        public readonly MetaTaggedValue CodeListName = new MetaTaggedValue("codeListName")
                                                       {
                                                           Cardinality = Cardinality.ZeroOrOne,
                                                       };

        public readonly MetaTaggedValue CodeName = new MetaTaggedValue("codeName");

        public readonly MetaTaggedValue Copyright = new MetaTaggedValue("copyright")
                                                    {
                                                        Cardinality = Cardinality.Many,
                                                    };

        public readonly MetaTaggedValue Definition = new MetaTaggedValue("definition");

        public readonly MetaTaggedValue DictionaryEntryName = new MetaTaggedValue("dictionaryEntryName");

        public readonly MetaTaggedValue EnumerationUri = new MetaTaggedValue("enumerationURI")
                                                         {
                                                             Cardinality = Cardinality.ZeroOrOne,
                                                         };

        public readonly MetaTaggedValue Enumeration = new MetaTaggedValue("enumeration")
                                                         {
                                                             Cardinality = Cardinality.ZeroOrOne,
                                                         };

        public readonly MetaTaggedValue FractionDigits = new MetaTaggedValue("fractionDigits");

        public readonly MetaTaggedValue IdentifierSchemeAgencyIdentifier = new MetaTaggedValue("identifierSchemeAgencyIdentifier")
                                                                           {
                                                                               Cardinality = Cardinality.ZeroOrOne,
                                                                           };

        public readonly MetaTaggedValue IdentifierSchemeAgencyName = new MetaTaggedValue("identifierSchemeAgencyName")
                                                                     {
                                                                         Cardinality = Cardinality.ZeroOrOne,
                                                                     };

        public readonly MetaTaggedValue LanguageCode = new MetaTaggedValue("languageCode")
                                                       {
                                                           Cardinality = Cardinality.ZeroOrOne,
                                                       };

        public readonly MetaTaggedValue Length = new MetaTaggedValue("length");

        public readonly MetaTaggedValue MaximumExclusive = new MetaTaggedValue("maximumExclusive");

        public readonly MetaTaggedValue MaximumInclusive = new MetaTaggedValue("maximumInclusive");

        public readonly MetaTaggedValue MaximumLength = new MetaTaggedValue("maximumLength");

        public readonly MetaTaggedValue MinimumExclusive = new MetaTaggedValue("minimumExclusive");

        public readonly MetaTaggedValue MinimumInclusive = new MetaTaggedValue("minimumInclusive");

        public readonly MetaTaggedValue MinimumLength = new MetaTaggedValue("minimumLength");

        public readonly MetaTaggedValue ModificationAllowedIndicator = new MetaTaggedValue("modificationAllowedIndicator")
                                                                       {
                                                                           Type = "bool",
                                                                       };

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

        public readonly MetaTaggedValue RestrictedPrimitive = new MetaTaggedValue("restrictedPrimitive");

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
                foreach (FieldInfo field in GetType().GetFields())
                {
                    yield return (MetaTaggedValue) field.GetValue(this);
                }
            }
        }
    }
}