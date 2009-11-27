namespace UpccModel
{
    public class MetaTaggedValue
    {
        public string Name { get; set; }
        public Cardinality Cardinality;
        public string Type;
        public string DefaultValue;

        public MetaTaggedValue(string name)
        {
            Name = name;
            Cardinality = Cardinality.One;
            Type = "string";
            DefaultValue = null;
        }

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