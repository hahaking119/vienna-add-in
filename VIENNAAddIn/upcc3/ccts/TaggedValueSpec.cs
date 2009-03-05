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

        public string Name
        {
            get { return Key.AsString(); }
        }

        public string Value { get; private set; }

        public override string ToString()
        {
            return string.Format("[{0}: {1}]", Name, Value);
        }
    }
}