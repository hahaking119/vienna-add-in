using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts
{
    public class ConnectorSpec
    {
        public ConnectorSpec(string connectorType, string stereotype, string name, int supplierId,
                             AggregationKind aggregationKind, string lowerBound, string upperBound)
        {
            ConnectorType = connectorType;
            Stereotype = stereotype;
            Name = name;
            SupplierId = supplierId;
            AggregationKind = aggregationKind;
            LowerBound = lowerBound;
            UpperBound = upperBound;
        }

        public string ConnectorType { get; set; }
        public string Stereotype { get; set; }
        public string Name { get; set; }
        public int SupplierId { get; set; }
        public AggregationKind AggregationKind { get; set; }
        public string LowerBound { get; set; }
        public string UpperBound { get; set; }

        public static ConnectorSpec CreateDependency(string stereotype, int supplierId, string lowerBound,
                                                     string upperBound)
        {
            return new ConnectorSpec(ConnectorTypes.Dependency, stereotype, "", supplierId, AggregationKind.None,
                                     lowerBound, upperBound);
        }

        public static ConnectorSpec CreateAggregation(AggregationKind aggregationKind, string stereotype, string name,
                                                      int supplierId, string lowerBound, string upperBound)
        {
            return new ConnectorSpec(ConnectorTypes.Aggregation, stereotype, name, supplierId, aggregationKind,
                                     lowerBound, upperBound);
        }
    }
}