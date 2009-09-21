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

        [TaggedValue]
        public string Pattern { get; set; }

        [TaggedValue]
        public string FractionDigits { get; set; }

        [TaggedValue]
        public string Length { get; set; }

        [TaggedValue]
        public string MaxExclusive { get; set; }

        [TaggedValue]
        public string MaxInclusive { get; set; }

        [TaggedValue]
        public string MaxLength { get; set; }

        [TaggedValue]
        public string MinExclusive { get; set; }

        [TaggedValue]
        public string MinInclusive { get; set; }

        [TaggedValue]
        public string MinLength { get; set; }

        [TaggedValue]
        public string TotalDigits { get; set; }

        [TaggedValue]
        public string WhiteSpace { get; set; }

        [Dependency]
        public IPRIM IsEquivalentTo { get; set; }
    }
}