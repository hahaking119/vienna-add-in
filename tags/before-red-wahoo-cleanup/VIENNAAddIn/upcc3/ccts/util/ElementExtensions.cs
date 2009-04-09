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
                if (tv.Name.Equals(key.AsString()))
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

        ///<summary>
        ///</summary>
        ///<param name="element"></param>
        ///<param name="connectorType">Must be one of <see cref="ConnectorTypes"/>.</param>
        ///<param name="stereotype"></param>
        ///<param name="supplierId"></param>
        private static void AddConnector(this Element element, string connectorType, string stereotype, string name, int supplierId, AggregationKind aggregationKind, string lowerBound, string upperBound)
        {
            var connector = (Connector) element.Connectors.AddNew("", "Association");
            connector.Type = connectorType;
            connector.Stereotype = stereotype;
            connector.SupplierID = supplierId;
            connector.SupplierEnd.Role = name;
            connector.SupplierEnd.Cardinality = lowerBound + ".." + upperBound;
            connector.ClientEnd.Aggregation = (int) aggregationKind;
            connector.Update();
        }

        public static void AddDependency(this Element element, string stereotype, int supplierId, string lowerBound, string upperBound)
        {
            element.AddConnector(ConnectorTypes.Dependency, stereotype, "", supplierId, AggregationKind.None, lowerBound, upperBound);
        }

        public static void AddAggregation(this Element element, AggregationKind aggregationKind, string stereotype, string name, int supplierId, string lowerBound, string upperBound)
        {
            element.AddConnector(ConnectorTypes.Aggregation, stereotype, name, supplierId, aggregationKind, lowerBound, upperBound);
        }

        ///<summary>
        ///</summary>
        ///<param name="element"></param>
        ///<param name="stereotype"></param>
        ///<param name="name"></param>
        ///<param name="typeName"></param>
        ///<param name="classifierId"></param>
        ///<param name="lowerBound"></param>
        ///<param name="upperBound"></param>
        ///<param name="taggedValueSpecs"></param>
        public static void AddAttribute(this Element element, string stereotype, string name, string typeName,
                                        int classifierId, string lowerBound, string upperBound,
                                        IEnumerable<TaggedValueSpec> taggedValueSpecs)
        {
            var attribute = (Attribute) element.Attributes.AddNew(name, typeName);
            attribute.Stereotype = stereotype;
            attribute.ClassifierID = classifierId;
            attribute.LowerBound = lowerBound;
            attribute.UpperBound = upperBound;
            Collection taggedValues = attribute.TaggedValues;
            foreach (TaggedValueSpec taggedValueSpec in taggedValueSpecs)
            {
                var taggedValue = (AttributeTag) taggedValues.AddNew(taggedValueSpec.Key.AsString(), "");
                taggedValue.Value = taggedValueSpec.Value;
                taggedValue.Update();
            }
            taggedValues.Refresh();
            attribute.Update();
        }
    }
}