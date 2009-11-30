using System.Collections.Generic;

namespace UpccModel
{
    public class UpccPackagePackageContainmentRelations
    {
        public UpccPackagePackageContainmentRelations(UpccPackages packages)
        {
            All = new[]
                  {
                      new PackageContainmentRelation
                      {
                          ContainerPackageCardinality = Cardinality.ZeroOrOne,
                          ContainerPackageType = packages.BLibrary,
                          ContainerPackageRole = "Parent",
                          ContainedPackageCardinality = Cardinality.Many,
                          ContainedPackageType = packages.BLibrary,
                          ContainedPackageRole = "BLibraries",
                      },
                      new PackageContainmentRelation
                      {
                          ContainerPackageCardinality = Cardinality.One,
                          ContainerPackageType = packages.BLibrary,
                          ContainerPackageRole = "BLibrary",
                          ContainedPackageCardinality = Cardinality.Many,
                          ContainedPackageType = packages.PrimLibrary,
                          ContainedPackageRole = "PrimLibraries",
                      },
                      new PackageContainmentRelation
                      {
                          ContainerPackageCardinality = Cardinality.One,
                          ContainerPackageType = packages.BLibrary,
                          ContainerPackageRole = "BLibrary",
                          ContainedPackageCardinality = Cardinality.Many,
                          ContainedPackageType = packages.EnumLibrary,
                          ContainedPackageRole = "EnumLibraries",
                      },
                      new PackageContainmentRelation
                      {
                          ContainerPackageCardinality = Cardinality.One,
                          ContainerPackageType = packages.BLibrary,
                          ContainerPackageRole = "BLibrary",
                          ContainedPackageCardinality = Cardinality.Many,
                          ContainedPackageType = packages.CdtLibrary,
                          ContainedPackageRole = "CdtLibraries",
                      },
                      new PackageContainmentRelation
                      {
                          ContainerPackageCardinality = Cardinality.One,
                          ContainerPackageType = packages.BLibrary,
                          ContainerPackageRole = "BLibrary",
                          ContainedPackageCardinality = Cardinality.Many,
                          ContainedPackageType = packages.CcLibrary,
                          ContainedPackageRole = "CcLibraries",
                      },
                      new PackageContainmentRelation
                      {
                          ContainerPackageCardinality = Cardinality.One,
                          ContainerPackageType = packages.BLibrary,
                          ContainerPackageRole = "BLibrary",
                          ContainedPackageCardinality = Cardinality.Many,
                          ContainedPackageType = packages.BdtLibrary,
                          ContainedPackageRole = "BdtLibraries",
                      },
                      new PackageContainmentRelation
                      {
                          ContainerPackageCardinality = Cardinality.One,
                          ContainerPackageType = packages.BLibrary,
                          ContainerPackageRole = "BLibrary",
                          ContainedPackageCardinality = Cardinality.Many,
                          ContainedPackageType = packages.BieLibrary,
                          ContainedPackageRole = "BieLibraries",
                      },
                      new PackageContainmentRelation
                      {
                          ContainerPackageCardinality = Cardinality.One,
                          ContainerPackageType = packages.BLibrary,
                          ContainerPackageRole = "BLibrary",
                          ContainedPackageCardinality = Cardinality.Many,
                          ContainedPackageType = packages.DocLibrary,
                          ContainedPackageRole = "DocLibraries",
                      },
                  };
        }

        public IEnumerable<PackageContainmentRelation> All { get; private set; }

        /// <summary>
        /// Returns the package-to-package containment relations specifying the possible sub-packages of the given package type.
        /// </summary>
        /// <param name="packageType"></param>
        /// <returns></returns>
        public IEnumerable<PackageContainmentRelation> GetSubPackageRelationsFor(MetaPackage packageType)
        {
            foreach (PackageContainmentRelation relation in All)
            {
                if (relation.ContainerPackageType == packageType)
                {
                    yield return relation;
                }
            }
        }

        /// <summary>
        /// Returns the package-to-package containment relations specifying the possible super-packages of the given package type.
        /// </summary>
        /// <param name="packageType"></param>
        /// <returns></returns>
        public IEnumerable<PackageContainmentRelation> GetSuperPackageRelationsFor(MetaPackage packageType)
        {
            foreach (PackageContainmentRelation relation in All)
            {
                if (relation.ContainedPackageType == packageType)
                {
                    yield return relation;
                }
            }
        }
    }
}