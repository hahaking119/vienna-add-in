using System;
using System.Collections.Generic;
using EA;
using Attribute=EA.Attribute;

namespace VIENNAAddIn.upcc3.ccts.util
{
    public static class EAElementExtensions
    {
        public static Element With(this Element element, params Action<Element>[] doSomethingWith)
        {
            foreach (var action in doSomethingWith)
            {
                action(element);
            }
            element.Update();
            return element;
        }

        public static Attribute AddAttribute(this Element element, string name, Element type)
        {
            var attribute = (Attribute) element.Attributes.AddNew(name, type.Name);
            attribute.ClassifierID = type.ElementID;
            attribute.Update();
            return attribute;
        }

        public static TaggedValue AddTaggedValue(this Element element, string name)
        {
            var taggedValue = (TaggedValue) element.TaggedValues.AddNew(name, "");
            taggedValue.Value = "";
            taggedValue.Update();
            return taggedValue;
        }

        public static Path GetPath(this Element element, Repository repository)
        {
            Package package = repository.GetPackageByID(element.PackageID);
            return package.GetPath(repository)/element.Name;
        }

        ///<summary>
        ///</summary>
        ///<param name="element"></param>
        ///<param name="key"></param>
        ///<returns></returns>
        public static IEnumerable<string> GetTaggedValues(this Element element, TaggedValues key)
        {
            string value = element.GetTaggedValue(key);
            return String.IsNullOrEmpty(value) ? new string[0] : value.Split('|');
        }

        ///<summary>
        ///</summary>
        ///<param name="element"></param>
        ///<param name="key"></param>
        ///<returns></returns>
        public static string GetTaggedValue(this Element element, TaggedValues key)
        {
            foreach (TaggedValue tv in element.TaggedValues)
            {
                if (tv.Name.Equals(key.ToString()))
                {
                    return tv.Value;
                }
            }
            return null;
        }

        ///<summary>
        ///</summary>
        ///<param name="element"></param>
        ///<param name="key"></param>
        ///<param name="values"></param>
        public static void SetTaggedValues(this Element element, TaggedValues key, IEnumerable<string> values)
        {
            element.SetTaggedValue(key, values.JoinToString("|"));
        }

        ///<summary>
        ///</summary>
        ///<param name="element"></param>
        ///<param name="key"></param>
        ///<param name="value"></param>
        ///<exception cref="EAException"></exception>
        public static void SetTaggedValue(this Element element, TaggedValues key, string value)
        {
            TaggedValue taggedValue = null;
            foreach (TaggedValue tv in element.TaggedValues)
            {
                if (tv.Name.Equals(key.ToString()))
                {
                    taggedValue = tv;
                    break;
                }
            }
            if (taggedValue == null)
            {
                taggedValue = (TaggedValue) element.TaggedValues.AddNew(key.ToString(), "");
            }
            taggedValue.Value = value;
            if (!taggedValue.Update())
            {
                throw new EAException(taggedValue.GetLastError());
            }
            element.TaggedValues.Refresh();
        }

        ///<summary>
        ///</summary>
        ///<param name="element"></param>
        ///<param name="connectorSpec"></param>
        public static void AddConnector(this Element element, ConnectorSpec connectorSpec)
        {
            var connector = (Connector) element.Connectors.AddNew("", "Association");
            connector.Type = connectorSpec.ConnectorType.ToString();
            connector.Stereotype = connectorSpec.Stereotype;
            connector.ClientID = element.ElementID;
            connector.SupplierEnd.Aggregation = (int) connectorSpec.AggregationKind;
            connector.SupplierID = connectorSpec.SupplierId;
            connector.SupplierEnd.Role = connectorSpec.Name;
            connector.SupplierEnd.Cardinality = connectorSpec.LowerBound + ".." + connectorSpec.UpperBound;
            connector.Update();
        }

        public static Connector AddConnector(this Element client, string name, string type, Action<Connector> initConnector)
        {
            var connector = (Connector) client.Connectors.AddNew(name, type);
            connector.Update();
            initConnector(connector);
            connector.Update();
            return connector;
        }

        ///<summary>
        ///</summary>
        ///<param name="element"></param>
        ///<param name="attributeSpec"></param>
        public static void AddAttribute(this Element element, AttributeSpec attributeSpec)
        {
            var attribute = (Attribute) element.Attributes.AddNew(attributeSpec.Name, attributeSpec.TypeName);
            attribute.Stereotype = attributeSpec.Stereotype;
            attribute.ClassifierID = attributeSpec.ClassifierId;
            attribute.LowerBound = attributeSpec.LowerBound;
            attribute.UpperBound = attributeSpec.UpperBound;
            if (attributeSpec.DefaultValue != null)
            {
                attribute.Default = attributeSpec.DefaultValue;
            }
            attribute.Update();
            Collection taggedValues = attribute.TaggedValues;
            foreach (TaggedValueSpec taggedValueSpec in attributeSpec.TaggedValueSpecs)
            {
                var taggedValue = (AttributeTag) taggedValues.AddNew(taggedValueSpec.Key.ToString(), "");
                taggedValue.Value = taggedValueSpec.Value;
                if (!taggedValue.Update())
                {
                    throw new EAException(taggedValue.GetLastError());
                }
            }
            taggedValues.Refresh();
        }

        private static TaggedValue GetTaggedValueByName(this Element element, string name)
        {
            foreach (TaggedValue tv in element.TaggedValues)
            {
                if (tv.Name == name)
                {
                    return tv;
                }
            }
            return null;
        }

        public static bool HasTaggedValue(this Element element, string name)
        {
            return element.GetTaggedValueByName(name) != null;
        }

    }
}