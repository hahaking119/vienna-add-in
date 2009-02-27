using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts
{
    public interface IENUMLibrary : IElementLibrary
    {
        IEnumerable<IENUM> ENUMs { get; }
    }
}