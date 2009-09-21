using System;
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

        [TaggedValue]
        public string AgencyIdentifier { get; set; }

        [TaggedValue]
        public string AgencyName { get; set; }

        [TaggedValue]
        public string EnumerationURI { get; set; }

        [Dependency]
        public IENUM IsEquivalentTo { get; set; }

        public IDictionary<string, string> Values { get; set; }

        public override IEnumerable<AttributeSpec> GetAttributes()
        {
            if (Values != null)
            {
                foreach (var value in Values)
                {
                    var attributeSpec = new AttributeSpec("", value.Key, "String", 0, "1", "1", new TaggedValueSpec[0])
                                        {DefaultValue = value.Value};
                    yield return attributeSpec;
                }
            }

        }
    }
}