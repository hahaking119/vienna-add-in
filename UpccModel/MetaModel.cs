using System.Linq;

namespace Upcc
{
    public class MetaModel
    {
        private readonly UpccAbstractClasses AbstractClasses;
        private readonly UpccAssociations Associations;
        private readonly UpccAttributes Attributes;
        private readonly UpccClasses Classes;
        private readonly UpccDataTypes DataTypes;
        private readonly UpccDependencies Dependencies;
        private readonly UpccEnumerationLiterals EnumerationLiterals;
        private readonly UpccEnumerations Enumerations;
        private readonly UpccPackageClassifierContainmentRelations PackageClassifierContainmentRelations;
        private readonly UpccPackagePackageContainmentRelations PackagePackageContainmentRelations;
        private readonly UpccPackages Packages;
        private readonly UpccTaggedValues TaggedValues;

        static MetaModel()
        {
            Instance = new MetaModel();
        }

        private MetaModel()
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

        public static MetaModel Instance { get; private set; }

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