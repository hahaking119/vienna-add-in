using System.Collections.Generic;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts
{
    public abstract class CCSpec : CCTSElementSpec
    {
        protected CCSpec(ICC cc) : base(cc)
        {
            UsageRules = new List<string>(cc.UsageRules);
        }

        protected CCSpec()
        {
        }

        [TaggedValue(TaggedValues.UsageRule)]
        public IEnumerable<string> UsageRules { get; set; }
    }
}