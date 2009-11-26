using System.Collections.Generic;

namespace CctsRepository.BieLibrary
{
    public class BieLibrarySpec
    {
        public BieLibrarySpec(IBieLibrary bieLibrary)
        {
            Name = bieLibrary.Name;
            Status = bieLibrary.Status;
            UniqueIdentifier = bieLibrary.UniqueIdentifier;
            VersionIdentifier = bieLibrary.VersionIdentifier;
            BaseURN = bieLibrary.BaseURN;
            NamespacePrefix = bieLibrary.NamespacePrefix;
            BusinessTerms = new List<string>(bieLibrary.BusinessTerms);
            Copyrights = new List<string>(bieLibrary.Copyrights);
            Owners = new List<string>(bieLibrary.Owners);
            References = new List<string>(bieLibrary.References);
        }

        public BieLibrarySpec()
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