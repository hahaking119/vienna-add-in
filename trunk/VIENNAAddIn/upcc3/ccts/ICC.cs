using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts
{
    public interface ICC
    {
        long Id { get; }
        string Name { get; }
        IBusinessLibrary Library { get; }
        string Definition { get; }
        string DictionaryEntryName { get; }
        string LanguageCode { get; }
        string UniqueIdentifier { get; }
        string VersionIdentifier { get; }
        IList<string> BusinessTerms { get; }
        IList<string> UsageRules { get; }
    }
}