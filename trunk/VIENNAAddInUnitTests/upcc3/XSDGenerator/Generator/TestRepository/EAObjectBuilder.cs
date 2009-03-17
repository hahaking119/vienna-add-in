using System.Collections.Generic;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    public class EAObjectBuilder<T> where T : class
    {
        private readonly string name;
        private readonly string stereotype;
        private readonly List<TaggedValueBuilder> taggedValues = new List<TaggedValueBuilder>();

        public EAObjectBuilder(string name, string stereotype)
        {
            this.name = name;
            this.stereotype = stereotype;
        }

        public string GetName()
        {
            return name;
        }

        public string GetStereotype()
        {
            return stereotype;
        }

        public T TaggedValues(params TaggedValueBuilder[] taggedValues)
        {
            this.taggedValues.AddRange(taggedValues);
            return this as T;
        }

        public IEnumerable<TaggedValueBuilder> GetTaggedValues()
        {
            return taggedValues;
        }
    }
}