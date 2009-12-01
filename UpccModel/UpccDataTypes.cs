using System.Collections.Generic;
using System.Reflection;

namespace UpccModel
{
    public class UpccDataTypes
    {
        public readonly MetaDataType IdScheme;
        public readonly MetaDataType Prim;

        public UpccDataTypes(UpccTaggedValues taggedValues, UpccAbstractClasses abstractClasses)
        {
            Prim = new MetaDataType
                   {
                       Name = "Prim",
                       Stereotype = "PRIM",
                       BaseType = abstractClasses.BasicType,
                       TaggedValues = new[]
                                      {
                                          taggedValues.BusinessTerm,
                                          taggedValues.Definition,
                                          taggedValues.DictionaryEntryName.WithDefaultValue("Name"),
                                          taggedValues.FractionDigits,
                                          taggedValues.LanguageCode,
                                          taggedValues.Length,
                                          taggedValues.MaximumExclusive,
                                          taggedValues.MaximumInclusive,
                                          taggedValues.MaximumLength,
                                          taggedValues.MinimumExclusive,
                                          taggedValues.MinimumInclusive,
                                          taggedValues.MinimumLength,
                                          taggedValues.Pattern,
                                          taggedValues.TotalDigits,
                                          taggedValues.UniqueIdentifier,
                                          taggedValues.VersionIdentifier,
                                          taggedValues.WhiteSpace,
                                      },
                   };

            IdScheme = new MetaDataType
                       {
                           Name = "IdScheme",
                           Stereotype = "IDSCHEME",
                           BaseType = abstractClasses.BasicType,
                           TaggedValues = new[]
                                          {
                                              taggedValues.BusinessTerm,
                                              taggedValues.Definition,
                                              taggedValues.DictionaryEntryName.WithDefaultValue("Name"),
                                              taggedValues.IdentifierSchemeAgencyIdentifier,
                                              taggedValues.IdentifierSchemeAgencyName,
                                              taggedValues.ModificationAllowedIndicator,
                                              taggedValues.Pattern,
                                              taggedValues.RestrictedPrimitive,
                                              taggedValues.UniqueIdentifier,
                                              taggedValues.VersionIdentifier,
                                          },
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