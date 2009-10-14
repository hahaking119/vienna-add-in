using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.XSDImporter.ebInterface
{
    public abstract class AbstractMapping : IMapping
    {
        public void TraverseDepthFirst(IMappingVisitor visitor)
        {
            visitor.VisitBeforeChildren(this);
            foreach (var child in Children)
            {
                child.TraverseDepthFirst(visitor);
            }
            visitor.VisitAfterChildren(this);
        }

        public abstract string BIEName { get; }

        protected abstract IEnumerable<ElementMapping> Children { get; }
    }
}