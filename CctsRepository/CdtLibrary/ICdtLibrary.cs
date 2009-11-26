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

namespace CctsRepository.CdtLibrary
{
    public interface ICDTLibrary
    {
        IEnumerable<ICDT> Elements { get; }
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
        ICDT ElementByName(string name);

        ///<summary>
        /// Creates a new element in this library, based on the given specification.
        ///</summary>
        ICDT CreateElement(CDTSpec spec);

        ///<summary>
        /// Updates the given element of this library to match the given specification.
        ///</summary>
        ICDT UpdateElement(ICDT element, CDTSpec spec);
    }
}