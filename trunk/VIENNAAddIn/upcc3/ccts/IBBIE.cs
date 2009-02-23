namespace VIENNAAddIn.upcc3.ccts
{
    public interface IBBIE:IBIE
    {
        string SequencingKey { get; }
        IBCC BasedOn { get; }
        IBDT Type { get; }
    }
}