using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts
{
    public class CCTSElementSpec
    {
        public string Name { get; set; }

        [TaggedValue(TaggedValues.DictionaryEntryName)]
        public string DictionaryEntryName{ get; set;}

        [TaggedValue(TaggedValues.Definition)]
        public string Definition { get; set; }

        [TaggedValue(TaggedValues.UniqueIdentifier)]
        public string UniqueIdentifier { get; set; }

        [TaggedValue(TaggedValues.VersionIdentifier)]
        public string VersionIdentifier { get; set; }

        [TaggedValue(TaggedValues.LanguageCode)]
        public string LanguageCode { get; set; }

        [TaggedValue(TaggedValues.BusinessTerm)]
        public IEnumerable<string> BusinessTerms { get; set; }

        public IEnumerable<TaggedValueSpec> GetTaggedValues()
        {
            Type type = GetType();
            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                object[] attributes = property.GetCustomAttributes(typeof (TaggedValueAttribute), true);
                if (attributes.Length > 0)
                {
                    var attribute = (TaggedValueAttribute) attributes[0];
                    if (property.PropertyType == typeof (string))
                    {
                        yield return
                            new TaggedValueSpec(attribute.Key, (string) property.GetValue(this, new object[] {}));
                    }
                    else if (property.PropertyType == typeof (IEnumerable<string>))
                    {
                        yield return
                            new TaggedValueSpec(attribute.Key,
                                                (IEnumerable<string>) property.GetValue(this, new object[] {}));
                    }
                    else if (property.PropertyType == typeof (bool))
                    {
                        yield return
                            new TaggedValueSpec(attribute.Key,
                                                (bool) property.GetValue(this, new object[] {}));
                    }
                }
            }
        }
    }

    internal class TaggedValueAttribute : Attribute
    {
        public TaggedValueAttribute(TaggedValues key)
        {
            Key = key;
        }

        public TaggedValues Key { get; private set; }
    }
}