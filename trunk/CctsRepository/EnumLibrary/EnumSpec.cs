
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
    public partial class EnumSpec
    {
		public string Name { get; set; }

		public IEnum IsEquivalentTo { get; set; }

		#region Tagged Values

        ///<summary>
        /// Tagged value 'businessTerm'.
        ///</summary>
		public IEnumerable<string> BusinessTerms { get; set; }

        ///<summary>
        /// Tagged value 'codeListAgencyIdentifier'.
        ///</summary>
		public string CodeListAgencyIdentifier { get; set; }

        ///<summary>
        /// Tagged value 'codeListAgencyName'.
        ///</summary>
		public string CodeListAgencyName { get; set; }

        ///<summary>
        /// Tagged value 'codeListIdentifier'.
        ///</summary>
		public string CodeListIdentifier { get; set; }

        ///<summary>
        /// Tagged value 'codeListName'.
        ///</summary>
		public string CodeListName { get; set; }

        ///<summary>
        /// Tagged value 'dictionaryEntryName'.
        ///</summary>
		public string DictionaryEntryName { get; set; }

        ///<summary>
        /// Tagged value 'definition'.
        ///</summary>
		public string Definition { get; set; }

        ///<summary>
        /// Tagged value 'enumerationURI'.
        ///</summary>
		public string EnumerationURI { get; set; }

        ///<summary>
        /// Tagged value 'languageCode'.
        ///</summary>
		public string LanguageCode { get; set; }

        ///<summary>
        /// Tagged value 'modificationAllowedIndicator'.
        ///</summary>
		public bool ModificationAllowedIndicator { get; set; }

        ///<summary>
        /// Tagged value 'restrictedPrimitive'.
        ///</summary>
		public string RestrictedPrimitive { get; set; }

        ///<summary>
        /// Tagged value 'status'.
        ///</summary>
		public string Status { get; set; }

        ///<summary>
        /// Tagged value 'uniqueIdentifier'.
        ///</summary>
		public string UniqueIdentifier { get; set; }

        ///<summary>
        /// Tagged value 'versionIdentifier'.
        ///</summary>
		public string VersionIdentifier { get; set; }

		#endregion
    }
}

