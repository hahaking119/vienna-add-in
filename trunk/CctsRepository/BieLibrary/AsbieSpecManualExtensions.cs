using System;
using System.Collections.Generic;
using CctsRepository.CcLibrary;

namespace CctsRepository.BieLibrary
{
    public partial class AsbieSpec
    {
        public static AsbieSpec CloneAscc(IAscc ascc, string name, IAbie associatedAbie)
        {
            return new AsbieSpec
                   {
                       BusinessTerms = new List<string>(ascc.BusinessTerms),
                       Definition = ascc.Definition,
                       DictionaryEntryName = ascc.DictionaryEntryName,
                       LanguageCode = ascc.LanguageCode,
                       SequencingKey = ascc.SequencingKey,
                       UniqueIdentifier = ascc.UniqueIdentifier,
                       UsageRules = new List<string>(ascc.UsageRules),
                       VersionIdentifier = ascc.VersionIdentifier,
                       Name = name,
                       AssociatedAbie = associatedAbie,
                       LowerBound = ascc.LowerBound,
                       UpperBound = ascc.UpperBound,
                   };
        }
    }
}