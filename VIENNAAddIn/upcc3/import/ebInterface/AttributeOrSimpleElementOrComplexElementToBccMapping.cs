using System;
using CctsRepository.CcLibrary;

namespace VIENNAAddIn.upcc3.import.ebInterface
{
    public class AttributeOrSimpleElementOrComplexElementToBccMapping : ElementMapping, IEquatable<AttributeOrSimpleElementOrComplexElementToBccMapping>
    {
        private readonly SourceElement sourceElement;
        private readonly TargetCCElement targetElement;

        public AttributeOrSimpleElementOrComplexElementToBccMapping(SourceElement sourceElement, TargetCCElement targetElement)
        {
            this.sourceElement = sourceElement;
            this.targetElement = targetElement;
            BCC = targetElement.Bcc;
            ACC = BCC.Acc;
            ElementName = sourceElement.Name;
        }

        public override string BIEName
        {
            get { return ElementName + "_" + BCC.Name; }
        }

        public IBcc BCC { get; private set; }

        public override string ToString()
        {
            return string.Format("AttributeOrSimpleElementOrComplexElementToBccMapping <SourceElement: {0}, TargetElement: {1}, ACC: {2} [{3}]>", sourceElement.Name, targetElement.Name, ACC.Name, ACC.Id);
        }

        public IAcc ACC { get; private set; }

        public string ElementName { get; private set; }

        public bool Equals(AttributeOrSimpleElementOrComplexElementToBccMapping other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.sourceElement.Name, sourceElement.Name) && Equals(other.targetElement.Bcc.Id, targetElement.Bcc.Id) && Equals(other.ACC.Id, ACC.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (AttributeOrSimpleElementOrComplexElementToBccMapping)) return false;
            return Equals((AttributeOrSimpleElementOrComplexElementToBccMapping) obj);
        }

        public override int GetHashCode()
        {
            return (ACC != null ? ACC.GetHashCode() : 0);
        }

        public static bool operator ==(AttributeOrSimpleElementOrComplexElementToBccMapping left, AttributeOrSimpleElementOrComplexElementToBccMapping right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(AttributeOrSimpleElementOrComplexElementToBccMapping left, AttributeOrSimpleElementOrComplexElementToBccMapping right)
        {
            return !Equals(left, right);
        }
    }
}