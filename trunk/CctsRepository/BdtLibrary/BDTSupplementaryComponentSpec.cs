// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System.Collections.Generic;
using CctsRepository.cdt;

namespace CctsRepository.bdt
{
    public class BDTSupplementaryComponentSpec : CCTSElementSpec
    {
        public BDTSupplementaryComponentSpec(ICDTSupplementaryComponent cdtSup) : base(cdtSup)
        {
            BasicType = cdtSup.BasicType;

            UpperBound = cdtSup.UpperBound;
            LowerBound = cdtSup.LowerBound;

            ModificationAllowedIndicator = cdtSup.ModificationAllowedIndicator;
            UsageRules = new List<string>(cdtSup.UsageRules);
        }

        public BDTSupplementaryComponentSpec(IBDTSupplementaryComponent bdtSup) : base(bdtSup)
        {
            BasicType = bdtSup.BasicType;

            UpperBound = bdtSup.UpperBound;
            LowerBound = bdtSup.LowerBound;

            ModificationAllowedIndicator = bdtSup.ModificationAllowedIndicator;
            UsageRules = new List<string>(bdtSup.UsageRules);
            Pattern = bdtSup.Pattern;
            FractionDigits = bdtSup.FractionDigits;
            Length = bdtSup.Length;
            MaxExclusive = bdtSup.MaxExclusive;
            MaxInclusive = bdtSup.MaxInclusive;
            MaxLength = bdtSup.MaxLength;
            MinExclusive = bdtSup.MinExclusive;
            MinInclusive = bdtSup.MinInclusive;
            MinLength = bdtSup.MinLength;
            TotalDigits = bdtSup.TotalDigits;
            WhiteSpace = bdtSup.WhiteSpace;
        }

        public BDTSupplementaryComponentSpec()
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