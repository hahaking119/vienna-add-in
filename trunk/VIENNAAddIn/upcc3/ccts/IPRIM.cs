namespace VIENNAAddIn.upcc3.ccts
{
    public interface IPRIM : IBasicType
    {
        string Pattern { get; }
        string FractionDigits { get; }
        string Length { get; }
        string MaxExclusive { get; }
        string MaxInclusive { get; }
        string MaxLength { get; }
        string MinExclusive { get; }
        string MinInclusive { get; }
        string MinLength { get; }
        string TotalDigits { get; }
        string WhiteSpace { get; }
    }
}