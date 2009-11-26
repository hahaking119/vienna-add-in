using System;
using System.Collections.Generic;
using CctsRepository.CcLibrary;

namespace VIENNAAddIn.upcc3.import.ebInterface
{
    public class TargetCCElement : IEquatable<TargetCCElement>
    {
        public static TargetCCElement ForBcc(string name, IBcc reference)
        {
            return new TargetCCElement(name, reference);
        }

        public static TargetCCElement ForAscc(string name, IAscc reference)
        {
            return new TargetCCElement(name, reference);
        }

        public static TargetCCElement ForAcc(string name, IAcc reference)
        {
            return new TargetCCElement(name, reference);
        }

        public string Name { get; private set; }
        private readonly object reference;
        public IBcc Bcc
        {
            get { return (IBcc) reference; }
        }
        public IAcc Acc
        {
            get { return (IAcc) reference; }
        }
        public IAscc Ascc
        {
            get { return (IAscc) reference; }
        }

        private readonly List<TargetCCElement> children;

        public List<TargetCCElement> Children
        {
            get { return new List<TargetCCElement>(children); }
        }

        private TargetCCElement(string name, object reference)
        {
            Name = name;
            this.reference = reference;
            children = new List<TargetCCElement>();
        }

        public bool Equals(TargetCCElement other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (!Equals(other.Name, Name)) return false;
            if (IsACC) return Equals(other.Acc.Id, Acc.Id);
            if (IsASCC) return Equals(other.Ascc.Id, Ascc.Id);
            if (IsBCC) return Equals(other.Bcc.Id, Bcc.Id);
            return false;
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
            get { return reference is IAcc; }
        }

        public bool IsBCC
        {
            get { return reference is IBcc; }
        }

        public bool IsASCC
        {
            get { return reference is IAscc; }
        }

        public Mapping Mapping { get; set; }

        public TargetCCElement Parent { get; set; }

        public void AddChild(TargetCCElement child)
        {
            children.Add(child);
            child.Parent = this;
        }
    }
}