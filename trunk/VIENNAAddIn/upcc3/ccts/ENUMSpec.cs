using System.Collections.Generic;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts
{
    public class ENUMSpec : CCTSElementSpec
    {
        public ENUMSpec(IENUM @enum) : base(@enum)
        {
            AgencyIdentifier = @enum.AgencyIdentifier;
            AgencyName = @enum.AgencyName;
            EnumerationURI = @enum.EnumerationURI;
            IsEquivalentTo = @enum.IsEquivalentTo;
            Values = new Dictionary<string, string>(@enum.Values);
        }

        public ENUMSpec()
        {
        }

        [TaggedValue(TaggedValues.AgencyIdentifier)]
        public string AgencyIdentifier { get; set; }

        [TaggedValue(TaggedValues.AgencyName)]
        public string AgencyName { get; set; }

        [TaggedValue(TaggedValues.EnumerationURI)]
        public string EnumerationURI { get; set; }

        public IENUM IsEquivalentTo { get; set; }

        public IDictionary<string, string> Values { get; set; }
    }
}