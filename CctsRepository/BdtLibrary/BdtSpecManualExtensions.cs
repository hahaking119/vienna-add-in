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
        public BdtSpec()
        {
            Sups = new List<BdtSupSpec>();
        }

        public BdtConSpec Con { get; set; }

        public List<BdtSupSpec> Sups { get; private set; }

        public static BdtSpec CloneBdt(IBdt bdt)
        {
            return new BdtSpec
                   {
                       Name = bdt.Name,
                       BasedOn = bdt.BasedOn,
                       Definition = bdt.Definition,
                       DictionaryEntryName = bdt.DictionaryEntryName,
                       LanguageCode = bdt.LanguageCode,
                       UniqueIdentifier = bdt.UniqueIdentifier,
                       VersionIdentifier = bdt.VersionIdentifier,
                       BusinessTerms = new List<string>(bdt.BusinessTerms),
                       UsageRules = new List<string>(bdt.UsageRules),
                       Con = BdtConSpec.CloneBdtCon(bdt.Con),
                       Sups = new List<BdtSupSpec>(bdt.Sups.Convert(sup => BdtSupSpec.CloneBdtSup(sup))),
                   };
        }

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

        public void AddSup(BdtSupSpec supSpec)
        {
            Sups.Add(supSpec);
        }

        public void AddSups(IEnumerable<BdtSupSpec> supSpecs)
        {
            Sups.AddRange(supSpecs);
        }

        public void RemoveSup(string name)
        {
            Sups.RemoveAll(sup => sup.Name == name);
        }
    }
}