using System;
using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts
{
    public interface IPRIMLibrary : IBusinessLibrary
    {
        IList<IPRIM> PRIMs { get; }
    }
}