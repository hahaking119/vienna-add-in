using System.Collections.Generic;

namespace CctsRepository.BdtLibrary
{
    public interface IBdtCon
    {
        IBdt BDT { get; }
        IBasicType BasicType { get; }
        bool ModificationAllowedIndicator { get; }
        string UpperBound { get; }
        string LowerBound { get; }
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

        ///<summary>
        ///</summary>
        int Id { get; }

        ///<summary>
        ///</summary>
        string Name { get; }

        ///<summary>
        ///</summary>
        string DictionaryEntryName { get; }

        ///<summary>
        ///</summary>
        string Definition { get; }

        ///<summary>
        ///</summary>
        string UniqueIdentifier { get; }

        ///<summary>
        ///</summary>
        string VersionIdentifier { get; }

        ///<summary>
        ///</summary>
        string LanguageCode { get; }

        ///<summary>
        ///</summary>
        IEnumerable<string> BusinessTerms { get; }

        bool IsOptional();
    }
}