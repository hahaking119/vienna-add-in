using System;
using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts
{
    public interface IDOCLibrary : IElementLibrary
    {
        IEnumerable<IBIE> BIEs { get; }
    }
}