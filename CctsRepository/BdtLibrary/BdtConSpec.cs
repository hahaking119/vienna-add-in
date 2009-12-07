
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

namespace CctsRepository.BdtLibrary
{
    public partial class BdtConSpec
    {
		public string Name { get; set; }
		
        public string UpperBound { get; set; }
		
        public string LowerBound { get; set; }
		
		public BasicType BasicType { get; set; }

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
        /// Tagged value 'enumeration'.
        ///</summary>
		public string Enumeration { get; set; }

        ///<summary>
        /// Tagged value 'fractionDigits'.
        ///</summary>
		public string FractionDigits { get; set; }

        ///<summary>
        /// Tagged value 'languageCode'.
        ///</summary>
		public string LanguageCode { get; set; }

        ///<summary>
        /// Tagged value 'maximumExclusive'.
        ///</summary>
		public string MaximumExclusive { get; set; }

        ///<summary>
        /// Tagged value 'maximumInclusive'.
        ///</summary>
		public string MaximumInclusive { get; set; }

        ///<summary>
        /// Tagged value 'maximumLength'.
        ///</summary>
		public string MaximumLength { get; set; }

        ///<summary>
        /// Tagged value 'minimumExclusive'.
        ///</summary>
		public string MinimumExclusive { get; set; }

        ///<summary>
        /// Tagged value 'minimumInclusive'.
        ///</summary>
		public string MinimumInclusive { get; set; }

        ///<summary>
        /// Tagged value 'minimumLength'.
        ///</summary>
		public string MinimumLength { get; set; }

        ///<summary>
        /// Tagged value 'modificationAllowedIndicator'.
        ///</summary>
		public bool ModificationAllowedIndicator { get; set; }

        ///<summary>
        /// Tagged value 'pattern'.
        ///</summary>
		public string Pattern { get; set; }

        ///<summary>
        /// Tagged value 'totalDigits'.
        ///</summary>
		public string TotalDigits { get; set; }

        ///<summary>
        /// Tagged value 'uniqueIdentifier'.
        ///</summary>
		public string UniqueIdentifier { get; set; }

        ///<summary>
        /// Tagged value 'usageRule'.
        ///</summary>
		public IEnumerable<string> UsageRules { get; set; }

        ///<summary>
        /// Tagged value 'versionIdentifier'.
        ///</summary>
		public string VersionIdentifier { get; set; }

		#endregion
    }
}

