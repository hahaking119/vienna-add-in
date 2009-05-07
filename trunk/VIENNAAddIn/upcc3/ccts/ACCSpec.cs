using System.Collections.Generic;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts
{
    ///<summary>
    ///</summary>
    public class ACCSpec : CCSpec
    {
        private List<ASCCSpec> asccs;
        private List<BCCSpec> bccs;

        ///<summary>
        ///</summary>
        ///<param name="acc"></param>
        public ACCSpec(IACC acc) : base(acc)
        {
            BCCs = acc.BCCs.Convert(bcc => new BCCSpec(bcc));
            ASCCs = acc.ASCCs.Convert(ascc => new ASCCSpec(ascc));
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
            set { bccs = new List<BCCSpec>(value); }
        }

        public IEnumerable<ASCCSpec> ASCCs
        {
            get { return asccs; }
            set { asccs = new List<ASCCSpec>(value); }
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
    }
}