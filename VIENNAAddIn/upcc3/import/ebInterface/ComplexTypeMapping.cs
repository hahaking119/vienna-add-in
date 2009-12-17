using System;
using System.Collections.Generic;
using System.Linq;
using CctsRepository.CcLibrary;
using VIENNAAddInUtils;

namespace VIENNAAddIn.upcc3.import.ebInterface
{
    public class ComplexTypeMapping : AbstractMapping, IEquatable<ComplexTypeMapping>
    {
        #region LibraryType enum

        public enum LibraryType
        {
            BIE,
            DOC
        }

        #endregion

        private readonly List<ElementMapping> children;

        public ComplexTypeMapping(string complexTypeName, IEnumerable<ElementMapping> children)
        {
            ComplexTypeName = complexTypeName;
            this.children = new List<ElementMapping>(children);
        }

        public LibraryType Library
        {
            get { return (IsMappedToSingleACC ? LibraryType.BIE : LibraryType.DOC); }
        }

        protected override IEnumerable<ElementMapping> Children
        {
            get { return children; }
        }

        public bool IsMappedToSingleACC
        {
            get { return TargetACCs.Count() == 1 && AsmaMappings.Count() == 0; }
        }

        public IEnumerable<IAcc> TargetACCs
        {
            get
            {
                var targetACCs = new List<IAcc>();
                foreach (ElementMapping child in Children)
                {
                    if (child is BCCMapping)
                    {
                        IAcc acc = ((BCCMapping) child).ACC;
                        if (!targetACCs.Contains(acc))
                        {
                            targetACCs.Add(acc);
                        }
                    }
                    else if (child is ASCCMapping)
                    {
                        IAcc acc = ((ASCCMapping) child).ACC;
                        if (!targetACCs.Contains(acc))
                        {
                            targetACCs.Add(acc);
                        }
                    }
                    // else ignore child
                }
                return targetACCs;
            }
        }

        public string ComplexTypeName { get; private set; }

        public IEnumerable<AsmaMapping> AsmaMappings
        {
            get { return Children.FilterByType<ElementMapping, AsmaMapping>(); }
        }

        public override string BIEName
        {
            get { return ComplexTypeName; }
        }

        public IEnumerable<BCCMapping> BCCMappings(IAcc targetACC)
        {
            foreach (BCCMapping bccMapping in Children.FilterByType<ElementMapping, BCCMapping>())
            {
                IAcc acc = bccMapping.ACC;
                if (targetACC.Id == acc.Id)
                {
                    yield return bccMapping;
                }
            }
        }

        public IEnumerable<ASCCMapping> ASCCMappings(IAcc targetACC)
        {
            foreach (ASCCMapping asccMapping in Children.FilterByType<ElementMapping, ASCCMapping>())
            {
                IAcc acc = asccMapping.ACC;
                if (targetACC.Id == acc.Id)
                {
                    yield return asccMapping;
                }
            }
        }

        public override string ToString()
        {
            return string.Format("ComplexTypeMapping <ComplexType: {0}>", ComplexTypeName);
        }

        public bool Equals(ComplexTypeMapping other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return ChildrenEqual(other) && Equals(other.ComplexTypeName, ComplexTypeName);
        }

        private bool ChildrenEqual(ComplexTypeMapping other)
        {
            if (other.children.Count != children.Count) return false;
            for (int i = 0; i < children.Count; i++)
            {
                if (!Equals(other.children[i], children[i])) return false;
            }
            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (ComplexTypeMapping)) return false;
            return Equals((ComplexTypeMapping) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ComplexTypeName != null ? ComplexTypeName.GetHashCode() : 0;
            }
        }

        public static bool operator ==(ComplexTypeMapping left, ComplexTypeMapping right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ComplexTypeMapping left, ComplexTypeMapping right)
        {
            return !Equals(left, right);
        }
    }
}