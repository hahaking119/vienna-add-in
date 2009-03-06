using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts
{
    public interface IPRIMRestrictions
    {
        [TaggedValue(TaggedValues.Pattern)]
        string Pattern { get; }

        [TaggedValue(TaggedValues.FractionDigits)]
        string FractionDigits { get; }

        [TaggedValue(TaggedValues.Length)]
        string Length { get; }

        [TaggedValue(TaggedValues.MaxExclusive)]
        string MaxExclusive { get; }

        [TaggedValue(TaggedValues.MaxInclusive)]
        string MaxInclusive { get; }

        [TaggedValue(TaggedValues.MaxLength)]
        string MaxLength { get; }

        [TaggedValue(TaggedValues.MinExclusive)]
        string MinExclusive { get; }

        [TaggedValue(TaggedValues.MinInclusive)]
        string MinInclusive { get; }

        [TaggedValue(TaggedValues.MinLength)]
        string MinLength { get; }

        [TaggedValue(TaggedValues.TotalDigits)]
        string TotalDigits { get; }

        [TaggedValue(TaggedValues.WhiteSpace)]
        string WhiteSpace { get; }
    }
}