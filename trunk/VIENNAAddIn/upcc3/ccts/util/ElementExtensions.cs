using System;
using System.Collections.Generic;
using EA;

namespace VIENNAAddIn.upcc3.ccts.util
{
    internal static class ElementExtensions
    {
        internal static IEnumerable<string> GetTaggedValues(this Element element, TaggedValues taggedValue)
        {
            foreach (TaggedValue tv in element.TaggedValues)
            {
                if (tv.Name.Equals(taggedValue.AsString()))
                {
                    yield return tv.Value;
                }
            }
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