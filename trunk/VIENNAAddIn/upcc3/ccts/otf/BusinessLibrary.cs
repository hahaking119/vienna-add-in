using System;
using System.Collections.Generic;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts.otf
{
    public abstract class BusinessLibrary : AbstractEAPackage, IBusinessLibrary
    {
        protected BusinessLibrary(ItemId id, string name, ItemId parentId, string status, string uniqueIdentifier, string versionIdentifier, string baseUrn, string namespacePrefix, IEnumerable<string> businessTerms, IEnumerable<string> copyrights, IEnumerable<string> owners, IEnumerable<string> references) : base(id, name, parentId)
        {
            Status = status;
            UniqueIdentifier = uniqueIdentifier;
            VersionIdentifier = versionIdentifier;
            BaseURN = baseUrn;
            NamespacePrefix = namespacePrefix;
            BusinessTerms = businessTerms;
            Copyrights = copyrights;
            Owners = owners;
            References = references;
        }

        #region IBusinessLibrary Members

        int IBusinessLibrary.Id
        {
            get { return Id.Value; }
        }

        public IBusinessLibrary Parent
        {
            get { return ParentPackage as IBusinessLibrary; }
        }

        public Path Path
        {
            get { return Parent != null ? Parent.Path/Name : null; }
        }

        public string Status { get; private set; }
        public string UniqueIdentifier { get; private set; }
        public string VersionIdentifier { get; private set; }
        public string BaseURN { get; private set; }
        public string NamespacePrefix { get; private set; }
        public IEnumerable<string> BusinessTerms { get; private set; }
        public IEnumerable<string> Copyrights { get; private set; }
        public IEnumerable<string> Owners { get; private set; }
        public IEnumerable<string> References { get; private set; }

        #endregion
    }
}