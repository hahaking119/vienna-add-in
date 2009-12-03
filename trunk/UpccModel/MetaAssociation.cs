namespace Upcc
{
    public class MetaAssociation
    {
        public string Stereotype;
        public string ClassName;
        public string Name;
        public Cardinality Cardinality;

        public MetaClass AssociatedClassifierType;
        public MetaClass AssociatingClassifierType;

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