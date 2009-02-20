using System;
using System.Collections.Generic;
using EA;
using Attribute=EA.Attribute;

namespace VIENNAAddIn.upcc3.ccts.util
{
    internal static class AttributeExtensions
    {
        internal static List<string> CollectTaggedValues(this Attribute attribute, TaggedValues taggedValue)
        {
            var values = new List<string>();
            foreach (AttributeTag tv in attribute.TaggedValues)
            {
                if (tv.Name.Equals(taggedValue))
                {
                    values.Add(tv.Value);
                }
            }
            return values;
        }

        internal static string GetTaggedValue(this Attribute attribute, TaggedValues taggedValue)
        {
            foreach (AttributeTag tv in attribute.TaggedValues)
            {
                if (tv.Name.Equals(taggedValue))
                {
                    return tv.Value;
                }
            }
            return String.Empty;
        }
    }
}