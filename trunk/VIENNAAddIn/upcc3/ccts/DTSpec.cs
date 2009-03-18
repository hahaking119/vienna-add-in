// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System.Collections.Generic;
using VIENNAAddIn.upcc3.ccts.util;
using System.Linq;

namespace VIENNAAddIn.upcc3.ccts
{
    public class DTSpec : CCTSElementSpec
    {
        [TaggedValue(TaggedValues.UsageRule)]
        public IEnumerable<string> UsageRules { get; set; }

        public List<SUPSpec> SUPs { get; set; }
        public CONSpec CON { get; set; }

        public void RemoveSUP(string name)
        {
            SUPs.RemoveAll(sup => sup.Name == name);
        }

        protected static IEnumerable<SUPSpec> CloneSUPs(ICDT cdt)
        {
            return cdt.SUPs.Convert(sup => CloneSUP(sup));
        }

        private static SUPSpec CloneSUP(ISUP sup)
        {
            return new SUPSpec
                   {
                       Name = sup.Name,
                       BasicType = sup.BasicType,
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
            ICON con = cdt.CON;
            return new CONSpec
                   {
                       BasicType = con.BasicType,
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