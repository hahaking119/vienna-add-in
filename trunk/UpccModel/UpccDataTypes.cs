using System.Collections.Generic;
using System.Reflection;

namespace UpccModel
{
    public class UpccDataTypes
    {
        public readonly MetaDataType Enum;
        public readonly MetaDataType Prim;

        public UpccDataTypes(UpccTaggedValues taggedValues)
        {
            Prim = new MetaDataType
                   {
                       Name = "Prim",
                       Stereotype = "PRIM",
                       TaggedValues = new[]
                                      {
                                          taggedValues.BusinessTerm,
                                          taggedValues.Definition,
                                          taggedValues.DictionaryEntryName.WithDefaultValue("Name"),
                                          taggedValues.Pattern,
                                          taggedValues.FractionDigits,
                                          taggedValues.Length,
                                          taggedValues.MaxExclusive,
                                          taggedValues.MaxInclusive,
                                          taggedValues.MaxLength,
                                          taggedValues.MinExclusive,
                                          taggedValues.MinInclusive,
                                          taggedValues.MinLength,
                                          taggedValues.TotalDigits,
                                          taggedValues.WhiteSpace,
                                          taggedValues.UniqueIdentifier,
                                          taggedValues.VersionIdentifier,
                                          taggedValues.LanguageCode,
                                      },
                   };

            Enum = new MetaDataType
                   {
                       Name = "Enum",
                       Stereotype = "ENUM",
                   };
        }

        public IEnumerable<MetaDataType> All
        {
            get
            {
                foreach (FieldInfo field in GetType().GetFields())
                {
                    yield return (MetaDataType) field.GetValue(this);
                }
            }
        }
    }
}