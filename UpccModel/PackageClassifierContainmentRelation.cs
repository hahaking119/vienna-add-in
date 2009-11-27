namespace UpccModel
{
    public class PackageClassifierContainmentRelation
    {
        public MetaPackage PackageType;
        public string PackageRole;

        public Cardinality ClassifierCardinality;
        public MetaClassifier ClassifierType;
        public string ClassifierRole;
    }
}