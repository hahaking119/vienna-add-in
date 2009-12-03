using System.Collections.Generic;
using System.Reflection;

namespace Upcc
{
    internal class Enumerations
    {
        internal readonly MetaEnumeration Enum;

        internal Enumerations(TaggedValues taggedValues, AbstractClasses abstractClasses)
        {
            Enum = new MetaEnumeration
                   {
                       Name = "Enum",
                       Stereotype = MetaStereotype.ENUM,
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

        internal IEnumerable<MetaEnumeration> All
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