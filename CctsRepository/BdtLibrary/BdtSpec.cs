
// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System.Collections.Generic;
using VIENNAAddInUtils;
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
    public partial class BdtSpec
    {
		public string Name { get; set; }

		public IBdt IsEquivalentTo { get; set; }

		public ICdt BasedOn { get; set; }

		public BdtConSpec Con { get; set; }

		public IEnumerable<BdtSupSpec> Sups { get; set; }

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
        /// Tagged value 'languageCode'.
        ///</summary>
		public string LanguageCode { get; set; }

        ///<summary>
        /// Tagged value 'uniqueIdentifier'.
        ///</summary>
		public string UniqueIdentifier { get; set; }

        ///<summary>
        /// Tagged value 'versionIdentifier'.
        ///</summary>
		public string VersionIdentifier { get; set; }

        ///<summary>
        /// Tagged value 'usageRule'.
        ///</summary>
		public IEnumerable<string> UsageRules { get; set; }

		#endregion

        public static BdtSpec CloneBdt(IBdt bdt)
        {
            return new BdtSpec
                   {
                   	   Name = bdt.Name,
					   IsEquivalentTo = bdt.IsEquivalentTo,
					   BasedOn = bdt.BasedOn,
					   Con = BdtConSpec.CloneBdtCon(bdt.Con),
					   Sups = new List<BdtSupSpec>(bdt.Sups.Convert(o => BdtSupSpec.CloneBdtSup(o))),
					   BusinessTerms = new List<string>(bdt.BusinessTerms),
					   Definition = bdt.Definition,
					   DictionaryEntryName = bdt.DictionaryEntryName,
					   LanguageCode = bdt.LanguageCode,
					   UniqueIdentifier = bdt.UniqueIdentifier,
					   VersionIdentifier = bdt.VersionIdentifier,
					   UsageRules = new List<string>(bdt.UsageRules),
                   };
        }
	}
}

