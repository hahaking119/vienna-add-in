using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts
{
    public class LibrarySpec
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public string UniqueIdentifier { get; set; }
        public string VersionIdentifier { get; set; }
        public string BaseURN { get; set;  }
        public string NamespacePrefix { get; set;  }
        public IEnumerable<string> BusinessTerms { get; set; }
        public IEnumerable<string> Copyrights { get; set;  }
        public IEnumerable<string> Owners { get; set; }
        public IEnumerable<string> References { get; set; }
    }
}