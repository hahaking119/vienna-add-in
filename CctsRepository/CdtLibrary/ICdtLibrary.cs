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
    public interface ICdtLibrary
    {
        IEnumerable<ICdt> Elements { get; }
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

        ///<summary>
        /// Retrieves an element by name.
        ///</summary>
        ICdt ElementByName(string name);

        ///<summary>
        /// Creates a new element in this library, based on the given specification.
        ///</summary>
        ICdt CreateElement(CdtSpec spec);

        ///<summary>
        /// Updates the given element of this library to match the given specification.
        ///</summary>
        ICdt UpdateElement(ICdt element, CdtSpec spec);
    }
}