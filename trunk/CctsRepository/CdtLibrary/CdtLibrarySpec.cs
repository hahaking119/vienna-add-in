using System.Collections.Generic;

namespace CctsRepository.CdtLibrary
{
    public class CdtLibrarySpec
    {
        public CdtLibrarySpec(ICdtLibrary cdtLibrary)
        {
            Name = cdtLibrary.Name;
            Status = cdtLibrary.Status;
            UniqueIdentifier = cdtLibrary.UniqueIdentifier;
            VersionIdentifier = cdtLibrary.VersionIdentifier;
            BaseURN = cdtLibrary.BaseURN;
            NamespacePrefix = cdtLibrary.NamespacePrefix;
            BusinessTerms = new List<string>(cdtLibrary.BusinessTerms);
            Copyrights = new List<string>(cdtLibrary.Copyrights);
            Owners = new List<string>(cdtLibrary.Owners);
            References = new List<string>(cdtLibrary.References);
        }

        public CdtLibrarySpec()
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