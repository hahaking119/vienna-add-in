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
using EA;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    internal abstract class UpccAssociation<TAssociatingClass> : ICCTSElement, IHasUsageRules, ISequenced,
                                                                 IHasMultiplicity
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

        #region ICCTSElement Members

        public int Id
        {
            get { return connector.ConnectorID; }
        }

        public string Name
        {
            get
            {
                return connector.ClientEnd.Aggregation == (int) AggregationKind.None
                           ? connector.ClientEnd.Role
                           : connector.SupplierEnd.Role;
            }
        }

        public string Definition
        {
            get { return GetTaggedValue(TaggedValues.Definition); }
        }

        public string DictionaryEntryName
        {
            get { return GetTaggedValue(TaggedValues.DictionaryEntryName); }
        }

        public string LanguageCode
        {
            get { return GetTaggedValue(TaggedValues.LanguageCode); }
        }

        public string UniqueIdentifier
        {
            get { return GetTaggedValue(TaggedValues.UniqueIdentifier); }
        }

        public string VersionIdentifier
        {
            get { return GetTaggedValue(TaggedValues.VersionIdentifier); }
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

        #region IHasMultiplicity Members

        public string UpperBound
        {
            get
            {
                string cardinality = connector.SupplierEnd.Cardinality;
                string[] parts = cardinality.Split(new[] {'.'}, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 1)
                {
                    return parts[0];
                }
                if (parts.Length == 2)
                {
                    return parts[1];
                }
                return "1";
            }
        }

        public string LowerBound
        {
            get
            {
                string cardinality = connector.SupplierEnd.Cardinality;
                string[] parts = cardinality.Split(new[] {'.'}, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 1)
                {
                    return (parts[0] == "*" ? "0" : parts[0]);
                }
                if (parts.Length == 2)
                {
                    return parts[0];
                }
                return "1";
            }
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
            get { return GetTaggedValue(TaggedValues.SequencingKey); }
        }

        #endregion

        private string GetTaggedValue(TaggedValues key)
        {
            return connector.GetTaggedValue(key);
        }
    }
}