using System.Collections.Generic;
using EA;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    public class BusinessLibrary : IBusinessLibrary
    {
        protected readonly Package package;
        protected readonly CCRepository repository;

        public BusinessLibrary(CCRepository repository, Package package)
        {
            this.repository = repository;
            this.package = package;
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

        public IEnumerable<IBusinessLibrary> Children
        {
            get
            {
                foreach (Package childPackage in package.Packages)
                {
                    yield return repository.GetLibrary(childPackage);
                }
            }
        }

        public IEnumerable<IBusinessLibrary> AllChildren
        {
            get
            {
                foreach (var childLib in Children)
                {
                    yield return childLib;
                    foreach (var grandChild in childLib.AllChildren)
                    {
                        yield return grandChild;
                    }
                }
            }
        }

        public string Status
        {
            get { return package.GetTaggedValue(TaggedValues.Status); }
        }

        public string UniqueIdentifier
        {
            get { return package.GetTaggedValue(TaggedValues.UniqueIdentifier); }
        }

        public string VersionIdentifier
        {
            get { return package.GetTaggedValue(TaggedValues.VersionIdentifier); }
        }

        public string BaseURN
        {
            get { return package.GetTaggedValue(TaggedValues.BaseURN); }
        }

        public string NamespacePrefix
        {
            get { return package.GetTaggedValue(TaggedValues.NamespacePrefix); }
        }

        public IEnumerable<string> BusinessTerms
        {
            get { return package.GetTaggedValues(TaggedValues.BusinessTerm); }
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