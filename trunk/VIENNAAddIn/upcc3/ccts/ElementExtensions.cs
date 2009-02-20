using System;
using System.Collections.Generic;
using EA;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts
{
    internal static class ElementExtensions
    {
        internal static List<string> CollectTaggedValues(this Element element, TaggedValues taggedValue)
        {
            var values = new List<string>();
            foreach (TaggedValue tv in element.TaggedValues)
            {
                if (tv.Name.Equals(taggedValue.AsString()))
                {
                    values.Add(tv.Value);
                }
            }
            return values;
        }

        internal static string GetTaggedValue(this Element element, TaggedValues taggedValue)
        {
            foreach (TaggedValue tv in element.TaggedValues)
            {
                if (tv.Name.Equals(taggedValue.AsString()))
                {
                    return tv.Value;
                }
            }
            return String.Empty;
        }
    }
}