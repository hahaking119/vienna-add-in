// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
namespace CctsRepository
{
    public interface IPRIMRestrictions
    {
        [TaggedValue]
        string Pattern { get; }

        [TaggedValue]
        string FractionDigits { get; }

        [TaggedValue]
        string Length { get; }

        [TaggedValue]
        string MaxExclusive { get; }

        [TaggedValue]
        string MaxInclusive { get; }

        [TaggedValue]
        string MaxLength { get; }

        [TaggedValue]
        string MinExclusive { get; }

        [TaggedValue]
        string MinInclusive { get; }

        [TaggedValue]
        string MinLength { get; }

        [TaggedValue]
        string TotalDigits { get; }

        [TaggedValue]
        string WhiteSpace { get; }
    }
}