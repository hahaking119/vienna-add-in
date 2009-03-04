namespace VIENNAAddIn.upcc3.ccts
{
    public interface IASBIE : IBIE, ISequenced
    {
        IABIE AssociatingElement { get; }
        IABIE AssociatedElement { get; }
    }
}