using System;

namespace VIENNAAddIn.upcc3.ccts.util
{
    public enum TaggedValues
    {
        Undefined,
        Status,
        UniqueIdentifier,
        VersionIdentifier,
        NamespacePrefix,
        BaseURN,
        BusinessTerm,
        Copyright,
        Owner,
        Reference,
        Definition,
        DictionaryEntryName,
        LanguageCode,
        UsageRule,
        ModificationAllowedIndicator,
        Pattern,
        FractionDigits,
        Length,
        MaxExclusive,
        MaxInclusive,
        MaxLength,
        MinExclusive,
        MinInclusive,
        MinLength,
        TotalDigits,
        WhiteSpace,
        ApplyTo,
        SequencingKey,
        AgencyIdentifier,
        AgencyName,
        EnumerationURI,
    }

    public static class TaggedValuesExtensions
    {
        public static string AsString(this TaggedValues tv)
        {
            switch (tv)
            {
                case TaggedValues.Undefined:
                    return "undefined";
                case TaggedValues.Status:
                    return "status";
                case TaggedValues.UniqueIdentifier:
                    return "uniqueIdentifier";
                case TaggedValues.VersionIdentifier:
                    return "versionIdentifier";
                case TaggedValues.NamespacePrefix:
                    return "namespacePrefix";
                case TaggedValues.BaseURN:
                    return "baseURN";
                case TaggedValues.BusinessTerm:
                    return "businessTerm";
                case TaggedValues.Copyright:
                    return "copyright";
                case TaggedValues.Owner:
                    return "owner";
                case TaggedValues.Reference:
                    return "reference";
                case TaggedValues.Definition:
                    return "definition";
                case TaggedValues.DictionaryEntryName:
                    return "dictionaryEntryName";
                case TaggedValues.LanguageCode:
                    return "languageCode";
                case TaggedValues.UsageRule:
                    return "usageRule";
                case TaggedValues.ModificationAllowedIndicator:
                    return "modificationAllowedIndicator";
                case TaggedValues.Pattern:
                    return "pattern";
                case TaggedValues.FractionDigits:
                    return "fractionDigits";
                case TaggedValues.Length:
                    return "length";
                case TaggedValues.MaxExclusive:
                    return "maxExclusive";
                case TaggedValues.MaxInclusive:
                    return "maxInclusive";
                case TaggedValues.MaxLength:
                    return "maxLength";
                case TaggedValues.MinExclusive:
                    return "minExclusive";
                case TaggedValues.MinInclusive:
                    return "minInclusive";
                case TaggedValues.MinLength:
                    return "minLength";
                case TaggedValues.TotalDigits:
                    return "totalDigits";
                case TaggedValues.WhiteSpace:
                    return "whiteSpace";
                case TaggedValues.ApplyTo:
                    return "applyTo";
                case TaggedValues.SequencingKey:
                    return "sequencingKey";
                case TaggedValues.AgencyIdentifier:
                    return "agencyIdentifier";
                case TaggedValues.AgencyName:
                    return "agencyName";
                case TaggedValues.EnumerationURI:
                    return "enumerationURI";
                default:
                    throw new ArgumentOutOfRangeException("tv");
            }
        }

        public static TaggedValues ForString(string str)
        {
            switch (str.ToLower())
            {
                case "status":
                    return TaggedValues.Status;
                case "uniqueidentifier":
                    return TaggedValues.UniqueIdentifier;
                case "versionidentifier":
                    return TaggedValues.VersionIdentifier;
                case "namespaceprefix":
                    return TaggedValues.NamespacePrefix;
                case "baseurn":
                    return TaggedValues.BaseURN;
                case "businessterm":
                    return TaggedValues.BusinessTerm;
                case "copyright":
                    return TaggedValues.Copyright;
                case "owner":
                    return TaggedValues.Owner;
                case "reference":
                    return TaggedValues.Reference;
                case "definition":
                    return TaggedValues.Definition;
                case "dictionaryentryname":
                    return TaggedValues.DictionaryEntryName;
                case "languagecode":
                    return TaggedValues.LanguageCode;
                case "usagerule":
                    return TaggedValues.UsageRule;
                case "modificationallowedindicator":
                    return TaggedValues.ModificationAllowedIndicator;
                case "pattern":
                    return TaggedValues.Pattern;
                case "fractiondigits":
                    return TaggedValues.FractionDigits;
                case "length":
                    return TaggedValues.Length;
                case "maxexclusive":
                    return TaggedValues.MaxExclusive;
                case "maxinclusive":
                    return TaggedValues.MaxInclusive;
                case "maxlength":
                    return TaggedValues.MaxLength;
                case "minexclusive":
                    return TaggedValues.MinExclusive;
                case "mininclusive":
                    return TaggedValues.MinInclusive;
                case "minlength":
                    return TaggedValues.MinLength;
                case "totaldigits":
                    return TaggedValues.TotalDigits;
                case "whitespace":
                    return TaggedValues.WhiteSpace;
                case "applyto":
                    return TaggedValues.ApplyTo;
                case "sequencingkey":
                    return TaggedValues.SequencingKey;
                case "agencyidentifier":
                    return TaggedValues.AgencyIdentifier;
                case "agencyname":
                    return TaggedValues.AgencyName;
                case "enumerationuri":
                    return TaggedValues.EnumerationURI;
                default:
                    return TaggedValues.Undefined;
            }
        }
    }
}