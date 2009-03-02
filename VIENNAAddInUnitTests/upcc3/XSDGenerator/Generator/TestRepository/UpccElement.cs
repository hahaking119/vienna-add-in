using System.Collections.Generic;
using VIENNAAddIn.upcc3.ccts;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    public abstract class UpccElement
    {
        public string Name { get; set; }
        private readonly List<UpccTaggedValue> taggedValues = new List<UpccTaggedValue>();

        public abstract string GetStereotype();

        public List<UpccTaggedValue> GetTaggedValues()
        {
            return taggedValues;
        }

        protected void AddTaggedValue(string name, string value)
        {
            taggedValues.Add(new UpccTaggedValue { Name = name, Value = value });
        }

        public void ResolvePath(Path path, UpccElement parent)
        {
        }

    }
}