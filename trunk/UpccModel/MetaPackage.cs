namespace Upcc
{
    public class MetaPackage
    {
        public string Name;
        public string Stereotype;
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