// ReSharper disable RedundantUsingDirective
using CctsRepository;
using CctsRepository.BdtLibrary;
using CctsRepository.BieLibrary;
using CctsRepository.BLibrary;
using CctsRepository.CcLibrary;
using CctsRepository.CdtLibrary;
using CctsRepository.DocLibrary;
using CctsRepository.EnumLibrary;
using CctsRepository.PrimLibrary;
// ReSharper restore RedundantUsingDirective
using System.Collections.Generic;
using VIENNAAddIn.upcc3.uml;

namespace VIENNAAddIn.upcc3.repo
{
    internal class UpccIdScheme : IIdScheme
    {
        public UpccIdScheme(IUmlDataType umlDataType)
        {
            UmlDataType = umlDataType;
        }

        public IUmlDataType UmlDataType { get; private set; }

        #region IIdScheme Members

        public int Id
        {
            get { return UmlDataType.Id; }
        }

        public string Name
        {
            get { return UmlDataType.Name; }
        }

		public IEnumLibrary EnumLibrary
        {
            get { return new UpccEnumLibrary(UmlDataType.Package); }
        }

        ///<summary>
        /// Tagged value 'businessTerm'.
        ///</summary>
        public IEnumerable<string> BusinessTerms
        {
            get { return UmlDataType.GetTaggedValue("businessTerm").SplitValues; }
        }

        ///<summary>
        /// Tagged value 'definition'.
        ///</summary>
        public string Definition
        {
            get { return UmlDataType.GetTaggedValue("definition").Value; }
        }

        ///<summary>
        /// Tagged value 'dictionaryEntryName'.
        ///</summary>
        public string DictionaryEntryName
        {
            get { return UmlDataType.GetTaggedValue("dictionaryEntryName").Value; }
        }

        ///<summary>
        /// Tagged value 'identifierSchemeAgencyIdentifier'.
        ///</summary>
        public string IdentifierSchemeAgencyIdentifier
        {
            get { return UmlDataType.GetTaggedValue("identifierSchemeAgencyIdentifier").Value; }
        }

        ///<summary>
        /// Tagged value 'identifierSchemeAgencyName'.
        ///</summary>
        public string IdentifierSchemeAgencyName
        {
            get { return UmlDataType.GetTaggedValue("identifierSchemeAgencyName").Value; }
        }

        ///<summary>
        /// Tagged value 'modificationAllowedIndicator'.
        ///</summary>
        public string ModificationAllowedIndicator
        {
            get { return UmlDataType.GetTaggedValue("modificationAllowedIndicator").Value; }
        }

        ///<summary>
        /// Tagged value 'pattern'.
        ///</summary>
        public string Pattern
        {
            get { return UmlDataType.GetTaggedValue("pattern").Value; }
        }

        ///<summary>
        /// Tagged value 'restrictedPrimitive'.
        ///</summary>
        public string RestrictedPrimitive
        {
            get { return UmlDataType.GetTaggedValue("restrictedPrimitive").Value; }
        }

        ///<summary>
        /// Tagged value 'uniqueIdentifier'.
        ///</summary>
        public string UniqueIdentifier
        {
            get { return UmlDataType.GetTaggedValue("uniqueIdentifier").Value; }
        }

        ///<summary>
        /// Tagged value 'versionIdentifier'.
        ///</summary>
        public string VersionIdentifier
        {
            get { return UmlDataType.GetTaggedValue("versionIdentifier").Value; }
        }

        #endregion
    }
}
