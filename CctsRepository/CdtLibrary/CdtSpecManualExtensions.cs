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
    public partial class CdtSpec
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
            CON = new CdtConSpec(cdt.Con);
            SUPs = new List<CdtSupSpec>(cdt.Sups.Convert(sup => new CdtSupSpec(sup)));
        }

        public CdtSpec()
        {
            SUPs = new List<CdtSupSpec>();
        }

        public List<CdtSupSpec> SUPs { get; set; }
        public CdtConSpec CON { get; set; }

        public void RemoveSUP(string name)
        {
            SUPs.RemoveAll(sup => sup.Name == name);
        }
    }
}