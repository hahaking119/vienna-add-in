using System.Collections.Generic;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal abstract class UpccElement
    {
        private static int nextId;
        private readonly int id;
        private readonly string name;
        private readonly List<UpccTaggedValue> taggedValues = new List<UpccTaggedValue>();

        protected UpccElement(string name)
        {
            this.name = name;
            id = nextId++;
        }

        public int GetId()
        {
            return id;
        }

        public abstract string GetStereotype();

        public string GetName()
        {
            return name;
        }

        public List<UpccTaggedValue> GetTaggedValues()
        {
            return taggedValues;
        }
    }
}