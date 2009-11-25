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
using CctsRepository.BieLibrary;
using CctsRepository.CcLibrary;
using EA;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    internal class ASBIE : IASBIE
    {
        private readonly IABIE associatingClass;
        private readonly Connector connector;
        private readonly CCRepository repository;

        public ASBIE(CCRepository repository, Connector connector, IABIE associatingBIE)
        {
            this.repository = repository;
            this.connector = connector;
            associatingClass = associatingBIE;
        }

        private Cardinality Cardinality
        {
            get { return new Cardinality(connector.GetAssociatedEnd(AssociatingElement.Id).Cardinality); }
        }

        #region IASBIE Members

        public IABIE AssociatingElement
        {
            get { return associatingClass; }
        }

        public AsbieAggregationKind AggregationKind
        {
            get
            {
                int value = connector.GetAssociatingEnd(AssociatingElement.Id).Aggregation;
                return AsbieAggregationKindFromInt(value);
            }
        }

        private static AsbieAggregationKind AsbieAggregationKindFromInt(int value)
        {
            if (Enum.IsDefined(typeof (AsbieAggregationKind), value))
            {
                return (AsbieAggregationKind) Enum.ToObject(typeof (AsbieAggregationKind), value);
            }
            return AsbieAggregationKind.Composite;
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

        public IABIE AssociatedElement
        {
            get { return repository.GetAbieById(connector.GetAssociatedElementId(AssociatingElement.Id)); }
        }

        public IASCC BasedOn
        {
            get
            {
                if (AssociatingElement == null)
                {
                    return null;
                }
                IACC acc = AssociatingElement.BasedOn;
                if (acc == null)
                {
                    return null;
                }
                string nameWithoutQualifiers = Name.Substring(Name.LastIndexOf('_') + 1);
                foreach (IASCC ascc in acc.ASCCs)
                {
                    if (nameWithoutQualifiers == ascc.Name)
                    {
                        return ascc;
                    }
                }
                return null;
            }
        }

        #endregion

        private string GetTaggedValue(TaggedValues key)
        {
            return connector.GetTaggedValue(key.ToString());
        }
    }
}