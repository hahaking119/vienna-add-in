using System.Collections.Generic;

namespace UPCCRepositoryInterface
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

        [TaggedValue]
        public IEnumerable<string> UsageRules { get; set; }
    }
}