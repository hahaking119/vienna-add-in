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
        public bool AssociatedClassifierUseBaseType;

        public string Stereotype;

        public MetaTaggedValue[] TaggedValues;

        public MetaAssociation()
        {
            AssociatedClassifierUseBaseType = false;
        }

        public bool HasTaggedValues
        {
            get
            {
                return TaggedValues != null && TaggedValues.Length > 0;
            }
        }
    }
}