using System;

namespace VIENNAAddIn.upcc3.import.ebInterface
{
    public class AsmaMapping : ElementMapping, IEquatable<AsmaMapping>
    {
        public AsmaMapping(string sourceElementName, ComplexTypeMapping targetMapping)
        {
            SourceElementName = sourceElementName;
            TargetMapping = targetMapping;
        }

        public string SourceElementName { get; private set; }

        public ComplexTypeMapping TargetMapping { get; private set; }

        public override string ToString()
        {
            return string.Format("AsmaMapping <SourceElement: {0}, ComplexType: {1}>", SourceElementName, TargetMapping.ComplexTypeName);
        }

        public bool Equals(AsmaMapping other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.SourceElementName, SourceElementName) && Equals(other.TargetMapping, TargetMapping);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (AsmaMapping)) return false;
            return Equals((AsmaMapping) obj);
        }

        public override int GetHashCode()
        {
            return (TargetMapping != null ? TargetMapping.GetHashCode() : 0);
        }

        public static bool operator ==(AsmaMapping left, AsmaMapping right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(AsmaMapping left, AsmaMapping right)
        {
            return !Equals(left, right);
        }

        public override string BIEName
        {
            get { return SourceElementName; }
        }
    }
}