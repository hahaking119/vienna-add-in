using System.Collections.Generic;

namespace CctsRepository.BLibrary
{
    public class BLibrarySpec
    {
        public BLibrarySpec(IBLibrary bLibrary)
        {
            Name = bLibrary.Name;
            Status = bLibrary.Status;
            UniqueIdentifier = bLibrary.UniqueIdentifier;
            VersionIdentifier = bLibrary.VersionIdentifier;
            BusinessTerms = new List<string>(bLibrary.BusinessTerms);
            Copyrights = new List<string>(bLibrary.Copyrights);
            Owners = new List<string>(bLibrary.Owners);
            References = new List<string>(bLibrary.References);
        }

        public BLibrarySpec()
        {
        }

        public string Name { get; set; }

        public string Status { get; set; }
        public string UniqueIdentifier { get; set; }
        public string VersionIdentifier { get; set; }
        public IEnumerable<string> BusinessTerms { get; set; }
        public IEnumerable<string> Copyrights { get; set; }
        public IEnumerable<string> Owners { get; set; }
        public IEnumerable<string> References { get; set; }
    }
}