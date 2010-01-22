using System;
using CctsRepository.CcLibrary;

namespace VIENNAAddIn.upcc3.import.ebInterface
{
    public class AttributeOrSimpleElementOrComplexElementToBccMapping : ElementMapping, IEquatable<AttributeOrSimpleElementOrComplexElementToBccMapping>
    {
        private readonly SourceElement sourceElement;

        public AttributeOrSimpleElementOrComplexElementToBccMapping(SourceElement sourceElement, IBcc targetBcc, IMapping bccTypeMapping)
        {
            BccTypeMapping = bccTypeMapping;
            this.sourceElement = sourceElement;
            Bcc = targetBcc;
            Acc = Bcc.Acc;
            ElementName = sourceElement.Name;
        }

        public override string BIEName
        {
            get { return ElementName + "_" + Bcc.Name; }
        }

        public IBcc Bcc { get; private set; }

        public override string ToString()
        {
            return string.Format("AttributeOrSimpleElementOrComplexElementToBccMapping <SourceElement: {0}, ACC: {1} [{2}]>", sourceElement.Name, Acc.Name, Acc.Id);
        }

        public IAcc Acc { get; private set; }

        public string ElementName { get; private set; }

        public IMapping BccTypeMapping { get; private set; }

        public bool Equals(AttributeOrSimpleElementOrComplexElementToBccMapping other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.sourceElement.Name, sourceElement.Name) && Equals(other.Bcc.Id, Bcc.Id) && Equals(other.Acc.Id, Acc.Id) && Equals(other.BccTypeMapping.BIEName, BccTypeMapping.BIEName);
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
            return (Acc != null ? Acc.GetHashCode() : 0);
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