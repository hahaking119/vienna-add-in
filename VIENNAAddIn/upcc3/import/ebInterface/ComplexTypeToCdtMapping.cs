using System;
using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.import.ebInterface
{
    public class ComplexTypeToCdtMapping : ComplexTypeMapping, IEquatable<ComplexTypeToCdtMapping>
    {
        public ComplexTypeToCdtMapping(string complexTypeName, IEnumerable<ElementMapping> childMappings) : base(complexTypeName, childMappings)
        {            
        }

        public override string ToString()
        {
            return string.Format("ComplexTypeToCdtMapping <ComplexType: {0}>", ComplexTypeName);
        }

        public override string BIEName
        {
            get { return TargetCdt.Name + "_" + ComplexTypeName; }
        }

        public bool Equals(ComplexTypeToCdtMapping other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return ChildrenEqual(other) && Equals(other.ComplexTypeName, ComplexTypeName);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(ComplexTypeToCdtMapping)) return false;
            return Equals((ComplexTypeToCdtMapping)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ComplexTypeName != null ? ComplexTypeName.GetHashCode() : 0;
            }
        }

        public static bool operator ==(ComplexTypeToCdtMapping left, ComplexTypeToCdtMapping right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ComplexTypeToCdtMapping left, ComplexTypeToCdtMapping right)
        {
            return !Equals(left, right);
        }
    }
}