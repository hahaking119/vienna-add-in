// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System.Collections.Generic;
using CctsRepository;

namespace CctsRepository
{
    public interface IDT : ICCTSElement, IHasUsageRules
    {
        IEnumerable<ISUP> SUPs { get; }
        ICON CON { get; }
    }
}