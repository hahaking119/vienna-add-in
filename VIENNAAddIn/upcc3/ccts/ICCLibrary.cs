using System;
using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts
{
    public interface ICCLibrary : IElementLibrary
    {
        IEnumerable<IACC> ACCs { get; }
    }
}