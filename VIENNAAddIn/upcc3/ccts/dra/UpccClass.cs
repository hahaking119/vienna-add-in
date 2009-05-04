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
    ///<summary>
    ///</summary>
    public abstract class UpccClass : ICCTSElement, IEquatable<UpccClass>
    {
        protected readonly Element element;
        protected readonly CCRepository repository;

        protected UpccClass(CCRepository repository, Element element, string stereotype)
        {
            if (element.Stereotype != stereotype)
            {
                throw new ArgumentException("invalid stereotype: " + element.Stereotype + ", expected: " + stereotype);
            }
            this.repository = repository;
            this.element = element;
        }

        protected IEnumerable<Attribute> Attributes
        {
            get { return element.Attributes.AsEnumerable<Attribute>(); }
        }

        protected IEnumerable<Connector> Connectors
        {
            get { return element.Connectors.AsEnumerable<Connector>(); }
        }

        public bool Equals(UpccClass other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.element.ElementID, element.ElementID);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj is UpccClass) return Equals((UpccClass) obj);
            return false;
        }

        public override int GetHashCode()
        {
            return (element != null ? element.GetHashCode() : 0);
        }

        #region ICCTSElement Members

        ///<summary>
        ///</summary>
        public int Id
        {
            get { return element.ElementID; }
        }

        ///<summary>
        ///</summary>
        public virtual string Name
        {
            get { return element.Name; }
        }

        ///<summary>
        ///</summary>
        public virtual IBusinessLibrary Library
        {
            get { return repository.GetLibrary(element.PackageID); }
        }

        ///<summary>
        ///</summary>
        public virtual IEnumerable<string> BusinessTerms
        {
            get { return element.GetTaggedValues(TaggedValues.BusinessTerm); }
        }

        ///<summary>
        ///</summary>
        public virtual string Definition
        {
            get { return GetTaggedValue(TaggedValues.Definition); }
        }

        ///<summary>
        ///</summary>
        public virtual string DictionaryEntryName
        {
            get { return GetTaggedValue(TaggedValues.DictionaryEntryName); }
        }

        ///<summary>
        ///</summary>
        public virtual string UniqueIdentifier
        {
            get { return GetTaggedValue(TaggedValues.UniqueIdentifier); }
        }

        ///<summary>
        ///</summary>
        public virtual string VersionIdentifier
        {
            get { return GetTaggedValue(TaggedValues.VersionIdentifier); }
        }

        ///<summary>
        ///</summary>
        public virtual string LanguageCode
        {
            get { return GetTaggedValue(TaggedValues.LanguageCode); }
        }

        #endregion

        protected string GetTaggedValue(TaggedValues key)
        {
            return element.GetTaggedValue(key) ?? string.Empty;
        }

        protected void Update(CCTSElementSpec spec)
        {
            element.Name = spec.Name;
            foreach (TaggedValueSpec taggedValueSpec in spec.GetTaggedValues())
            {
                element.SetTaggedValue(taggedValueSpec.Key, taggedValueSpec.Value);
            }
        }
    }
}