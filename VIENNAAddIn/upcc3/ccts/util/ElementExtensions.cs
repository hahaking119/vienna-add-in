// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System.Collections.Generic;
using System.Text;
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
            return element.TaggedValues.GetTaggedValues(key);
        }

        ///<summary>
        ///</summary>
        ///<param name="element"></param>
        ///<param name="key"></param>
        ///<returns></returns>
        public static string GetTaggedValue(this Element element, TaggedValues key)
        {
            return element.TaggedValues.GetTaggedValue(key);
        }

        ///<summary>
        ///</summary>
        ///<param name="element"></param>
        ///<param name="key"></param>
        ///<param name="values"></param>
        public static void SetTaggedValues(this Element element, TaggedValues key, IEnumerable<string> values)
        {
            // TODO provide some means to actually manage multiple values (e.g. using a standard separator character)
            // currently, the values are simply concatenated
            var concatenatedValues = new StringBuilder();
            foreach (string value in values)
            {
                concatenatedValues.Append(value);
            }
            element.SetTaggedValue(key, concatenatedValues.ToString());
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
    }
}