using System.Collections.Generic;
using VIENNAAddInUtils;

namespace CctsRepository
{
    ///<summary>
    ///</summary>
    public class ACCSpec : CCSpec
    {
        private readonly List<ASCCSpec> asccs;
        private readonly List<BCCSpec> bccs;

        ///<summary>
        ///</summary>
        ///<param name="acc"></param>
        public ACCSpec(IACC acc) : base(acc)
        {
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

        ///<summary>
        ///</summary>
        public IACC IsEquivalentTo { get; set; }

        ///<summary>
        ///</summary>
        ///<param name="name"></param>
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