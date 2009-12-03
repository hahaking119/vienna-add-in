namespace Upcc
{
    public class PackageContainmentRelation
    {
        public Cardinality ContainerPackageCardinality;
        public MetaPackage ContainerPackageType;
        public string ContainerPackageRole;

        public Cardinality ContainedPackageCardinality;
        public MetaPackage ContainedPackageType;
        public string ContainedPackageRole;
    }
}