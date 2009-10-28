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
using UPCCRepositoryInterface;

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
            return new BLibrary(repository, CreateLibraryPackage(spec, UPCCRepositoryInterface.Stereotype.bLibrary));
        }

        public ICDTLibrary CreateCDTLibrary(LibrarySpec spec)
        {
            return new CDTLibrary(repository, CreateLibraryPackage(spec, UPCCRepositoryInterface.Stereotype.CDTLibrary));
        }

        public ICCLibrary CreateCCLibrary(LibrarySpec spec)
        {
            return new CCLibrary(repository, CreateLibraryPackage(spec, UPCCRepositoryInterface.Stereotype.CCLibrary));
        }

        public IBDTLibrary CreateBDTLibrary(LibrarySpec spec)
        {
            return new BDTLibrary(repository, CreateLibraryPackage(spec, UPCCRepositoryInterface.Stereotype.BDTLibrary));
        }

        public IBIELibrary CreateBIELibrary(LibrarySpec spec)
        {
            return new BIELibrary(repository, CreateLibraryPackage(spec, UPCCRepositoryInterface.Stereotype.BIELibrary));
        }

        public IPRIMLibrary CreatePRIMLibrary(LibrarySpec spec)
        {
            return new PRIMLibrary(repository, CreateLibraryPackage(spec, UPCCRepositoryInterface.Stereotype.PRIMLibrary));
        }

        public IENUMLibrary CreateENUMLibrary(LibrarySpec spec)
        {
            return new ENUMLibrary(repository, CreateLibraryPackage(spec, UPCCRepositoryInterface.Stereotype.ENUMLibrary));
        }

        public IDOCLibrary CreateDOCLibrary(LibrarySpec spec)
        {
            return new DOCLibrary(repository, CreateLibraryPackage(spec, UPCCRepositoryInterface.Stereotype.DOCLibrary));
        }

        #endregion

        private Package CreateLibraryPackage(LibrarySpec spec, string stereotype)
        {
            Package libraryPackage = CreateLibraryPackage(spec, package, stereotype);
            AddPackageToDiagram(libraryPackage);
            return libraryPackage;
        }

        private void AddPackageToDiagram(Package libraryPackage)
        {
            var diagram = (Diagram) package.Diagrams.GetByName(Name);
            var newDiagramObject = (DiagramObject) diagram.DiagramObjects.AddNew("", "");
            newDiagramObject.DiagramID = diagram.DiagramID;
            newDiagramObject.ElementID = libraryPackage.Element.ElementID;
            newDiagramObject.Update();
        }
    }
}