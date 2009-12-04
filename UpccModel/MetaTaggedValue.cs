using System;

namespace Upcc
{
    public class MetaTaggedValue : IEquatable<MetaTaggedValue>
    {
        public MetaTaggedValue(string name)
        {
            Name = name;
            Cardinality = MetaCardinality.One;
            Type = "string";
            DefaultValue = null;
        }

        public string Name { get; private set; }
        public MetaCardinality Cardinality { get; internal set; }
        public string Type { get; internal set; }
        public string DefaultValue { get; internal set; }

        #region IEquatable<MetaTaggedValue> Members

        public bool Equals(MetaTaggedValue other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Name, Name);
        }

        #endregion

        public MetaTaggedValue WithDefaultValue(string defaultValue)
        {
            return new MetaTaggedValue(Name)
                   {
                       Cardinality = Cardinality,
                       Type = Type,
                       DefaultValue = defaultValue,
                   };
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (MetaTaggedValue)) return false;
            return Equals((MetaTaggedValue) obj);
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }

        public static bool operator ==(MetaTaggedValue left, MetaTaggedValue right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(MetaTaggedValue left, MetaTaggedValue right)
        {
            return !Equals(left, right);
        }
    }
}