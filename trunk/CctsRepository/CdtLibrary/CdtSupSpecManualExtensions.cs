using System.Collections.Generic;

namespace CctsRepository.CdtLibrary
{
    public partial class CdtSupSpec
    {
        public CdtSupSpec(ICdtSup sup)
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

        public CdtSupSpec()
        {
        }
    }
}