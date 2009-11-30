namespace UpccModel
{
    public class UpccAssociations
    {
        public readonly MetaAssociation Ascc;

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
        }
    }
}