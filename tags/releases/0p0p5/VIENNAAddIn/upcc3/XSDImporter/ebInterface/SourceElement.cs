using System;
using System.Collections.Generic;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.XSDImporter.ebInterface
{
    public class SourceElement : IEquatable<SourceElement>
    {
        public string Name { get; private set; }

        private readonly List<SourceElement> children;

        public List<SourceElement> Children
        {
            get { return new List<SourceElement>(children); }
        }

        public Mapping Mapping { get; set; }

        public SourceElement(string name)
        {
            Name = name;
            children = new List<SourceElement>();
        }

        public void AddChild(SourceElement element)
        {
            children.Add(element);
        }

        public bool Equals(SourceElement other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Name, Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (SourceElement)) return false;
            return Equals((SourceElement) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Name != null ? Name.GetHashCode() : 0);
            }
        }

        public static bool operator ==(SourceElement left, SourceElement right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(SourceElement left, SourceElement right)
        {
            return !Equals(left, right);
        }
    }
}