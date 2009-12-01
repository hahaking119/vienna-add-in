namespace UpccModel
{
    public class MetaEnumerationLiteral
    {
        public string Stereotype;

        public MetaEnumeration ContainingEnumerationType;

        public Cardinality Cardinality;
        public string Name;

        public MetaTaggedValue[] TaggedValues;

        public bool HasTaggedValues
        {
            get
            {
                return TaggedValues != null && TaggedValues.Length > 0;
            }
        }
    }
}