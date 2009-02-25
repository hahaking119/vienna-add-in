using System.Collections.Generic;
using EA;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    internal class ENUM : IENUM
    {
        private readonly Element element;
        private readonly CCRepository repository;

        public ENUM(CCRepository repository, Element element)
        {
            this.repository = repository;
            this.element = element;
        }

        #region IENUM Members

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

        public string AgencyIdentifier
        {
            get { return element.GetTaggedValue(TaggedValues.AgencyIdentifier); }
        }

        public string AgencyName
        {
            get { return element.GetTaggedValue(TaggedValues.AgencyName); }
        }

        public IEnumerable<string> BusinessTerms
        {
            get { return element.GetTaggedValues(TaggedValues.BusinessTerm); }
        }

        public string LanguageCode
        {
            get { return element.GetTaggedValue(TaggedValues.LanguageCode); }
        }

        public string DictionaryEntryName
        {
            get { return element.GetTaggedValue(TaggedValues.DictionaryEntryName); }
        }

        public string UniqueIdentifier
        {
            get { return element.GetTaggedValue(TaggedValues.UniqueIdentifier); }
        }

        public string VersionIdentifier
        {
            get { return element.GetTaggedValue(TaggedValues.VersionIdentifier); }
        }

        public string EnumerationURI
        {
            get { return element.GetTaggedValue(TaggedValues.EnumerationURI); }
        }

        #endregion
    }
}