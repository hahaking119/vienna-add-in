// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System.Collections.Generic;

namespace CctsRepository
{
    public interface IENUM : IBasicType
    {
        string CodeListAgencyIdentifier { get; }
        string CodeListAgencyName { get; }
        string CodeListIdentifier { get; }
        string CodeListName { get; }
        string EnumerationURI { get; }
        bool ModificationAllowedIndicator { get; }
        string RestrictedPrimitive { get; }
        string Status { get; }

        IENUM IsEquivalentTo { get; }

        IEnumerable<ICodelistEntry> CodelistEntries { get; }
    }
}