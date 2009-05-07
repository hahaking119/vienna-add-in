// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System.Collections.Generic;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts
{
    public class TaggedValueSpec
    {
        public TaggedValueSpec(TaggedValues key, string value)
        {
            Key = key;
            Value = value;
        }

        public TaggedValueSpec(TaggedValues key, IEnumerable<string> values) : this(key, values.JoinToString("|"))
        {
        }

        public TaggedValueSpec(TaggedValues key, bool value) : this(key, value.ToString())
        {
        }

        public TaggedValues Key { get; private set; }

        public string Value { get; private set; }

        public override string ToString()
        {
            return string.Format("[{0}: {1}]", Key.AsString(), Value);
        }
    }
}