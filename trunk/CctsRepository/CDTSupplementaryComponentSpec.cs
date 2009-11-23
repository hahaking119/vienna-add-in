using System.Collections.Generic;

namespace CctsRepository
{
    public class CDTSupplementaryComponentSpec : CCTSElementSpec
    {
        public CDTSupplementaryComponentSpec(ICDTSupplementaryComponent sup) : base(sup)
        {
            BasicType = sup.BasicType;

            UpperBound = sup.UpperBound;
            LowerBound = sup.LowerBound;

            ModificationAllowedIndicator = sup.ModificationAllowedIndicator;
            UsageRules = new List<string>(sup.UsageRules);
        }

        public CDTSupplementaryComponentSpec()
        {
        }

        public IBasicType BasicType { get; set; }

        public string UpperBound { get; set; }
        public string LowerBound { get; set; }

        public bool ModificationAllowedIndicator { get; set; }
        public IEnumerable<string> UsageRules { get; set; }
    }
}