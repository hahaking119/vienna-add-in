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
    internal static class AttributeExtensions
    {
        internal static IEnumerable<string> GetTaggedValues(this Attribute attribute, TaggedValues key)
        {
            string value = attribute.GetTaggedValue(key);
            return string.IsNullOrEmpty(value) ? new string[0] : value.Split('|');
        }

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
    }
}