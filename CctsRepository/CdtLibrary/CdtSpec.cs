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
    public class CDTSpec
    {
        public CDTSpec(ICDT cdt)
        {
            Name = cdt.Name;
            DictionaryEntryName = cdt.DictionaryEntryName;
            Definition = cdt.Definition;
            UniqueIdentifier = cdt.UniqueIdentifier;
            VersionIdentifier = cdt.VersionIdentifier;
            LanguageCode = cdt.LanguageCode;
            BusinessTerms = new List<string>(cdt.BusinessTerms);

            UsageRules = new List<string>(cdt.UsageRules);
            CON = new CDTContentComponentSpec(cdt.CON);
            SUPs = new List<CDTSupplementaryComponentSpec>(cdt.SUPs.Convert(sup => new CDTSupplementaryComponentSpec(sup)));
        }

        public CDTSpec()
        {
            SUPs = new List<CDTSupplementaryComponentSpec>();
        }

        public ICDT IsEquivalentTo { get; set; }

        public IEnumerable<string> UsageRules { get; set; }

        public List<CDTSupplementaryComponentSpec> SUPs { get; set; }
        public CDTContentComponentSpec CON { get; set; }
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