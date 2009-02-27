using System.Collections.Generic;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal abstract class UpccClass : UpccElement
    {
        private readonly List<UpccAttribute> attributes = new List<UpccAttribute>();

        private readonly List<UpccConnector> connectors = new List<UpccConnector>();

        protected UpccClass(string name) : base(name)
        {
        }

        public List<UpccConnector> GetConnectors()
        {
            return connectors;
        }

        protected void AddConnector(UpccConnector connector)
        {
            connectors.Add(connector);
        }

        public List<UpccAttribute> GetAttributes()
        {
            return attributes;
        }

        protected void AddAttribute(UpccAttribute attribute)
        {
            attributes.Add(attribute);
        }

        protected void AddAttributes<T>(List<T> attributes) where T : UpccAttribute
        {
            this.attributes.AddRange(attributes.ConvertAll(a => (UpccAttribute) a));
        }
    }
}