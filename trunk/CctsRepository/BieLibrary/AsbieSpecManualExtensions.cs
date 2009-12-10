using System;
using System.Collections.Generic;
using CctsRepository.CcLibrary;

namespace CctsRepository.BieLibrary
{
    public partial class AsbieSpec
    {
        public AsbieSpec()
        {
            ResolveAssociatedAbie = () => AssociatedAbie;
        }

        /// <summary>
        /// Set a function to resolve the associated ACC.
        /// </summary>
        public Func<IAbie> ResolveAssociatedAbie { get; set; }

        public static AsbieSpec CloneASCC(IAscc ascc, string name, IAbie associatedAbie)
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

        public static AsbieSpec CloneASCC(IAscc ascc, string name, Func<IAbie> associatedABIEResolver)
        {
            var asbieSpec = CloneASCC(ascc, name, (IAbie) null);
            asbieSpec.ResolveAssociatedAbie = associatedABIEResolver;
            return asbieSpec;
        }
    }
}