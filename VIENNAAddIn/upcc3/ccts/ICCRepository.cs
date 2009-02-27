using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts
{
    public interface ICCRepository
    {
        IBusinessLibrary GetLibrary(int id);
        IEnumerable<IBusinessLibrary> AllLibraries();
        IEnumerable<T> Libraries<T>() where T : IBusinessLibrary;
        T LibraryByName<T>(string name) where T : IBusinessLibrary;
        T FindByPath<T>(Path path) where T : class;
    }
}