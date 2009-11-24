// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System;
using System.Collections.Generic;
using CctsRepository;
using CctsRepository.CcLibrary;
using EA;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    internal class ASCC : IASCC
    {
        private readonly IACC associatingAcc;
        private readonly Connector connector;
        private readonly CCRepository repository;

        public ASCC(CCRepository repository, Connector connector, IACC associatingAcc)
        {
            this.repository = repository;
            this.connector = connector;
            this.associatingAcc = associatingAcc;
        }

        private Cardinality Cardinality
        {
            get { return new Cardinality(connector.GetAssociatedEnd(AssociatingElement.Id).Cardinality); }
        }

        #region IASCC Members

        public IACC AssociatingElement
        {
            get { return associatingAcc; }
        }

        public AggregationKind AggregationKind
        {
            get
            {
                int value = connector.GetAssociatingEnd(AssociatingElement.Id).Aggregation;
                if (Enum.IsDefined(typeof (AggregationKind), value))
                {
                    return (AggregationKind) Enum.ToObject(typeof (AggregationKind), value);
                }
                return AggregationKind.None;
            }
        }

        public string UpperBound
        {
            get { return Cardinality.UpperBound; }
        }

        public string LowerBound
        {
            get { return Cardinality.LowerBound; }
        }

        public IEnumerable<string> UsageRules
        {
            get { return connector.GetTaggedValues(TaggedValues.usageRule); }
        }

        public string SequencingKey
        {
            get { return GetTaggedValue(TaggedValues.sequencingKey); }
        }

        public int Id
        {
            get { return connector.ConnectorID; }
        }

        public string Name
        {
            get { return connector.GetAssociatedEnd(AssociatingElement.Id).Role; }
        }

        public string Definition
        {
            get { return GetTaggedValue(TaggedValues.definition); }
        }

        public string DictionaryEntryName
        {
            get { return GetTaggedValue(TaggedValues.dictionaryEntryName); }
        }

        public string LanguageCode
        {
            get { return GetTaggedValue(TaggedValues.languageCode); }
        }

        public string UniqueIdentifier
        {
            get { return GetTaggedValue(TaggedValues.uniqueIdentifier); }
        }

        public string VersionIdentifier
        {
            get { return GetTaggedValue(TaggedValues.versionIdentifier); }
        }

        public IEnumerable<string> BusinessTerms
        {
            get { return connector.GetTaggedValues(TaggedValues.businessTerm); }
        }

        public IBusinessLibrary Library
        {
            get { return associatingAcc.Library; }
        }

        public IACC AssociatedElement
        {
            get { return repository.GetACC(connector.GetAssociatedElementId(AssociatingElement.Id)); }
        }

        #endregion

        private string GetTaggedValue(TaggedValues key)
        {
            return connector.GetTaggedValue(key.ToString());
        }
    }
}