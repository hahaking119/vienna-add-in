using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts
{
    public interface IBIE
    {
        long Id { get; }
        string Name { get; }
        IBIELibrary Library { get; }

        string UniqueIdentifier { get; }
        string VersionIdentifier { get; }
        string LanguageCode { get; }
        string DictionaryEntryName { get; }
        string Definition { get; }
        IList<string> BusinessTerms { get; }
        IList<string> UsageRules { get; }
    }
}