using System;
using System.Collections.Generic;
using EA;
using Attribute=EA.Attribute;

namespace VIENNAAddIn.upcc3.ccts.util
{
    internal static class AttributeExtensions
    {
        internal static IEnumerable<string> GetTaggedValues(this Attribute attribute, TaggedValues taggedValue)
        {
            foreach (AttributeTag tv in attribute.TaggedValues)
            {
                if (tv.Name.Equals(taggedValue))
                {
                    yield return tv.Value;
                }
            }
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

        public static bool IsCON(this Attribute attribute)
        {
            return attribute.Stereotype == "CON";
        }

        public static bool IsSUP(this Attribute attribute)
        {
            return attribute.Stereotype == "SUP";
        }
    }
}