using System.Collections.Generic;
using EA;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    internal class PRIM : UpccElement, IPRIM
    {
        private readonly CCRepository repository;

        public PRIM(CCRepository repository, Element element) : base(element, "PRIM")
        {
            this.repository = repository;
        }

        #region IPRIM Members

        public int Id
        {
            get { return element.ElementID; }
        }

        public string Name
        {
            get { return element.Name; }
        }

        public IBusinessLibrary Library
        {
            get { return repository.GetLibrary(element.PackageID); }
        }

        public IEnumerable<string> BusinessTerms
        {
            get { return element.GetTaggedValues(TaggedValues.BusinessTerm); }
        }

        public string Definition
        {
            get { return element.GetTaggedValue(TaggedValues.Definition); }
        }

        public string DictionaryEntryName
        {
            get { return element.GetTaggedValue(TaggedValues.DictionaryEntryName); }
        }

        public string Pattern
        {
            get { return element.GetTaggedValue(TaggedValues.Pattern); }
        }

        public string FractionDigits
        {
            get { return element.GetTaggedValue(TaggedValues.FractionDigits); }
        }

        public string Length
        {
            get { return element.GetTaggedValue(TaggedValues.Length); }
        }

        public string MaxExclusive
        {
            get { return element.GetTaggedValue(TaggedValues.MaxExclusive); }
        }

        public string MaxInclusive
        {
            get { return element.GetTaggedValue(TaggedValues.MaxInclusive); }
        }

        public string MaxLength
        {
            get { return element.GetTaggedValue(TaggedValues.MaxLength); }
        }

        public string MinExclusive
        {
            get { return element.GetTaggedValue(TaggedValues.MinExclusive); }
        }

        public string MinInclusive
        {
            get { return element.GetTaggedValue(TaggedValues.MinInclusive); }
        }

        public string MinLength
        {
            get { return element.GetTaggedValue(TaggedValues.MinLength); }
        }

        public string TotalDigits
        {
            get { return element.GetTaggedValue(TaggedValues.TotalDigits); }
        }

        public string WhiteSpace
        {
            get { return element.GetTaggedValue(TaggedValues.WhiteSpace); }
        }

        public string UniqueIdentifier
        {
            get { return element.GetTaggedValue(TaggedValues.UniqueIdentifier); }
        }

        public string VersionIdentifier
        {
            get { return element.GetTaggedValue(TaggedValues.VersionIdentifier); }
        }

        public string LanguageCode
        {
            get { return element.GetTaggedValue(TaggedValues.LanguageCode); }
        }

        #endregion
    }
}