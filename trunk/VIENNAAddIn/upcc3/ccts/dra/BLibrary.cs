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

        public IBLibrary CreateBLibrary(LibrarySpec spec)
        {
            return new BLibrary(repository, CreateLibraryPackage(spec, util.Stereotype.BLibrary));
        }

        public ICDTLibrary CreateCDTLibrary(LibrarySpec spec)
        {
            return new CDTLibrary(repository, CreateLibraryPackage(spec, util.Stereotype.CDTLibrary));
        }

        public ICCLibrary CreateCCLibrary(LibrarySpec spec)
        {
            return new CCLibrary(repository, CreateLibraryPackage(spec, util.Stereotype.CCLibrary));
        }

        public IBDTLibrary CreateBDTLibrary(LibrarySpec spec)
        {
            return new BDTLibrary(repository, CreateLibraryPackage(spec, util.Stereotype.BDTLibrary));
        }

        public IBIELibrary CreateBIELibrary(LibrarySpec spec)
        {
            return new BIELibrary(repository, CreateLibraryPackage(spec, util.Stereotype.BIELibrary));
        }

        public IPRIMLibrary CreatePRIMLibrary(LibrarySpec spec)
        {
            return new PRIMLibrary(repository, CreateLibraryPackage(spec, util.Stereotype.PRIMLibrary));
        }

        public IENUMLibrary CreateENUMLibrary(LibrarySpec spec)
        {
            return new ENUMLibrary(repository, CreateLibraryPackage(spec, util.Stereotype.ENUMLibrary));
        }

        #endregion

        private Package CreateLibraryPackage(LibrarySpec spec, string stereotype)
        {
            var libraryPackage = (Package) package.Packages.AddNew(spec.Name, "");
            libraryPackage.Update();
            libraryPackage.ParentID = Id;
            libraryPackage.Element.Stereotype = stereotype;
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

            return libraryPackage;
        }
    }
}