using System;
using System.Collections.Generic;
using CctsRepository.CcLibrary;

namespace CctsRepository.BieLibrary
{
    public partial class AsbieSpec
    {
        public AsbieSpec(IAsbie asbie)
        {
            Name = asbie.Name;
            DictionaryEntryName = asbie.DictionaryEntryName;
            Definition = asbie.Definition;
            UniqueIdentifier = asbie.UniqueIdentifier;
            VersionIdentifier = asbie.VersionIdentifier;
            LanguageCode = asbie.LanguageCode;
            BusinessTerms = new List<string>(asbie.BusinessTerms);
            UsageRules = new List<string>(asbie.UsageRules);
            SequencingKey = asbie.SequencingKey;
            AssociatedAbie = asbie.AssociatedAbie;
            LowerBound = asbie.LowerBound;
            UpperBound = asbie.UpperBound;
            AggregationKind = asbie.AggregationKind;
            ResolveAssociatedAbie = () => AssociatedAbie;
        }

        public AsbieSpec()
        {
            AggregationKind = AggregationKind.Composite;
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
                       ResolveAssociatedAbie = associatedABIEResolver,
                       LowerBound = ascc.LowerBound,
                       UpperBound = ascc.UpperBound,
                   };
        }
    }
}