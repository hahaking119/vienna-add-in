namespace UpccModel
{
    public class UpccClasses
    {
        public readonly MetaClass Ma;
        public readonly MetaClass Abie;
        public readonly MetaClass Acc;
        public readonly MetaClass Bdt;
        public readonly MetaClass Cdt;

        public UpccClasses(UpccTaggedValues taggedValues)
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
                  };

            Abie = new MetaClass
                   {
                       Name = "Abie",
                       Stereotype = "ABIE",
                   };

            Ma = new MetaClass
                   {
                       Name = "Ma",
                       Stereotype = "MA",
                   };
        }
    }
}