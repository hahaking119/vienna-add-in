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
using EA;

namespace VIENNAAddIn.upcc3.ccts.util
{
    internal static class ConnectorExtensions
    {
        internal static IEnumerable<string> GetTaggedValues(this Connector con, TaggedValues taggedValue)
        {
            foreach (TaggedValue tv in con.TaggedValues)
            {
                if (tv.Name.Equals(taggedValue.AsString()))
                {
                    yield return tv.Value;
                }
            }
        }

        internal static string GetTaggedValue(this Connector con, TaggedValues taggedValue)
        {
            foreach (TaggedValue tv in con.TaggedValues)
            {
                if (tv.Name.Equals(taggedValue.AsString()))
                {
                    return tv.Value;
                }
            }
            return String.Empty;
        }
    }
}