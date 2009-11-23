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
using EA;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    internal abstract class UpccAssociation<TAssociatingClass> : ICCTSElement
        where TAssociatingClass : ICCTSElement
    {
        private readonly TAssociatingClass associatingClass;
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

        private Cardinality Cardinality
        {
            get { return new Cardinality(connector.GetAssociatedEnd(AssociatingElement.Id).Cardinality); }
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

        #region ICCTSElement Members

        public int Id
        {
            get { return connector.ConnectorID; }
        }

        public string GUID
        {
            get { return connector.ConnectorGUID; }
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
            get { return associatingClass.Library; }
        }

        #endregion

        private string GetTaggedValue(TaggedValues key)
        {
            return connector.GetTaggedValue(key.ToString());
        }
    }

    internal class Cardinality
    {
        public Cardinality(string lowerBound, string upperBound)
        {
            LowerBound = lowerBound;
            UpperBound = upperBound;
        }

        public Cardinality(string cardinality)
        {
            string[] parts = cardinality.Split(new[] {'.'}, StringSplitOptions.RemoveEmptyEntries);
            switch (parts.Length)
            {
                case 1:
                    LowerBound = (parts[0] == "*" ? "0" : parts[0]);
                    UpperBound = parts[0];
                    break;
                case 2:
                    LowerBound = parts[0];
                    UpperBound = parts[1];
                    break;
                default:
                    LowerBound = "1";
                    UpperBound = "1";
                    break;
            }
        }

        public string LowerBound { get; private set; }
        public string UpperBound { get; private set; }
    }
}