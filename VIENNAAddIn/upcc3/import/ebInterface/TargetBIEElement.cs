using System;
using CctsRepository.bie;

namespace VIENNAAddIn.upcc3.import.ebInterface
{
    public class TargetBIEElement : IEquatable<TargetBIEElement>
    {
        public string Name { get; private set; }
        public IBIE Reference { get; private set; }

        public TargetBIEElement(string name, IBIE reference)
        {
            Name = name;
            Reference = reference;
        }

        public bool IsABIE
        {
            get { return Reference is IABIE; }
        }

        public bool IsBBIE
        {
            get { return Reference is IBBIE; }
        }

        public Mapping Mapping { get; set; }

        public bool Equals(TargetBIEElement other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Name, Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (TargetBIEElement)) return false;
            return Equals((TargetBIEElement) obj);
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }

        public static bool operator ==(TargetBIEElement left, TargetBIEElement right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(TargetBIEElement left, TargetBIEElement right)
        {
            return !Equals(left, right);
        }
    }
}