// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System.Collections.Generic;
using CctsRepository.CdtLibrary;

namespace CctsRepository.CcLibrary
{
    public interface IBCC
    {
        int Id { get; }
        string Name { get; }

        string UpperBound { get; }
        string LowerBound { get; }

        string DictionaryEntryName { get; }
        string Definition { get; }
        string UniqueIdentifier { get; }
        string VersionIdentifier { get; }
        string LanguageCode { get; }
        IEnumerable<string> BusinessTerms { get; }
        string SequencingKey { get; }
        IEnumerable<string> UsageRules { get; }

        ICDT Cdt { get; }
        IACC Acc { get; }
    }
}