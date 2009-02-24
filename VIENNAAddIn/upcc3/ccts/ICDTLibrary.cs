using System;
using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts
{
    public interface ICDTLibrary : IBusinessLibrary
    {
        IList<ICDT> CDTs { get; }
    }
}