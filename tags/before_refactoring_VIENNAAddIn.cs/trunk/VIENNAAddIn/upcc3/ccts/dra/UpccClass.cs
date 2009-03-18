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
using Attribute=EA.Attribute;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    public abstract class UpccClass : ICCTSElement
    {
        protected Element element;
        protected CCRepository repository;

        protected UpccClass(CCRepository repository, Element element, string stereotype)
        {
            if (element.Stereotype != stereotype)
            {
                throw new ArgumentException("invalid stereotype: " + element.Stereotype + ", expected: " + stereotype);
            }
            this.repository = repository;
            this.element = element;
        }

        public int Id
        {
            get { return element.ElementID; }
        }

        public virtual string Name
        {
            get { return element.Name; }
        }

        public virtual IBusinessLibrary Library
        {
            get { return repository.GetLibrary(element.PackageID); }
        }

        public virtual IEnumerable<string> BusinessTerms
        {
            get { return element.GetTaggedValues(TaggedValues.BusinessTerm); }
        }

        public virtual string Definition
        {
            get { return element.GetTaggedValue(TaggedValues.Definition); }
        }

        public virtual string DictionaryEntryName
        {
            get { return element.GetTaggedValue(TaggedValues.DictionaryEntryName); }
        }

        public virtual string UniqueIdentifier
        {
            get { return element.GetTaggedValue(TaggedValues.UniqueIdentifier); }
        }

        public virtual string VersionIdentifier
        {
            get { return element.GetTaggedValue(TaggedValues.VersionIdentifier); }
        }

        public virtual string LanguageCode
        {
            get { return element.GetTaggedValue(TaggedValues.LanguageCode); }
        }

        protected IEnumerable<Attribute> Attributes
        {
            get { return element.Attributes.AsEnumerable<Attribute>(); }
        }

        protected IEnumerable<Connector> Connectors
        {
            get { return element.Connectors.AsEnumerable<Connector>(); }
        }
    }
}