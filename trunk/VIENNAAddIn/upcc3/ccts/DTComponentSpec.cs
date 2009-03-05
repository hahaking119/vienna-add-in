using System.Collections.Generic;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts
{
    public class DTComponentSpec : CCTSElementSpec
    {
        public IBasicType BasicType { get; set; }

        [TaggedValue(TaggedValues.ModificationAllowedIndicator)]
        public bool ModificationAllowedIndicator { get; set; }

        [TaggedValue(TaggedValues.UsageRule)]
        public IEnumerable<string> UsageRules { get; set; }

        public string UpperBound { get; set; }
        public string LowerBound { get; set; }
    }
}