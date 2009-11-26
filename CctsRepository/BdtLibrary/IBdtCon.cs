using System.Collections.Generic;

namespace CctsRepository.BdtLibrary
{
    public interface IBdtCon
    {
        int Id { get; }

        IBdt Bdt { get; }
        IBasicType BasicType { get; }

        string UpperBound { get; }
        string LowerBound { get; }
        bool IsOptional();

        #region Tagged Values

        string DictionaryEntryName { get; }
        string Definition { get; }
        string UniqueIdentifier { get; }
        string VersionIdentifier { get; }
        string LanguageCode { get; }
        IEnumerable<string> BusinessTerms { get; }
        bool ModificationAllowedIndicator { get; }
        IEnumerable<string> UsageRules { get; }
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

        #endregion
    }
}