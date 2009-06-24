using System.Collections.Generic;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts
{
    public class ASBIESpec : BIESpec
    {
        public ASBIESpec(IASBIE asbie) : base(asbie)
        {
            SequencingKey = asbie.SequencingKey;
            AssociatedABIEId = asbie.AssociatedElement.Id;
            LowerBound = asbie.LowerBound;
            UpperBound = asbie.UpperBound;
            AggregationKind = asbie.AggregationKind;
        }

        public ASBIESpec()
        {
        }

        [TaggedValue]
        public string SequencingKey { get; set; }

        public int AssociatedABIEId { get; set; }

        public string LowerBound { get; set; }
        public string UpperBound { get; set; }

        private EAAggregationKind aggregationKind = EAAggregationKind.Composite;
        public EAAggregationKind AggregationKind
        {
            get { return aggregationKind; }
            set { aggregationKind = value; }
        }

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
    }
}