// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System.Collections.Generic;
using CctsRepository;
using VIENNAAddInUtils;

namespace CctsRepository
{
    public class BDTSpec : DTSpec
    {
        public BDTSpec(IBDT bdt) : base(bdt)
        {
            BasedOn = bdt.BasedOn;
        }

        public BDTSpec()
        {
        }

        [Dependency]
        public ICDT BasedOn { get; set; }

        [Dependency]
        public IBDT IsEquivalentTo { get; set; }

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
                           CON = new CONSpec(cdt.CON),
                           SUPs = new List<SUPSpec>(cdt.SUPs.Convert(sup => new SUPSpec(sup))),
                       };
        }
    }
}