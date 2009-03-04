using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts
{
    public interface ICCTSElement
    {
        int Id { get; }
        string Name { get; }
        string DictionaryEntryName { get; }
        string Definition { get; }
        string UniqueIdentifier { get; }
        string VersionIdentifier { get; }
        string LanguageCode { get; }
        IEnumerable<string> BusinessTerms { get; }
        IBusinessLibrary Library { get; }
    }
}