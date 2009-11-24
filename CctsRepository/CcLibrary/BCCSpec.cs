using System.Collections.Generic;
using CctsRepository.CdtLibrary;

namespace CctsRepository.CcLibrary
{
    public class BCCSpec : CCTSElementSpec
    {
        public BCCSpec(IBCC bcc) : base(bcc)
        {
            UsageRules = new List<string>(bcc.UsageRules);
            SequencingKey = bcc.SequencingKey;
            UpperBound = bcc.UpperBound;
            LowerBound = bcc.LowerBound;
            Type = bcc.Type;
        }

        public BCCSpec()
        {
        }

        public string SequencingKey { get; set; }

        public string UpperBound { get; set; }
        public string LowerBound { get; set; }

        public ICDT Type { get; set; }
        public IEnumerable<string> UsageRules { get; set; }
    }
}