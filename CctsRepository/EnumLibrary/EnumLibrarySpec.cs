using System.Collections.Generic;

namespace CctsRepository.EnumLibrary
{
    public class EnumLibrarySpec
    {
        public EnumLibrarySpec(IENUMLibrary enumLibrary)
        {
            Name = enumLibrary.Name;
            Status = enumLibrary.Status;
            UniqueIdentifier = enumLibrary.UniqueIdentifier;
            VersionIdentifier = enumLibrary.VersionIdentifier;
            BaseURN = enumLibrary.BaseURN;
            NamespacePrefix = enumLibrary.NamespacePrefix;
            BusinessTerms = new List<string>(enumLibrary.BusinessTerms);
            Copyrights = new List<string>(enumLibrary.Copyrights);
            Owners = new List<string>(enumLibrary.Owners);
            References = new List<string>(enumLibrary.References);
        }

        public EnumLibrarySpec()
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