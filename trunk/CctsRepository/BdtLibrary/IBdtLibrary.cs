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

namespace CctsRepository.BdtLibrary
{
    public interface IBdtLibrary
    {
        int Id { get; }
        string Name { get; }

        IBLibrary BLibrary { get; }
        IEnumerable<IBdt> Bdts { get; }

        string Status { get; }
        string UniqueIdentifier { get; }
        string VersionIdentifier { get; }
        string BaseURN { get; }
        string NamespacePrefix { get; }
        IEnumerable<string> BusinessTerms { get; }
        IEnumerable<string> Copyrights { get; }
        IEnumerable<string> Owners { get; }
        IEnumerable<string> References { get; }

        IBdt GetBdtByName(string name);

        IBdt CreateBdt(BdtSpec spec);

        ///<summary>
        /// Updates the given BDT to match the given specification.
        ///</summary>
        IBdt UpdateBdt(IBdt element, BdtSpec spec);
    }
}