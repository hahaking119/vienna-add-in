namespace VIENNAAddIn.upcc3.ccts
{
    public interface IDTComponent : ICCTSElement, IHasUsageRules
    {
        IBasicType BasicType { get; }
        bool ModificationAllowedIndicator { get; }
        string UpperBound { get; }
        string LowerBound { get; }
        bool IsOptional();
        IDT DT { get; }
    }
}