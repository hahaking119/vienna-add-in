using System;

namespace VIENNAAddIn.upcc3.ccts
{
    public interface ICCRepository
    {
        IBusinessLibrary GetRootLibrary();
        IBusinessLibrary GetLibrary(int id);
        ICDT GetCDT(int id);
        void EachCDT(int cdtLibraryId, Action<ICDT> visit);
        void TraverseLibrariesDepthFirst(Action<IBusinessLibrary> visit);
    }
}