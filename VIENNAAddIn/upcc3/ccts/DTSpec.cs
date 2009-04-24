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

namespace VIENNAAddIn.upcc3.ccts
{
    public abstract class DTSpec : CCTSElementSpec
    {
        protected DTSpec(IDT dt) : base(dt)
        {
            UsageRules = new List<string>(dt.UsageRules);
            CON = new CONSpec(dt.CON);
            SUPs = new List<SUPSpec>(dt.SUPs.Convert(sup => new SUPSpec(sup)));
        }

        protected DTSpec()
        {
        }

        [TaggedValue(TaggedValues.UsageRule)]
        public IEnumerable<string> UsageRules { get; set; }

        public List<SUPSpec> SUPs { get; set; }
        public CONSpec CON { get; set; }

        public void RemoveSUP(string name)
        {
            SUPs.RemoveAll(sup => sup.Name == name);
        }
    }
}