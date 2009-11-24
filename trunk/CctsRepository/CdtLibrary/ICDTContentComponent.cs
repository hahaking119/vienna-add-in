// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System.Collections.Generic;

namespace CctsRepository.CdtLibrary
{
    public interface ICDTContentComponent : ICCTSElement
    {
        ICDT CDT { get; }
        IBasicType BasicType { get; }
        bool ModificationAllowedIndicator { get; }
        string UpperBound { get; }
        string LowerBound { get; }
        IEnumerable<string> UsageRules { get; }
        bool IsOptional();
    }
}