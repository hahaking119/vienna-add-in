using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts
{
    public interface ICCRepository
    {
        IBusinessLibrary GetLibrary(int id);
        IEnumerable<IBusinessLibrary> AllLibraries();
        IEnumerable<T> Libraries<T>() where T : IBusinessLibrary;
    }
}