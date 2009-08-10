using System;
using System.Collections.Generic;
using VIENNAAddIn.upcc3.ccts;

namespace VIENNAAddIn.upcc3.XSDImporter.ebInterface
{
    public class TargetCCElement : IEquatable<TargetCCElement>
    {
        public string Name { get; private set; }
        public ICC Reference { get; private set; }

        private readonly List<TargetCCElement> children;

        public List<TargetCCElement> Children
        {
            get { return new List<TargetCCElement>(children); }
        }

        public TargetCCElement(string name, ICC reference)
        {
            Name = name;
            Reference = reference;
            children = new List<TargetCCElement>();
        }

        public bool Equals(TargetCCElement other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Name, Name) && Equals(other.Reference.Id, Reference.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (TargetCCElement)) return false;
            return Equals((TargetCCElement) obj);
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }

        public static bool operator ==(TargetCCElement left, TargetCCElement right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(TargetCCElement left, TargetCCElement right)
        {
            return !Equals(left, right);
        }

        public bool IsACC
        {
            get { return Reference is IACC; }
        }

        public bool IsBCC
        {
            get { return Reference is IBCC; }
        }

        public Mapping Mapping { get; set; }

        public void AddChild(TargetCCElement element)
        {
            children.Add(element);
        }
    }
}