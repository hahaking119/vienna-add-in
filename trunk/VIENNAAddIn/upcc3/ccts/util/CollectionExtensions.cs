using System;
using System.Collections.Generic;
using EA;
using Attribute=EA.Attribute;

namespace VIENNAAddIn.upcc3.ccts.util
{
    public static class CollectionExtensions
    {
        public static IEnumerable<T> AsEnumerable<T>(this Collection collection)
        {
            foreach (T item in collection)
            {
                yield return item;
            }
        }

        /// <summary>
        /// Add a tagged value to a TaggedValues collection.
        /// </summary>
        /// <param name="collection">Must be a collection of EA.TaggedValue.</param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void AddTaggedValue(this Collection collection, TaggedValues key, string value)
        {
            var taggedValue = (TaggedValue) collection.AddNew(key.AsString(), "");
            taggedValue.Value = value;
            taggedValue.Update();
        }

        public static void AddAttributeTag(this Collection collection, TaggedValues key, string value)
        {
            var taggedValue = (AttributeTag) collection.AddNew(key.AsString(), "");
            taggedValue.Value = value;
            taggedValue.Update();
        }

        public static void AddTaggedValues(this Collection collection, TaggedValues key, IEnumerable<string> values)
        {
            // TODO provide some means to actually manage multiple values (e.g. using a standard separator character) currently, the values are simply concatenated
            collection.AddTaggedValue(key, values.JoinToString("|"));
        }

        public static string GetTaggedValue(this Collection collection, TaggedValues key)
        {
            try
            {
                return ((TaggedValue) collection.GetByName(key.AsString())).Value;
            }
            catch (Exception)
            {
                return String.Empty;
            }
        }

        public static IEnumerable<string> GetTaggedValues(this Collection collection, TaggedValues key)
        {
            string value = collection.GetTaggedValue(key);
            return value.Split('|');
        }

        public static void AddConnector(this Collection collection, string stereotype, int supplierId)
        {
            var connector = (Connector) collection.AddNew(stereotype, "Association");
            connector.Stereotype = stereotype;
            connector.SupplierID = supplierId;
            connector.Update();
        }

        public static void AddAttribute(this Collection collection, string stereotype, string name, string typeName, int classifierId, string lowerBound, string upperBound, IEnumerable<TaggedValueSpec> taggedValueSpecs)
        {
            var attribute = (Attribute) collection.AddNew(name, typeName);
            attribute.Stereotype = stereotype;
            attribute.ClassifierID = classifierId;
            attribute.LowerBound = lowerBound;
            attribute.UpperBound = upperBound;
            var taggedValues = attribute.TaggedValues;
            foreach (var taggedValueSpec in taggedValueSpecs)
            {
                taggedValues.AddAttributeTag(taggedValueSpec.Key, taggedValueSpec.Value);
            }
            taggedValues.Refresh();
            attribute.Update();
        }
    }
}