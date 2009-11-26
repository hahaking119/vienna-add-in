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
using CctsRepository.bLibrary;

namespace CctsRepository.DocLibrary
{
    public interface IDocLibrary
    {
        IEnumerable<IAbie> Elements { get; }

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

        ///<summary>
        /// Returns the root elements of documents defined in this DOCLibrary.
        ///</summary>
        IEnumerable<IAbie> RootElements { get; }

        int Id { get; }
        string Name { get; }
        IBLibrary BLibrary { get; }
        string Status { get; }
        string UniqueIdentifier { get; }
        string VersionIdentifier { get; }
        string BaseURN { get; }
        string NamespacePrefix { get; }
        IEnumerable<string> BusinessTerms { get; }
        IEnumerable<string> Copyrights { get; }
        IEnumerable<string> Owners { get; }
        IEnumerable<string> References { get; }
    }
}