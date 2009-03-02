using System.Collections.Generic;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts
{
    public class DTSpec
    {
        public string Name { get; set; }
        public string Definition { get; set; }
        public string DictionaryEntryName { get; set; }
        public string LanguageCode { get; set; }
        public string UniqueIdentifier { get; set; }
        public string VersionIdentifier { get; set; }
        public IEnumerable<string> BusinessTerms { get; set; }
        public IEnumerable<string> UsageRules { get; set; }
        public IEnumerable<SUPSpec> SUPs { get; set; }
        public CONSpec CON { get; set; }

        protected static IEnumerable<SUPSpec> CloneSUPs(ICDT cdt)
        {
            return cdt.SUPs.Convert(sup => CloneSUP(sup));
        }

        private static SUPSpec CloneSUP(IDTComponent sup)
        {
            return new SUPSpec
                   {
                       Name = sup.Name,
                       Type = sup.Type,
                       Definition = sup.Definition,
                       DictionaryEntryName = sup.DictionaryEntryName,
                       LanguageCode = sup.LanguageCode,
                       ModificationAllowedIndicator = sup.ModificationAllowedIndicator,
                       LowerBound = sup.LowerBound,
                       UpperBound = sup.UpperBound,
                       UniqueIdentifier = sup.UniqueIdentifier,
                       VersionIdentifier = sup.VersionIdentifier,
                       BusinessTerms = sup.BusinessTerms,
                       UsageRules = sup.UsageRules,
                   };
        }

        protected static CONSpec CloneCON(ICDT cdt)
        {
            IDTComponent con = cdt.CON;
            return new CONSpec
                   {
                       Type = con.Type,
                       Definition = con.Definition,
                       DictionaryEntryName = con.DictionaryEntryName,
                       LanguageCode = con.LanguageCode,
                       ModificationAllowedIndicator = con.ModificationAllowedIndicator,
                       LowerBound = con.LowerBound,
                       UpperBound = con.UpperBound,
                       UniqueIdentifier = con.UniqueIdentifier,
                       VersionIdentifier = con.VersionIdentifier,
                       BusinessTerms = con.BusinessTerms,
                       UsageRules = con.UsageRules,
                   };
        }
    }
}