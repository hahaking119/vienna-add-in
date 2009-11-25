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
    public interface IDOCLibrary
    {
        IEnumerable<IABIE> Elements { get; }

        ///<summary>
        /// Retrieves an element by name.
        ///</summary>
        IABIE ElementByName(string name);

        ///<summary>
        /// Creates a new element in this library, based on the given specification.
        ///</summary>
        IABIE CreateElement(ABIESpec spec);

        ///<summary>
        /// Updates the given element of this library to match the given specification.
        ///</summary>
        IABIE UpdateElement(IABIE element, ABIESpec spec);

        ///<summary>
        /// Returns the root elements of documents defined in this DOCLibrary.
        ///</summary>
        IEnumerable<IABIE> RootElements { get; }

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
    }
}