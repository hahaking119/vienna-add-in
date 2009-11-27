using System.Collections.Generic;

namespace UpccModel
{
    public enum Cardinality
    {
        Zero,
        ZeroOrOne,
        One,
        Many,
    }

    public class MetaTaggedValue
    {
        public Cardinality Cardinality;
        public string DefaultValue;
        public string Name;

        public MetaTaggedValue WithDefaultValue(string defaultValue)
        {
            return new MetaTaggedValue
                   {
                       Name = Name,
                       Cardinality = Cardinality,
                       DefaultValue = defaultValue,
                   };
        }
    }

    public abstract class MetaClassifier
    {
        public string Name;
    }

    public class MetaDataType : MetaClassifier
    {
        public MetaAssociation[] Associations;
        public MetaAttribute[] Attributes;
        public bool HasIsEquivalentTo;
        public string Stereotype;
        public MetaTaggedValue[] TaggedValues;
    }

    public class MetaClass : MetaClassifier
    {
        public MetaAssociation[] Associations;
        public MetaAttribute[] Attributes;
        public bool HasIsEquivalentTo;
        public string Stereotype;
        public MetaTaggedValue[] TaggedValues;
    }

    public class MetaAttribute
    {
        public Cardinality Cardinality;
        public string Stereotype;
        public MetaTaggedValue[] TaggedValues;
    }

    public class PackageContainmentRelation
    {
        public Cardinality ContainerPackageCardinality;
        public MetaPackage ContainerPackageType;
        public string ContainerPackageRole;

        public Cardinality ContainedPackageCardinality;
        public MetaPackage ContainedPackageType;
        public string ContainedPackageRole;
    }

    public class PackageClassifierContainmentRelation
    {
        public MetaPackage PackageType;
        public string PackageRole;

        public Cardinality ClassifierCardinality;
        public MetaClassifier ClassifierType;
        public string ClassifierRole;
    }

    public class MetaPackage
    {
        public string Name;
        public string Stereotype;
        public MetaTaggedValue[] TaggedValues;
    }

    public class MetaAssociation
    {
        public string AssociatedElementType;
        public Cardinality Cardinality;
        public string Stereotype;
        public MetaTaggedValue[] TaggedValues;
    }

    public static class UpccModel
    {
        #region Packages

        public static readonly MetaPackage BdtLibrary;
        public static readonly MetaPackage BieLibrary;
        public static readonly MetaPackage BLibrary;
        public static readonly MetaPackage CcLibrary;
        public static readonly MetaPackage CdtLibrary;
        public static readonly MetaPackage DocLibrary;
        public static readonly MetaPackage[] ElementLibraries;
        public static readonly MetaPackage EnumLibrary;
        public static readonly MetaPackage PrimLibrary;
        public static readonly PackageContainmentRelation[] PackageContainmentRelations;
        public static readonly PackageClassifierContainmentRelation[] PackageClassifierContainmentRelations;

        #endregion

        #region DataTypes

        public static readonly MetaDataType Prim;
        public static readonly MetaDataType Enum;

        #endregion

        #region Classes

        public static readonly MetaClass Cdt;
        public static readonly MetaClass Acc;
        public static readonly MetaClass Bdt;
        public static readonly MetaClass Abie;

#endregion

        #region Attributes

        public static readonly MetaAttribute Bcc;
        public static readonly MetaAttribute Bbie;

        #endregion

