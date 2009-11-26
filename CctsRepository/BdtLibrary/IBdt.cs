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

namespace CctsRepository.BdtLibrary
{
    public interface IBdt
    {
        int Id { get; }
        string Name { get; }

        IBdtLibrary BdtLibrary { get; }

        IBdtCon Con { get; }
        IEnumerable<IBdtSup> Sups { get; }

        IBdt IsEquivalentTo { get; }
        ICdt BasedOn { get; }

        #region Tagged Values

        string DictionaryEntryName { get; }
        string Definition { get; }
        string UniqueIdentifier { get; }
        string VersionIdentifier { get; }
        string LanguageCode { get; }
        IEnumerable<string> BusinessTerms { get; }
        IEnumerable<string> UsageRules { get; }

        #endregion
    }
}