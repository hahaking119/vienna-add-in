// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System.Collections.Generic;
using CctsRepository.CdtLibrary;

namespace CctsRepository.BdtLibrary
{
    public class BdtSupSpec
    {
        public string Name { get; set; }

        public IBasicType BasicType { get; set; }

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
        public string Length { get; set; }
        public string MaxExclusive { get; set; }
        public string MaxInclusive { get; set; }
        public string MaxLength { get; set; }
        public string MinExclusive { get; set; }
        public string MinInclusive { get; set; }
        public string MinLength { get; set; }
        public string TotalDigits { get; set; }
        public string WhiteSpace { get; set; }

        #endregion

        public static BdtSupSpec CloneBdtSup(IBdtSup bdtSup)
        {
            return new BdtSupSpec
                   {
                       Name = bdtSup.Name,
                       DictionaryEntryName = bdtSup.DictionaryEntryName,
                       Definition = bdtSup.Definition,
                       UniqueIdentifier = bdtSup.UniqueIdentifier,
                       VersionIdentifier = bdtSup.VersionIdentifier,
                       LanguageCode = bdtSup.LanguageCode,
                       BusinessTerms = new List<string>(bdtSup.BusinessTerms),
                       BasicType = bdtSup.BasicType,
                       UpperBound = bdtSup.UpperBound,
                       LowerBound = bdtSup.LowerBound,
                       ModificationAllowedIndicator = bdtSup.ModificationAllowedIndicator,
                       UsageRules = new List<string>(bdtSup.UsageRules),
                       Pattern = bdtSup.Pattern,
                       FractionDigits = bdtSup.FractionDigits,
                       Length = bdtSup.Length,
                       MaxExclusive = bdtSup.MaxExclusive,
                       MaxInclusive = bdtSup.MaxInclusive,
                       MaxLength = bdtSup.MaxLength,
                       MinExclusive = bdtSup.MinExclusive,
                       MinInclusive = bdtSup.MinInclusive,
                       MinLength = bdtSup.MinLength,
                       TotalDigits = bdtSup.TotalDigits,
                       WhiteSpace = bdtSup.WhiteSpace,
                   }
                ;
        }

        public static BdtSupSpec CloneCdtSup(ICdtSup cdtSup)
        {
            return new BdtSupSpec
                   {
                       Name = cdtSup.Name,
                       DictionaryEntryName = cdtSup.DictionaryEntryName,
                       Definition = cdtSup.Definition,
                       UniqueIdentifier = cdtSup.UniqueIdentifier,
                       VersionIdentifier = cdtSup.VersionIdentifier,
                       LanguageCode = cdtSup.LanguageCode,
                       BusinessTerms = new List<string>(cdtSup.BusinessTerms),
                       BasicType = cdtSup.BasicType,
                       UpperBound = cdtSup.UpperBound,
                       LowerBound = cdtSup.LowerBound,
                       ModificationAllowedIndicator = cdtSup.ModificationAllowedIndicator,
                       UsageRules = new List<string>(cdtSup.UsageRules),
                   }
                ;
        }
    }
}