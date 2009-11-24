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
    public class BDTSpec
    {
        public BDTSpec(IBDT bdt)
        {
            Name = bdt.Name;
            DictionaryEntryName = bdt.DictionaryEntryName;
            Definition = bdt.Definition;
            UniqueIdentifier = bdt.UniqueIdentifier;
            VersionIdentifier = bdt.VersionIdentifier;
            LanguageCode = bdt.LanguageCode;
            BusinessTerms = new List<string>(bdt.BusinessTerms);

            UsageRules = new List<string>(bdt.UsageRules);
            CON = new BDTContentComponentSpec(bdt.CON);
            SUPs = new List<BDTSupplementaryComponentSpec>(bdt.SUPs.Convert(sup => new BDTSupplementaryComponentSpec(sup)));
            BasedOn = bdt.BasedOn;
        }

        public BDTSpec()
        {
            SUPs = new List<BDTSupplementaryComponentSpec>();
        }

        public ICDT BasedOn { get; set; }
        public IBDT IsEquivalentTo { get; set; }

        public IEnumerable<string> UsageRules { get; set; }

        public List<BDTSupplementaryComponentSpec> SUPs { get; set; }
        public BDTContentComponentSpec CON { get; set; }
        public string Name { get; set; }
        public string DictionaryEntryName { get; set; }
        public string Definition { get; set; }
        public string UniqueIdentifier { get; set; }
        public string VersionIdentifier { get; set; }
        public string LanguageCode { get; set; }
        public IEnumerable<string> BusinessTerms { get; set; }

        public static BDTSpec CloneCDT(ICDT cdt, string name)
        {
            return new BDTSpec
                   {
                       Name = (string.IsNullOrEmpty(name) ? cdt.Name : name),
                       BasedOn = cdt,
                       BusinessTerms = new List<string>(cdt.BusinessTerms),
                       Definition = cdt.Definition,
                       DictionaryEntryName = cdt.DictionaryEntryName,
                       LanguageCode = cdt.LanguageCode,
                       UniqueIdentifier = cdt.UniqueIdentifier,
                       UsageRules = new List<string>(cdt.UsageRules),
                       VersionIdentifier = cdt.VersionIdentifier,
                       CON = new BDTContentComponentSpec(cdt.CON),
                       SUPs = new List<BDTSupplementaryComponentSpec>(cdt.SUPs.Convert(sup => new BDTSupplementaryComponentSpec(sup))),
                   };
        }

        public void RemoveSUP(string name)
        {
            SUPs.RemoveAll(sup => sup.Name == name);
        }
    }
}