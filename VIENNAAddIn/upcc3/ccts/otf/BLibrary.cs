using System;
using System.Collections.Generic;
using EA;
using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddIn.upcc3.XSDGenerator.ccts;

namespace VIENNAAddIn.upcc3.ccts.otf
{
    internal abstract class ElementLibrary<TElement, TElementSpec>: BusinessLibrary
    {
        public ElementLibrary(Package package) : base(package)
        {
        }

        public IEnumerable<TElement> Elements
        {
            get { throw new NotImplementedException(); }
        }

        public TElement ElementByName(string name)
        {
            throw new NotImplementedException();
        }

        public TElement CreateElement(TElementSpec spec)
        {
            throw new NotImplementedException();
        }

        public TElement UpdateElement(TElement element, TElementSpec spec)
        {
            throw new NotImplementedException();
        }
    }

    internal class PRIMLibrary : ElementLibrary<IPRIM, PRIMSpec>, IPRIMLibrary
    {
        public PRIMLibrary(Package package) : base(package)
        {
        }

        public override IEnumerable<IValidationIssue> Validate()
        {
            yield break;
        }
    }

    internal class ENUMLibrary : ElementLibrary<IENUM, ENUMSpec>, IENUMLibrary
    {
        public ENUMLibrary(Package package) : base(package)
        {
        }

        public override IEnumerable<IValidationIssue> Validate()
        {
            yield break;
        }
    }

    internal class CDTLibrary : ElementLibrary<ICDT, CDTSpec>, ICDTLibrary
    {
        public CDTLibrary(Package package) : base(package)
        {
        }

        public override IEnumerable<IValidationIssue> Validate()
        {
            yield break;
        }
    }

    internal class CCLibrary : ElementLibrary<IACC, ACCSpec>, ICCLibrary
    {
        public CCLibrary(Package package) : base(package)
        {
        }

        public override IEnumerable<IValidationIssue> Validate()
        {
            yield break;
        }
    }

    internal class BDTLibrary : ElementLibrary<IBDT, BDTSpec>, IBDTLibrary
    {
        public BDTLibrary(Package package) : base(package)
        {
        }

        public override IEnumerable<IValidationIssue> Validate()
        {
            yield break;
        }
    }

    internal class BIELibrary : ElementLibrary<IABIE, ABIESpec>, IBIELibrary
    {
        public BIELibrary(Package package) : base(package)
        {
        }

        public override IEnumerable<IValidationIssue> Validate()
        {
            yield break;
        }
    }

    internal class DOCLibrary : ElementLibrary<IABIE, ABIESpec>, IDOCLibrary
    {
        public DOCLibrary(Package package) : base(package)
        {
        }

        public override IEnumerable<IValidationIssue> Validate()
        {
            yield break;
        }

        public IEnumerable<IABIE> RootElements
        {
            get { throw new NotImplementedException(); }
        }
    }

    internal abstract class BusinessLibrary : AbstractEAPackage, IBusinessLibrary
    {
        protected BusinessLibrary(Package package) : base(package.PackageID)
        {
            Name = package.Name;
            Status = package.GetTaggedValue(TaggedValues.status).DefaultTo(string.Empty);
            UniqueIdentifier = package.GetTaggedValue(TaggedValues.uniqueIdentifier).DefaultTo(string.Empty);
            VersionIdentifier = package.GetTaggedValue(TaggedValues.versionIdentifier).DefaultTo(string.Empty);
            BaseURN = package.GetTaggedValue(TaggedValues.baseURN).DefaultTo(string.Empty);
            NamespacePrefix = package.GetTaggedValue(TaggedValues.namespacePrefix).DefaultTo(string.Empty);
            BusinessTerms = package.GetTaggedValues(TaggedValues.businessTerm);
            Copyrights = package.GetTaggedValues(TaggedValues.copyright);
            Owners = package.GetTaggedValues(TaggedValues.owner);
            References = package.GetTaggedValues(TaggedValues.reference);
        }

        public IBusinessLibrary Parent
        {
            get { return ParentPackage as IBusinessLibrary; }
        }

        public Path Path
        {
            get { return Parent != null ? Parent.Path/Name : null; }
        }

