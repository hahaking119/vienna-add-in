using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts
{
    public interface IPRIMLibrary : IBusinessLibrary
    {
        IEnumerable<IPRIM> PRIMs { get; }
    }
}