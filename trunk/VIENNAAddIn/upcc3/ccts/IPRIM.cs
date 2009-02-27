using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts
{
    public interface IPRIM : IType
    {
        IBusinessLibrary Library { get; }
        IEnumerable<string> BusinessTerms { get; }
        string Definition { get; }
        string DictionaryEntryName { get; }
        string Pattern { get; }
        string FractionDigits { get; }
        string Length { get; }
        string MaxExclusive { get; }
        string MaxInclusive { get; }
        string MaxLength { get; }
        string MinExclusive { get; }
        string MinInclusive { get; }
        string MinLength { get; }
        string TotalDigits { get; }
        string WhiteSpace { get; }
        string UniqueIdentifier { get; }
        string VersionIdentifier { get; }
        string LanguageCode { get; }
    }
}