using System;
using CctsRepository.CcLibrary;
using CctsRepository.CdtLibrary;

namespace VIENNAAddIn.upcc3.import.ebInterface
{
    public class SupMapping : ElementMapping, IEquatable<SupMapping>
    {
        private readonly SourceElement sourceElement;
        private readonly TargetCCElement targetElement;

        public SupMapping(SourceElement sourceElement, TargetCCElement targetElement)
        {
            this.sourceElement = sourceElement;
            this.targetElement = targetElement;
            Sup = targetElement.Sup;
            Cdt = Sup.Cdt;
            ElementName = sourceElement.Name;
        }

        // TODO
        public override string BIEName
        {
            get { return Sup.Name; }
        }

        public ICdtSup Sup { get; private set; }

        public override string ToString()
        {
            return string.Format("SUPMapping <SourceElement: {0}, TargetElement: {1}, CDT: {2} [{3}]>", sourceElement.Name, targetElement.Name, Cdt.Name, Cdt.Id);
        }

        public ICdt Cdt { get; private set; }

        public string ElementName { get; private set; }

        public bool Equals(SupMapping other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.sourceElement.Name, sourceElement.Name) && Equals(other.targetElement.Sup.Id, targetElement.Sup.Id) && Equals(other.Cdt.Id, Cdt.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (SupMapping)) return false;
            return Equals((SupMapping) obj);
        }

        public override int GetHashCode()
        {
            return (Cdt != null ? Cdt.GetHashCode() : 0);
        }

        public static bool operator ==(SupMapping left, SupMapping right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(SupMapping left, SupMapping right)
        {
            return !Equals(left, right);
        }
    }
}