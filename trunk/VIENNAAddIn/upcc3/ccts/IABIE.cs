using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts
{
    public interface IABIE : IBIE
    {
        IEnumerable<IBBIE> BBIEs { get; }
        IEnumerable<IASBIE> ASBIEs { get; }
        IACC BasedOn { get; }
        IABIE IsEquivalentTo { get; }
    }
}