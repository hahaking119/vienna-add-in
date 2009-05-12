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
    internal static class ConnectorExtensions
    {
        internal static IEnumerable<string> GetTaggedValues(this Connector connector, TaggedValues key)
        {
            string value = connector.GetTaggedValue(key);
            return string.IsNullOrEmpty(value) ? new string[0] : value.Split('|');
        }

        internal static string GetTaggedValue(this Connector connector, TaggedValues key)
        {
            foreach (ConnectorTag tv in connector.TaggedValues)
            {
                if (tv.Name.Equals(key.ToString()))
                {
                    return tv.Value;
                }
            }
            return null;
        }

        internal static void SetTaggedValue(this Connector connector, TaggedValues key, string value)
        {
            ConnectorTag taggedValue = null;
            foreach (ConnectorTag tv in connector.TaggedValues)
            {
                if (tv.Name.Equals(key.ToString()))
                {
                    taggedValue = tv;
                    break;
                }
            }
            if (taggedValue == null)
            {
                taggedValue = (ConnectorTag) connector.TaggedValues.AddNew(key.ToString(), "");
            }
            taggedValue.Value = value;
            if (!taggedValue.Update())
            {
                throw new EAException(taggedValue.GetLastError());
            }
            connector.TaggedValues.Refresh();
        }
    }
}