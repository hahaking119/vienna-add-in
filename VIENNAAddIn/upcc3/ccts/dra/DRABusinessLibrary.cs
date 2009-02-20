using System;
using System.Collections.Generic;
using EA;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    internal class DRABusinessLibrary : IBusinessLibrary
    {
        private readonly BusinessLibraryType libraryType;
        protected readonly Package package;
        protected readonly DRACCRepository repository;

        public DRABusinessLibrary(DRACCRepository repository, Package package, BusinessLibraryType libraryType)
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

        public T CreateElement<T>(T spec)
        {
            throw new NotImplementedException();
        }

        public virtual IList<T> Elements<T>()
        {
            throw new NotImplementedException("Trying to receive wrong type of elements from business library.");
        }

        public bool IsA(BusinessLibraryType type)
        {
            return Type == type;
        }

        public void Each<T>(Action<T> action)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    class DRACDTLibrary : DRABusinessLibrary, ICDTLibrary
    {
        public DRACDTLibrary(DRACCRepository repository, Package package, BusinessLibraryType libraryType) : base(repository, package, libraryType)
        {
        }

        public IList<ICDT> CDTs
        {
            get
            {
                var elements = new List<ICDT>();
                foreach (Element element in package.Elements)
                {
                    elements.Add(repository.GetCDT(element));
                }
                return elements;
            }
        }

        public void EachCDT(Action<ICDT> action)
        {
            foreach (var cdt in CDTs)
            {
                action(cdt);
            }
        }
    }
}