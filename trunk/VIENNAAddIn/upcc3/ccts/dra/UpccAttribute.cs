// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System.Collections.Generic;
using EA;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    internal class UpccAttribute<TContainer> where TContainer : ICCTSElement
    {
        protected Attribute attribute;
        protected CCRepository repository;

        public UpccAttribute(CCRepository repository, Attribute attribute, TContainer container)
        {
            this.repository = repository;
            this.attribute = attribute;
            this.Container = container;
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
            get {
                var tv = AttributeExtensions.GetTaggedValue(attribute, (TaggedValues) TaggedValues.Definition);
                return tv ?? string.Empty;
            }
        }

        public string DictionaryEntryName
        {
            get {
                var tv = AttributeExtensions.GetTaggedValue(attribute, (TaggedValues) TaggedValues.DictionaryEntryName);
                return tv ?? string.Empty;
            }
        }

        public string LanguageCode
        {
            get {
                var tv = AttributeExtensions.GetTaggedValue(attribute, (TaggedValues) TaggedValues.LanguageCode);
                return tv ?? string.Empty;
            }
        }

        public string UniqueIdentifier
        {
            get {
                var tv = AttributeExtensions.GetTaggedValue(attribute, (TaggedValues) TaggedValues.UniqueIdentifier);
                return tv ?? string.Empty;
            }
        }

        public string VersionIdentifier
        {
            get {
                var tv = AttributeExtensions.GetTaggedValue(attribute, (TaggedValues) TaggedValues.VersionIdentifier);
                return tv ?? string.Empty;
            }
        }

        public IEnumerable<string> BusinessTerms
        {
            get { return attribute.GetTaggedValues(TaggedValues.BusinessTerm); }
        }

        public IBusinessLibrary Library
        {
            get { return Container.Library; }
        }

        public IEnumerable<string> UsageRules
        {
            get { return attribute.GetTaggedValues(TaggedValues.UsageRule); }
        }

        public string SequencingKey
        {
            get {
                var tv = AttributeExtensions.GetTaggedValue(attribute, (TaggedValues) TaggedValues.SequencingKey);
                return tv ?? string.Empty;
            }
        }

        public TContainer Container { get; private set; }
    }
}