namespace VIENNAAddIn.upcc3.ccts
{
    public interface IBBIE : IBIE, ISequenced
    {
        IBDT Type { get; }
        IABIE Container { get; }
    }
}