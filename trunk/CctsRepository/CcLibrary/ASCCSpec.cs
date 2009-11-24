using System;
using System.Collections.Generic;

namespace CctsRepository.CcLibrary
{
    public class ASCCSpec
    {
        public ASCCSpec(IASCC ascc)
        {
            Name = ascc.Name;
            DictionaryEntryName = ascc.DictionaryEntryName;
            Definition = ascc.Definition;
            UniqueIdentifier = ascc.UniqueIdentifier;
            VersionIdentifier = ascc.VersionIdentifier;
            LanguageCode = ascc.LanguageCode;
            BusinessTerms = new List<string>(ascc.BusinessTerms);
            UsageRules = new List<string>(ascc.UsageRules);
            SequencingKey = ascc.SequencingKey;
            ResolveAssociatedACC = () => ascc.AssociatedElement;
            LowerBound = ascc.LowerBound;
            UpperBound = ascc.UpperBound;
        }

        public ASCCSpec()
        {
        }

        public string SequencingKey { get; set; }

        public string LowerBound { get; set; }

        public string UpperBound { get; set; }

        public IACC AssociatedACC
        {
            get { return ResolveAssociatedACC(); }
        }

        /// <summary>
        /// Set a function to resolve the associated ACC.
        /// </summary>
        public Func<IACC> ResolveAssociatedACC { get; set; }

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