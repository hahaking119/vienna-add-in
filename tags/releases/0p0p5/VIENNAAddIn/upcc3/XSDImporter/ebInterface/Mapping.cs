using System;

namespace VIENNAAddIn.upcc3.XSDImporter.ebInterface
{
    public class Mapping : IEquatable<Mapping>
    {
        private TargetBIEElement targetBIE;

        public Mapping(SourceElement source, TargetCCElement targetCC)
        {
            Source = source;
            source.Mapping = this;
            TargetCC = targetCC;
            targetCC.Mapping = this;
        }

        public SourceElement Source { get; private set; }
        public TargetCCElement TargetCC { get; private set; }
        public TargetBIEElement TargetBIE
        {
            get { return targetBIE; }
            set
            {
                targetBIE = value;
                targetBIE.Mapping = this;
            }
        }

        #region IEquatable<Mapping> Members

        public bool Equals(Mapping other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Source, Source) && Equals(other.TargetCC, TargetCC);
        }

        #endregion

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Mapping)) return false;
            return Equals((Mapping) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Source != null ? Source.GetHashCode() : 0)*397) ^ (TargetCC != null ? TargetCC.GetHashCode() : 0);
            }
        }

        public static bool operator ==(Mapping left, Mapping right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Mapping left, Mapping right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return string.Format("{0} -> {1} ({2})", Source.Name, TargetCC.Name, TargetCC.Reference.GetType().Name);
        }
    }
}