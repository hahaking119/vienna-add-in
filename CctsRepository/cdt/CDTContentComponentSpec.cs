using System.Collections.Generic;

namespace CctsRepository.cdt
{
    public class CDTContentComponentSpec : CCTSElementSpec
    {
        public CDTContentComponentSpec(ICDTContentComponent con): base(con)
        {
            BasicType = con.BasicType;

            UpperBound = con.UpperBound;
            LowerBound = con.LowerBound;

            ModificationAllowedIndicator = con.ModificationAllowedIndicator;
            UsageRules = new List<string>(con.UsageRules);
        }

        public CDTContentComponentSpec()
        {
        }

        public IBasicType BasicType { get; set; }

        public string UpperBound { get; set; }
        public string LowerBound { get; set; }

        public bool ModificationAllowedIndicator { get; set; }
        public IEnumerable<string> UsageRules { get; set; }
    }
}