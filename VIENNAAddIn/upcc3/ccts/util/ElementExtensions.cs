using System;
using System.Collections.Generic;
using EA;
using Attribute=EA.Attribute;

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

        public static void AddConnector(this Element element, string stereotype, string name, int supplierId)
        {
            var connector = (Connector) element.Connectors.AddNew(name, "Association");
            connector.Stereotype = stereotype;
            connector.SupplierID = supplierId;
        }

        public static void AddAttribute(this Element element, string stereotype, string name, string typeName,
                                        int classifierId)
        {
            var attribute = (Attribute) element.Attributes.AddNew(name, typeName);
            attribute.Stereotype = stereotype;
            attribute.ClassifierID = classifierId;
        }
    }
}