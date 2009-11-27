namespace UpccModel
{
    public class MetaClass : MetaClassifier
    {
        public MetaAssociation[] Associations;
        public MetaAttribute[] Attributes;
        public bool HasIsEquivalentTo;
        public MetaTaggedValue[] TaggedValues;
    }
}