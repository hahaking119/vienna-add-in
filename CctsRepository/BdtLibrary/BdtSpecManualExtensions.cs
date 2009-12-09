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
using VIENNAAddInUtils;

namespace CctsRepository.BdtLibrary
{
    public partial class BdtSpec
    {
        public static BdtSpec CloneCdt(ICdt cdt, string name)
        {
            return new BdtSpec
                   {
                       Name = (string.IsNullOrEmpty(name) ? cdt.Name : name),
                       BasedOn = cdt,
                       Definition = cdt.Definition,
                       DictionaryEntryName = cdt.DictionaryEntryName,
                       LanguageCode = cdt.LanguageCode,
                       UniqueIdentifier = cdt.UniqueIdentifier,
                       VersionIdentifier = cdt.VersionIdentifier,
                       BusinessTerms = new List<string>(cdt.BusinessTerms),
                       UsageRules = new List<string>(cdt.UsageRules),
                       Con = BdtConSpec.CloneCdtCon(cdt.Con),
                       Sups = new List<BdtSupSpec>(cdt.Sups.Convert(sup => BdtSupSpec.CloneCdtSup(sup))),
                   };
        }
    }
}