// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System.Collections.Generic;

namespace CctsRepository
{
    public class BDTContentComponentSpec : CCTSElementSpec
    {
        public BDTContentComponentSpec(IBDTContentComponent bdtCon): base(bdtCon)
        {
            BasicType = bdtCon.BasicType;

            UpperBound = bdtCon.UpperBound;
            LowerBound = bdtCon.LowerBound;

            ModificationAllowedIndicator = bdtCon.ModificationAllowedIndicator;
            UsageRules = new List<string>(bdtCon.UsageRules);
            Pattern = bdtCon.Pattern;
            FractionDigits = bdtCon.FractionDigits;
            Length = bdtCon.Length;
            MaxExclusive = bdtCon.MaxExclusive;
            MaxInclusive = bdtCon.MaxInclusive;
            MaxLength = bdtCon.MaxLength;
            MinExclusive = bdtCon.MinExclusive;
            MinInclusive = bdtCon.MinInclusive;
            MinLength = bdtCon.MinLength;
            TotalDigits = bdtCon.TotalDigits;
            WhiteSpace = bdtCon.WhiteSpace;
        }

        public BDTContentComponentSpec(ICDTContentComponent cdtCon): base(cdtCon)
        {
            BasicType = cdtCon.BasicType;

            UpperBound = cdtCon.UpperBound;
            LowerBound = cdtCon.LowerBound;

            ModificationAllowedIndicator = cdtCon.ModificationAllowedIndicator;
            UsageRules = new List<string>(cdtCon.UsageRules);
        }

        public BDTContentComponentSpec()
        {
        }

        public IBasicType BasicType { get; set; }

        public string UpperBound { get; set; }
        public string LowerBound { get; set; }

        public bool ModificationAllowedIndicator { get; set; }
        public IEnumerable<string> UsageRules { get; set; }
        public string Pattern { get; set; }
        public string FractionDigits { get; set; }
        public string Length { get; set; }
        public string MaxExclusive { get; set; }
        public string MaxInclusive { get; set; }
        public string MaxLength { get; set; }
        public string MinExclusive { get; set; }
        public string MinInclusive { get; set; }
        public string MinLength { get; set; }
        public string TotalDigits { get; set; }
        public string WhiteSpace { get; set; }
    }
}