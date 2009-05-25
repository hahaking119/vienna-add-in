using System;
using System.Collections.Generic;
using EA;
using Attribute=EA.Attribute;

namespace VIENNAAddIn.upcc3.ccts.util
{
    public static class EAAttributeExtensions
    {
        public static AttributeTag AddTaggedValue(this Attribute attribute, string name, string value)
        {
            var tag = (AttributeTag) attribute.TaggedValues.AddNew(name, "");
            tag.Value = value;
            tag.Update();
            return tag;
        }

        public static Attribute With(this Attribute attribute, Action<Attribute> doSomethingWith)
        {
            doSomethingWith(attribute);
            attribute.Update();
            return attribute;
        }

        internal static IEnumerable<string> GetTaggedValues(this Attribute attribute, TaggedValues key)
        {
            string value = attribute.GetTaggedValue(key);
            return String.IsNullOrEmpty(value) ? new string[0] : value.Split('|');
        }

        internal static string GetTaggedValue(this Attribute attribute, TaggedValues key)
        {
            foreach (AttributeTag tv in attribute.TaggedValues)
            {
                if (tv.Name.Equals(key.ToString()))
                {
                    return tv.Value;
                }
            }
            return null;
        }
    }
}