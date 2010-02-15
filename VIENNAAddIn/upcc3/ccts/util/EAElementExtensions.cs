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

        public static Connector AddConnector(this Element client, string name, string type, Action<Connector> initConnector)
        {
            var connector = (Connector) client.Connectors.AddNew(name, type);
            connector.Update();
            initConnector(connector);
            connector.Update();
            return connector;
        }

        /// <returns>True if the element has the given stereotype, false otherwise.</returns>
        public static bool IsA(this Element element, string stereotype)
        {
            return element != null && element.Stereotype == stereotype;
        }

        /// <returns>True if the element has the ABIE stereotype, false otherwise.</returns>
        public static bool IsABIE(this Element element)
        {
            return element.IsA(Stereotype.ABIE);
        }
    }
}