namespace VIENNAAddIn.upcc3.ccts
{
    public interface IASBIE:IBIE
    {
        string SequencingKey { get; }
//        IASCC BasedOn { get; }
        IABIE AssociatedBIE { get; }
    }
}