// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System.Collections.Generic;

namespace CctsRepository
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

        [TaggedValue]
        public IEnumerable<string> UsageRules { get; set; }
    }
}