namespace VIENNAAddIn.upcc3.import.ebInterface
{
    public interface IMappingVisitor
    {
        void VisitBeforeChildren(IMapping mapping);
        void VisitAfterChildren(IMapping mapping);
    }
}