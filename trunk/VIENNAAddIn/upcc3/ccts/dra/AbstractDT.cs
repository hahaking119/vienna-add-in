using System;
using System.Collections.Generic;
using System.Linq;
using EA;
using VIENNAAddIn.upcc3.ccts.util;
using Attribute=EA.Attribute;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    public class AbstractDT : IDT
    {
        protected readonly Element element;
        protected readonly IBusinessLibrary library;

        public AbstractDT(IBusinessLibrary library, Element element)
        {
            this.library = library;
            this.element = element;
        }

        private IEnumerable<Attribute> Attributes
        {
            get { return element.Attributes.AsEnumerable<Attribute>(); }
        }

        #region IDT Members

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
            get { return library; }
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

        public IEnumerable<IDTComponent> SUPs
        {
            get
            {
                return
                    Attributes.Where(AttributeExtensions.IsSUP).Convert(a => (IDTComponent) new DTComponent(a, DTComponentType.SUP, this));
            }
        }

        public IDTComponent CON
        {
            get
            {
                Attribute conAttribute =
                    Attributes.FirstOrDefault(AttributeExtensions.IsCON);
                if (conAttribute == null)
                {
                    throw new Exception("data type contains no attribute with stereotype <<CON>>");
                }
                return new DTComponent(conAttribute, DTComponentType.CON, this);
            }
        }

        #endregion
    }
}