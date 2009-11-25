using System.Collections.Generic;

namespace CctsRepository.PrimLibrary
{
    public class PrimLibrarySpec
    {
        public PrimLibrarySpec(IPRIMLibrary primLibrary)
        {
            Name = primLibrary.Name;
            Status = primLibrary.Status;
            UniqueIdentifier = primLibrary.UniqueIdentifier;
            VersionIdentifier = primLibrary.VersionIdentifier;
            BaseURN = primLibrary.BaseURN;
            NamespacePrefix = primLibrary.NamespacePrefix;
            BusinessTerms = new List<string>(primLibrary.BusinessTerms);
            Copyrights = new List<string>(primLibrary.Copyrights);
            Owners = new List<string>(primLibrary.Owners);
            References = new List<string>(primLibrary.References);
        }

        public PrimLibrarySpec()
        {
        }

        public string Name { get; set; }

        public string Status { get; set; }
        public string UniqueIdentifier { get; set; }
        public string VersionIdentifier { get; set; }
        public string BaseURN { get; set; }
        public string NamespacePrefix { get; set; }
        public IEnumerable<string> BusinessTerms { get; set; }
        public IEnumerable<string> Copyrights { get; set; }
        public IEnumerable<string> Owners { get; set; }
        public IEnumerable<string> References { get; set; }
    }
}