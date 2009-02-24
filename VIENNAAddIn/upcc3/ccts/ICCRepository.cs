using System;
using System.Collections;
using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts
{
    public interface ICCRepository
    {
        IBusinessLibrary GetRootLibrary();
        IBusinessLibrary GetLibrary(int id);
        IEnumerable<IBusinessLibrary> Libraries { get; }
    }
}