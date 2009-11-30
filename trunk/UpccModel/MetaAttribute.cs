namespace UpccModel
{
    public class MetaAttribute
    {
        public string Stereotype;

        public MetaClass ContainingClassifierType;

        public Cardinality Cardinality;
        public string Name;
        public MetaClass Type;

        public MetaTaggedValue[] TaggedValues;
    }
}