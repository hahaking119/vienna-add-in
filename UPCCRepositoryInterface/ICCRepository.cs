// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System.Collections.Generic;
using VIENNAAddInUtils;

namespace UPCCRepositoryInterface
{
    ///<summary>
    ///</summary>
    public interface ICCRepository
    {
        IBusinessLibrary GetLibrary(int id);
        IEnumerable<IBusinessLibrary> AllLibraries();
        IEnumerable<T> Libraries<T>() where T : IBusinessLibrary;
        T LibraryByName<T>(string name) where T : IBusinessLibrary;
        object FindByPath(Path path);
    }
}