namespace VIENNAAddIn.upcc3.ccts
{
    public interface IBCC : ICC, ISequenced
    {
        ICDT Type { get; }
        IACC Container { get; }
    }
}