using System;
using System.Collections.Generic;
using EA;
using VIENNAAddIn.upcc3.ccts.dra;
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

        public static AttributeTag AddTaggedValue(this Attribute attribute, string name)
        {
            var taggedValue = (AttributeTag) attribute.TaggedValues.AddNew(name, string.Empty);
            taggedValue.Value = string.Empty;
            taggedValue.Update();
            return taggedValue;
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

        public static void SetOrGenerateTaggedValue(this Attribute attribute, TaggedValueSpec taggedValueSpec, string defaultValue)
        {
            if (String.IsNullOrEmpty(taggedValueSpec.Value))
            {
                if (String.IsNullOrEmpty(attribute.GetTaggedValue(taggedValueSpec.Key)))
                {
                    attribute.AddTaggedValue(taggedValueSpec.Key.ToString()).WithValue(defaultValue);
                }
            }
            else
            {
                attribute.AddTaggedValue(taggedValueSpec.Key.ToString()).WithValue(taggedValueSpec.Value);
            }
        }

        /// <returns>True if the attribute has the given stereotype, false otherwise.</returns>
        public static bool IsA(this Attribute attribute, string stereotype)
        {
            return attribute != null && attribute.Stereotype == stereotype;
        }

        /// <returns>True if the attribute has the CON stereotype, false otherwise.</returns>
        public static bool IsCON(this Attribute attribute)
        {
            return attribute.IsA(Stereotype.CON);
        }

        /// <returns>True if the attribute has the SUP stereotype, false otherwise.</returns>
        public static bool IsSUP(this Attribute attribute)
        {
            return attribute.Stereotype == Stereotype.SUP;
        }
    }
}