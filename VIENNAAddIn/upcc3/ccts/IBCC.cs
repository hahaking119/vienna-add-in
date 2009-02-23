namespace VIENNAAddIn.upcc3.ccts
{
    public interface IBCC : ICC
    {
        string SequencingKey { get; }
        ICDT Type { get; }
    }
}