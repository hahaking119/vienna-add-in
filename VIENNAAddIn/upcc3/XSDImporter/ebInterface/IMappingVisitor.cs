namespace VIENNAAddIn.upcc3.XSDImporter.ebInterface
{
    public interface IMappingVisitor
    {
        void VisitBeforeChildren(IMapping mapping);
        void VisitAfterChildren(IMapping mapping);
    }
}