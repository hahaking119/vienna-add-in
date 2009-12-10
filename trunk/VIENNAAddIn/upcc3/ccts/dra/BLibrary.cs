// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using CctsRepository.BdtLibrary;
using CctsRepository.BieLibrary;
using CctsRepository.BLibrary;
using CctsRepository.CcLibrary;
using CctsRepository.CdtLibrary;
using CctsRepository.DocLibrary;
using CctsRepository.EnumLibrary;
using CctsRepository.PrimLibrary;
using EA;
using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddIn.upcc3.export.cctsndr;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    internal class BLibrary : BusinessLibrary, IBLibrary
    {
        public BLibrary(CCRepository repository, Package package) : base(repository, package)
        {
        }

        private IEnumerable<Package> ContainedPackages
        {
            get { return package.EnumerateContainedPackages(); }
        }

        #region IBLibrary Members

        public IEnumerable<IBLibrary> GetBLibraries()
        {
            return from p in ContainedPackages
                   where p.IsBLibrary()
                   select (IBLibrary) new BLibrary(repository, p);
        }

        public IBLibrary GetBLibraryByName(string name)
        {
            throw new NotImplementedException();
        }

        public IBLibrary CreateBLibrary(BLibrarySpec spec)
        {
            var libraryPackage = (Package)package.Packages.AddNew(spec.Name, string.Empty);
            libraryPackage.Update();
            libraryPackage.ParentID = package.PackageID;
            libraryPackage.Element.Stereotype = util.Stereotype.bLibrary;

            var packageDiagram = (Diagram)libraryPackage.Diagrams.AddNew(spec.Name, "Package");
            packageDiagram.Update();
            libraryPackage.Diagrams.Refresh();

            package.Packages.Refresh();
            AddPackageToDiagram(libraryPackage);

            BLibrary bLibrary = new BLibrary(repository, libraryPackage);
            bLibrary.Update(spec);
            return bLibrary;
        }

        public IBLibrary UpdateBLibrary(IBLibrary bLibrary, BLibrarySpec specification)
        {
            ((BLibrary)bLibrary).Update(specification);
            return bLibrary;
        }

        public void Update(BLibrarySpec spec)
        {
            package.Element.SetTaggedValues(TaggedValues.businessTerm, spec.BusinessTerms);
            package.Element.SetTaggedValues(TaggedValues.copyright, spec.Copyrights);
            package.Element.SetTaggedValues(TaggedValues.owner, spec.Owners);
            package.Element.SetTaggedValues(TaggedValues.reference, spec.References);
            package.Element.SetTaggedValue(TaggedValues.status, spec.Status);
            package.Element.SetTaggedValue(TaggedValues.uniqueIdentifier, spec.UniqueIdentifier.DefaultTo(package.PackageGUID));
            package.Element.SetTaggedValue(TaggedValues.versionIdentifier, spec.VersionIdentifier);
            package.Update();
        }

        public void RemoveBLibrary(IBLibrary bLibrary)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IPrimLibrary> GetPrimLibraries()
        {
            return from p in ContainedPackages
                   where p.IsPRIMLibrary()
                   select (IPrimLibrary) new PRIMLibrary(repository, p);
        }

        public IPrimLibrary GetPrimLibraryByName(string name)
        {
            throw new NotImplementedException();
        }

        public IPrimLibrary CreatePrimLibrary(PrimLibrarySpec specification)
        {
            var libraryPackage = (Package) package.Packages.AddNew(specification.Name, string.Empty);
            libraryPackage.Update();
            libraryPackage.ParentID = package.PackageID;
            libraryPackage.Element.Stereotype = util.Stereotype.PRIMLibrary;

            var classDiagram = (Diagram) libraryPackage.Diagrams.AddNew(specification.Name, "Class");
            classDiagram.Update();
            libraryPackage.Diagrams.Refresh();

            package.Packages.Refresh();
            AddPackageToDiagram(libraryPackage);
            
            PRIMLibrary primLibrary = new PRIMLibrary(repository, libraryPackage);
            primLibrary.Update(specification);

            return primLibrary;
        }

        public IPrimLibrary UpdatePrimLibrary(IPrimLibrary primLibrary, PrimLibrarySpec specification)
        {
            ((PRIMLibrary)primLibrary).Update(specification);
            return primLibrary;
        }

        public void RemovePrimLibrary(IPrimLibrary primLibrary)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IEnumLibrary> GetEnumLibraries()
        {
            return from p in ContainedPackages
                   where p.IsENUMLibrary()
                   select (IEnumLibrary) new ENUMLibrary(repository, p);
        }

        public IEnumLibrary GetEnumLibraryByName(string name)
        {
            throw new NotImplementedException();
        }

        public IEnumLibrary CreateEnumLibrary(EnumLibrarySpec specification)
        {
            var libraryPackage = (Package) package.Packages.AddNew(specification.Name, string.Empty);
            libraryPackage.Update();
            libraryPackage.ParentID = package.PackageID;
            libraryPackage.Element.Stereotype = util.Stereotype.ENUMLibrary;
           
            var classDiagram = (Diagram) libraryPackage.Diagrams.AddNew(specification.Name, "Class");
            classDiagram.Update();
            libraryPackage.Diagrams.Refresh();

            package.Packages.Refresh();
            AddPackageToDiagram(libraryPackage);

            ENUMLibrary enumLibrary = new ENUMLibrary(repository, libraryPackage);
            enumLibrary.Update(specification);

            return enumLibrary;
        }

        public IEnumLibrary UpdateEnumLibrary(IEnumLibrary enumLibrary, EnumLibrarySpec specification)
        {
            ((ENUMLibrary) enumLibrary).Update(specification);
            return enumLibrary;
        }

        public void RemoveEnumLibrary(IEnumLibrary enumLibrary)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ICdtLibrary> GetCdtLibraries()
        {
            return from p in ContainedPackages
                   where p.IsCDTLibrary()
                   select (ICdtLibrary) new CDTLibrary(repository, p);
        }

        public ICdtLibrary GetCdtLibraryByName(string name)
        {
            return GetCdtLibraries().FirstOrDefault(cdtLibrary => cdtLibrary.Name == name);
        }

        public ICdtLibrary CreateCdtLibrary(CdtLibrarySpec specification)
        {
            var libraryPackage = (Package) package.Packages.AddNew(specification.Name, string.Empty);
            libraryPackage.Update();
            libraryPackage.ParentID = package.PackageID;
            libraryPackage.Element.Stereotype = util.Stereotype.CDTLibrary;

            var classDiagram = (Diagram) libraryPackage.Diagrams.AddNew(specification.Name, "Class");
            classDiagram.Update();
            libraryPackage.Diagrams.Refresh();

            package.Packages.Refresh();
            AddPackageToDiagram(libraryPackage);

            CDTLibrary cdtLibrary = new CDTLibrary(repository, libraryPackage);
            cdtLibrary.Update(specification);
            return cdtLibrary;
        }

        public ICdtLibrary UpdateCdtLibrary(ICdtLibrary cdtLibrary, CdtLibrarySpec specification)
        {
            ((CDTLibrary)cdtLibrary).Update(specification);
            return cdtLibrary;
        }

        public void RemoveCdtLibrary(ICdtLibrary cdtLibrary)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ICcLibrary> GetCcLibraries()
        {
            return from p in ContainedPackages
                   where p.IsCCLibrary()
                   select (ICcLibrary) new CCLibrary(repository, p);
        }

        public ICcLibrary GetCcLibraryByName(string name)
        {
            throw new NotImplementedException();
        }

        public ICcLibrary CreateCcLibrary(CcLibrarySpec specification)
        {
            var libraryPackage = (Package) package.Packages.AddNew(specification.Name, string.Empty);
            libraryPackage.Update();
            libraryPackage.ParentID = package.PackageID;
            libraryPackage.Element.Stereotype = util.Stereotype.CCLibrary;
           
            var classDiagram = (Diagram) libraryPackage.Diagrams.AddNew(specification.Name, "Class");
            classDiagram.Update();
            libraryPackage.Diagrams.Refresh();

            package.Packages.Refresh();
            AddPackageToDiagram(libraryPackage);

            CCLibrary ccLibrary = new CCLibrary(repository, libraryPackage);
            ccLibrary.Update(specification);

            return ccLibrary;
        }

        public ICcLibrary UpdateCcLibrary(ICcLibrary ccLibrary, CcLibrarySpec specification)
        {
            ((CCLibrary) ccLibrary).Update(specification);
            return ccLibrary;
        }

        public void RemoveCcLibrary(ICcLibrary ccLibrary)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IBdtLibrary> GetBdtLibraries()
        {
            return from p in ContainedPackages
                   where p.IsBDTLibrary()
                   select (IBdtLibrary) new BDTLibrary(repository, p);
        }

        public IBdtLibrary GetBdtLibraryByName(string name)
        {
            throw new NotImplementedException();
        }

        public IBdtLibrary CreateBdtLibrary(BdtLibrarySpec specification)
        {
            var libraryPackage = (Package) package.Packages.AddNew(specification.Name, string.Empty);
            libraryPackage.Update();
            libraryPackage.ParentID = package.PackageID;
            libraryPackage.Element.Stereotype = util.Stereotype.BDTLibrary;
            
            var classDiagram = (Diagram) libraryPackage.Diagrams.AddNew(specification.Name, "Class");
            classDiagram.Update();
            libraryPackage.Diagrams.Refresh();

            package.Packages.Refresh();
            AddPackageToDiagram(libraryPackage);

            BDTLibrary bdtLibrary = new BDTLibrary(repository, libraryPackage);
            bdtLibrary.Update(specification);
            return bdtLibrary;
        }

        public IBdtLibrary UpdateBdtLibrary(IBdtLibrary bdtLibrary, BdtLibrarySpec specification)
        {
            ((BDTLibrary) bdtLibrary).Update(specification);
            return bdtLibrary;
        }

        public void RemoveBdtLibrary(IBdtLibrary bdtLibrary)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IBieLibrary> GetBieLibraries()
        {
            return from p in ContainedPackages
                   where p.IsBIELibrary()
                   select (IBieLibrary) new BIELibrary(repository, p);
        }

        public IBieLibrary GetBieLibraryByName(string name)
        {
            throw new NotImplementedException();
        }

        public IBieLibrary CreateBieLibrary(BieLibrarySpec specification)
        {
            var libraryPackage = (Package) package.Packages.AddNew(specification.Name, string.Empty);
            libraryPackage.Update();
            libraryPackage.ParentID = package.PackageID;
            libraryPackage.Element.Stereotype = util.Stereotype.BIELibrary;

            var classDiagram = (Diagram) libraryPackage.Diagrams.AddNew(specification.Name, "Class");
            classDiagram.Update();
            libraryPackage.Diagrams.Refresh();

            package.Packages.Refresh();
            AddPackageToDiagram(libraryPackage);

            BIELibrary bieLibrary = new BIELibrary(repository, libraryPackage);
            bieLibrary.Update(specification);
            return bieLibrary;
        }

        public IBieLibrary UpdateBieLibrary(IBieLibrary bieLibrary, BieLibrarySpec specification)
        {
            ((BIELibrary) bieLibrary).Update(specification);
            return bieLibrary;
        }

        public void RemoveBieLibrary(IBieLibrary bieLibrary)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IDocLibrary> GetDocLibraries()
        {
            return from p in ContainedPackages
                   where p.IsDOCLibrary()
                   select (IDocLibrary) new DOCLibrary(repository, p);
        }

        public IDocLibrary GetDocLibraryByName(string name)
        {
            throw new NotImplementedException();
        }

        public IDocLibrary CreateDocLibrary(DocLibrarySpec specification)
        {
            var libraryPackage = (Package) package.Packages.AddNew(specification.Name, string.Empty);
            libraryPackage.Update();
            libraryPackage.ParentID = package.PackageID;
            libraryPackage.Element.Stereotype = util.Stereotype.DOCLibrary;

            var classDiagram = (Diagram) libraryPackage.Diagrams.AddNew(specification.Name, "Class");
            classDiagram.Update();
            libraryPackage.Diagrams.Refresh();

            package.Packages.Refresh();
            AddPackageToDiagram(libraryPackage);
            DOCLibrary docLibrary = new DOCLibrary(repository, libraryPackage);
            docLibrary.Update(specification);
            return docLibrary;
        }

        public IDocLibrary UpdateDocLibrary(IDocLibrary docLibrary, DocLibrarySpec specification)
        {
            ((DOCLibrary) docLibrary).Update(specification);
            return docLibrary;
        }

        public void RemoveDocLibrary(IDocLibrary docLibrary)
        {
            throw new NotImplementedException();
        }

        public IBLibrary Parent
        {
            get { return BLibrary; }
        }

        #endregion

        private void AddPackageToDiagram(Package libraryPackage)
        {
            var diagram = (Diagram) package.Diagrams.GetByName(Name);
            var newDiagramObject = (DiagramObject) diagram.DiagramObjects.AddNew(string.Empty, string.Empty);
            newDiagramObject.DiagramID = diagram.DiagramID;
            newDiagramObject.ElementID = libraryPackage.Element.ElementID;
            newDiagramObject.Update();
        }
    }
}