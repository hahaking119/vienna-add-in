using System.Collections.Generic;
using System.Reflection;

namespace Upcc
{
    public class UpccEnumerations
    {
        public readonly MetaEnumeration Enum;

        public UpccEnumerations(UpccTaggedValues taggedValues, UpccAbstractClasses abstractClasses)
        {
            Enum = new MetaEnumeration
                   {
                       Name = "Enum",
                       Stereotype = "ENUM",
                       BaseType = abstractClasses.BasicType,
                       TaggedValues = new[]
                                      {
                                          taggedValues.BusinessTerm,
                                          taggedValues.CodeListAgencyIdentifier,
                                          taggedValues.CodeListAgencyName,
                                          taggedValues.CodeListIdentifier,
                                          taggedValues.CodeListName,
                                          taggedValues.DictionaryEntryName,
                                          taggedValues.EnumerationUri,
                                          taggedValues.LanguageCode,
                                          taggedValues.ModificationAllowedIndicator,
                                          taggedValues.RestrictedPrimitive,
                                          taggedValues.Status,
                                          taggedValues.UniqueIdentifier,
                                          taggedValues.VersionIdentifier,
                                      }
                   };
        }

        public IEnumerable<MetaEnumeration> All
        {
            get
            {
                foreach (FieldInfo field in GetType().GetFields())
                {
                    yield return (MetaEnumeration) field.GetValue(this);
                }
            }
        }
    }
}