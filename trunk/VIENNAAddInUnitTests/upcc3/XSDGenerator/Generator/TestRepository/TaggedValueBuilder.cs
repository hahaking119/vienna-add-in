namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    public class TaggedValueBuilder
    {
        public TaggedValueBuilder(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; private set; }
        public string Value { get; private set; }
    }
}