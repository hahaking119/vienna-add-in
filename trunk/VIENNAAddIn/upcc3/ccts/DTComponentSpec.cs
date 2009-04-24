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
    public abstract class DTComponentSpec : CCTSElementSpec
    {
        protected DTComponentSpec(IDTComponent dtComponent) : base(dtComponent)
        {
            BasicType = dtComponent.BasicType;
            ModificationAllowedIndicator = dtComponent.ModificationAllowedIndicator;
            UsageRules = new List<string>(dtComponent.UsageRules);
            UpperBound = dtComponent.UpperBound;
            LowerBound = dtComponent.LowerBound;
        }

        protected DTComponentSpec()
        {
        }

        public IBasicType BasicType { get; set; }

        [TaggedValue(TaggedValues.ModificationAllowedIndicator)]
        public bool ModificationAllowedIndicator { get; set; }

        [TaggedValue(TaggedValues.UsageRule)]
        public IEnumerable<string> UsageRules { get; set; }

        public string UpperBound { get; set; }
        public string LowerBound { get; set; }
    }
}