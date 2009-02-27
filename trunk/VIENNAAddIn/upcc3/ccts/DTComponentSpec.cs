using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts
{
    public class DTComponentSpec
    {
        public IType Type { get; set; }
        public string Definition { get; set; }
        public string DictionaryEntryName { get; set; }
        public string LanguageCode { get; set; }
        public string UniqueIdentifier { get; set; }
        public string VersionIdentifier { get; set; }
        public bool ModificationAllowedIndicator { get; set; }
        public IEnumerable<string> BusinessTerms { get; set; }
        public IEnumerable<string> UsageRules { get; set; }
        public string UpperBound { get; set; }
        public string LowerBound { get; set; }
    }
}