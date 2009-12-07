using System.Collections.Generic;
using CctsRepository.CdtLibrary;

namespace CctsRepository.BdtLibrary
{
    public partial class BdtConSpec
    {
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
                       MaximumExclusive = bdtCon.MaximumExclusive,
                       MaximumInclusive = bdtCon.MaximumInclusive,
                       MaximumLength = bdtCon.MaximumLength,
                       MinimumExclusive = bdtCon.MinimumExclusive,
                       MinimumInclusive = bdtCon.MinimumInclusive,
                       MinimumLength = bdtCon.MinimumLength,
                       TotalDigits = bdtCon.TotalDigits,
                       Enumeration = bdtCon.Enumeration,
                       Name = bdtCon.Name,
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
                       Name = cdtCon.Name,
                   };
        }
    }
}