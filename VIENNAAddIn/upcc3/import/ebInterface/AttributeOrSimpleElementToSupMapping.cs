using System;
using CctsRepository.CdtLibrary;

namespace VIENNAAddIn.upcc3.import.ebInterface
{
    public class AttributeOrSimpleElementToSupMapping : ElementMapping, IEquatable<AttributeOrSimpleElementToSupMapping>
    {
        private readonly SourceItem sourceElement;

        public AttributeOrSimpleElementToSupMapping(SourceItem sourceElement, ICdtSup targetSup)
        {
            this.sourceElement = sourceElement;
            Sup = targetSup;
            Cdt = Sup.Cdt;
        }

        // TODO
        public override string BIEName
        {
            get { return Sup.Name; }
        }

        public ICdtSup Sup { get; private set; }

        public override string ToString()
        {
            return string.Format("SUPMapping <SourceItem: {0}, CDT: {1} [{2}]>", sourceElement.Name, Cdt.Name, Cdt.Id);
        }

        public ICdt Cdt { get; private set; }

        public bool Equals(AttributeOrSimpleElementToSupMapping other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.sourceElement.Name, sourceElement.Name) && Equals(other.Sup.Id, Sup.Id) && Equals(other.Cdt.Id, Cdt.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (AttributeOrSimpleElementToSupMapping)) return false;
            return Equals((AttributeOrSimpleElementToSupMapping) obj);
        }

        public override int GetHashCode()
        {
            return (Cdt != null ? Cdt.GetHashCode() : 0);
        }

        public static bool operator ==(AttributeOrSimpleElementToSupMapping left, AttributeOrSimpleElementToSupMapping right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(AttributeOrSimpleElementToSupMapping left, AttributeOrSimpleElementToSupMapping right)
        {
            return !Equals(left, right);
        }

        public override bool ResolveTypeMapping(SchemaMapping schemaMapping)
        {
            // do nothing
            return true;
        }
    }
}