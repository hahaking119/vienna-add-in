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

namespace CctsRepository.cdt
{
    public class CDTSpec : CCTSElementSpec
    {
        public CDTSpec(ICDT cdt) : base(cdt)
        {
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

        public void RemoveSUP(string name)
        {
            SUPs.RemoveAll(sup => sup.Name == name);
        }
    }
}