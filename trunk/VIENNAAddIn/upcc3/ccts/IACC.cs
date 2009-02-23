using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts
{
    public interface IACC : ICC
    {
        IList<IBCC> BCCs { get; }
        IList<IASCC> ASCCs { get; }
        IACC IsEquivalentTo { get; }
    }
}