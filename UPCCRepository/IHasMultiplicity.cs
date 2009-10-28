namespace UPCCRepositoryInterface
{
    public interface IHasMultiplicity
    {
        string UpperBound { get; }
        string LowerBound { get; }
    }
}