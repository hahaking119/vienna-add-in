namespace CctsRepository
{
    public interface IBDTContentComponent : ICCTSElement, IHasUsageRules, IHasMultiplicity, IPRIMRestrictions
    {
        IBDT BDT { get; }
        IBasicType BasicType { get; }
        bool ModificationAllowedIndicator { get; }
        bool IsOptional();
    }
}