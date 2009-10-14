namespace VIENNAAddIn.upcc3.import.ebInterface
{
    public interface IMapping
    {
        void TraverseDepthFirst(IMappingVisitor visitor);
        string BIEName { get; }
    }
}