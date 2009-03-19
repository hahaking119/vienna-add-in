// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System;
using System.Collections.Generic;
using System.Diagnostics;
using EA;
using Attribute = EA.Attribute;

namespace VIENNAAddIn.upcc3.ccts.util
{
    ///<summary>
    ///</summary>
    public static class CollectionExtensions
    {
        ///<summary>
        ///</summary>
        ///<param name="collection"></param>
        ///<typeparam name="T"></typeparam>
        ///<returns></returns>
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
            var taggedValue = (TaggedValue)collection.AddNew(key.AsString(), "");
            taggedValue.Value = value;
            taggedValue.Update();
        }

        private static void AddAttributeTag(this Collection collection, TaggedValues key, string value)
        {
            var taggedValue = (AttributeTag)collection.AddNew(key.AsString(), "");
            taggedValue.Value = value;
            taggedValue.Update();
        }

        ///<summary>
        ///</summary>
        ///<param name="collection"></param>
        ///<param name="key"></param>
        ///<param name="values"></param>
        public static void AddTaggedValues(this Collection collection, TaggedValues key, IEnumerable<string> values)
        {
            // TODO provide some means to actually manage multiple values (e.g. using a standard separator character) currently, the values are simply concatenated
            collection.AddTaggedValue(key, values.JoinToString("|"));
        }

        ///<summary>
        ///</summary>
        ///<param name="collection"></param>
        ///<param name="key"></param>
        ///<returns></returns>
        public static string GetTaggedValue(this Collection collection, TaggedValues key)
        {
            object tv;
            try
            {
                tv = collection.GetAt(0);
            }
            catch (Exception e)
            {
                Debug.WriteLine("caught exception "+e.Message);
                return null;
            }
            if (tv is TaggedValue)
            {
                    foreach (TaggedValue tvtemp in collection)
                    {
                        if(tvtemp.Name.Equals(key.AsString()))
                            return tvtemp.Value;
                    }
                    return null;
            }
            if (tv is AttributeTag)
            {
                foreach (AttributeTag attag in collection)
                {
                    if (attag.Name.Equals(key.AsString()))
                        return attag.Value;
                }
                return null;
            }
            if (tv is ConnectorTag){
                foreach (ConnectorTag contag in collection)
                {
                    if (contag.Name.Equals(key.AsString()))
                    {
                        Debug.WriteLine("returning connectortag");
                        return contag.Value;
                    }
                }
                return null;
             }
            throw new ArgumentException("collection contains an unknown tagged value type: " + collection.GetType());
        }
        
        ///<summary>
        ///</summary>
        ///<param name="collection"></param>
        ///<param name="key"></param>
        ///<returns></returns>
        public static IEnumerable<string> GetTaggedValues(this Collection collection, TaggedValues key)
        {
            string value = collection.GetTaggedValue(key);
            return string.IsNullOrEmpty(value) ? new string[0] : value.Split('|');
        }

        ///<summary>
        ///</summary>
        ///<param name="collection"></param>
        ///<param name="stereotype"></param>
        ///<param name="supplierId"></param>
        public static void AddConnector(this Collection collection, string stereotype, int supplierId)
        {
            var connector = (Connector)collection.AddNew(stereotype, "Association");
            connector.Stereotype = stereotype;
            connector.SupplierID = supplierId;
            connector.Update();
        }

        ///<summary>
        ///</summary>
        ///<param name="collection"></param>
        ///<param name="stereotype"></param>
        ///<param name="name"></param>
        ///<param name="typeName"></param>
        ///<param name="classifierId"></param>
        ///<param name="lowerBound"></param>
        ///<param name="upperBound"></param>
        ///<param name="taggedValueSpecs"></param>
        public static void AddAttribute(this Collection collection, string stereotype, string name, string typeName,
                                        int classifierId, string lowerBound, string upperBound,
                                        IEnumerable<TaggedValueSpec> taggedValueSpecs)
        {
            var attribute = (Attribute)collection.AddNew(name, typeName);
            attribute.Stereotype = stereotype;
            attribute.ClassifierID = classifierId;
            attribute.LowerBound = lowerBound;
            attribute.UpperBound = upperBound;
            Collection taggedValues = attribute.TaggedValues;
            foreach (TaggedValueSpec taggedValueSpec in taggedValueSpecs)
            {
                taggedValues.AddAttributeTag(taggedValueSpec.Key, taggedValueSpec.Value);
            }
            taggedValues.Refresh();
            attribute.Update();
        }
    }
}