using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts
{
    public interface IABIE : IBIE
    {
        IList<IBBIE> BBIEs { get; }
        IList<IASBIE> ASBIEs { get; }
        IACC BasedOn { get; }
        IABIE IsEquivalentTo { get; }
    }
}