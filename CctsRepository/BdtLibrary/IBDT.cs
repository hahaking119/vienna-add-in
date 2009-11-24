// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System.Collections.Generic;
using CctsRepository.CdtLibrary;

namespace CctsRepository.BdtLibrary
{
    public interface IBDT : ICCTSElement
    {
        IBDTContentComponent CON { get; }
        IEnumerable<IBDTSupplementaryComponent> SUPs { get; }

        IBDT IsEquivalentTo { get; }
        ICDT BasedOn { get; }
        IEnumerable<string> UsageRules { get; }
    }
}