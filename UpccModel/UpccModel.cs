namespace UpccModel
{
    public class UpccModel
    {
        public readonly UpccAttributes Attributes;
        public readonly UpccClasses Classes;
        public readonly UpccDataTypes DataTypes;
        public readonly UpccPackageClassifierContainmentRelations PackageClassifierContainmentRelations;
        public readonly UpccPackageContainmentRelations PackageContainmentRelations;
        public readonly UpccModelPackages Packages;
        public readonly UpccModelTaggedValues TaggedValues;

        static UpccModel()
        {
            Instance = new UpccModel();
        }

        private UpccModel()
        {
            TaggedValues = new UpccModelTaggedValues();

            Packages = new UpccModelPackages(TaggedValues);
            DataTypes = new UpccDataTypes(TaggedValues);
            Classes = new UpccClasses(TaggedValues);
            Attributes = new UpccAttributes(TaggedValues);

            PackageContainmentRelations = new UpccPackageContainmentRelations(Packages);
            PackageClassifierContainmentRelations = new UpccPackageClassifierContainmentRelations(Packages, DataTypes, Classes);
        }

        public static UpccModel Instance { get; private set; }
    }
}