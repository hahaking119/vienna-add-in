using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts
{
    public interface IBIE : IElement
    {
        string UniqueIdentifier { get; }
        string VersionIdentifier { get; }
        string LanguageCode { get; }
        string DictionaryEntryName { get; }
        string Definition { get; }
        IEnumerable<string> BusinessTerms { get; }
        IEnumerable<string> UsageRules { get; }
    }
}