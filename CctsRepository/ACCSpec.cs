using System.Collections.Generic;
using VIENNAAddInUtils;

namespace CctsRepository
{
    public class ACCSpec : CCTSElementSpec
    {
        private readonly List<ASCCSpec> asccs;
        private readonly List<BCCSpec> bccs;

        public ACCSpec(IACC acc) : base(acc)
        {
            UsageRules = new List<string>(acc.UsageRules);
            bccs = new List<BCCSpec>(acc.BCCs.Convert(bcc => new BCCSpec(bcc)));
            asccs = new List<ASCCSpec>(acc.ASCCs.Convert(ascc => new ASCCSpec(ascc)));
            IsEquivalentTo = acc.IsEquivalentTo;
        }

        public ACCSpec()
        {
            bccs = new List<BCCSpec>();
            asccs = new List<ASCCSpec>();
        }

        public IEnumerable<BCCSpec> BCCs
        {
            get { return bccs; }
        }

        public IEnumerable<ASCCSpec> ASCCs
        {
            get { return asccs; }
        }

        public IACC IsEquivalentTo { get; set; }
        public IEnumerable<string> UsageRules { get; set; }

        public void RemoveASCC(string name)
        {
            asccs.RemoveAll(ascc => ascc.Name == name);
        }

        public void RemoveBCC(string name)
        {
            bccs.RemoveAll(bcc => bcc.Name == name);
        }

        public void AddBCC(BCCSpec bcc)
        {
            bccs.Add(bcc);
        }

        public void AddASCC(ASCCSpec ascc)
        {
            asccs.Add(ascc);
        }
    }
}