        static UpccModel()
        {
            #region Packages
            var bLibraryTaggedValues = new[]
                                       {
                                           UpccModelTaggedValues.BusinessTerm,
                                           UpccModelTaggedValues.Copyright,
                                           UpccModelTaggedValues.Owner,
                                           UpccModelTaggedValues.Reference,
                                           UpccModelTaggedValues.Status,
                                           UpccModelTaggedValues.UniqueIdentifier,
                                           UpccModelTaggedValues.VersionIdentifier,
                                       };

            MetaTaggedValue[] elementLibraryTaggedValues = new List<MetaTaggedValue>(bLibraryTaggedValues)
                                                           {
                                                               UpccModelTaggedValues.BaseUrn,
                                                               UpccModelTaggedValues.NamespacePrefix,
                                                           }.ToArray();

            PrimLibrary = new MetaPackage
            {
                Name = "PrimLibrary",
                Stereotype = "PRIMLibrary",
                TaggedValues = elementLibraryTaggedValues,
            };

            EnumLibrary = new MetaPackage
            {
                Name = "EnumLibrary",
                Stereotype = "ENUMLibrary",
                TaggedValues = elementLibraryTaggedValues,
            };

            CdtLibrary = new MetaPackage
            {
                Name = "CdtLibrary",
                Stereotype = "CDTLibrary",
                TaggedValues = elementLibraryTaggedValues,
            };

            CcLibrary = new MetaPackage
            {
                Name = "CCLibrary",
                Stereotype = "CcLibrary",
                TaggedValues = elementLibraryTaggedValues,
            };

            BdtLibrary = new MetaPackage
            {
                Name = "BdtLibrary",
                Stereotype = "BDTLibrary",
                TaggedValues = elementLibraryTaggedValues,
            };

            BieLibrary = new MetaPackage
            {
                Name = "BieLibrary",
                Stereotype = "BIELibrary",
                TaggedValues = elementLibraryTaggedValues,
            };

            DocLibrary = new MetaPackage
            {
                Name = "DocLibrary",
                Stereotype = "DOCLibrary",
                TaggedValues = elementLibraryTaggedValues,
            };

            BLibrary = new MetaPackage
            {
                Name = "BLibrary",
                TaggedValues = bLibraryTaggedValues,
            };

            PackageContainmentRelations = new[]
                                  {
                                      new PackageContainmentRelation
                                      {
                                          ContainerPackageCardinality = Cardinality.ZeroOrOne,
                                          ContainerPackageType = BLibrary,
                                          ContainerPackageRole = "Parent",
                                          ContainedPackageCardinality = Cardinality.Many,
                                          ContainedPackageType = BLibrary,
                                          ContainedPackageRole = "BLibraries",
                                      }, 
                                      new PackageContainmentRelation
                                      {
                                          ContainerPackageCardinality = Cardinality.One,
                                          ContainerPackageType = BLibrary,
                                          ContainerPackageRole = "BLibrary",
                                          ContainedPackageCardinality = Cardinality.Many,
                                          ContainedPackageType = PrimLibrary,
                                          ContainedPackageRole = "PrimLibraries",
                                      }, 
                                      new PackageContainmentRelation
                                      {
                                          ContainerPackageCardinality = Cardinality.One,
                                          ContainerPackageType = BLibrary,
                                          ContainerPackageRole = "BLibrary",
                                          ContainedPackageCardinality = Cardinality.Many,
                                          ContainedPackageType = EnumLibrary,
                                          ContainedPackageRole = "EnumLibraries",
                                      }, 
                                      new PackageContainmentRelation
                                      {
                                          ContainerPackageCardinality = Cardinality.One,
                                          ContainerPackageType = BLibrary,
                                          ContainerPackageRole = "BLibrary",
                                          ContainedPackageCardinality = Cardinality.Many,
                                          ContainedPackageType = CdtLibrary,
                                          ContainedPackageRole = "CdtLibraries",
                                      }, 
                                      new PackageContainmentRelation
                                      {
                                          ContainerPackageCardinality = Cardinality.One,
                                          ContainerPackageType = BLibrary,
                                          ContainerPackageRole = "BLibrary",
                                          ContainedPackageCardinality = Cardinality.Many,
                                          ContainedPackageType = CcLibrary,
                                          ContainedPackageRole = "CcLibraries",
                                      }, 
                                      new PackageContainmentRelation
                                      {
                                          ContainerPackageCardinality = Cardinality.One,
                                          ContainerPackageType = BLibrary,
                                          ContainerPackageRole = "BLibrary",
                                          ContainedPackageCardinality = Cardinality.Many,
                                          ContainedPackageType = BdtLibrary,
                                          ContainedPackageRole = "BdtLibraries",
                                      }, 
                                      new PackageContainmentRelation
                                      {
                                          ContainerPackageCardinality = Cardinality.One,
                                          ContainerPackageType = BLibrary,
                                          ContainerPackageRole = "BLibrary",
                                          ContainedPackageCardinality = Cardinality.Many,
                                          ContainedPackageType = BieLibrary,
                                          ContainedPackageRole = "BieLibraries",
                                      }, 
                                      new PackageContainmentRelation
                                      {
                                          ContainerPackageCardinality = Cardinality.One,
                                          ContainerPackageType = BLibrary,
                                          ContainerPackageRole = "BLibrary",
                                          ContainedPackageCardinality = Cardinality.Many,
                                          ContainedPackageType = DocLibrary,
                                          ContainedPackageRole = "DocLibraries",
                                      }, 
                                  };

            ElementLibraries = new[]
                               {
                                   PrimLibrary,
                                   EnumLibrary,
                                   CdtLibrary,
                                   CcLibrary,
                                   BdtLibrary,
                                   BieLibrary,
                                   DocLibrary,
                               }
                ;

            #endregion

            #region DataTypes

            Prim = new MetaDataType
                   {
                       Name = "Prim",
                       Stereotype = "PRIM",
                       HasIsEquivalentTo = true,
                       TaggedValues = new[]
                                      {
                                          UpccModelTaggedValues.BusinessTerm,
                                          UpccModelTaggedValues.Definition,
                                          UpccModelTaggedValues.DictionaryEntryName.WithDefaultValue("Name"),
                                          UpccModelTaggedValues.Pattern,
                                          UpccModelTaggedValues.FractionDigits,
                                          UpccModelTaggedValues.Length,
                                          UpccModelTaggedValues.MaxExclusive,
                                          UpccModelTaggedValues.MaxInclusive,
                                          UpccModelTaggedValues.MaxLength,
                                          UpccModelTaggedValues.MinExclusive,
                                          UpccModelTaggedValues.MinInclusive,
                                          UpccModelTaggedValues.MinLength,
                                          UpccModelTaggedValues.TotalDigits,
                                          UpccModelTaggedValues.WhiteSpace,
                                          UpccModelTaggedValues.UniqueIdentifier,
                                          UpccModelTaggedValues.VersionIdentifier,
                                          UpccModelTaggedValues.LanguageCode,
                                      },
                   };

            Enum = new MetaDataType
                   {
                       Name = "Enum",
                   };

            #endregion

            #region Classes

            Cdt = new MetaClass
                  {
                      Name = "Cdt",
                  };

            Acc = new MetaClass
                  {
                      Name = "Acc",
                      Stereotype = "ACC",
                      HasIsEquivalentTo = true,
                      TaggedValues = new[]
                                     {
                                         UpccModelTaggedValues.BusinessTerm,
                                         UpccModelTaggedValues.Definition,
                                         UpccModelTaggedValues.DictionaryEntryName.WithDefaultValue("Name + \". Details\""),
                                         UpccModelTaggedValues.LanguageCode,
                                         UpccModelTaggedValues.UniqueIdentifier,
                                         UpccModelTaggedValues.VersionIdentifier,
                                         UpccModelTaggedValues.UsageRule,
                                     },
                      Attributes = new[]
                                   {
                                       Bcc,
                                   },
                      Associations = new[]
                                     {
                                         new MetaAssociation
                                         {
                                             Stereotype = "ASCC",
                                             AssociatedElementType = "ACC",
                                             Cardinality = Cardinality.Many,
                                             TaggedValues = new[]
                                                            {
                                                                UpccModelTaggedValues.BusinessTerm,
                                                                UpccModelTaggedValues.Definition,
                                                                UpccModelTaggedValues.DictionaryEntryName,
                                                                UpccModelTaggedValues.LanguageCode,
                                                                UpccModelTaggedValues.SequencingKey,
                                                                UpccModelTaggedValues.UniqueIdentifier,
                                                                UpccModelTaggedValues.VersionIdentifier,
                                                                UpccModelTaggedValues.UsageRule,
                                                            },
                                         },
                                     },
                  };

            Bdt = new MetaClass
            {
                Name = "Bdt",
            };

            Abie = new MetaClass
            {
                Name = "Abie",
            };

            #endregion

            #region Attributes
            Bcc = new MetaAttribute
                  {
                      Stereotype = "BCC",
                      Cardinality = Cardinality.Many,
                      TaggedValues = new[]
                                     {
                                         UpccModelTaggedValues.BusinessTerm,
                                         UpccModelTaggedValues.Definition,
                                         UpccModelTaggedValues.DictionaryEntryName.WithDefaultValue("Container.Name + \". \" + Name + \". \" + Type.Name"),
                                         UpccModelTaggedValues.LanguageCode,
                                         UpccModelTaggedValues.SequencingKey,
                                         UpccModelTaggedValues.UniqueIdentifier,
                                         UpccModelTaggedValues.VersionIdentifier,
                                         UpccModelTaggedValues.UsageRule,
                                     },
                  };
            #endregion

            PackageClassifierContainmentRelations = new[]
                                  {
                                      new PackageClassifierContainmentRelation
                                      {
                                          PackageType = PrimLibrary,
                                          PackageRole = "PrimLibrary",
                                          ClassifierCardinality = Cardinality.Many,
                                          ClassifierType = Prim,
                                          ClassifierRole = "Prims",
                                      },
                                      new PackageClassifierContainmentRelation
                                      {
                                          PackageType = EnumLibrary,
                                          PackageRole = "EnumLibrary",
                                          ClassifierCardinality = Cardinality.Many,
                                          ClassifierType = Enum,
                                          ClassifierRole = "Enums",
                                      },
                                      new PackageClassifierContainmentRelation
                                      {
                                          PackageType = CdtLibrary,
                                          PackageRole = "CdtLibrary",
                                          ClassifierCardinality = Cardinality.Many,
                                          ClassifierType = Cdt,
                                          ClassifierRole = "Cdts",
                                      },
                                      new PackageClassifierContainmentRelation
                                      {
                                          PackageType = CcLibrary,
                                          PackageRole = "CcLibrary",
                                          ClassifierCardinality = Cardinality.Many,
                                          ClassifierType = Acc,
                                          ClassifierRole = "Accs",
                                      },
                                      new PackageClassifierContainmentRelation
                                      {
                                          PackageType = BdtLibrary,
                                          PackageRole = "BdtLibrary",
                                          ClassifierCardinality = Cardinality.Many,
                                          ClassifierType = Bdt,
                                          ClassifierRole = "Bdts",
                                      },
                                      new PackageClassifierContainmentRelation
                                      {
                                          PackageType = BieLibrary,
                                          PackageRole = "BieLibrary",
                                          ClassifierCardinality = Cardinality.Many,
                                          ClassifierType = Abie,
                                          ClassifierRole = "Abies",
                                      },
                                      new PackageClassifierContainmentRelation
                                      {
                                          PackageType = DocLibrary,
                                          PackageRole = "DocLibrary",
                                          ClassifierCardinality = Cardinality.Many,
                                          ClassifierType = Abie,
                                          ClassifierRole = "Abies",
                                      },
                                  };


        }

        public static IEnumerable<PackageContainmentRelation> GetSubPackageRelationsFor(MetaPackage packageType)
        {
            foreach (var relation in PackageContainmentRelations)
            {
                if (relation.ContainerPackageType == packageType)
                {
                    yield return relation;
                }
            }
        }

        public static IEnumerable<PackageContainmentRelation> GetSuperPackageRelationsFor(MetaPackage packageType)
        {
            foreach (var relation in PackageContainmentRelations)
            {
                if (relation.ContainedPackageType == packageType)
                {
                    yield return relation;
                }
            }
        }

        public static IEnumerable<PackageClassifierContainmentRelation> GetClassifierRelationsFor(MetaPackage packageType)
        {
            foreach (var relation in PackageClassifierContainmentRelations)
            {
                if (relation.PackageType == packageType)
                {
                    yield return relation;
                }
            }
        }

        public static IEnumerable<PackageClassifierContainmentRelation> GetPackageRelationsFor(MetaClassifier classifier)
        {
            foreach (var relation in PackageClassifierContainmentRelations)
            {
                if (relation.ClassifierType == classifier)
                {
                    yield return relation;
                }
            }
        }

    }
}