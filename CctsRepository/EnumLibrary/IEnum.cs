// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System.Collections.Generic;

namespace CctsRepository.EnumLibrary
{
    public interface IEnum : IBasicType
    {
        string CodeListAgencyIdentifier { get; }
        string CodeListAgencyName { get; }
        string CodeListIdentifier { get; }
        string CodeListName { get; }
        string EnumerationURI { get; }
        bool ModificationAllowedIndicator { get; }
        string RestrictedPrimitive { get; }
        string Status { get; }

        IEnum IsEquivalentTo { get; }

        IEnumerable<ICodelistEntry> CodelistEntries { get; }

        ///<summary>
        ///</summary>
        string Definition { get; }

        ///<summary>
        ///</summary>
        string UniqueIdentifier { get; }

        ///<summary>
        ///</summary>
        string VersionIdentifier { get; }

        ///<summary>
        ///</summary>
        string LanguageCode { get; }

        ///<summary>
        ///</summary>
        IEnumerable<string> BusinessTerms { get; }

        ///<summary>
        ///</summary>
        IEnumLibrary Library { get; }
    }
}