using System.Collections.Generic;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal abstract class UpccElement
    {
        private static int nextId;
        private readonly List<UpccTaggedValue> taggedValues = new List<UpccTaggedValue>();

        protected UpccElement(string name)
        {
            Name = name;
            Id = nextId++;
        }

        public int Id { get; private set; }

        public string Name { get; private set; }

        public abstract string Stereotype { get; }

        public List<UpccTaggedValue> TaggedValues
        {
            get { return taggedValues; }
        }
    }
}