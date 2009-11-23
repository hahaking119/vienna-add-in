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
    public interface IBDT : ICCTSElement, IHasUsageRules
    {
        IBDTContentComponent CON { get; }
        IEnumerable<IBDTSupplementaryComponent> SUPs { get; }

        IBDT IsEquivalentTo { get; }
        ICDT BasedOn { get; }
    }
}