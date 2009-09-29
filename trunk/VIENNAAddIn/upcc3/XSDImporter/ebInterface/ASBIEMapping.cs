using System;

namespace VIENNAAddIn.upcc3.XSDImporter.ebInterface
{
    public class ASBIEMapping : ElementMapping, IEquatable<ASBIEMapping>
    {
        public ASBIEMapping(string sourceElementName, ComplexTypeMapping targetMapping)
        {
            SourceElementName = sourceElementName;
            TargetMapping = targetMapping;
        }

        public string SourceElementName { get; private set; }

        public ComplexTypeMapping TargetMapping { get; private set; }

        public override string ToString()
        {
            return string.Format("ASBIEMapping <SourceElement: {0}, ComplexType: {1}>", SourceElementName, TargetMapping.ComplexTypeName);
        }

        public bool Equals(ASBIEMapping other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.SourceElementName, SourceElementName) && Equals(other.TargetMapping, TargetMapping);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (ASBIEMapping)) return false;
            return Equals((ASBIEMapping) obj);
        }

        public override int GetHashCode()
        {
            return (TargetMapping != null ? TargetMapping.GetHashCode() : 0);
        }

        public static bool operator ==(ASBIEMapping left, ASBIEMapping right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ASBIEMapping left, ASBIEMapping right)
        {
            return !Equals(left, right);
        }

        public override string BIEName
        {
            get { return SourceElementName; }
        }
    }
}