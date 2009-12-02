using System.Linq;

namespace UpccModel
{
    public class UpccModel
    {
        public readonly UpccAbstractClasses AbstractClasses;
        public readonly UpccAssociations Associations;
        public readonly UpccAttributes Attributes;
        public readonly UpccClasses Classes;
        public readonly UpccDataTypes DataTypes;
        public readonly UpccDependencies Dependencies;
        public readonly UpccEnumerationLiterals EnumerationLiterals;
        public readonly UpccEnumerations Enumerations;
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

        public bool HasSubPackages(MetaPackage package)
        {
            return PackagePackageContainmentRelations.GetSubPackageRelationsFor(package).Count() > 0;
        }

        public bool HasClassifiers(MetaPackage package)
        {
            return PackageClassifierContainmentRelations.GetClassifierRelationsFor(package).Count() > 0;
        }

        public bool HasAttributes(MetaClassifier classifier)
        {
            return Attributes.GetAttributesFor(classifier).Count() > 0;
        }

        public bool HasEnumerationLiterals(MetaClassifier classifier)
        {
            return EnumerationLiterals.GetEnumerationLiteralsFor(classifier).Count() > 0;
        }

        public bool HasAssociations(MetaClassifier classifier)
        {
            return Associations.GetAssociationsForAssociatingClassifier(classifier).Count() > 0;
        }

        public bool HasDependencies(MetaClassifier classifier)
        {
            return Dependencies.GetDependenciesForSource(classifier).Count() > 0;
        }
    }
}