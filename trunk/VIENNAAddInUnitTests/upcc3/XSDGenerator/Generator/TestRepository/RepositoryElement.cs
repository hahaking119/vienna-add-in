using System.Collections.Generic;
using EA;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal abstract class RepositoryElement
    {
        private static int nextId;
        protected readonly List<EATaggedValue> taggedValues = new List<EATaggedValue>();

        protected RepositoryElement(string name)
        {
            Name = name;
            Id = nextId++;
        }

        public int Id { get; private set; }

        public string Name { get; private set; }

        public abstract string Stereotype { get; }

        public virtual Collection TaggedValues
        {
            get { return new EACollection<EATaggedValue>(taggedValues); }
        }
    }
}