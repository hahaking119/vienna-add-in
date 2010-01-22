using System;
using CctsRepository.CcLibrary;
using CctsRepository.CdtLibrary;

namespace VIENNAAddIn.upcc3.import.ebInterface
{
    public class TargetCcElement : IEquatable<TargetCcElement>
    {
        public static TargetCcElement ForBcc(IBcc reference)
        {
            return new TargetCcElement(reference);
        }

        public static TargetCcElement ForAscc(IAscc reference)
        {
            return new TargetCcElement(reference);
        }

        public static TargetCcElement ForAcc(IAcc reference)
        {
            return new TargetCcElement(reference);
        }

        public static TargetCcElement ForSup(ICdtSup reference)
        {
            return new TargetCcElement(reference);
        }

        public object Reference { get; private set; }

        public IBcc Bcc
        {
            get { return (IBcc) Reference; }
        }
        public IAcc Acc
        {
            get { return (IAcc) Reference; }
        }
        public IAscc Ascc
        {
            get { return (IAscc) Reference; }
        }
        public ICdtSup Sup
        {
            get { return (ICdtSup)Reference; }
        }

        private TargetCcElement(object reference)
        {
            Reference = reference;
        }

        public bool Equals(TargetCcElement other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (Reference is IAcc) return Equals(other.Acc.Id, Acc.Id);
            if (Reference is IAscc) return Equals(other.Ascc.Id, Ascc.Id);
            if (Reference is IBcc) return Equals(other.Bcc.Id, Bcc.Id);
            if (Reference is ICdtSup) return Equals(other.Sup.Id, Sup.Id);
            return false;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (TargetCcElement)) return false;
            return Equals((TargetCcElement) obj);
        }

        public override int GetHashCode()
        {
            return (Reference != null ? Reference.GetHashCode() : 0);
        }

        public static bool operator ==(TargetCcElement left, TargetCcElement right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(TargetCcElement left, TargetCcElement right)
        {
            return !Equals(left, right);
        }
    }
}