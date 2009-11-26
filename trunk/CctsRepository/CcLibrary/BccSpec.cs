using System.Collections.Generic;
using CctsRepository.CdtLibrary;

namespace CctsRepository.CcLibrary
{
    public class BccSpec
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
            Type = bcc.Cdt;
        }

        public BccSpec()
        {
        }

        public string SequencingKey { get; set; }

        public string UpperBound { get; set; }
        public string LowerBound { get; set; }

        public ICDT Type { get; set; }
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