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
            return connector.TaggedValues.GetTaggedValues(key);
        }

        internal static string GetTaggedValue(this Connector connector, TaggedValues key)
        {
            return connector.TaggedValues.GetTaggedValue(key);
        }
    }
}