using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts
{
    public interface IENUMLibrary : IBusinessLibrary
    {
        IEnumerable<IENUM> ENUMs { get; }
    }
}