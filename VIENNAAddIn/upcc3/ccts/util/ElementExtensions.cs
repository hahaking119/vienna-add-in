using System;
using System.Collections.Generic;
using System.Text;
using EA;
using Attribute=EA.Attribute;

namespace VIENNAAddIn.upcc3.ccts.util
{
    internal static class ElementExtensions
    {
        internal static IEnumerable<string> GetTaggedValues(this Element element, TaggedValues key)
        {
            return element.TaggedValues.GetTaggedValues(key);
        }

        internal static string GetTaggedValue(this Element element, TaggedValues key)
        {
            return element.TaggedValues.GetTaggedValue(key);
        }

        internal static void SetTaggedValues(this Element element, TaggedValues key, IEnumerable<string> values)
        {
            // TODO provide some means to actually manage multiple values (e.g. using a standard separator character)
            // currently, the values are simply concatenated
            var concatenatedValues = new StringBuilder();
            foreach (var value in values)
            {
                concatenatedValues.Append(value);
            }
            element.SetTaggedValue(key, concatenatedValues.ToString());
        }

        internal static void SetTaggedValue(this Element element, TaggedValues key, string value)
        {
            TaggedValue taggedValue = null;
            foreach (TaggedValue tv in element.TaggedValues)
            {
                if (tv.Name.Equals(key.AsString()))
                {
                    taggedValue = tv;
                    break;
                }
            }
            if (taggedValue == null)
            {
                taggedValue = (TaggedValue) element.TaggedValues.AddNew(key.AsString(), "");
            }
            taggedValue.Value = value;
            if (!taggedValue.Update())
            {
                throw new EAException(taggedValue.GetLastError());
            }
            element.TaggedValues.Refresh();
        }

    }
}