using System;
using System.Collections.Generic;
using EA;
using Attribute=EA.Attribute;

namespace VIENNAAddIn.upcc3.ccts.util
{
    /// <summary>
    /// Extension methods for EA.Attribute.
    /// </summary>
    public static class EAAttributeExtensions
    {
        /// <summary>
        /// Execute the given actions for the attribute (e.g. setting its tagged value, ...) and save the attribute to the EA.Repository.
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="doSomethingWith"></param>
        /// <returns></returns>
        public static Attribute With(this Attribute attribute, params Action<Attribute>[] doSomethingWith)
        {
            foreach (var action in doSomethingWith)
            {
                action(attribute);
            }
            attribute.Update();
            return attribute;
        }

        /// <returns>
        /// En enumeration of the values of a multi-valued tagged value of the attribute. The values must be separated with '|'.
        /// If the tagged value is not defined or empty, an empty enumeration is returned.
        /// </returns>
        internal static IEnumerable<string> GetTaggedValues(this Attribute attribute, TaggedValues key)
        {
            return MultiPartTaggedValue.Split(attribute.GetTaggedValue(key));
        }

        /// <returns>
        /// The value of the given tagged value of this attribute or null if the tagged value is not defined.
        /// </returns>
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