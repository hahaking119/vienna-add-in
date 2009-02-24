using System;
using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts
{
    public interface ICCLibrary : IBusinessLibrary
    {
        IEnumerable<IACC> ACCs { get; }
    }
}