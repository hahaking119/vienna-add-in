// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System.Collections.Generic;
using CctsRepository.CcLibrary;

namespace CctsRepository.BieLibrary
{
    public interface IAbie
    {
        IEnumerable<IBbie> BBIEs { get; }
        IEnumerable<IAsbie> ASBIEs { get; }
        IACC BasedOn { get; }
        IAbie IsEquivalentTo { get; }
        IEnumerable<string> UsageRules { get; }

        ///<summary>
        ///</summary>
        int Id { get; }

        ///<summary>
        ///</summary>
        string Name { get; }

        ///<summary>
        ///</summary>
        string DictionaryEntryName { get; }

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
        IBieLibrary Library { get; }
    }
}