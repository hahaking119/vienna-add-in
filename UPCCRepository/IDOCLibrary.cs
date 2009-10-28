// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System.Collections.Generic;

namespace UPCCRepositoryInterface
{
    ///<summary>
    ///</summary>
    public interface IDOCLibrary : IElementLibrary<IABIE, ABIESpec>
    {
        ///<summary>
        /// Returns the root elements of documents defined in this DOCLibrary.
        ///</summary>
        IEnumerable<IABIE> RootElements { get; }
    }
}