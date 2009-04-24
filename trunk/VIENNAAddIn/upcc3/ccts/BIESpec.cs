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
    public abstract class BIESpec : CCTSElementSpec
    {
        protected BIESpec(IBIE bie) : base(bie)
        {
            UsageRules = new List<string>(bie.UsageRules);
        }

        protected BIESpec()
        {
        }

        [TaggedValue(TaggedValues.UsageRule)]
        public IEnumerable<string> UsageRules { get; set; }
    }
}