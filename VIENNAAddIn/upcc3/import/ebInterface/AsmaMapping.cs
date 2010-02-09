using System;

namespace VIENNAAddIn.upcc3.import.ebInterface
{
    public class AsmaMapping : ElementMapping, IEquatable<AsmaMapping>
    {
        private readonly SourceItem sourceElement;

        public AsmaMapping(SourceItem sourceElement)
        {
            this.sourceElement = sourceElement;
        }

        public string SourceElementName
        {
            get { return sourceElement.Name; }
        }

        public ComplexTypeMapping TargetMapping { get; set; }

        public override string ToString()
        {
            return string.Format("AsmaMapping <SourceItem: {0}, ComplexType: {1}>", SourceElementName, TargetMapping.ComplexTypeName);
        }

        public bool Equals(AsmaMapping other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            bool b = Equals(other.TargetMapping, TargetMapping);
            bool b1 = Equals(other.SourceElementName, SourceElementName);
            return b1 && b;
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

        public override bool ResolveTypeMapping(SchemaMapping schemaMapping)
        {
            TargetMapping = schemaMapping.GetComplexTypeMapping(sourceElement.XsdType);
            return TargetMapping is ComplexTypeToAccMapping || TargetMapping is ComplexTypeToMaMapping;
        }
    }
}