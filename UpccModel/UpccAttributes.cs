namespace UpccModel
{
    public class UpccAttributes
    {
        public readonly MetaAttribute Bbie;
        public readonly MetaAttribute Bcc;
        public readonly MetaAttribute BdtCon;
        public readonly MetaAttribute BdtSup;
        public readonly MetaAttribute CdtCon;
        public readonly MetaAttribute CdtSup;

        public UpccAttributes(UpccModelTaggedValues taggedValues)
        {
            Bcc = new MetaAttribute
                  {
                      Stereotype = "BCC",
                      Cardinality = Cardinality.Many,
                      TaggedValues = new[]
                                     {
                                         taggedValues.BusinessTerm,
                                         taggedValues.Definition,
                                         taggedValues.DictionaryEntryName.WithDefaultValue("Container.Name + \". \" + Name + \". \" + Type.Name"),
                                         taggedValues.LanguageCode,
                                         taggedValues.SequencingKey,
                                         taggedValues.UniqueIdentifier,
                                         taggedValues.VersionIdentifier,
                                         taggedValues.UsageRule,
                                     },
                  };
        }
    }
}