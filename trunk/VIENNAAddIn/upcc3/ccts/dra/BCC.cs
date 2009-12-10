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
using CctsRepository.CdtLibrary;
using VIENNAAddIn.upcc3.ccts.util;
using Attribute=EA.Attribute;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    internal class BCC : IBcc
    {
        private readonly Attribute attribute;
        private readonly CCRepository repository;

        public BCC(CCRepository repository, Attribute attribute, IAcc container)
        {
            this.repository = repository;
            this.attribute = attribute;
            Acc = container;
        }

        #region IBcc Members

        public ICdt Cdt
        {
            get { return repository.GetCdtById(attribute.ClassifierID); }
        }

        public int Id
        {
            get { return attribute.AttributeID; }
        }

        public string Name
        {
            get { return attribute.Name; }
        }

        public string Definition
        {
            get { return GetTaggedValue(TaggedValues.definition); }
        }

        public string DictionaryEntryName
        {
            get
            {
                // TODO default dictionary name is incorrect if Name contains CDT name
                string value = GetTaggedValue(TaggedValues.dictionaryEntryName);
                if (string.IsNullOrEmpty(value))
                {
                    value = Acc.Name + ". " + Name + ". " + Cdt.Name;
                }
                return value;
            }
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
            get { return attribute.GetTaggedValues(TaggedValues.businessTerm); }
        }

        public IEnumerable<string> UsageRules
        {
            get { return attribute.GetTaggedValues(TaggedValues.usageRule); }
        }

        public string SequencingKey
        {
            get { return GetTaggedValue(TaggedValues.sequencingKey); }
        }

        public bool IsOptional()
        {
            int i;
            return Int32.TryParse(LowerBound, out i) && i == 0;
        }

        public IAcc Acc { get; protected set; }

        public string UpperBound
        {
            get { return attribute.UpperBound; }
        }

        public string LowerBound
        {
            get { return attribute.LowerBound; }
        }

        #endregion

        private string GetTaggedValue(TaggedValues key)
        {
            return attribute.GetTaggedValue(key) ?? string.Empty;
        }
    }
}