using System.Collections.Generic;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal abstract class UpccClass : UpccElement
    {
        protected UpccClass(string name) : base(name)
        {
            Connectors = new List<UpccConnector>();
            Attributes = new List<UpccAttribute>();
        }

        public List<UpccAttribute> Attributes { get; private set; }

        public List<UpccConnector> Connectors { get; private set; }

        protected void AddAttributes<T>(List<T> attributes) where T:UpccAttribute
        {
            Attributes.AddRange(attributes.ConvertAll(a => (UpccAttribute) a));
        }
    }
}