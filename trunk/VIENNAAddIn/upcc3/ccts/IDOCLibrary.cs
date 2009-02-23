using System;
using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts
{
    public interface IDOCLibrary : IBusinessLibrary
    {
        IList<IBIE> BIEs { get; }
        void EachBIE(Action<IBIE> action);
    }
}