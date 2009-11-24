using System;
using System.Collections.Generic;

namespace CctsRepository.cc
{
    public class ASCCSpec : CCTSElementSpec
    {
        public ASCCSpec(IASCC ascc) : base(ascc)
        {
            UsageRules = new List<string>(ascc.UsageRules);
            SequencingKey = ascc.SequencingKey;
            ResolveAssociatedACC = () => ascc.AssociatedElement;
            LowerBound = ascc.LowerBound;
            UpperBound = ascc.UpperBound;
        }

        public ASCCSpec()
        {
        }

        public string SequencingKey { get; set; }

        public string LowerBound { get; set; }

        public string UpperBound { get; set; }

        public IACC AssociatedACC
        {
            get { return ResolveAssociatedACC(); }
        }

        /// <summary>
        /// Set a function to resolve the associated ACC.
        /// </summary>
        public Func<IACC> ResolveAssociatedACC { get; set; }

        public IEnumerable<string> UsageRules { get; set; }
    }
}