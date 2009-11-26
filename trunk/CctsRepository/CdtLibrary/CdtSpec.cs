// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System.Collections.Generic;
using VIENNAAddInUtils;

namespace CctsRepository.CdtLibrary
{
    public class CdtSpec
    {
        public CdtSpec(ICdt cdt)
        {
            Name = cdt.Name;
            DictionaryEntryName = cdt.DictionaryEntryName;
            Definition = cdt.Definition;
            UniqueIdentifier = cdt.UniqueIdentifier;
            VersionIdentifier = cdt.VersionIdentifier;
            LanguageCode = cdt.LanguageCode;
            BusinessTerms = new List<string>(cdt.BusinessTerms);

            UsageRules = new List<string>(cdt.UsageRules);
            CON = new CdtConSpec(cdt.CON);
            SUPs = new List<CdtSupSpec>(cdt.SUPs.Convert(sup => new CdtSupSpec(sup)));
        }

        public CdtSpec()
        {
            SUPs = new List<CdtSupSpec>();
        }

        public ICdt IsEquivalentTo { get; set; }

        public IEnumerable<string> UsageRules { get; set; }

        public List<CdtSupSpec> SUPs { get; set; }
        public CdtConSpec CON { get; set; }
        public string Name { get; set; }
        public string DictionaryEntryName { get; set; }
        public string Definition { get; set; }
        public string UniqueIdentifier { get; set; }
        public string VersionIdentifier { get; set; }
        public string LanguageCode { get; set; }
        public IEnumerable<string> BusinessTerms { get; set; }

        public void RemoveSUP(string name)
        {
            SUPs.RemoveAll(sup => sup.Name == name);
        }
    }
}