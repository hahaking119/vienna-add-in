using System;
using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts
{
    public interface IBDTLibrary : IBusinessLibrary
    {
        IEnumerable<IBDT> BDTs { get; }
    }
}