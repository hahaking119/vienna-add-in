namespace Upcc
{
    public abstract class MetaClassifier
    {
        public string Name;
        public string Stereotype;
        public MetaTaggedValue[] TaggedValues;
        public MetaClassifier BaseType;

        public bool HasTaggedValues
        {
            get
            {
                return TaggedValues != null && TaggedValues.Length > 0;
            }
        }
    }
}