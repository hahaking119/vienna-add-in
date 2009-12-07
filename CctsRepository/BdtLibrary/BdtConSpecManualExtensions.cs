using System.Collections.Generic;
using CctsRepository.CdtLibrary;

namespace CctsRepository.BdtLibrary
{
    public partial class BdtConSpec
    {
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