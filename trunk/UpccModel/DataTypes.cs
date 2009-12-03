using System.Collections.Generic;
using System.Reflection;

namespace Upcc
{
    internal class DataTypes
    {
        internal readonly MetaDataType IdScheme;
        internal readonly MetaDataType Prim;

        internal DataTypes(TaggedValues taggedValues, AbstractClasses abstractClasses)
        {
            Prim = new MetaDataType
                   {
                       Name = "Prim",
                       Stereotype = MetaStereotype.PRIM,
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
                           Stereotype = MetaStereotype.IDSCHEME,
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

        internal IEnumerable<MetaDataType> All
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