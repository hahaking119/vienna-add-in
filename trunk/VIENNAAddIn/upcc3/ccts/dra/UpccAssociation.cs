using System.Collections.Generic;
using EA;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    internal abstract class UpccAssociation<TAssociatingClass> : ICCTSElement, IHasUsageRules, ISequenced
        where TAssociatingClass : ICCTSElement
    {
        protected readonly TAssociatingClass associatingClass;
        protected readonly Connector connector;
        protected readonly CCRepository repository;

        protected UpccAssociation(CCRepository repository, Connector connector, TAssociatingClass associatingClass)
        {
            this.repository = repository;
            this.connector = connector;
            this.associatingClass = associatingClass;
        }

        public TAssociatingClass AssociatingElement
        {
            get { return associatingClass; }
        }

        #region ICCTSElement Members

        public int Id
        {
            get { return connector.ConnectorID; }
        }

        public string Name
        {
            get { return connector.Name; }
        }

        public string Definition
        {
            get { return connector.GetTaggedValue(TaggedValues.Definition); }
        }

        public string DictionaryEntryName
        {
            get { return connector.GetTaggedValue(TaggedValues.DictionaryEntryName); }
        }

        public string LanguageCode
        {
            get { return connector.GetTaggedValue(TaggedValues.LanguageCode); }
        }

        public string UniqueIdentifier
        {
            get { return connector.GetTaggedValue(TaggedValues.UniqueIdentifier); }
        }

        public string VersionIdentifier
        {
            get { return connector.GetTaggedValue(TaggedValues.VersionIdentifier); }
        }

        public IEnumerable<string> BusinessTerms
        {
            get { return connector.GetTaggedValues(TaggedValues.BusinessTerm); }
        }

        public IBusinessLibrary Library
        {
            get { return associatingClass.Library; }
        }

        #endregion

        #region IHasUsageRules Members

        public IEnumerable<string> UsageRules
        {
            get { return connector.GetTaggedValues(TaggedValues.UsageRule); }
        }

        #endregion

        #region ISequenced Members

        public string SequencingKey
        {
            get { return connector.GetTaggedValue(TaggedValues.SequencingKey); }
        }

        #endregion
    }
}