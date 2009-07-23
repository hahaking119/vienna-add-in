using System.Collections.Generic;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts.otf
{
    public abstract class BusinessLibrary : AbstractEAPackage, IBusinessLibrary
    {
        protected BusinessLibrary(int id, string name, int parentId, string status, string uniqueIdentifier, string versionIdentifier, string baseUrn, string namespacePrefix, IEnumerable<string> businessTerms, IEnumerable<string> copyrights, IEnumerable<string> owners, IEnumerable<string> references) : base(id, name, parentId)
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

        protected override IEnumerable<IValidationIssue> PerformValidation()
        {
            if (string.IsNullOrEmpty(Name))
            {
                yield return new LibraryNameNotSpecified(Id);
            }
            if (string.IsNullOrEmpty(UniqueIdentifier))
            {
                yield return new LibraryMandatoryTaggedValueNotSpecified(Id, Name, TaggedValues.uniqueIdentifier);
            }
            if (string.IsNullOrEmpty(VersionIdentifier))
            {
                yield return new LibraryMandatoryTaggedValueNotSpecified(Id, Name, TaggedValues.versionIdentifier);
            }
            if (string.IsNullOrEmpty(BaseURN))
            {
                yield return new LibraryMandatoryTaggedValueNotSpecified(Id, Name, TaggedValues.baseURN);
            }
        }
    }
}