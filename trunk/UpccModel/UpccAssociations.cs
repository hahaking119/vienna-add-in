using System.Collections.Generic;
using System.Reflection;

namespace UpccModel
{
    public class UpccAssociations
    {
        public readonly MetaAssociation AbieAsma;
        public readonly MetaAssociation Asbie;
        public readonly MetaAssociation Ascc;
        public readonly MetaAssociation MaAsma;

        public UpccAssociations(UpccTaggedValues taggedValues, UpccClasses classes)
        {
            Ascc = new MetaAssociation
                   {
                       Stereotype = "ASCC",
                       AssociatingClassifierType = classes.Acc,
                       AssociatingClassifierCardinality = Cardinality.Many,
                       AssociatingClassifierRole = "AssociatingAccs",
                       AssociatedClassifierType = classes.Acc,
                       AssociatedClassifierCardinality = Cardinality.Many,
                       AssociatedClassifierRole = "AssociatedAccs",
                       TaggedValues = new[]
                                      {
                                          taggedValues.BusinessTerm,
                                          taggedValues.Definition,
                                          taggedValues.DictionaryEntryName,
                                          taggedValues.LanguageCode,
                                          taggedValues.SequencingKey,
                                          taggedValues.UniqueIdentifier,
                                          taggedValues.VersionIdentifier,
                                          taggedValues.UsageRule,
                                      },
                   };

            Asbie = new MetaAssociation
                    {
                        Stereotype = "ASBIE",
                        AssociatingClassifierType = classes.Abie,
                        AssociatingClassifierCardinality = Cardinality.Many,
                        AssociatingClassifierRole = "AssociatingAbies",
                        AssociatedClassifierType = classes.Abie,
                        AssociatedClassifierCardinality = Cardinality.Many,
                        AssociatedClassifierRole = "AssociatedAbies",
                        TaggedValues = new[]
                                       {
                                           taggedValues.BusinessTerm,
                                           taggedValues.Definition,
                                           taggedValues.DictionaryEntryName,
                                           taggedValues.LanguageCode,
                                           taggedValues.SequencingKey,
                                           taggedValues.UniqueIdentifier,
                                           taggedValues.VersionIdentifier,
                                           taggedValues.UsageRule,
                                       },
                    };

            MaAsma = new MetaAssociation
                     {
                         Stereotype = "ASMA",
                         AssociatingClassifierType = classes.Ma,
                         AssociatingClassifierCardinality = Cardinality.Many,
                         AssociatingClassifierRole = "AssociatingMas",
                         AssociatedClassifierType = classes.Ma,
                         AssociatedClassifierCardinality = Cardinality.Many,
                         AssociatedClassifierRole = "AssociatedMas",
                         AssociatedClassifierUseBaseType = true,
                         TaggedValues = new MetaTaggedValue[0],
                     };

            AbieAsma = new MetaAssociation
                       {
                           Stereotype = "ASMA",
                           AssociatingClassifierType = classes.Ma,
                           AssociatingClassifierCardinality = Cardinality.Many,
                           AssociatingClassifierRole = "AssociatingMas",
                           AssociatedClassifierType = classes.Abie,
                           AssociatedClassifierCardinality = Cardinality.Many,
                           AssociatedClassifierRole = "AssociatedAbies",
                           AssociatedClassifierUseBaseType = true,
                           TaggedValues = new MetaTaggedValue[0],
                       };
        }

        public IEnumerable<MetaAssociation> All
        {
            get
            {
                foreach (FieldInfo field in GetType().GetFields())
                {
                    yield return (MetaAssociation) field.GetValue(this);
                }
            }
        }
    }
}