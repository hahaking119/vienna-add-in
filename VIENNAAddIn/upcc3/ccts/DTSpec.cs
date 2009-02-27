using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts
{
    public class DTSpec
    {
        public string Name { get; set; }
        public string Definition { get; set; }
        public string DictionaryEntryName { get; set; }
        public string LanguageCode { get; set; }
        public string UniqueIdentifier { get; set; }
        public string VersionIdentifier { get; set; }
        public IEnumerable<string> BusinessTerms { get; set; }
        public IEnumerable<string> UsageRules { get; set; }
        public IEnumerable<SUPSpec> SUPs { get; set; }
        public CONSpec CON { get; set; }
    }
}