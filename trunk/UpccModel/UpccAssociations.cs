using System.Collections.Generic;
using System.Reflection;

namespace Upcc
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
                       ClassName = "Ascc",
                       Name = "Asccs",
                       Cardinality = Cardinality.Many,
                       AssociatingClassifierType = classes.Acc,
                       AssociatedClassifierType = classes.Acc,
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
                        ClassName = "Asbie",
                        Name = "Asbies",
                        Cardinality = Cardinality.Many,
                        AssociatingClassifierType = classes.Abie,
                        AssociatedClassifierType = classes.Abie,
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
                         Name = "Mas",
                         Cardinality = Cardinality.Many,
                         AssociatingClassifierType = classes.Ma,
                         AssociatedClassifierType = classes.Ma,
                         TaggedValues = new MetaTaggedValue[0],
                     };

            AbieAsma = new MetaAssociation
                       {
                           Stereotype = "ASMA",
                           Name = "Abies",
                           Cardinality = Cardinality.Many,
                           AssociatingClassifierType = classes.Ma,
                           AssociatedClassifierType = classes.Abie,
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

        public IEnumerable<MetaAssociation> GetAssociationsForAssociatingClassifier(MetaClassifier classifier)
        {
            foreach (var relation in All)
            {
                if (relation.AssociatingClassifierType == classifier)
                {
                    yield return relation;
                }
            }
        }

        public IEnumerable<MetaAssociation> GetAssociationsForAssociatedClassifier(MetaClassifier classifier)
        {
            foreach (var relation in All)
            {
                if (relation.AssociatedClassifierType == classifier)
                {
                    yield return relation;
                }
            }
        }
    }
}