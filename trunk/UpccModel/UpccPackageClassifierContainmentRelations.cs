using System.Collections.Generic;

namespace UpccModel
{
    public class UpccPackageClassifierContainmentRelations
    {
        public UpccPackageClassifierContainmentRelations(UpccPackages packages, UpccDataTypes dataTypes, UpccClasses classes, UpccEnumerations enumerations)
        {
            All = new[]
                  {
                      new PackageClassifierContainmentRelation
                      {
                          PackageType = packages.PrimLibrary,
                          PackageRole = "PrimLibrary",
                          ClassifierCardinality = Cardinality.Many,
                          ClassifierType = dataTypes.Prim,
                          ClassifierRole = "Prims",
                      },
                      new PackageClassifierContainmentRelation
                      {
                          PackageType = packages.EnumLibrary,
                          PackageRole = "EnumLibrary",
                          ClassifierCardinality = Cardinality.Many,
                          ClassifierType = enumerations.Enum,
                          ClassifierRole = "Enums",
                      },
                      new PackageClassifierContainmentRelation
                      {
                          PackageType = packages.EnumLibrary,
                          PackageRole = "EnumLibrary",
                          ClassifierCardinality = Cardinality.Many,
                          ClassifierType = dataTypes.IdScheme,
                          ClassifierRole = "IdSchemes",
                      },
                      new PackageClassifierContainmentRelation
                      {
                          PackageType = packages.CdtLibrary,
                          PackageRole = "CdtLibrary",
                          ClassifierCardinality = Cardinality.Many,
                          ClassifierType = classes.Cdt,
                          ClassifierRole = "Cdts",
                      },
                      new PackageClassifierContainmentRelation
                      {
                          PackageType = packages.CcLibrary,
                          PackageRole = "CcLibrary",
                          ClassifierCardinality = Cardinality.Many,
                          ClassifierType = classes.Acc,
                          ClassifierRole = "Accs",
                      },
                      new PackageClassifierContainmentRelation
                      {
                          PackageType = packages.BdtLibrary,
                          PackageRole = "BdtLibrary",
                          ClassifierCardinality = Cardinality.Many,
                          ClassifierType = classes.Bdt,
                          ClassifierRole = "Bdts",
                      },
                      new PackageClassifierContainmentRelation
                      {
                          PackageType = packages.BieLibrary,
                          PackageRole = "BieLibrary",
                          ClassifierCardinality = Cardinality.Many,
                          ClassifierType = classes.Abie,
                          ClassifierRole = "Abies",
                      },
                      new PackageClassifierContainmentRelation
                      {
                          PackageType = packages.DocLibrary,
                          PackageRole = "DocLibrary",
                          ClassifierCardinality = Cardinality.Many,
                          ClassifierType = classes.Ma,
                          ClassifierRole = "Mas",
                      },
                  };
        }

        public IEnumerable<PackageClassifierContainmentRelation> All { get; private set; }

        public IEnumerable<PackageClassifierContainmentRelation> GetClassifierRelationsFor(MetaPackage packageType)
        {
            foreach (PackageClassifierContainmentRelation relation in All)
            {
                if (relation.PackageType == packageType)
                {
                    yield return relation;
                }
            }
        }

        public IEnumerable<PackageClassifierContainmentRelation> GetPackageRelationsFor(MetaClassifier classifier)
        {
            foreach (PackageClassifierContainmentRelation relation in All)
            {
                if (relation.ClassifierType == classifier)
                {
                    yield return relation;
                }
            }
        }
    }
}