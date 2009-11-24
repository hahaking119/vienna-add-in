using System.Collections.Generic;

namespace CctsRepository.CdtLibrary
{
    public class CDTSupplementaryComponentSpec
    {
        public CDTSupplementaryComponentSpec(ICDTSupplementaryComponent sup)
        {
            Name = sup.Name;
            DictionaryEntryName = sup.DictionaryEntryName;
            Definition = sup.Definition;
            UniqueIdentifier = sup.UniqueIdentifier;
            VersionIdentifier = sup.VersionIdentifier;
            LanguageCode = sup.LanguageCode;
            BusinessTerms = new List<string>(sup.BusinessTerms);

            BasicType = sup.BasicType;

            UpperBound = sup.UpperBound;
            LowerBound = sup.LowerBound;

            ModificationAllowedIndicator = sup.ModificationAllowedIndicator;
            UsageRules = new List<string>(sup.UsageRules);
        }

        public CDTSupplementaryComponentSpec()
        {
        }

        public IBasicType BasicType { get; set; }

        public string UpperBound { get; set; }
        public string LowerBound { get; set; }

        public bool ModificationAllowedIndicator { get; set; }
        public IEnumerable<string> UsageRules { get; set; }
        public string Name { get; set; }
        public string DictionaryEntryName { get; set; }
        public string Definition { get; set; }
        public string UniqueIdentifier { get; set; }
        public string VersionIdentifier { get; set; }
        public string LanguageCode { get; set; }
        public IEnumerable<string> BusinessTerms { get; set; }
    }
}