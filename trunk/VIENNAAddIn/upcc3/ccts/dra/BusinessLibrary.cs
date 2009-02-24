using System.Collections.Generic;
using EA;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    internal class BusinessLibrary : IBusinessLibrary
    {
        private readonly BusinessLibraryType libraryType;
        protected readonly Package package;
        protected readonly CCRepository repository;

        public BusinessLibrary(CCRepository repository, Package package, BusinessLibraryType libraryType)
        {
            this.repository = repository;
            this.package = package;
            this.libraryType = libraryType;
        }

        #region IBusinessLibrary Members

        public int Id
        {
            get { return package.PackageID; }
        }

        public BusinessLibraryType Type
        {
            get { return libraryType; }
        }

        public string Name
        {
            get { return package.Name; }
        }

        public IBusinessLibrary Parent
        {
            get { return repository.GetLibrary(package.ParentID); }
        }

        public IList<IBusinessLibrary> Children
        {
            get
            {
                var children = new List<IBusinessLibrary>();
                foreach (Package childPackage in package.Packages)
                {
                    children.Add(repository.GetLibrary(childPackage));
                }
                return children;
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

        public IList<string> BusinessTerms
        {
            get { return package.CollectTaggedValues(TaggedValues.BusinessTerm); }
        }

        public IList<string> Copyrights
        {
            get { return package.CollectTaggedValues(TaggedValues.Copyright); }
        }

        public IList<string> Owners
        {
            get { return package.CollectTaggedValues(TaggedValues.Owner); }
        }

        public IList<string> References
        {
            get { return package.CollectTaggedValues(TaggedValues.Reference); }
        }

        public bool IsA(BusinessLibraryType type)
        {
            return Type == type;
        }

        #endregion
    }
}