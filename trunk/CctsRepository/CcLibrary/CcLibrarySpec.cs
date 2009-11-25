using System.Collections.Generic;

namespace CctsRepository.CcLibrary
{
    public class CcLibrarySpec
    {
        public CcLibrarySpec(ICCLibrary ccLibrary)
        {
            Name = ccLibrary.Name;
            Status = ccLibrary.Status;
            UniqueIdentifier = ccLibrary.UniqueIdentifier;
            VersionIdentifier = ccLibrary.VersionIdentifier;
            BaseURN = ccLibrary.BaseURN;
            NamespacePrefix = ccLibrary.NamespacePrefix;
            BusinessTerms = new List<string>(ccLibrary.BusinessTerms);
            Copyrights = new List<string>(ccLibrary.Copyrights);
            Owners = new List<string>(ccLibrary.Owners);
            References = new List<string>(ccLibrary.References);
        }

        public CcLibrarySpec()
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