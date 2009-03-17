using System.Collections.Generic;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    public class ElementBuilder : EAObjectBuilder<ElementBuilder>
    {
        private readonly List<AttributeBuilder> attributes = new List<AttributeBuilder>();
        private readonly List<ConnectorBuilder> connectors = new List<ConnectorBuilder>();

        public ElementBuilder(string name, string stereotype) : base(name, stereotype)
        {
        }

        public ElementBuilder Attributes(params AttributeBuilder[] attributes)
        {
            this.attributes.AddRange(attributes);
            return this;
        }

        public IEnumerable<AttributeBuilder> GetAttributes()
        {
            return attributes;
        }

        public ElementBuilder Connectors(params ConnectorBuilder[] connectors)
        {
            this.connectors.AddRange(connectors);
            return this;
        }

        public IEnumerable<ConnectorBuilder> GetConnectors()
        {
            return connectors;
        }
    }
}