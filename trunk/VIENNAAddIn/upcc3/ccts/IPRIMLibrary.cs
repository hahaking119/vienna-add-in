using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts
{
    public interface IPRIMLibrary : IElementLibrary
    {
        IEnumerable<IPRIM> PRIMs { get; }
    }
}