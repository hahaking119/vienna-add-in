using System;
using System.Collections.Generic;

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

        public UpccAttributes(UpccTaggedValues taggedValues, UpccClasses classes, UpccDataTypes dataTypes)
        {
            Bcc = new MetaAttribute
                  {
                      Stereotype = "BCC",
                      ContainingClassifierType = classes.Acc,
                      Name = "Bccs",
                      Type = classes.Cdt,
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
            All = new[]{Bcc};
        }

        public IEnumerable<MetaAttribute> All { get; private set; }

        public IEnumerable<MetaAttribute> GetAttributesFor(MetaClassifier classifier)
        {
            foreach (var attribute in All)
            {
                if (attribute.ContainingClassifierType == classifier)
                {
                    yield return attribute;
                }
            }
        }

        public MetaAttribute GetByName(string name)
        {
            foreach (var attribute in All)
            {
                if (attribute.Name == name)
                {
                    return attribute;
                }
            }
            throw new Exception("Attribute '" + name + "' not defined.");
        }
    }
}