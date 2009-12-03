using System.Collections.Generic;
using System.Reflection;

namespace Upcc
{
    public class UpccClasses
    {
        public readonly MetaClass Abie;
        public readonly MetaClass Acc;
        public readonly MetaClass Bdt;
        public readonly MetaClass Cdt;
        public readonly MetaClass Ma;

        public UpccClasses(UpccTaggedValues taggedValues, UpccAbstractClasses abstractClasses)
        {
            Cdt = new MetaClass
                  {
                      Name = "Cdt",
                      Stereotype = "CDT",
                      TaggedValues = new[]
                                     {
                                         taggedValues.BusinessTerm,
                                         taggedValues.Definition,
                                         taggedValues.DictionaryEntryName.WithDefaultValue("Name + \". Type\""),
                                         taggedValues.LanguageCode,
                                         taggedValues.UniqueIdentifier,
                                         taggedValues.VersionIdentifier,
                                         taggedValues.UsageRule,
                                     },
                  };

            Acc = new MetaClass
                  {
                      Name = "Acc",
                      Stereotype = "ACC",
                      TaggedValues = new[]
                                     {
                                         taggedValues.BusinessTerm,
                                         taggedValues.Definition,
                                         taggedValues.DictionaryEntryName.WithDefaultValue("Name + \". Details\""),
                                         taggedValues.LanguageCode,
                                         taggedValues.UniqueIdentifier,
                                         taggedValues.VersionIdentifier,
                                         taggedValues.UsageRule,
                                     },
                  };

            Bdt = new MetaClass
                  {
                      Name = "Bdt",
                      Stereotype = "BDT",
                      TaggedValues = new[]
                                     {
                                         taggedValues.BusinessTerm,
                                         taggedValues.Definition,
                                         taggedValues.DictionaryEntryName.WithDefaultValue("Name + \". Type\""),
                                         taggedValues.LanguageCode,
                                         taggedValues.UniqueIdentifier,
                                         taggedValues.VersionIdentifier,
                                         taggedValues.UsageRule,
                                     },
                  };

            Abie = new MetaClass
                   {
                       Name = "Abie",
                       Stereotype = "ABIE",
                       BaseType = abstractClasses.BieAggregator,
                       TaggedValues = new[]
                                      {
                                          taggedValues.BusinessTerm,
                                          taggedValues.Definition,
                                          taggedValues.DictionaryEntryName.WithDefaultValue("Name + \". Details\""),
                                          taggedValues.LanguageCode,
                                          taggedValues.UniqueIdentifier,
                                          taggedValues.VersionIdentifier,
                                          taggedValues.UsageRule,
                                      },
                   };

            Ma = new MetaClass
                 {
                     Name = "Ma",
                     Stereotype = "MA",
                     BaseType = abstractClasses.BieAggregator,
                     TaggedValues = new MetaTaggedValue[0],
                 };
        }

        public IEnumerable<MetaClass> All
        {
            get
            {
                foreach (FieldInfo field in GetType().GetFields())
                {
                    yield return (MetaClass) field.GetValue(this);
                }
            }
        }
    }
}