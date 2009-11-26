using System.Collections.Generic;

namespace CctsRepository.BdtLibrary
{
    public class BdtLibrarySpec
    {
        public BdtLibrarySpec(IBdtLibrary bdtLibrary)
        {
            Name = bdtLibrary.Name;
            Status = bdtLibrary.Status;
            UniqueIdentifier = bdtLibrary.UniqueIdentifier;
            VersionIdentifier = bdtLibrary.VersionIdentifier;
            BaseURN = bdtLibrary.BaseURN;
            NamespacePrefix = bdtLibrary.NamespacePrefix;
            BusinessTerms = new List<string>(bdtLibrary.BusinessTerms);
            Copyrights = new List<string>(bdtLibrary.Copyrights);
            Owners = new List<string>(bdtLibrary.Owners);
            References = new List<string>(bdtLibrary.References);
        }

        public BdtLibrarySpec()
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