// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System.Collections.Generic;

namespace UPCCRepositoryInterface
{
    public abstract class DTComponentSpec : CCTSElementSpec
    {
        protected DTComponentSpec(IDTComponent dtComponent) : base(dtComponent)
        {
            BasicType = dtComponent.BasicType;
            ModificationAllowedIndicator = dtComponent.ModificationAllowedIndicator;
            UsageRules = new List<string>(dtComponent.UsageRules);
            Pattern = dtComponent.Pattern;
            FractionDigits = dtComponent.FractionDigits;
            Length = dtComponent.Length;
            MaxExclusive = dtComponent.MaxExclusive;
            MaxInclusive = dtComponent.MaxInclusive;
            MaxLength = dtComponent.MaxLength;
            MinExclusive = dtComponent.MinExclusive;
            MinInclusive = dtComponent.MinInclusive;
            MinLength = dtComponent.MinLength;
            TotalDigits = dtComponent.TotalDigits;
            WhiteSpace = dtComponent.WhiteSpace;
            UpperBound = dtComponent.UpperBound;
            LowerBound = dtComponent.LowerBound;
        }

        protected DTComponentSpec()
        {
        }

        public IBasicType BasicType { get; set; }

        [TaggedValue]
        public bool ModificationAllowedIndicator { get; set; }

        [TaggedValue]
        public IEnumerable<string> UsageRules { get; set; }

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

        public string UpperBound { get; set; }
        public string LowerBound { get; set; }
    }
}