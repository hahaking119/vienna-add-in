using System.Collections.Generic;
using CctsRepository.PrimLibrary;
using VIENNAAddIn.upcc3.uml;

namespace VIENNAAddIn.upcc3.repo
{
    internal class UpccPrim : IPrim
    {
        private readonly IUmlDataType umlDataType;

        public UpccPrim(IUmlDataType umlDataType)
        {
            this.umlDataType = umlDataType;
        }

        #region IPrim Members

        public int Id
        {
            get { return umlDataType.Id; }
        }

        public string Name
        {
            get { return umlDataType.Name; }
        }

		public IPrimLibrary PrimLibrary
        {
            get { return new UpccPrimLibrary(umlDataType.Package); }
        }

		public IPrim IsEquivalentTo
        {
            get
            {
                var dependency = umlDataType.GetFirstDependencyByStereotype("isEquivalentTo");
                return dependency == null ? null : new UpccPrim(dependency.Target);
            }
        }

        public IEnumerable<string> BusinessTerms
        {
            get { return umlDataType.GetTaggedValue("businessTerm").SplitValues; }
        }

        public string Definition
        {
            get { return umlDataType.GetTaggedValue("definition").Value; }
        }

        public string DictionaryEntryName
        {
            get { return umlDataType.GetTaggedValue("dictionaryEntryName").Value; }
        }

        public string FractionDigits
        {
            get { return umlDataType.GetTaggedValue("fractionDigits").Value; }
        }

        public string LanguageCode
        {
            get { return umlDataType.GetTaggedValue("languageCode").Value; }
        }

        public string Length
        {
            get { return umlDataType.GetTaggedValue("length").Value; }
        }

        public string MaximumExclusive
        {
            get { return umlDataType.GetTaggedValue("maximumExclusive").Value; }
        }

        public string MaximumInclusive
        {
            get { return umlDataType.GetTaggedValue("maximumInclusive").Value; }
        }

        public string MaximumLength
        {
            get { return umlDataType.GetTaggedValue("maximumLength").Value; }
        }

        public string MinimumExclusive
        {
            get { return umlDataType.GetTaggedValue("minimumExclusive").Value; }
        }

        public string MinimumInclusive
        {
            get { return umlDataType.GetTaggedValue("minimumInclusive").Value; }
        }

        public string MinimumLength
        {
            get { return umlDataType.GetTaggedValue("minimumLength").Value; }
        }

        public string Pattern
        {
            get { return umlDataType.GetTaggedValue("pattern").Value; }
        }

        public string TotalDigits
        {
            get { return umlDataType.GetTaggedValue("totalDigits").Value; }
        }

        public string UniqueIdentifier
        {
            get { return umlDataType.GetTaggedValue("uniqueIdentifier").Value; }
        }

        public string VersionIdentifier
        {
            get { return umlDataType.GetTaggedValue("versionIdentifier").Value; }
        }

        public string WhiteSpace
        {
            get { return umlDataType.GetTaggedValue("whiteSpace").Value; }
        }

        #endregion
    }
}