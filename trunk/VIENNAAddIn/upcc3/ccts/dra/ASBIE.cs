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
using VIENNAAddIn.upcc3.export.cctsndr;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    internal class ASBIE : IAsbie
    {
        private readonly IAbie associatingClass;
        private readonly Connector connector;
        private readonly CCRepository repository;

        public ASBIE(CCRepository repository, Connector connector, IAbie associatingBIE)
        {
            this.repository = repository;
            this.connector = connector;
            associatingClass = associatingBIE;
        }

        private Cardinality Cardinality
        {
            get { return new Cardinality(connector.GetAssociatedEnd(AssociatingAbie.Id).Cardinality); }
        }

        #region IAsbie Members

        public bool IsOptional()
        {
            int i;
            return Int32.TryParse(LowerBound, out i) && i == 0;
        }

        public IAbie AssociatingAbie
        {
            get { return associatingClass; }
        }

        public AggregationKind AggregationKind
        {
            get
            {
                int value = connector.GetAssociatingEnd(AssociatingAbie.Id).Aggregation;
                return AsbieAggregationKindFromInt(value);
            }
        }

        private static AggregationKind AsbieAggregationKindFromInt(int value)
        {
            if (Enum.IsDefined(typeof (AggregationKind), value))
            {
                return (AggregationKind) Enum.ToObject(typeof (AggregationKind), value);
            }
            return AggregationKind.Composite;
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
            get { return connector.GetAssociatedEnd(AssociatingAbie.Id).Role; }
        }

        public string Definition
        {
            get { return GetTaggedValue(TaggedValues.definition); }
        }

        public string DictionaryEntryName
        {
            // TODO default dictionary name is incorrect if Name contains associated ABIE name
            get { return GetTaggedValue(TaggedValues.dictionaryEntryName).DefaultTo(AssociatingAbie.Name + ". " + Name + ". " + AssociatedAbie.Name); }
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

        public IAbie AssociatedAbie
        {
            get { return repository.GetAbieById(connector.GetAssociatedElementId(AssociatingAbie.Id)); }
        }

        public IAscc BasedOn
        {
            get
            {
                if (AssociatingAbie == null)
                {
                    return null;
                }
                IAcc acc = AssociatingAbie.BasedOn;
                if (acc == null)
                {
                    return null;
                }
                string nameWithoutQualifiers = Name.Substring(Name.LastIndexOf('_') + 1);
                foreach (IAscc ascc in acc.Asccs)
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