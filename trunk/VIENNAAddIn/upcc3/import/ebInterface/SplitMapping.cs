using System;
using System.Collections.Generic;
using System.Text;
using CctsRepository.CcLibrary;
using CctsRepository.CdtLibrary;

namespace VIENNAAddIn.upcc3.import.ebInterface
{
    public class SplitMapping : ElementMapping, IEquatable<SplitMapping>
    {
        private readonly SourceElement sourceElement;
        private readonly object[] targetCcs;

        public SplitMapping(SourceElement sourceElement, IEnumerable<object> targetCcs)
        {
            this.sourceElement = sourceElement;
            this.targetCcs = new List<object>(targetCcs).ToArray();
        }

        public override string BIEName
        {
            get { throw new NotImplementedException(); }
        }

        public override string ToString()
        {
            StringBuilder s = new StringBuilder();
            foreach (object targetCc in targetCcs)
            {
                if (targetCc is IBcc)
                {
                    s.Append("BCC[").Append(((IBcc)targetCc).Name).Append("],");
                } else if (targetCc is ICdtSup)
                {
                    s.Append("SUP[").Append(((ICdtSup)targetCc).Name).Append("],");
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

            if (other.targetCcs.Length != targetCcs.Length)
            {
                return false;
            }

            for (int i = 0; i < other.targetCcs.Length; i++ )
            {
                if (other.targetCcs[i] is IBcc)
                {
                    if (!(targetCcs[i] is IBcc))
                    {
                        return false;
                    }

                    if (((IBcc)other.targetCcs[i]).Id != ((IBcc)targetCcs[i]).Id)
                    {
                        return false;
                    }
                } 
                else if (other.targetCcs[i] is ICdtSup)
                {
                    if (!(targetCcs[i] is ICdtSup))
                    {
                        return false;
                    }

                    if (((ICdtSup)other.targetCcs[i]).Id != ((ICdtSup)targetCcs[i]).Id)
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
                       (targetCcs != null ? targetCcs.GetHashCode() : 0);
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