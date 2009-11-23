using System.Collections.Generic;

namespace CctsRepository
{
    public class ConnectorSpec
    {
        public ConnectorSpec(EAConnectorTypes connectorType, string stereotype, string name, int supplierId, EAAggregationKind aggregationKind, string lowerBound, string upperBound, IEnumerable<TaggedValueSpec> taggedValues)
        {
            ConnectorType = connectorType;
            Stereotype = stereotype;
            Name = name;
            SupplierId = supplierId;
            AggregationKind = aggregationKind;
            LowerBound = lowerBound;
            UpperBound = upperBound;
            TaggedValueSpecs = taggedValues;
        }

        public EAConnectorTypes ConnectorType { get; set; }
        public string Stereotype { get; set; }
        public string Name { get; set; }
        public int SupplierId { get; set; }
        public EAAggregationKind AggregationKind { get; set; }
        public string LowerBound { get; set; }
        public string UpperBound { get; set; }
        public IEnumerable<TaggedValueSpec> TaggedValueSpecs { get; set; }

        public static ConnectorSpec CreateDependency(string stereotype, int supplierId, string lowerBound,
                                                     string upperBound)
        {
            return new ConnectorSpec(EAConnectorTypes.Dependency, stereotype, "", supplierId, EAAggregationKind.None,
                                     lowerBound, upperBound, new TaggedValueSpec[0]);
        }

        public static ConnectorSpec CreateAggregation(EAAggregationKind aggregationKind, string stereotype, string name, int supplierId, string lowerBound, string upperBound, IEnumerable<TaggedValueSpec> taggedValues)
        {
            return new ConnectorSpec(EAConnectorTypes.Aggregation, stereotype, name, supplierId, aggregationKind,
                                     lowerBound, upperBound, taggedValues);
        }
    }
}