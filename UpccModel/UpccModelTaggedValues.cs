namespace UpccModel
{
    public static class UpccModelTaggedValues
    {
        public static readonly MetaTaggedValue BaseUrn;
        public static readonly MetaTaggedValue BusinessTerm;
        public static readonly MetaTaggedValue Copyright;
        public static readonly MetaTaggedValue Definition;
        public static readonly MetaTaggedValue DictionaryEntryName;
        public static readonly MetaTaggedValue FractionDigits;
        public static readonly MetaTaggedValue LanguageCode;
        public static readonly MetaTaggedValue Length;
        public static readonly MetaTaggedValue MaxExclusive;
        public static readonly MetaTaggedValue MaxInclusive;
        public static readonly MetaTaggedValue MaxLength;
        public static readonly MetaTaggedValue MinExclusive;
        public static readonly MetaTaggedValue MinInclusive;
        public static readonly MetaTaggedValue MinLength;
        public static readonly MetaTaggedValue NamespacePrefix;
        public static readonly MetaTaggedValue Owner;
        public static readonly MetaTaggedValue Pattern;
        public static readonly MetaTaggedValue Reference;
        public static readonly MetaTaggedValue SequencingKey;
        public static readonly MetaTaggedValue Status;
        public static readonly MetaTaggedValue[] TaggedValues;
        public static readonly MetaTaggedValue TotalDigits;
        public static readonly MetaTaggedValue UniqueIdentifier;
        public static readonly MetaTaggedValue UsageRule;
        public static readonly MetaTaggedValue VersionIdentifier;
        public static readonly MetaTaggedValue WhiteSpace;

        static UpccModelTaggedValues()
        {
            Definition = new MetaTaggedValue
                         {
                             Name = "definition",
                             Cardinality = Cardinality.One,
                         };

            DictionaryEntryName = new MetaTaggedValue
                                  {
                                      Name = "dictionaryEntryName",
                                      Cardinality = Cardinality.One,
                                  };

            BusinessTerm = new MetaTaggedValue
                           {
                               Name = "businessTerm",
                               Cardinality = Cardinality.Many,
                           };

            Copyright = new MetaTaggedValue
                        {
                            Name = "copyright",
                            Cardinality = Cardinality.Many,
                        };

            Owner = new MetaTaggedValue
                    {
                        Name = "owner",
                        Cardinality = Cardinality.Many,
                    };

            Reference = new MetaTaggedValue
                        {
                            Name = "reference",
                            Cardinality = Cardinality.Many,
                        };

            Pattern = new MetaTaggedValue
                      {
                          Name = "pattern",
                          Cardinality = Cardinality.One,
                      };

            FractionDigits = new MetaTaggedValue
                             {
                                 Name = "fractionDigits",
                                 Cardinality = Cardinality.One,
                             };

            TotalDigits = new MetaTaggedValue
                          {
                              Name = "totalDigits",
                              Cardinality = Cardinality.One,
                          };

            Length = new MetaTaggedValue
                     {
                         Name = "length",
                         Cardinality = Cardinality.One,
                     };

            MaxExclusive = new MetaTaggedValue
                           {
                               Name = "maxExclusive",
                               Cardinality = Cardinality.One,
                           };

            MaxInclusive = new MetaTaggedValue
                           {
                               Name = "maxInclusive",
                               Cardinality = Cardinality.One,
                           };

            MaxLength = new MetaTaggedValue
                        {
                            Name = "maxLength",
                            Cardinality = Cardinality.One,
                        };

            MinExclusive = new MetaTaggedValue
                           {
                               Name = "minExclusive",
                               Cardinality = Cardinality.One,
                           };

            MinInclusive = new MetaTaggedValue
                           {
                               Name = "minInclusive",
                               Cardinality = Cardinality.One,
                           };

            MinLength = new MetaTaggedValue
                        {
                            Name = "minLength",
                            Cardinality = Cardinality.One,
                        };

            WhiteSpace = new MetaTaggedValue
                         {
                             Name = "whiteSpace",
                             Cardinality = Cardinality.One,
                         };

            UniqueIdentifier = new MetaTaggedValue
                               {
                                   Name = "uniqueIdentifier",
                                   Cardinality = Cardinality.One,
                               };

            VersionIdentifier = new MetaTaggedValue
                                {
                                    Name = "versionIdentifier",
                                    Cardinality = Cardinality.One,
                                };

            UsageRule = new MetaTaggedValue
                        {
                            Name = "usageRule",
                            Cardinality = Cardinality.Many,
                        };

            LanguageCode = new MetaTaggedValue
                           {
                               Name = "languageCode",
                               Cardinality = Cardinality.One,
                           };

            Status = new MetaTaggedValue
                     {
                         Name = "status",
                         Cardinality = Cardinality.ZeroOrOne,
                     };

            BaseUrn = new MetaTaggedValue
                      {
                          Name = "baseURN",
                          Cardinality = Cardinality.One,
                      };

            NamespacePrefix = new MetaTaggedValue
                              {
                                  Name = "namespacePrefix",
                                  Cardinality = Cardinality.ZeroOrOne,
                              };

            SequencingKey = new MetaTaggedValue
                            {
                                Name = "sequencingKey",
                                Cardinality = Cardinality.ZeroOrOne,
                            };

            TaggedValues = new[]
                           {
                               BaseUrn,
                               BusinessTerm,
                               Copyright,
                               Definition,
                               DictionaryEntryName,
                               FractionDigits,
                               LanguageCode,
                               Length,
                               MaxExclusive,
                               MaxInclusive,
                               MaxLength,
                               MinExclusive,
                               MinInclusive,
                               MinLength,
                               NamespacePrefix,
                               Owner,
                               Pattern,
                               Reference,
                               SequencingKey,
                               Status,
                               TotalDigits,
                               UniqueIdentifier,
                               UsageRule,
                               VersionIdentifier,
                               WhiteSpace,
                           };
        }
    }
}