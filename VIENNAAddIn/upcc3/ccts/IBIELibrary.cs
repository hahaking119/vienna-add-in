using System;
using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts
{
    public interface IBIELibrary : IBusinessLibrary
    {
        IEnumerable<IBIE> BIEs { get; }
    }
}