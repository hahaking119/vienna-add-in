using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts
{
    public class ABIESpec : BIESpec
    {
        public IEnumerable<BBIESpec> BBIEs { get; set; }
        public IABIE IsEquivalentTo { get; set;  }
        public IACC BasedOn { get; set; }
    }
}