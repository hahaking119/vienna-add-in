using System.Collections.Generic;

namespace CctsRepository.CcLibrary
{
    public partial class BccSpec
    {
        public BccSpec(IBcc bcc)
        {
            Name = bcc.Name;
            DictionaryEntryName = bcc.DictionaryEntryName;
            Definition = bcc.Definition;
            UniqueIdentifier = bcc.UniqueIdentifier;
            VersionIdentifier = bcc.VersionIdentifier;
            LanguageCode = bcc.LanguageCode;
            BusinessTerms = new List<string>(bcc.BusinessTerms);
            UsageRules = new List<string>(bcc.UsageRules);
            SequencingKey = bcc.SequencingKey;
            UpperBound = bcc.UpperBound;
            LowerBound = bcc.LowerBound;
            Cdt = bcc.Cdt;
        }

        public BccSpec()
        {
        }
    }
}