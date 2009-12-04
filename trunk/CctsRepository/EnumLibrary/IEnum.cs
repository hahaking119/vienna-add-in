
// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System.Collections.Generic;
// ReSharper disable RedundantUsingDirective
using CctsRepository.BdtLibrary;
using CctsRepository.BieLibrary;
using CctsRepository.BLibrary;
using CctsRepository.CcLibrary;
using CctsRepository.CdtLibrary;
using CctsRepository.DocLibrary;
using CctsRepository.EnumLibrary;
using CctsRepository.PrimLibrary;
// ReSharper restore RedundantUsingDirective

namespace CctsRepository.EnumLibrary
{
    public interface IEnum
    {
		int Id { get; }
		
		string Name { get; }
		
        IEnumLibrary EnumLibrary { get; }

		IEnum IsEquivalentTo { get; }

		IEnumerable<ICodelistEntry> CodelistEntries { get; }

		#region Tagged Values

        ///<summary>
        /// Tagged value 'businessTerm'.
        ///</summary>
		IEnumerable<string> BusinessTerms { get; }

        ///<summary>
        /// Tagged value 'codeListAgencyIdentifier'.
        ///</summary>
		string CodeListAgencyIdentifier { get; }

        ///<summary>
        /// Tagged value 'codeListAgencyName'.
        ///</summary>
		string CodeListAgencyName { get; }

        ///<summary>
        /// Tagged value 'codeListIdentifier'.
        ///</summary>
		string CodeListIdentifier { get; }

        ///<summary>
        /// Tagged value 'codeListName'.
        ///</summary>
		string CodeListName { get; }

        ///<summary>
        /// Tagged value 'dictionaryEntryName'.
        ///</summary>
		string DictionaryEntryName { get; }

        ///<summary>
        /// Tagged value 'definition'.
        ///</summary>
		string Definition { get; }

        ///<summary>
        /// Tagged value 'enumerationURI'.
        ///</summary>
		string EnumerationURI { get; }

        ///<summary>
        /// Tagged value 'languageCode'.
        ///</summary>
		string LanguageCode { get; }

        ///<summary>
        /// Tagged value 'modificationAllowedIndicator'.
        ///</summary>
		bool ModificationAllowedIndicator { get; }

        ///<summary>
        /// Tagged value 'restrictedPrimitive'.
        ///</summary>
		string RestrictedPrimitive { get; }

        ///<summary>
        /// Tagged value 'status'.
        ///</summary>
		string Status { get; }

        ///<summary>
        /// Tagged value 'uniqueIdentifier'.
        ///</summary>
		string UniqueIdentifier { get; }

        ///<summary>
        /// Tagged value 'versionIdentifier'.
        ///</summary>
		string VersionIdentifier { get; }

		#endregion
    }
}

