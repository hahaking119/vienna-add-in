using System;

namespace UpccModel
{
    public class UpccModel
    {
        public readonly UpccAssociations Associations;
        public readonly UpccAttributes Attributes;
        public readonly UpccClasses Classes;
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
            DataTypes = new UpccDataTypes(TaggedValues);
            Classes = new UpccClasses(TaggedValues);

            Attributes = new UpccAttributes(TaggedValues, Classes, DataTypes);
            Associations = new UpccAssociations(TaggedValues, Classes);
            Dependencies = new UpccDependencies(Classes);

            PackagePackageContainmentRelations = new UpccPackagePackageContainmentRelations(Packages);
            PackageClassifierContainmentRelations = new UpccPackageClassifierContainmentRelations(Packages, DataTypes, Classes);
        }

        public static UpccModel Instance { get; private set; }
    }
}