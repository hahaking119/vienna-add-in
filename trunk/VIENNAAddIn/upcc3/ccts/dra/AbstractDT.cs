using System;
using System.Collections.Generic;
using EA;
using VIENNAAddIn.upcc3.ccts.util;
using Attribute=EA.Attribute;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    public class AbstractDT: IDT
    {
        private readonly Element element;
        private readonly CCRepository repository;

        public AbstractDT(CCRepository repository, Element element)
        {
            this.repository = repository;
            this.element = element;
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

        public IList<string> BusinessTerms
        {
            get { return element.CollectTaggedValues(TaggedValues.BusinessTerm); }
        }

        public IList<string> UsageRules
        {
            get { return element.CollectTaggedValues(TaggedValues.UsageRule); }
        }

        public IList<IDTComponent> SUPs
        {
            get { return element.Attributes.Convert((Attribute a) => repository.GetSUP(a)); }
        }

        public IDTComponent CON
        {
            get
            {
                foreach (Attribute attribute in element.Attributes)
                {
                    if (attribute.Stereotype == "CON")
                    {
                        return repository.GetCON(attribute);
                    }
                }
                throw new Exception("data type contains no content component");
            }
        }
    }
}