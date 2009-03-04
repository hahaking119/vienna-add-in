namespace VIENNAAddIn.upcc3.ccts
{
    public class TaggedValueSpec
    {
        public TaggedValueSpec(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; private set; }
        public string Value { get; private set; }
    }
}