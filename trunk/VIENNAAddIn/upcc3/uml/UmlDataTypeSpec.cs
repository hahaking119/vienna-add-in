using System;
using System.Collections.Generic;
using System.Linq;

namespace VIENNAAddIn.upcc3.uml
{
    public class UmlDataTypeSpec : IEquatable<UmlDataTypeSpec>
    {
        public string Name { get; set; }

        public IEnumerable<UmlTaggedValueSpec> TaggedValues { get; set; }

        #region IEquatable<UmlDataTypeSpec> Members

        public bool Equals(UmlDataTypeSpec other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Name, Name) && other.TaggedValues.SequenceEqual(TaggedValues);
        }

        #endregion

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (UmlDataTypeSpec)) return false;
            return Equals((UmlDataTypeSpec) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name != null ? Name.GetHashCode() : 0)*397) ^ (TaggedValues != null ? TaggedValues.GetHashCode() : 0);
            }
        }

        public static bool operator ==(UmlDataTypeSpec left, UmlDataTypeSpec right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(UmlDataTypeSpec left, UmlDataTypeSpec right)
        {
            return !Equals(left, right);
        }
    }
}