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
    public interface IBDTSupplementaryComponent : ICCTSElement, IHasUsageRules, IHasMultiplicity, IPRIMRestrictions
    {
        IBDT BDT { get; }
        IBasicType BasicType { get; }
        bool ModificationAllowedIndicator { get; }
        bool IsOptional();
    }
}