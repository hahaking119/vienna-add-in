using System.Collections.Generic;
using EA;

namespace VIENNAAddIn.upcc3.ccts.util
{
    internal static class AttributeExtensions
    {
        internal static IEnumerable<string> GetTaggedValues(this Attribute attribute, TaggedValues key)
        {
            return attribute.TaggedValues.GetTaggedValues(key);
        }

        internal static string GetTaggedValue(this Attribute attribute, TaggedValues key)
        {
            return attribute.TaggedValues.GetTaggedValue(key);
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