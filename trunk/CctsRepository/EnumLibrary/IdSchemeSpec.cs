
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
    public partial class IdSchemeSpec
    {
		public string Name { get; set; }

		#region Tagged Values

        ///<summary>
        /// Tagged value 'businessTerm'.
        ///</summary>
		public IEnumerable<string> BusinessTerms { get; set; }

        ///<summary>
        /// Tagged value 'definition'.
        ///</summary>
		public string Definition { get; set; }

        ///<summary>
        /// Tagged value 'dictionaryEntryName'.
        ///</summary>
		public string DictionaryEntryName { get; set; }

        ///<summary>
        /// Tagged value 'identifierSchemeAgencyIdentifier'.
        ///</summary>
		public string IdentifierSchemeAgencyIdentifier { get; set; }

        ///<summary>
        /// Tagged value 'identifierSchemeAgencyName'.
        ///</summary>
		public string IdentifierSchemeAgencyName { get; set; }

        ///<summary>
        /// Tagged value 'modificationAllowedIndicator'.
        ///</summary>
		public bool ModificationAllowedIndicator { get; set; }

        ///<summary>
        /// Tagged value 'pattern'.
        ///</summary>
		public string Pattern { get; set; }

        ///<summary>
        /// Tagged value 'restrictedPrimitive'.
        ///</summary>
		public string RestrictedPrimitive { get; set; }

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

