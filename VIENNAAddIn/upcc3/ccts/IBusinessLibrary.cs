using System.Collections.Generic;
using VIENNAAddIn.upcc3.ccts.dra;

namespace VIENNAAddIn.upcc3.ccts
{
    public interface IBusinessLibrary
    {
        int Id { get; }
        string Name { get; }
        IBusinessLibrary Parent { get; }
        string Status { get; }
        string UniqueIdentifier { get; }
        string VersionIdentifier { get; }
        string BaseURN { get; }
        string NamespacePrefix { get; }
        IEnumerable<string> BusinessTerms { get; }
        IEnumerable<string> Copyrights { get; }
        IEnumerable<string> Owners { get; }
        IEnumerable<string> References { get; }
    }

    public interface IElementLibrary : IBusinessLibrary
    {
        ICCTSElement ElementByName(string name);
    }

    public interface IBLibrary : IBusinessLibrary
    {
        /// <summary>
        /// Returns the direct sub-libraries of this library.
        /// </summary>
        IEnumerable<IBusinessLibrary> Children { get; }
        /// <summary>
        /// Returns all of this library's sub-libraries and their sub-libraries (depth-first).
        /// </summary>
        IEnumerable<IBusinessLibrary> AllChildren { get; }

        IBusinessLibrary FindChildByName(string name);

        IBDTLibrary CreateBDTLibrary(LibrarySpec spec);
    }
}