using System;
using System.Collections.Generic;
using System.Reflection;

namespace Upcc
{
    public class UpccAttributes
    {
        public readonly MetaAttribute Bbie;
        public readonly MetaAttribute Bcc;
        public readonly MetaAttribute BdtCon;
        public readonly MetaAttribute BdtSup;
        public readonly MetaAttribute CdtCon;
        public readonly MetaAttribute CdtSup;

        public UpccAttributes(UpccTaggedValues taggedValues, UpccClasses classes, UpccAbstractClasses abstractClasses)
        {
            CdtCon = new MetaAttribute
                     {
                         Stereotype = "CON",
                         ContainingClassifierType = classes.Cdt,
                         ClassName = "CdtCon",
                         AttributeName = "Con",
                         Type = abstractClasses.BasicType,
                         Cardinality = Cardinality.One,
                         TaggedValues = new[]
                                        {
                                            taggedValues.BusinessTerm,
                                            taggedValues.Definition,
                                            taggedValues.DictionaryEntryName.WithDefaultValue("Cdt.Name + \". Content\""),
                                            taggedValues.LanguageCode,
                                            taggedValues.ModificationAllowedIndicator,
                                            taggedValues.UniqueIdentifier,
                                            taggedValues.VersionIdentifier,
                                            taggedValues.UsageRule,
                                        },
                     };

            CdtSup = new MetaAttribute
                     {
                         Stereotype = "SUP",
                         ContainingClassifierType = classes.Cdt,
                         ClassName = "CdtSup",
                         AttributeName = "Sups",
                         Type = abstractClasses.BasicType,
                         Cardinality = Cardinality.Many,
                         TaggedValues = new[]
                                        {
                                            taggedValues.BusinessTerm,
                                            taggedValues.Definition,
                                            taggedValues.DictionaryEntryName.WithDefaultValue("Cdt.Name + \". \" + Name + \". \" + Type.Name"),
                                            taggedValues.LanguageCode,
                                            taggedValues.ModificationAllowedIndicator,
                                            taggedValues.UniqueIdentifier,
                                            taggedValues.VersionIdentifier,
                                            taggedValues.UsageRule,
                                        },
                     };

            Bcc = new MetaAttribute
                  {
                      Stereotype = "BCC",
                      ContainingClassifierType = classes.Acc,
                      ClassName = "Bcc",
                      AttributeName = "Bccs",
                      Type = classes.Cdt,
                      Cardinality = Cardinality.Many,
                      TaggedValues = new[]
                                     {
                                         taggedValues.BusinessTerm,
                                         taggedValues.Definition,
                                         taggedValues.DictionaryEntryName.WithDefaultValue("Acc.Name + \". \" + Name + \". \" + Type.Name"),
                                         taggedValues.LanguageCode,
                                         taggedValues.SequencingKey,
                                         taggedValues.UniqueIdentifier,
                                         taggedValues.VersionIdentifier,
                                         taggedValues.UsageRule,
                                     },
                  };

            BdtCon = new MetaAttribute
                     {
                         Stereotype = "CON",
                         ContainingClassifierType = classes.Bdt,
                         ClassName = "BdtCon",
                         AttributeName = "Con",
                         Type = abstractClasses.BasicType,
                         Cardinality = Cardinality.One,
                         TaggedValues = new[]
                                        {
                                            taggedValues.BusinessTerm,
                                            taggedValues.Definition,
                                            taggedValues.DictionaryEntryName.WithDefaultValue("Bdt.Name + \". Content\""),
                                            taggedValues.Enumeration,
                                            taggedValues.FractionDigits,
                                            taggedValues.LanguageCode,
                                            taggedValues.MaximumExclusive,
                                            taggedValues.MaximumInclusive,
                                            taggedValues.MaximumLength,
                                            taggedValues.MinimumExclusive,
                                            taggedValues.MinimumInclusive,
                                            taggedValues.MinimumLength,
                                            taggedValues.ModificationAllowedIndicator,
                                            taggedValues.Pattern,
                                            taggedValues.TotalDigits,
                                            taggedValues.UniqueIdentifier,
                                            taggedValues.UsageRule,
                                            taggedValues.VersionIdentifier,
                                        },
                     };

            BdtSup = new MetaAttribute
                     {
                         Stereotype = "SUP",
                         ContainingClassifierType = classes.Bdt,
                         ClassName = "BdtSup",
                         AttributeName = "Sups",
                         Type = abstractClasses.BasicType,
                         Cardinality = Cardinality.Many,
                         TaggedValues = new[]
                                        {
                                            taggedValues.BusinessTerm,
                                            taggedValues.Definition,
                                            taggedValues.DictionaryEntryName.WithDefaultValue("Bdt.Name + \". \" + Name + \". \" + Type.Name"),
                                            taggedValues.Enumeration,
                                            taggedValues.FractionDigits,
                                            taggedValues.LanguageCode,
                                            taggedValues.MaximumExclusive,
                                            taggedValues.MaximumInclusive,
                                            taggedValues.MaximumLength,
                                            taggedValues.MinimumExclusive,
                                            taggedValues.MinimumInclusive,
                                            taggedValues.MinimumLength,
                                            taggedValues.ModificationAllowedIndicator,
                                            taggedValues.Pattern,
                                            taggedValues.TotalDigits,
                                            taggedValues.UniqueIdentifier,
                                            taggedValues.UsageRule,
                                            taggedValues.VersionIdentifier,
                                        },
                     };

            Bbie = new MetaAttribute
                   {
                       Stereotype = "BBIE",
                       ContainingClassifierType = classes.Abie,
                       ClassName = "Bbie",
                       AttributeName = "Bbies",
                       Type = classes.Bdt,
                       Cardinality = Cardinality.Many,
                       TaggedValues = new[]
                                      {
                                          taggedValues.BusinessTerm,
                                          taggedValues.Definition,
                                          taggedValues.DictionaryEntryName.WithDefaultValue("Abie.Name + \". \" + Name + \". \" + Type.Name"),
                                          taggedValues.LanguageCode,
                                          taggedValues.SequencingKey,
                                          taggedValues.UniqueIdentifier,
                                          taggedValues.VersionIdentifier,
                                          taggedValues.UsageRule,
                                      },
                   };
        }

        public IEnumerable<MetaAttribute> All
        {
            get
            {
                foreach (FieldInfo field in GetType().GetFields())
                {
                    yield return (MetaAttribute) field.GetValue(this);
                }
            }
        }

        public IEnumerable<MetaAttribute> GetAttributesFor(MetaClassifier classifier)
        {
            foreach (MetaAttribute attribute in All)
            {
                if (attribute.ContainingClassifierType == classifier)
                {
                    yield return attribute;
                }
            }
        }

        public MetaAttribute GetByName(string name)
        {
            foreach (MetaAttribute attribute in All)
            {
                if (attribute.AttributeName == name)
                {
                    return attribute;
                }
            }
            throw new Exception("Attribute '" + name + "' not defined.");
        }
    }
}