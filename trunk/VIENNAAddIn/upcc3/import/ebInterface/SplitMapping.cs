using System;
using System.Collections.Generic;
using System.Text;
using CctsRepository.CcLibrary;
using CctsRepository.CdtLibrary;
using VIENNAAddInUtils;

namespace VIENNAAddIn.upcc3.import.ebInterface
{
    public class SplitMapping : ElementMapping, IEquatable<SplitMapping>
    {
        private readonly SourceElement sourceElement;
        private readonly TargetCcElement[] targetCcElements;

        public SplitMapping(SourceElement sourceElement, TargetCcElement[] targetCcElements)
        {
            this.sourceElement = sourceElement;
            this.targetCcElements = new List<TargetCcElement>(targetCcElements).ToArray();
        }

        public override string BIEName
        {
            get { throw new NotImplementedException(); }
        }

        public override string ToString()
        {
            StringBuilder s = new StringBuilder();
            foreach (TargetCcElement targetCCElement in targetCcElements)
            {
                if (targetCCElement.Reference is IBcc)
                {
                    s.Append(", BCC[").Append(targetCCElement.Bcc.Id).Append("]");
                } else if (targetCCElement.Reference is ICdtSup)
                {
                    s.Append(", SUP[").Append(targetCCElement.Sup.Id).Append("]");
                }
                else
                {
                    s.Append("ERROR: Wrong target CC type");
                }
            }
            return string.Format("SplitMapping <SourceElement: {0}, Targets: {1}>", sourceElement.Name, s);
        }


        public bool Equals(SplitMapping other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            if (other.targetCcElements.Length != targetCcElements.Length)
            {
                return false;
            }

            for (int i = 0; i < other.targetCcElements.Length; i++ )
            {
                if (other.targetCcElements[i].Reference is IBcc)
                {
                    if (!(targetCcElements[i].Reference is IBcc))
                    {
                        return false;
                    }

                    if (other.targetCcElements[i].Bcc.Id != targetCcElements[i].Bcc.Id)
                    {
                        return false;
                    }
                } 
                else if (other.targetCcElements[i].Reference is ICdtSup)
                {
                    if (!(targetCcElements[i].Reference is ICdtSup))
                    {
                        return false;
                    }

                    if (other.targetCcElements[i].Sup.Id != targetCcElements[i].Sup.Id)
                    {
                        return false;
                    }
                }

            }

            return Equals(other.sourceElement.Name, sourceElement.Name);
        }
       
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (SplitMapping)) return false;
            return Equals((SplitMapping) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((sourceElement != null ? sourceElement.GetHashCode() : 0)*397) ^
                       (targetCcElements != null ? targetCcElements.GetHashCode() : 0);
            }
        }

        public static bool operator ==(SplitMapping left, SplitMapping right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(SplitMapping left, SplitMapping right)
        {
            return !Equals(left, right);
        }
    }
}