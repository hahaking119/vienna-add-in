// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System.Collections.Generic;
using EA;

namespace VIENNAAddIn.upcc3.ccts.util
{
    ///<summary>
    /// Extension methods for EA.<see cref="Collection"/>.
    ///</summary>
    public static class CollectionExtensions
    {
        ///<summary>
        /// Access the collection's contents via <see cref="IEnumerable{T}"/>.
        ///</summary>
        ///<param name="collection">A collection.</param>
        ///<typeparam name="T">The type of elements contained in the collection.</typeparam>
        ///<returns></returns>
        public static IEnumerable<T> AsEnumerable<T>(this Collection collection)
        {
            foreach (T item in collection)
            {
                yield return item;
            }
        }
    }
}