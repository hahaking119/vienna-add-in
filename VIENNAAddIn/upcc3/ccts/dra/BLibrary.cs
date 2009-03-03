using System;
using System.Collections.Generic;
using EA;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    internal class BLibrary : BusinessLibrary, IBLibrary
    {
        public BLibrary(CCRepository repository, Package package) : base(repository, package)
        {
        }

        #region IBLibrary Members

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
                foreach (IBusinessLibrary childLib in Children)
                {
                    yield return childLib;
                    if (childLib is IBLibrary)
                    {
                        foreach (IBusinessLibrary grandChild in ((IBLibrary) childLib).AllChildren)
                        {
                            yield return grandChild;
                        }
                    }
                }
            }
        }

        public IBusinessLibrary FindChildByName(string name)
        {
            foreach (IBusinessLibrary child in Children)
            {
                if (child.Name == name)
                {
                    return child;
                }
            }
            return null;
        }

        public IBDTLibrary CreateBDTLibrary(LibrarySpec spec)
        {
            var libraryPackage = (Package) package.Packages.AddNew(spec.Name, "");
            Console.WriteLine("lib: " + libraryPackage.Name);
            libraryPackage.Update();
            libraryPackage.Element.Stereotype = "BDTLibrary";
            libraryPackage.ParentID = Id;
            Collection taggedValues = libraryPackage.Element.TaggedValues;
            taggedValues.AddTaggedValue(TaggedValues.BaseURN, spec.BaseURN);
            taggedValues.AddTaggedValues(TaggedValues.BusinessTerm, spec.BusinessTerms);
            taggedValues.AddTaggedValues(TaggedValues.Copyright, spec.Copyrights);
            taggedValues.AddTaggedValue(TaggedValues.NamespacePrefix, spec.NamespacePrefix);
            taggedValues.AddTaggedValues(TaggedValues.Owner, spec.Owners);
            taggedValues.AddTaggedValues(TaggedValues.Reference, spec.References);
            taggedValues.AddTaggedValue(TaggedValues.Status, spec.Status);
            taggedValues.AddTaggedValue(TaggedValues.UniqueIdentifier, spec.UniqueIdentifier);
            taggedValues.AddTaggedValue(TaggedValues.VersionIdentifier, spec.VersionIdentifier);
            taggedValues.Refresh();
            libraryPackage.Update();
            package.Packages.Refresh();
            return new BDTLibrary(repository, libraryPackage);
        }

        #endregion
    }
}