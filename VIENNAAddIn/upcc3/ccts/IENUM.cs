using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts
{
    public interface IENUM
    {
        int Id { get; }
        string Name { get; }
        IENUMLibrary Library { get; }

        string AgencyIdentifier { get; }
        string AgencyName { get; }
        IList<string> BusinessTerms { get; }
        string LanguageCode { get; }
        string DictionaryEntryName { get; }
        string UniqueIdentifier { get; }
        string VersionIdentifier { get; }
        string EnumerationURI { get; }
    }
}