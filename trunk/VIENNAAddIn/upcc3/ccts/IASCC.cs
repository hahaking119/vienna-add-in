namespace VIENNAAddIn.upcc3.ccts
{
    public interface IASCC : ICC, ISequenced
    {
        IACC AssociatingElement { get; }
        IACC AssociatedElement { get; }
    }
}