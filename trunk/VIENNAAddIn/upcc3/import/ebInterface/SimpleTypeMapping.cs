using System;
using System.Collections.Generic;
using CctsRepository.CdtLibrary;

namespace VIENNAAddIn.upcc3.import.ebInterface
{
    public class SimpleTypeMapping : AbstractMapping
    {
        public SimpleTypeMapping(string simpleTypeName, ICdt targetCdt)
        {
            SimpleTypeName = simpleTypeName;
            TargetCDT = targetCdt;
        }

        public ICdt TargetCDT { get; private set; }

        public string SimpleTypeName { get; private set; }

        public override string ToString()
        {
            return string.Format("SimpleTypeMapping <SimpleType: {0}>", SimpleTypeName);
        }

        public bool Equals(SimpleTypeMapping other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.TargetCDT.Id, TargetCDT.Id) && Equals(other.SimpleTypeName, SimpleTypeName);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (SimpleTypeMapping)) return false;
            return Equals((SimpleTypeMapping) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((TargetCDT != null ? TargetCDT.GetHashCode() : 0)*397) ^ (SimpleTypeName != null ? SimpleTypeName.GetHashCode() : 0);
            }
        }

        public override string BIEName
        {
            get { return SimpleTypeName + "_" + TargetCDT.Name; }
        }

        protected override IEnumerable<ElementMapping> Children
        {
            get { yield break; }
        }
    } 
}