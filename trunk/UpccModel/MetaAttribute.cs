namespace Upcc
{
    public class MetaAttribute
    {
        public string Stereotype;

        public MetaClass ContainingClassifierType;

        public Cardinality Cardinality;
        public string ClassName;
        public string AttributeName;
        public MetaClassifier Type;

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