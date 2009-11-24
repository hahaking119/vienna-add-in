// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using CctsRepository.BdtLibrary;
using CctsRepository.CcLibrary;

namespace CctsRepository.BieLibrary
{
    public interface IBBIE : IBIE
    {
        IBDT Type { get; }
        IABIE Container { get; }

        /// <summary>
        /// Returns the BCC on which the BBIE is based or <c>null</c>, if the BCC cannot be determined.
        /// </summary>
        IBCC BasedOn { get; }

        string UpperBound { get; }
        string LowerBound { get; }
        string SequencingKey { get; }
    }
}