        public string Name { get; private set; }
        public string Status { get; private set; }
        public string UniqueIdentifier { get; private set; }
        public string VersionIdentifier { get; private set; }
        public string BaseURN { get; private set; }
        public string NamespacePrefix { get; private set; }
        public IEnumerable<string> BusinessTerms { get; private set; }
        public IEnumerable<string> Copyrights { get; private set; }
        public IEnumerable<string> Owners { get; private set; }
        public IEnumerable<string> References { get; private set; }
    }

    internal class BLibrary : BusinessLibrary, IBLibrary
    {
        internal BLibrary(Package package) : base(package)
        {
        }

        #region IBLibrary Members

        public IEnumerable<IBusinessLibrary> Children
        {
            get
            {
                foreach (var subPackage in subPackages)
                {
                    if (subPackage is IBusinessLibrary)
                    {
                        yield return (IBusinessLibrary) subPackage;
                    }
                }
            }
        }

        public IEnumerable<IBusinessLibrary> AllChildren
        {
            get
            {
                foreach (IBusinessLibrary child in Children)
                {
                    yield return child;
                    if (child is IBLibrary)
                    {
                        foreach (IBusinessLibrary grandChild in ((IBLibrary) child).AllChildren)
                        {
                            yield return grandChild;
                        }
                    }
                }
            }
        }

        public IBusinessLibrary FindChildByName(string name)
        {
            throw new NotImplementedException();
        }

        public IBLibrary CreateBLibrary(LibrarySpec spec)
        {
            throw new NotImplementedException();
        }

        public ICDTLibrary CreateCDTLibrary(LibrarySpec spec)
        {
            throw new NotImplementedException();
        }

        public ICCLibrary CreateCCLibrary(LibrarySpec spec)
        {
            throw new NotImplementedException();
        }

        public IBDTLibrary CreateBDTLibrary(LibrarySpec spec)
        {
            throw new NotImplementedException();
        }

        public IBIELibrary CreateBIELibrary(LibrarySpec spec)
        {
            throw new NotImplementedException();
        }

        public IPRIMLibrary CreatePRIMLibrary(LibrarySpec spec)
        {
            throw new NotImplementedException();
        }

        public IENUMLibrary CreateENUMLibrary(LibrarySpec spec)
        {
            throw new NotImplementedException();
        }

        public IDOCLibrary CreateDOCLibrary(LibrarySpec spec)
        {
            throw new NotImplementedException();
        }

        #endregion

        public override IEnumerable<IValidationIssue> Validate()
        {
            if (!(ParentPackage is EAModel || ParentPackage is BInformationV || ParentPackage is BLibrary))
            {
                yield return new InvalidParentPackage(Id);
            }
            foreach (var subPackage in subPackages)
            {
                if (!(subPackage is IBusinessLibrary))
                {
                    yield return new InvalidSubPackage(subPackage.Id);
                }
            }
            foreach (var element in elements)
            {
                yield return new NoElementsAllowed(element.Id);
            }
        }
    }

    internal abstract class AbstractValidationIssue : IValidationIssue
    {
        private static int nextId;

        protected AbstractValidationIssue(int itemId)
        {
            Id = nextId++;
            ItemId = itemId;
        }

        public int ItemId { get; private set; }
        public int Id { get; private set; }
        public abstract object ResolveItem(Repository repository);
    }

    internal abstract class PackageValidationIssue : AbstractValidationIssue
    {
        protected PackageValidationIssue(int itemId) : base(itemId)
        {
        }

        public override object ResolveItem(Repository repository)
        {
            return repository.GetPackageByID(ItemId);
        }
    }

    internal abstract class ElementValidationIssue : AbstractValidationIssue
    {
        protected ElementValidationIssue(int itemId) : base(itemId)
        {
        }

        public override object ResolveItem(Repository repository)
        {
            return repository.GetElementByID(ItemId);
        }
    }

    internal class NoElementsAllowed : ElementValidationIssue
    {
        public NoElementsAllowed(int elementId) : base(elementId)
        {
        }
    }

    internal class InvalidSubPackage : PackageValidationIssue
    {
        public InvalidSubPackage(int subPackageId) : base(subPackageId)
        {
        }
    }

    internal class InvalidParentPackage : PackageValidationIssue
    {
        public InvalidParentPackage(int packageId) : base(packageId)
        {
        }
    }
}