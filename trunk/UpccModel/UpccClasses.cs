namespace UpccModel
{
    public class UpccClasses
    {
        public readonly MetaClass Abie;
        public readonly MetaClass Acc;
        public readonly MetaClass Bdt;
        public readonly MetaClass Cdt;

        public UpccClasses(UpccModelTaggedValues taggedValues)
        {
            Cdt = new MetaClass
                  {
                      Name = "Cdt",
                      Stereotype = "CDT",
                  };

            Acc = new MetaClass
                  {
                      Name = "Acc",
                      Stereotype = "ACC",
                      HasIsEquivalentTo = true,
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
                      Associations = new[]
                                     {
                                         new MetaAssociation
                                         {
                                             Stereotype = "ASCC",
                                             AssociatedElementType = "ACC",
                                             Cardinality = Cardinality.Many,
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
                                         },
                                     },
                  };

            Bdt = new MetaClass
                  {
                      Name = "Bdt",
                      Stereotype = "BDT",
                  };

            Abie = new MetaClass
                   {
                       Name = "Abie",
                       Stereotype = "ABIE",
                   };
        }
    }
}