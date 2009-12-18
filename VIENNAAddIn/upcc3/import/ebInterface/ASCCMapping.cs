using System;
using CctsRepository.CcLibrary;

namespace VIENNAAddIn.upcc3.import.ebInterface
{
    public class ASCCMapping : ElementMapping, IEquatable<ASCCMapping>
    {
        private readonly SourceElement sourceElement;
        private readonly TargetCCElement targetElement;

        public ASCCMapping(SourceElement sourceElement, TargetCCElement targetElement, ComplexTypeMapping targetMapping)
        {
            TargetMapping = targetMapping;
            this.sourceElement = sourceElement;
            this.targetElement = targetElement;
            ASCC = targetElement.Ascc;
            ACC = ASCC.AssociatingAcc;
        }

        public override string ToString()
        {
            return string.Format("ASCCMapping <SourceElement: {0}, TargetElement: {1}, ACC: {2} [{3}]>", sourceElement.Name, targetElement.Name, ACC.Name, ACC.Id);
        }

        public override string BIEName
        {
            get { return sourceElement.Name + "_" + ASCC.Name; }
        }

        public IAcc ACC { get; private set; }

        public IAscc ASCC { get; private set; }

        public ComplexTypeMapping TargetMapping { get; private set; }

        public bool Equals(ASCCMapping other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.sourceElement.Name, sourceElement.Name) && Equals(other.targetElement.Ascc.Id, targetElement.Ascc.Id) && Equals(other.ACC.Id, ACC.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (ASCCMapping)) return false;
            return Equals((ASCCMapping) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = (sourceElement != null ? sourceElement.GetHashCode() : 0);
                result = (result*397) ^ (targetElement != null ? targetElement.GetHashCode() : 0);
                result = (result*397) ^ (ACC != null ? ACC.GetHashCode() : 0);
                return result;
            }
        }

        public static bool operator ==(ASCCMapping left, ASCCMapping right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ASCCMapping left, ASCCMapping right)
        {
            return !Equals(left, right);
        }
    }
}