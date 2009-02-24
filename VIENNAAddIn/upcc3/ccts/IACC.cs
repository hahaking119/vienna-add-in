using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts
{
    public interface IACC : ICC
    {
        IEnumerable<IBCC> BCCs { get; }
        IEnumerable<IASCC> ASCCs { get; }
        IACC IsEquivalentTo { get; }
        IBusinessLibrary Library { get; }
    }
}