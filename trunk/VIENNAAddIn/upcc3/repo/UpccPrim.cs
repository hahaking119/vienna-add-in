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
    internal class UpccPrim : IPrim
    {
        public UpccPrim(IUmlDataType umlDataType)
        {
            UmlDataType = umlDataType;
        }

        public IUmlDataType UmlDataType { get; private set; }

        #region IPrim Members

        public int Id
        {
            get { return UmlDataType.Id; }
        }

        public string Name
        {
            get { return UmlDataType.Name; }
        }

		public IPrimLibrary PrimLibrary
        {
            get { return new UpccPrimLibrary(UmlDataType.Package); }
        }

		public IPrim IsEquivalentTo
        {
            get
            {
                var dependency = UmlDataType.GetFirstDependencyByStereotype("isEquivalentTo");
                return dependency == null ? null : new UpccPrim(dependency.Target);
            }
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
        /// Tagged value 'fractionDigits'.
        ///</summary>
        public string FractionDigits
        {
            get { return UmlDataType.GetTaggedValue("fractionDigits").Value; }
        }

        ///<summary>
        /// Tagged value 'languageCode'.
        ///</summary>
        public string LanguageCode
        {
            get { return UmlDataType.GetTaggedValue("languageCode").Value; }
        }

        ///<summary>
        /// Tagged value 'length'.
        ///</summary>
        public string Length
        {
            get { return UmlDataType.GetTaggedValue("length").Value; }
        }

        ///<summary>
        /// Tagged value 'maximumExclusive'.
        ///</summary>
        public string MaximumExclusive
        {
            get { return UmlDataType.GetTaggedValue("maximumExclusive").Value; }
        }

        ///<summary>
        /// Tagged value 'maximumInclusive'.
        ///</summary>
        public string MaximumInclusive
        {
            get { return UmlDataType.GetTaggedValue("maximumInclusive").Value; }
        }

        ///<summary>
        /// Tagged value 'maximumLength'.
        ///</summary>
        public string MaximumLength
        {
            get { return UmlDataType.GetTaggedValue("maximumLength").Value; }
        }

        ///<summary>
        /// Tagged value 'minimumExclusive'.
        ///</summary>
        public string MinimumExclusive
        {
            get { return UmlDataType.GetTaggedValue("minimumExclusive").Value; }
        }

        ///<summary>
        /// Tagged value 'minimumInclusive'.
        ///</summary>
        public string MinimumInclusive
        {
            get { return UmlDataType.GetTaggedValue("minimumInclusive").Value; }
        }

        ///<summary>
        /// Tagged value 'minimumLength'.
        ///</summary>
        public string MinimumLength
        {
            get { return UmlDataType.GetTaggedValue("minimumLength").Value; }
        }

        ///<summary>
        /// Tagged value 'pattern'.
        ///</summary>
        public string Pattern
        {
            get { return UmlDataType.GetTaggedValue("pattern").Value; }
        }

        ///<summary>
        /// Tagged value 'totalDigits'.
        ///</summary>
        public string TotalDigits
        {
            get { return UmlDataType.GetTaggedValue("totalDigits").Value; }
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

        ///<summary>
        /// Tagged value 'whiteSpace'.
        ///</summary>
        public string WhiteSpace
        {
            get { return UmlDataType.GetTaggedValue("whiteSpace").Value; }
        }

        #endregion
    }
}
