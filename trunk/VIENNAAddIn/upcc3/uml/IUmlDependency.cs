namespace VIENNAAddIn.upcc3.uml
{
    public interface IUmlDependency<TTarget>
    {
        TTarget Target { get; }
    }
}