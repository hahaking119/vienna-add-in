namespace UpccModel
{
    public class UpccModel
    {
        public readonly UpccAssociations Associations;
        public readonly UpccAttributes Attributes;
        public readonly UpccEnumerationLiterals EnumerationLiterals;
        public readonly UpccClasses Classes;
        public readonly UpccAbstractClasses AbstractClasses;
        public readonly UpccEnumerations Enumerations;
        public readonly UpccDataTypes DataTypes;
        public readonly UpccDependencies Dependencies;
        public readonly UpccPackageClassifierContainmentRelations PackageClassifierContainmentRelations;
        public readonly UpccPackagePackageContainmentRelations PackagePackageContainmentRelations;
        public readonly UpccPackages Packages;
        public readonly UpccTaggedValues TaggedValues;

        static UpccModel()
        {
            Instance = new UpccModel();
        }

        private UpccModel()
        {
            TaggedValues = new UpccTaggedValues();

            Packages = new UpccPackages(TaggedValues);

            AbstractClasses = new UpccAbstractClasses();
            Enumerations = new UpccEnumerations(TaggedValues, AbstractClasses);
            DataTypes = new UpccDataTypes(TaggedValues, AbstractClasses);
            Classes = new UpccClasses(TaggedValues, AbstractClasses);

            Attributes = new UpccAttributes(TaggedValues, Classes, AbstractClasses);
            EnumerationLiterals = new UpccEnumerationLiterals(TaggedValues, Enumerations);
            Associations = new UpccAssociations(TaggedValues, Classes);
            Dependencies = new UpccDependencies(Classes, DataTypes, Enumerations);

            PackagePackageContainmentRelations = new UpccPackagePackageContainmentRelations(Packages);
            PackageClassifierContainmentRelations = new UpccPackageClassifierContainmentRelations(Packages, DataTypes, Classes, Enumerations);
        }

        public static UpccModel Instance { get; private set; }
    }
}