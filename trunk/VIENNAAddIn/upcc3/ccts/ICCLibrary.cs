using System;
using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts
{
    public interface ICCLibrary : IBusinessLibrary
    {
        IList<ICC> CCs { get; }
        void EachCC(Action<ICC> action);
    }
}