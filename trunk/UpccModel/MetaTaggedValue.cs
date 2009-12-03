namespace Upcc
{
    public class MetaTaggedValue
    {
        public MetaTaggedValue(string name)
        {
            Name = name;
            Cardinality = MetaCardinality.One;
            Type = "string";
            DefaultValue = null;
        }

        public string Name { get; private set; }
        public MetaCardinality Cardinality { get; internal set; }
        public string Type { get; internal set; }
        public string DefaultValue { get; internal set; }

        public MetaTaggedValue WithDefaultValue(string defaultValue)
        {
            return new MetaTaggedValue(Name)
                   {
                       Cardinality = Cardinality,
                       Type = Type,
                       DefaultValue = defaultValue,
                   };
        }
    }
}