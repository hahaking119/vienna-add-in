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
    public interface ICCTSElement
    {
        int Id { get; }
        string Name { get; }
        string DictionaryEntryName { get; }
        string Definition { get; }
        string UniqueIdentifier { get; }
        string VersionIdentifier { get; }
        string LanguageCode { get; }
        IEnumerable<string> BusinessTerms { get; }
        IBusinessLibrary Library { get; }
    }
}