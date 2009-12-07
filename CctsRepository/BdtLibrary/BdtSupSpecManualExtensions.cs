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
    public partial class BdtSupSpec
    {
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