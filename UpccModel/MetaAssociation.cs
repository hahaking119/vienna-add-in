namespace UpccModel
{
    public class MetaAssociation
    {
        public Cardinality AssociatedClassifierCardinality;
        public string AssociatedClassifierRole;
        public MetaClass AssociatedClassifierType;

        public Cardinality AssociatingClassifierCardinality;
        public string AssociatingClassifierRole;
        public MetaClass AssociatingClassifierType;

        public string Stereotype;

        public MetaTaggedValue[] TaggedValues;
    }
}