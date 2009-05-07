// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts
{
    ///<summary>
    ///</summary>
    public interface ICCLibrary : IElementLibrary<IACC, ACCSpec>
    {
        ///<summary>
        ///</summary>
        IEnumerable<IACC> ACCs { get; }
    }
}