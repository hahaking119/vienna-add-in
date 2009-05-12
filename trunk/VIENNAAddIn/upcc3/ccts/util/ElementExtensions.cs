// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System.Collections.Generic;
using EA;

namespace VIENNAAddIn.upcc3.ccts.util
{
    ///<summary>
    ///</summary>
    public static class ElementExtensions
    {
        ///<summary>
        ///</summary>
        ///<param name="element"></param>
        ///<param name="key"></param>
        ///<returns></returns>
        public static IEnumerable<string> GetTaggedValues(this Element element, TaggedValues key)
        {
            string value = element.GetTaggedValue(key);
            return string.IsNullOrEmpty(value) ? new string[0] : value.Split('|');
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
            connector.Type = connectorSpec.ConnectorType;
            connector.Stereotype = connectorSpec.Stereotype;
            connector.ClientID = element.ElementID;
            connector.ClientEnd.Aggregation = (int) connectorSpec.AggregationKind;
            connector.SupplierID = connectorSpec.SupplierId;
            connector.SupplierEnd.Role = connectorSpec.Name;
            connector.SupplierEnd.Cardinality = connectorSpec.LowerBound + ".." + connectorSpec.UpperBound;
            connector.Update();
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
    }
}