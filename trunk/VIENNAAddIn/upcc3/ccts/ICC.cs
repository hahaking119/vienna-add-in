using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts
{
    public interface ICC
    {
        int Id { get; }
        string Name { get; }
        string Definition { get; }
        string DictionaryEntryName { get; }
        string LanguageCode { get; }
        string UniqueIdentifier { get; }
        string VersionIdentifier { get; }
        IEnumerable<string> BusinessTerms { get; }
        IEnumerable<string> UsageRules { get; }
    }
}