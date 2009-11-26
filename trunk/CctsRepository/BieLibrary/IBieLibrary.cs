// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System.Collections.Generic;
using CctsRepository.bLibrary;

namespace CctsRepository.BieLibrary
{
    public interface IBieLibrary
    {
        IEnumerable<IAbie> Elements { get; }
        int Id { get; }
        string Name { get; }
        IBLibrary Parent { get; }
        string Status { get; }
        string UniqueIdentifier { get; }
        string VersionIdentifier { get; }
        string BaseURN { get; }
        string NamespacePrefix { get; }
        IEnumerable<string> BusinessTerms { get; }
        IEnumerable<string> Copyrights { get; }
        IEnumerable<string> Owners { get; }
        IEnumerable<string> References { get; }

        ///<summary>
        /// Retrieves an element by name.
        ///</summary>
        IAbie ElementByName(string name);

        ///<summary>
        /// Creates a new element in this library, based on the given specification.
        ///</summary>
        IAbie CreateElement(AbieSpec spec);

        ///<summary>
        /// Updates the given element of this library to match the given specification.
        ///</summary>
        IAbie UpdateElement(IAbie element, AbieSpec spec);
    }
}