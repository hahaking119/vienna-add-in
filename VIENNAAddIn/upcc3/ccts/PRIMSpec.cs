using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts
{
    public class PRIMSpec : CCTSElementSpec
    {
        public PRIMSpec(IPRIM prim) : base(prim)
        {
            Pattern = prim.Pattern;
            FractionDigits = prim.FractionDigits;
            Length = prim.Length;
            MaxExclusive = prim.MaxExclusive;
            MaxInclusive = prim.MaxInclusive;
            MaxLength = prim.MaxLength;
            MinExclusive = prim.MinExclusive;
            MinInclusive = prim.MinInclusive;
            MinLength = prim.MinLength;
            TotalDigits = prim.TotalDigits;
            WhiteSpace = prim.WhiteSpace;
            IsEquivalentTo = prim.IsEquivalentTo;
        }

        public PRIMSpec()
        {
        }

        [TaggedValue(TaggedValues.Pattern)]
        public string Pattern { get; set; }

        [TaggedValue(TaggedValues.FractionDigits)]
        public string FractionDigits { get; set; }

        [TaggedValue(TaggedValues.Length)]
        public string Length { get; set; }

        [TaggedValue(TaggedValues.MaxExclusive)]
        public string MaxExclusive { get; set; }

        [TaggedValue(TaggedValues.MaxInclusive)]
        public string MaxInclusive { get; set; }

        [TaggedValue(TaggedValues.MaxLength)]
        public string MaxLength { get; set; }

        [TaggedValue(TaggedValues.MinExclusive)]
        public string MinExclusive { get; set; }

        [TaggedValue(TaggedValues.MinInclusive)]
        public string MinInclusive { get; set; }

        [TaggedValue(TaggedValues.MinLength)]
        public string MinLength { get; set; }

        [TaggedValue(TaggedValues.TotalDigits)]
        public string TotalDigits { get; set; }

        [TaggedValue(TaggedValues.WhiteSpace)]
        public string WhiteSpace { get; set; }

        public IPRIM IsEquivalentTo { get; set; }
    }
}