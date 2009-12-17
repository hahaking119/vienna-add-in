using System.Collections.Generic;
using CctsRepository.PrimLibrary;
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

        public IEnumerable<string> BusinessTerms
        {
            get { return UmlDataType.GetTaggedValue("businessTerm").SplitValues; }
        }

        public string Definition
        {
            get { return UmlDataType.GetTaggedValue("definition").Value; }
        }

        public string DictionaryEntryName
        {
            get { return UmlDataType.GetTaggedValue("dictionaryEntryName").Value; }
        }

        public string FractionDigits
        {
            get { return UmlDataType.GetTaggedValue("fractionDigits").Value; }
        }

        public string LanguageCode
        {
            get { return UmlDataType.GetTaggedValue("languageCode").Value; }
        }

        public string Length
        {
            get { return UmlDataType.GetTaggedValue("length").Value; }
        }

        public string MaximumExclusive
        {
            get { return UmlDataType.GetTaggedValue("maximumExclusive").Value; }
        }

        public string MaximumInclusive
        {
            get { return UmlDataType.GetTaggedValue("maximumInclusive").Value; }
        }

        public string MaximumLength
        {
            get { return UmlDataType.GetTaggedValue("maximumLength").Value; }
        }

        public string MinimumExclusive
        {
            get { return UmlDataType.GetTaggedValue("minimumExclusive").Value; }
        }

        public string MinimumInclusive
        {
            get { return UmlDataType.GetTaggedValue("minimumInclusive").Value; }
        }

        public string MinimumLength
        {
            get { return UmlDataType.GetTaggedValue("minimumLength").Value; }
        }

        public string Pattern
        {
            get { return UmlDataType.GetTaggedValue("pattern").Value; }
        }

        public string TotalDigits
        {
            get { return UmlDataType.GetTaggedValue("totalDigits").Value; }
        }

        public string UniqueIdentifier
        {
            get { return UmlDataType.GetTaggedValue("uniqueIdentifier").Value; }
        }

        public string VersionIdentifier
        {
            get { return UmlDataType.GetTaggedValue("versionIdentifier").Value; }
        }

        public string WhiteSpace
        {
            get { return UmlDataType.GetTaggedValue("whiteSpace").Value; }
        }

        #endregion
    }
}