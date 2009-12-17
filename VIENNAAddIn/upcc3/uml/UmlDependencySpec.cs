using System;

namespace VIENNAAddIn.upcc3.uml
{
    public class UmlDependencySpec<TTarget> : IEquatable<UmlDependencySpec<TTarget>>
    {
        public string Stereotype { get; set; }
        public TTarget Target { get; set; }
        public string LowerBound { get; set; }
        public string UpperBound { get; set; }

        #region IEquatable<UmlDependencySpec<TTarget>> Members

        public bool Equals(UmlDependencySpec<TTarget> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Stereotype, Stereotype) && Equals(other.Target, Target) && Equals(other.LowerBound, LowerBound) && Equals(other.UpperBound, UpperBound);
        }

        #endregion

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (UmlDependencySpec<TTarget>)) return false;
            return Equals((UmlDependencySpec<TTarget>) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = (Stereotype != null ? Stereotype.GetHashCode() : 0);
                result = (result*397) ^ Target.GetHashCode();
                result = (result*397) ^ (LowerBound != null ? LowerBound.GetHashCode() : 0);
                result = (result*397) ^ (UpperBound != null ? UpperBound.GetHashCode() : 0);
                return result;
            }
        }

        public static bool operator ==(UmlDependencySpec<TTarget> left, UmlDependencySpec<TTarget> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(UmlDependencySpec<TTarget> left, UmlDependencySpec<TTarget> right)
        {
            return !Equals(left, right);
        }
    }
}