using System;
using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts
{
    public interface IBDTLibrary : IBusinessLibrary
    {
        IList<IBDT> BDTs { get; }
        void EachBDT(Action<IBDT> action);
    }
}