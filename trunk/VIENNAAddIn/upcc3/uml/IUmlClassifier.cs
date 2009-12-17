namespace VIENNAAddIn.upcc3.uml
{
    public interface IUmlClassifier
    {
        int Id { get; }
        string GUID { get; }
        string Name { get; }
        IUmlPackage Package { get; }
        IUmlTaggedValue GetTaggedValue(string name);
    }
}