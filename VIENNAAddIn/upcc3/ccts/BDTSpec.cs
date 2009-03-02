using System.Collections.Generic;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts
{
    public class BDTSpec : DTSpec
    {
        public ICDT BasedOn { get; set; }

        public static BDTSpec CloneCDT(ICDT cdt, string qualifier)
        {
            return new BDTSpec
                   {
                       Name = (qualifier != null ? qualifier + "_" + cdt.Name : cdt.Name),
                       BasedOn = cdt,
                       BusinessTerms = new List<string>(cdt.BusinessTerms),
                       Definition = cdt.Definition,
                       DictionaryEntryName = cdt.DictionaryEntryName,
                       LanguageCode = cdt.LanguageCode,
                       UniqueIdentifier = cdt.UniqueIdentifier,
                       UsageRules = new List<string>(cdt.UsageRules),
                       VersionIdentifier = cdt.VersionIdentifier,
                       CON = CloneCON(cdt),
                       SUPs = CloneSUPs(cdt),
                   };
        }
    }
}