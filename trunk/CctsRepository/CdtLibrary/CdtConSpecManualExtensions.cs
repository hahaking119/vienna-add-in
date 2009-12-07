using System.Collections.Generic;

namespace CctsRepository.CdtLibrary
{
    public partial class CdtConSpec
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
    }
}