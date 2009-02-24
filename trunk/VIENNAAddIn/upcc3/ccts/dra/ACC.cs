using System.Collections.Generic;
using EA;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    public class ACC : IACC
    {
        private readonly CCRepository repository;
        private readonly Element element;

        public ACC(CCRepository repository, Element element)
        {
            this.repository = repository;
            this.element = element;
        }

        private IEnumerable<Attribute> Attributes
        {
            get { return element.Attributes.AsEnumerable<Attribute>(); }
        }

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

        public string Definition
        {
            get { return element.GetTaggedValue(TaggedValues.Definition); }
        }

        public string DictionaryEntryName
        {
            get { return element.GetTaggedValue(TaggedValues.DictionaryEntryName); }
        }

        public string LanguageCode
        {
            get { return element.GetTaggedValue(TaggedValues.LanguageCode); }
        }

        public string UniqueIdentifier
        {
            get { return element.GetTaggedValue(TaggedValues.UniqueIdentifier); }
        }

        public string VersionIdentifier
        {
            get { return element.GetTaggedValue(TaggedValues.VersionIdentifier); }
        }

        public IEnumerable<string> BusinessTerms
        {
            get { return element.GetTaggedValues(TaggedValues.BusinessTerm); }
        }

        public IEnumerable<string> UsageRules
        {
            get { return element.GetTaggedValues(TaggedValues.UsageRule); }
        }

        public IEnumerable<IBCC> BCCs
        {
            get
            {
                return
                    Attributes.Convert(a => (IBCC)new BCC(repository, a));
            }
        }

        public IEnumerable<IASCC> ASCCs
        {
            get { throw new System.NotImplementedException(); }
        }

        public IACC IsEquivalentTo
        {
            get { throw new System.NotImplementedException(); }
        }
    }
}