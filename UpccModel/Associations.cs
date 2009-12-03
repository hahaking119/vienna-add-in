using System.Collections.Generic;
using System.Reflection;

namespace Upcc
{
    internal class Associations
    {
        internal readonly MetaAssociation AbieAsma;
        internal readonly MetaAssociation Asbie;
        internal readonly MetaAssociation Ascc;
        internal readonly MetaAssociation MaAsma;

        internal Associations(TaggedValues taggedValues, Classes classes)
        {
            Ascc = new MetaAssociation
                   {
                       Stereotype = MetaStereotype.ASCC,
                       ClassName = "Ascc",
                       Name = "Asccs",
                       Cardinality = MetaCardinality.Many,
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
                        Stereotype = MetaStereotype.ASBIE,
                        ClassName = "Asbie",
                        Name = "Asbies",
                        Cardinality = MetaCardinality.Many,
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
                         Stereotype = MetaStereotype.ASMA,
                         Name = "Mas",
                         Cardinality = MetaCardinality.Many,
                         AssociatingClassifierType = classes.Ma,
                         AssociatedClassifierType = classes.Ma,
                         TaggedValues = new MetaTaggedValue[0],
                     };

            AbieAsma = new MetaAssociation
                       {
                           Stereotype = MetaStereotype.ASMA,
                           Name = "Abies",
                           Cardinality = MetaCardinality.Many,
                           AssociatingClassifierType = classes.Ma,
                           AssociatedClassifierType = classes.Abie,
                           TaggedValues = new MetaTaggedValue[0],
                       };
        }

        internal IEnumerable<MetaAssociation> All
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