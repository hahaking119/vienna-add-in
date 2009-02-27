using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts
{
    public interface ICDTLibrary : IElementLibrary
    {
        IEnumerable<ICDT> CDTs { get; }
        ICDT CdtByName(string name);
    }
}