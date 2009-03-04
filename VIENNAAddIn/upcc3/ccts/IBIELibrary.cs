using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts
{
    public interface IBIELibrary : IElementLibrary
    {
        IEnumerable<IBIE> BIEs { get; }
        IABIE CreateABIE(ABIESpec spec);
    }
}