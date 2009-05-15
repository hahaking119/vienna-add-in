using System.Collections.Generic;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts
{
    public class ASBIESpec : BIESpec
    {
        [TaggedValue(TaggedValues.SequencingKey)]
        public string SequencingKey { get; set; }

        public int AssociatedABIEId { get; set; }

        public string LowerBound { get; set; }
        public string UpperBound { get; set; }

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