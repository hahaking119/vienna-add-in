using System;
using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts
{
    public interface IBIELibrary : IBusinessLibrary
    {
        IList<IBIE> BIEs { get; }
        void EachBIE(Action<IBIE> action);
    }
}