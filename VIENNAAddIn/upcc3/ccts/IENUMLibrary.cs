using System;
using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts
{
    public interface IENUMLibrary : IBusinessLibrary
    {
        IList<IENUM> ENUMs { get; }
        void EachENUM(Action<IENUM> action);
    }
}