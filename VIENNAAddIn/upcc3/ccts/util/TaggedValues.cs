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
        ModificationAllowedIndicator
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
                default:
                    throw new ArgumentOutOfRangeException("tv");
            }
        }
    }
}