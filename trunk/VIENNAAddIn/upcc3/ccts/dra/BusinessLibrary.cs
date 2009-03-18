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
    public abstract class BusinessLibrary : IBusinessLibrary
    {
        protected readonly Package package;
        protected readonly CCRepository repository;

        protected BusinessLibrary(CCRepository repository, Package package)
        {
            this.repository = repository;
            this.package = package;
        }

        public string Stereotype
        {
            get { return package.Element.Stereotype; }
            set { package.Element.Stereotype = value; }
        }

        #region IBusinessLibrary Members

        public int Id
        {
            get { return package.PackageID; }
        }

        public string Name
        {
            get { return package.Name; }
        }

        public IBusinessLibrary Parent
        {
            get { return repository.GetLibrary(package.ParentID); }
        }

        public string Status
        {
            get { return package.GetTaggedValue(TaggedValues.Status); }
            set { package.SetTaggedValue(TaggedValues.Status, value); }
        }

        public string UniqueIdentifier
        {
            get { return package.GetTaggedValue(TaggedValues.UniqueIdentifier); }
            set { package.SetTaggedValue(TaggedValues.UniqueIdentifier, value); }
        }

        public string VersionIdentifier
        {
            get { return package.GetTaggedValue(TaggedValues.VersionIdentifier); }
            set { package.SetTaggedValue(TaggedValues.VersionIdentifier, value); }
        }

        public string BaseURN
        {
            get { return package.GetTaggedValue(TaggedValues.BaseURN); }
            set { package.SetTaggedValue(TaggedValues.BaseURN, value); }
        }

        public string NamespacePrefix
        {
            get { return package.GetTaggedValue(TaggedValues.NamespacePrefix); }
            set { package.SetTaggedValue(TaggedValues.NamespacePrefix, value); }
        }

        public IEnumerable<string> BusinessTerms
        {
            get { return package.GetTaggedValues(TaggedValues.BusinessTerm); }
            set { package.SetTaggedValues(TaggedValues.BusinessTerm, value); }
        }

        public IEnumerable<string> Copyrights
        {
            get { return package.GetTaggedValues(TaggedValues.Copyright); }
        }

        public IEnumerable<string> Owners
        {
            get { return package.GetTaggedValues(TaggedValues.Owner); }
        }

        public IEnumerable<string> References
        {
            get { return package.GetTaggedValues(TaggedValues.Reference); }
        }

        #endregion
    }
}