namespace CctsRepository
{
    public interface ICDTSupplementaryComponent : ICCTSElement, IHasUsageRules, IHasMultiplicity
    {
        ICDT CDT { get; }
        IBasicType BasicType { get; }
        bool ModificationAllowedIndicator { get; }
        bool IsOptional();
    }
}