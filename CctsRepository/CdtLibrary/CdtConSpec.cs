using System.Collections.Generic;

namespace CctsRepository.CdtLibrary
{
    public class CdtConSpec
    {
        public CdtConSpec(ICdtCon con)
        {
            Name = con.Name;
            DictionaryEntryName = con.DictionaryEntryName;
            Definition = con.Definition;
            UniqueIdentifier = con.UniqueIdentifier;
            VersionIdentifier = con.VersionIdentifier;
            LanguageCode = con.LanguageCode;
            BusinessTerms = new List<string>(con.BusinessTerms);

            BasicType = con.BasicType;

            UpperBound = con.UpperBound;
            LowerBound = con.LowerBound;

            ModificationAllowedIndicator = con.ModificationAllowedIndicator;
            UsageRules = new List<string>(con.UsageRules);
        }

        public CdtConSpec()
        {
        }

        public BasicType BasicType { get; set; }

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