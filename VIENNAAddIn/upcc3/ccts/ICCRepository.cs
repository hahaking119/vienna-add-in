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
    public interface ICCRepository
    {
        IBusinessLibrary GetLibrary(int id);
        IEnumerable<IBusinessLibrary> AllLibraries();
        IEnumerable<T> Libraries<T>() where T : IBusinessLibrary;
        T LibraryByName<T>(string name) where T : IBusinessLibrary;
        object FindByPath(Path path);

        ///<summary>
        ///</summary>
        ///<param name="item"></param>
        ///<param name="spec"></param>
        ///<typeparam name="TItem"></typeparam>
        ///<typeparam name="TSpec"></typeparam>
        ///<returns></returns>
        TItem Update<TItem, TSpec>(TItem item, TSpec spec);
    }
}