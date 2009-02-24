using System.Collections.Generic;
using EA;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    internal class ASCC : IASCC
    {
        private readonly CCRepository repository;
        private readonly Connector connector;

        public ASCC(CCRepository repository, Connector connector)
        {
            this.repository = repository;
            this.connector = connector;
        }

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

        public IEnumerable<string> UsageRules
        {
            get { return connector.GetTaggedValues(TaggedValues.UsageRule); }
        }

        public string SequencingKey
        {
            get { return connector.GetTaggedValue(TaggedValues.SequencingKey); }
        }

        public IACC AssociatedCC
        {
            get
            {
                return repository.GetACC(connector.SupplierID);
            }
        }
    }
}