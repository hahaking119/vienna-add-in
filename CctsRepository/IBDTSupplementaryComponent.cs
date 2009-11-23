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
    public interface IBDTSupplementaryComponent : ICCTSElement
    {
        IBDT BDT { get; }
        IBasicType BasicType { get; }
        bool ModificationAllowedIndicator { get; }
        string UpperBound { get; }
        string LowerBound { get; }
        IEnumerable<string> UsageRules { get; }
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
        bool IsOptional();
    }
}