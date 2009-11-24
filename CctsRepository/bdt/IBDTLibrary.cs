// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System.Collections.Generic;

namespace CctsRepository.bdt
{
    public interface IBDTLibrary : IBusinessLibrary
    {
        IEnumerable<IBDT> Elements { get; }

        ///<summary>
        /// Retrieves an element by name.
        ///</summary>
        IBDT ElementByName(string name);

        ///<summary>
        /// Creates a new element in this library, based on the given specification.
        ///</summary>
        IBDT CreateElement(BDTSpec spec);

        ///<summary>
        /// Updates the given element of this library to match the given specification.
        ///</summary>
        IBDT UpdateElement(IBDT element, BDTSpec spec);
    }
}