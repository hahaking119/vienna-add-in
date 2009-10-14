namespace VIENNAAddIn.upcc3.XSDImporter.ebInterface
{
    public interface IMapping
    {
        void TraverseDepthFirst(IMappingVisitor visitor);
        string BIEName { get; }
    }
}