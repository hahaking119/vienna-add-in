using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts
{
    public interface IBusinessLibrary
    {
        int Id { get; }
        string Name { get; }
        IBusinessLibrary Parent { get; }
        /// <summary>
        /// Returns the direct sub-libraries of this library.
        /// </summary>
        IEnumerable<IBusinessLibrary> Children { get; }
        /// <summary>
        /// Returns all of this library's sub-libraries and their sub-libraries (depth-first).
        /// </summary>
        IEnumerable<IBusinessLibrary> AllChildren { get; }
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
}