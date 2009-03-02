using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts
{
    public interface IBDTLibrary : IElementLibrary
    {
        IEnumerable<IBDT> BDTs { get; }
        IBDT CreateBDT(BDTSpec spec);
    }
}