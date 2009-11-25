using System;
using System.Collections.Generic;
using CctsRepository.CcLibrary;

namespace CctsRepository.BieLibrary
{
    public class ASBIESpec
    {
        private AsbieAggregationKind aggregationKind = AsbieAggregationKind.Composite;
        private int associatedABIEId;

        public ASBIESpec(IASBIE asbie)
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
            AssociatedABIEId = asbie.AssociatedElement.Id;
            LowerBound = asbie.LowerBound;
            UpperBound = asbie.UpperBound;
            AggregationKind = asbie.AggregationKind;
        }

        public ASBIESpec()
        {
        }

        public string SequencingKey { get; set; }

        public int AssociatedABIEId
        {
            get
            {
                if (ResolveAssociatedABIE != null)
                {
                    return ResolveAssociatedABIE().Id;
                }
                return associatedABIEId;
            }
            set { associatedABIEId = value; }
        }

        /// <summary>
        /// Set a function to resolve the associated ACC.
        /// </summary>
        public Func<IABIE> ResolveAssociatedABIE { get; set; }

        public string LowerBound { get; set; }
        public string UpperBound { get; set; }

        public AsbieAggregationKind AggregationKind
        {
            get { return aggregationKind; }
            set { aggregationKind = value; }
        }

        public IEnumerable<string> UsageRules { get; set; }
        public string Name { get; set; }
        public string DictionaryEntryName { get; set; }
        public string Definition { get; set; }
        public string UniqueIdentifier { get; set; }
        public string VersionIdentifier { get; set; }
        public string LanguageCode { get; set; }
        public IEnumerable<string> BusinessTerms { get; set; }

        public static ASBIESpec CloneASCC(IASCC ascc, string name, int associatedABIEId)
        {
            return new ASBIESpec
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
                       AssociatedABIEId = associatedABIEId,
                       LowerBound = ascc.LowerBound,
                       UpperBound = ascc.UpperBound,
                   };
        }

        public static ASBIESpec CloneASCC(IASCC ascc, string name, Func<IABIE> associatedABIEResolver)
        {
            return new ASBIESpec
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
                       ResolveAssociatedABIE = associatedABIEResolver,
                       LowerBound = ascc.LowerBound,
                       UpperBound = ascc.UpperBound,
                   };
        }
    }
}