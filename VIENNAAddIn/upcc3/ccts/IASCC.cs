namespace VIENNAAddIn.upcc3.ccts
{
    public interface IASCC : ICC
    {
        string SequencingKey { get; }
        IACC AssociatedCC { get; }
    }
}