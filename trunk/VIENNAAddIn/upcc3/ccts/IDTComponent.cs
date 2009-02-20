using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts
{
    public enum DTComponentType
    {
        CON,
        SUP
    }

    public interface IDTComponent
    {
        DTComponentType ComponentType { get; }
        int Id { get; }
        string Name { get; }
        string Type { get; }
        string Definition { get; }
        string DictionaryEntryName { get; }
        string LanguageCode { get; }
        string UniqueIdentifier { get; }
        string VersionIdentifier { get; }

        /// <summary>
        /// TODO this should be a boolean
        /// </summary>
        string ModificationAllowedIndicator { get; }

        IList<string> BusinessTerms { get; }
        IList<string> UsageRules { get; }
        string UpperBound { get; }
        string LowerBound { get; }
        IDT DT { get; }
        bool IsOptional();
    }
}