// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System.Collections.Generic;
using CctsRepository.BieLibrary;

namespace CctsRepository.DocLibrary
{
    public partial interface IDocLibrary
    {
        ///<summary>
        /// Returns the root ABIEs of documents defined in this DOCLibrary.
        ///</summary>
        IEnumerable<IAbie> RootAbies { get; }
    }
}