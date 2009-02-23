using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts
{
    public enum BusinessLibraryType
    {
        bLibrary,
        BDTLibrary,
        BIELibrary,
        CCLibrary,
        CDTLibrary,
        DOCLibrary,
        ENUMLibrary,
        PRIMLibrary
    }

    public interface IBusinessLibrary
    {
        int Id { get; }
        BusinessLibraryType Type { get; }
        string Name { get; }
        IBusinessLibrary Parent { get; }
        IList<IBusinessLibrary> Children { get; }
        string Status { get; }
        string UniqueIdentifier { get; }
        string VersionIdentifier { get; }
        string BaseURN { get; }
        string NamespacePrefix { get; }
        IList<string> BusinessTerms { get; }
        IList<string> Copyrights { get; }
        IList<string> Owners { get; }
        IList<string> References { get; }
        T CreateElement<T>(T spec);
        IList<T> Elements<T>();
        bool IsA(BusinessLibraryType type);
    }
}