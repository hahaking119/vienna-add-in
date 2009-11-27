namespace UpccModel
{
    public class MetaDataType : MetaClassifier
    {
        public MetaAssociation[] Associations;
        public MetaAttribute[] Attributes;
        public bool HasIsEquivalentTo;
        public MetaTaggedValue[] TaggedValues;
    }
}