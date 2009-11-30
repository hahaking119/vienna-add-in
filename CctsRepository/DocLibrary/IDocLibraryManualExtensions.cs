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
        /// Returns the root ABIE of the document defined in this DOCLibrary.
        ///</summary>
        IAbie RootAbie { get; }

        ///<summary>
        /// Returns the ABIEs contained in this DOCLibrary that are not the root element of the defined document.
        ///</summary>
        IEnumerable<IAbie> NonRootAbies { get; }
    }
}