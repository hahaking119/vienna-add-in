using System;
using System.Collections.Generic;

namespace CctsRepository.CcLibrary
{
    public partial class AsccSpec
    {
        public AsccSpec(IAscc ascc)
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
            AssociatedAcc = ascc.AssociatedAcc;
            LowerBound = ascc.LowerBound;
            UpperBound = ascc.UpperBound;
            ResolveAssociatedAcc = () => AssociatedAcc;
        }

        public AsccSpec()
        {
            ResolveAssociatedAcc = () => AssociatedAcc;
        }

        /// <summary>
        /// Set a function to resolve the associated ACC.
        /// </summary>
        public Func<IAcc> ResolveAssociatedAcc { get; set; }
    }
}