using System;
using System.Collections.Generic;
using CctsRepository;
using EA;
using Attribute=EA.Attribute;
using Stereotype=CctsRepository.Stereotype;

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

        public static Attribute AddAttribute(this Element element, string name, string typeName)
        {
            var attribute = (Attribute) element.Attributes.AddNew(name, typeName);
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

        ///<summary>
        ///</summary>
        ///<param name="element"></param>
        ///<param name="key"></param>
        ///<returns></returns>
        public static IEnumerable<string> GetTaggedValues(this Element element, TaggedValues key)
        {
            return MultiPartTaggedValue.Split(element.GetTaggedValue(key));
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
            element.SetTaggedValue(key, MultiPartTaggedValue.Merge(values));
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
            var connector = (Connector) element.Connectors.AddNew("", connectorSpec.ConnectorType.ToString());
            connector.Type = connectorSpec.ConnectorType.ToString();
            connector.Stereotype = connectorSpec.Stereotype;
            connector.ClientID = element.ElementID;
            connector.ClientEnd.Aggregation = (int) connectorSpec.AggregationKind;
            connector.SupplierID = connectorSpec.SupplierId;
            connector.SupplierEnd.Role = connectorSpec.Name;
            connector.SupplierEnd.Cardinality = connectorSpec.LowerBound + ".." + connectorSpec.UpperBound;
            connector.Update();
            Collection taggedValues = connector.TaggedValues;
            foreach (TaggedValueSpec taggedValueSpec in connectorSpec.TaggedValueSpecs)
            {
                var taggedValue = (ConnectorTag) taggedValues.AddNew(taggedValueSpec.Key.ToString(), "");
                taggedValue.Value = taggedValueSpec.Value;
                if (!taggedValue.Update())
                {
                    throw new EAException(taggedValue.GetLastError());
                }
            }
            taggedValues.Refresh();
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

        /// <returns>True if the element has the given stereotype, false otherwise.</returns>
        public static bool IsA(this Element element, string stereotype)
        {
            return element != null && element.Stereotype == stereotype;
        }

        /// <returns>True if the element has the ACC stereotype, false otherwise.</returns>
        public static bool IsACC(this Element element)
        {
            return element.IsA(Stereotype.ACC);
        }

        /// <returns>True if the element has the CDT stereotype, false otherwise.</returns>
        public static bool IsCDT(this Element element)
        {
            return element.IsA(Stereotype.CDT);
        }

        /// <returns>True if the element has the ABIE stereotype, false otherwise.</returns>
        public static bool IsABIE(this Element element)
        {
            return element.IsA(Stereotype.ABIE);
        }

        /// <returns>True if the element has the BDT stereotype, false otherwise.</returns>
        public static bool IsBDT(this Element element)
        {
            return element.IsA(Stereotype.BDT);
        }

        /// <returns>True if the element has the PRIM stereotype, false otherwise.</returns>
        public static bool IsPRIM(this Element element)
        {
            return element.IsA(Stereotype.PRIM);
        }

        /// <returns>True if the element has the ENUM stereotype, false otherwise.</returns>
        public static bool IsENUM(this Element element)
        {
            return element.IsA(Stereotype.ENUM);
        }
    }
}