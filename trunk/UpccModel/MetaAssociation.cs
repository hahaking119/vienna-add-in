namespace Upcc
{
    public class MetaAssociation
    {
        public MetaStereotype Stereotype { get; internal set; }
        public string ClassName { get; internal set; }
        public string Name { get; internal set; }
        public MetaCardinality Cardinality { get; internal set; }

        public MetaClass AssociatedClassifierType { get; internal set; }
        public MetaClass AssociatingClassifierType { get; internal set; }

        public MetaTaggedValue[] TaggedValues { get; internal set; }

        public bool HasTaggedValues
        {
            get
            {
                return TaggedValues != null && TaggedValues.Length > 0;
            }
        }
    }
}