using System;
using System.Collections.Generic;
using System.Linq;
using CctsRepository.CcLibrary;
using CctsRepository.CdtLibrary;
using VIENNAAddInUtils;

namespace VIENNAAddIn.upcc3.import.ebInterface
{
    public abstract class ComplexTypeMapping : AbstractMapping
    {
        #region LibraryType enum

        public enum LibraryType
        {
            BIE,
            DOC
        }

        #endregion

        private readonly List<ElementMapping> children;

        protected ComplexTypeMapping(string complexTypeName, IEnumerable<ElementMapping> children)
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
                    if (child is AttributeOrSimpleElementOrComplexElementToBccMapping)
                    {
                        IAcc acc = ((AttributeOrSimpleElementOrComplexElementToBccMapping) child).Acc;
                        if (!targetACCs.Contains(acc))
                        {
                            targetACCs.Add(acc);
                        }
                    }
                    else if (child is ComplexElementToAsccMapping)
                    {
                        IAcc acc = ((ComplexElementToAsccMapping) child).Acc;
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

        public bool IsMappedToCdt
        {
            get { return TargetCdt != null; }
        }

        public ICdt TargetCdt
        {
            get
            {
                ICdt targetCdt = null;
                foreach (ElementMapping child in Children)
                {
                    if (child is AttributeOrSimpleElementToSupMapping)
                    {
                        ICdt cdt = ((AttributeOrSimpleElementToSupMapping)child).Cdt;
                        if (targetCdt == null)
                        {
                            targetCdt = cdt;
                        }
                        else
                        {
                            if (targetCdt != cdt)
                            {
                                throw new MappingError("Complex type mapped to more than one CDTs");
                            }
                        }
                    }
                }
                return targetCdt;
            }
        }

        public string ComplexTypeName { get; private set; }

        public IEnumerable<AsmaMapping> AsmaMappings
        {
            get { return Children.FilterByType<ElementMapping, AsmaMapping>(); }
        }

        public IEnumerable<AttributeOrSimpleElementOrComplexElementToBccMapping> BCCMappings(IAcc targetACC)
        {
            foreach (AttributeOrSimpleElementOrComplexElementToBccMapping bccMapping in Children.FilterByType<ElementMapping, AttributeOrSimpleElementOrComplexElementToBccMapping>())
            {
                IAcc acc = bccMapping.Acc;
                if (targetACC.Id == acc.Id)
                {
                    yield return bccMapping;
                }
            }
        }

        public IEnumerable<ComplexElementToAsccMapping> ASCCMappings(IAcc targetACC)
        {
            foreach (ComplexElementToAsccMapping asccMapping in Children.FilterByType<ElementMapping, ComplexElementToAsccMapping>())
            {
                IAcc acc = asccMapping.Acc;
                if (targetACC.Id == acc.Id)
                {
                    yield return asccMapping;
                }
            }
        }

        protected bool ChildrenEqual(ComplexTypeMapping other)
        {
            if (other.children.Count != children.Count) return false;
            for (int i = 0; i < children.Count; i++)
            {
                if (!Equals(other.children[i], children[i])) return false;
            }
            return true;
        }

        public static readonly ComplexTypeMapping NullComplexTypeMapping = new NullComplexTypeMapping();
    }
}