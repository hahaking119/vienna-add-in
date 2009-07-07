// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.ccts
{
    ///<summary>
    ///</summary>
    public interface ICCTSElement
    {
        ///<summary>
        ///</summary>
        int Id { get; }

        ///<summary>
        ///</summary>
        string GUID { get; }

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
        IBusinessLibrary Library { get; }
    }
}