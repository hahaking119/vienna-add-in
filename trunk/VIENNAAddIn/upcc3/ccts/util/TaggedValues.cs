using System;

namespace VIENNAAddIn.upcc3.ccts.util
{
    public enum TaggedValues
    {
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
        EnumerationURI
    }

    public static class TaggedValuesExtensions
    {
        public static string AsString(this TaggedValues tv)
        {
            switch (tv)
            {
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
    }
}