using System.Collections.Generic;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts
{
    public class LibrarySpec : CCTSSpec
    {
        public string Name { get; set; }

        [TaggedValue(TaggedValues.Status)]
        public string Status { get; set; }

        [TaggedValue(TaggedValues.UniqueIdentifier)]
        public string UniqueIdentifier { get; set; }

        [TaggedValue(TaggedValues.VersionIdentifier)]
        public string VersionIdentifier { get; set; }

        [TaggedValue(TaggedValues.BaseURN)]
        public string BaseURN { get; set; }

        [TaggedValue(TaggedValues.NamespacePrefix)]
        public string NamespacePrefix { get; set; }

        [TaggedValue(TaggedValues.BusinessTerm)]
        public IEnumerable<string> BusinessTerms { get; set; }

        [TaggedValue(TaggedValues.Copyright)]
        public IEnumerable<string> Copyrights { get; set; }

        [TaggedValue(TaggedValues.Owner)]
        public IEnumerable<string> Owners { get; set; }

        [TaggedValue(TaggedValues.Reference)]
        public IEnumerable<string> References { get; set; }
    }
}