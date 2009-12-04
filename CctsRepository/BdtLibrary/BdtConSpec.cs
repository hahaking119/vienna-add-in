using System.Collections.Generic;
using CctsRepository.CdtLibrary;

namespace CctsRepository.BdtLibrary
{
    public class BdtConSpec
    {
        public BasicType BasicType { get; set; }

        public string UpperBound { get; set; }
        public string LowerBound { get; set; }

        #region Tagged Values

        public string DictionaryEntryName { get; set; }
        public string Definition { get; set; }
        public string UniqueIdentifier { get; set; }
        public string VersionIdentifier { get; set; }
        public string LanguageCode { get; set; }
        public IEnumerable<string> BusinessTerms { get; set; }
        public bool ModificationAllowedIndicator { get; set; }
        public IEnumerable<string> UsageRules { get; set; }
        public string Pattern { get; set; }
        public string FractionDigits { get; set; }
        public string MaxExclusive { get; set; }
        public string MaxInclusive { get; set; }
        public string MaxLength { get; set; }
        public string MinExclusive { get; set; }
        public string MinInclusive { get; set; }
        public string MinLength { get; set; }
        public string TotalDigits { get; set; }

        #endregion

        public static BdtConSpec CloneBdtCon(IBdtCon bdtCon)
        {
            return new BdtConSpec
                   {
                       BasicType = bdtCon.BasicType,
                       UpperBound = bdtCon.UpperBound,
                       LowerBound = bdtCon.LowerBound,
                       DictionaryEntryName = bdtCon.DictionaryEntryName,
                       Definition = bdtCon.Definition,
                       UniqueIdentifier = bdtCon.UniqueIdentifier,
                       VersionIdentifier = bdtCon.VersionIdentifier,
                       LanguageCode = bdtCon.LanguageCode,
                       BusinessTerms = new List<string>(bdtCon.BusinessTerms),
                       ModificationAllowedIndicator = bdtCon.ModificationAllowedIndicator,
                       UsageRules = new List<string>(bdtCon.UsageRules),
                       Pattern = bdtCon.Pattern,
                       FractionDigits = bdtCon.FractionDigits,
                       MaxExclusive = bdtCon.MaximumExclusive,
                       MaxInclusive = bdtCon.MaximumInclusive,
                       MaxLength = bdtCon.MaximumLength,
                       MinExclusive = bdtCon.MinimumExclusive,
                       MinInclusive = bdtCon.MinimumInclusive,
                       MinLength = bdtCon.MinimumLength,
                       TotalDigits = bdtCon.TotalDigits,
                   };
        }

        public static BdtConSpec CloneCdtCon(ICdtCon cdtCon)
        {
            return new BdtConSpec
                   {
                       BasicType = cdtCon.BasicType,
                       UpperBound = cdtCon.UpperBound,
                       LowerBound = cdtCon.LowerBound,
                       DictionaryEntryName = cdtCon.DictionaryEntryName,
                       Definition = cdtCon.Definition,
                       UniqueIdentifier = cdtCon.UniqueIdentifier,
                       VersionIdentifier = cdtCon.VersionIdentifier,
                       LanguageCode = cdtCon.LanguageCode,
                       BusinessTerms = new List<string>(cdtCon.BusinessTerms),
                       ModificationAllowedIndicator = cdtCon.ModificationAllowedIndicator,
                       UsageRules = new List<string>(cdtCon.UsageRules),
                   };
        }
    }
}