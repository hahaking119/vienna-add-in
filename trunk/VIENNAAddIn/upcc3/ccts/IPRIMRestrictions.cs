// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
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