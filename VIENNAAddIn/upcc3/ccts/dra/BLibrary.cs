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

            libraryPackage.Element.SetTaggedValues(TaggedValues.businessTerm, spec.BusinessTerms);
            libraryPackage.Element.SetTaggedValues(TaggedValues.copyright, spec.Copyrights);
            libraryPackage.Element.SetTaggedValues(TaggedValues.owner, spec.Owners);
            libraryPackage.Element.SetTaggedValues(TaggedValues.reference, spec.References);
            libraryPackage.Element.SetTaggedValue(TaggedValues.status, spec.Status);
            libraryPackage.Element.SetTaggedValue(TaggedValues.uniqueIdentifier, spec.UniqueIdentifier);
            libraryPackage.Element.SetTaggedValue(TaggedValues.versionIdentifier, spec.VersionIdentifier);
            libraryPackage.Update();

            var packageDiagram = (Diagram)libraryPackage.Diagrams.AddNew(spec.Name, "Package");
            packageDiagram.Update();
            libraryPackage.Diagrams.Refresh();

            package.Packages.Refresh();
            AddPackageToDiagram(libraryPackage);
            return new BLibrary(repository, libraryPackage);
        }

        public IBLibrary UpdateBLibrary(IBLibrary bLibrary, BLibrarySpec specification)
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

            libraryPackage.Element.SetTaggedValue(TaggedValues.baseURN, specification.BaseURN);
            libraryPackage.Element.SetTaggedValues(TaggedValues.businessTerm, specification.BusinessTerms);
            libraryPackage.Element.SetTaggedValues(TaggedValues.copyright, specification.Copyrights);
            libraryPackage.Element.SetTaggedValue(TaggedValues.namespacePrefix, specification.NamespacePrefix);
            libraryPackage.Element.SetTaggedValues(TaggedValues.owner, specification.Owners);
            libraryPackage.Element.SetTaggedValues(TaggedValues.reference, specification.References);
            libraryPackage.Element.SetTaggedValue(TaggedValues.status, specification.Status);
            libraryPackage.Element.SetTaggedValue(TaggedValues.uniqueIdentifier, specification.UniqueIdentifier);
            libraryPackage.Element.SetTaggedValue(TaggedValues.versionIdentifier, specification.VersionIdentifier);
            libraryPackage.Update();
            
            var classDiagram = (Diagram) libraryPackage.Diagrams.AddNew(specification.Name, "Class");
            classDiagram.Update();
            libraryPackage.Diagrams.Refresh();

            package.Packages.Refresh();
            AddPackageToDiagram(libraryPackage);
            return new PRIMLibrary(repository, libraryPackage);
        }

        public IPrimLibrary UpdatePrimLibrary(IPrimLibrary primLibrary, PrimLibrarySpec specification)
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

            libraryPackage.Element.SetTaggedValue(TaggedValues.baseURN, specification.BaseURN);
            libraryPackage.Element.SetTaggedValues(TaggedValues.businessTerm, specification.BusinessTerms);
            libraryPackage.Element.SetTaggedValues(TaggedValues.copyright, specification.Copyrights);
            libraryPackage.Element.SetTaggedValue(TaggedValues.namespacePrefix, specification.NamespacePrefix);
            libraryPackage.Element.SetTaggedValues(TaggedValues.owner, specification.Owners);
            libraryPackage.Element.SetTaggedValues(TaggedValues.reference, specification.References);
            libraryPackage.Element.SetTaggedValue(TaggedValues.status, specification.Status);
            libraryPackage.Element.SetTaggedValue(TaggedValues.uniqueIdentifier, specification.UniqueIdentifier);
            libraryPackage.Element.SetTaggedValue(TaggedValues.versionIdentifier, specification.VersionIdentifier);
            libraryPackage.Update();
            
            var classDiagram = (Diagram) libraryPackage.Diagrams.AddNew(specification.Name, "Class");
            classDiagram.Update();
            libraryPackage.Diagrams.Refresh();

            package.Packages.Refresh();
            AddPackageToDiagram(libraryPackage);
            return new ENUMLibrary(repository, libraryPackage);
        }

        public IEnumLibrary UpdateEnumLibrary(IEnumLibrary enumLibrary, EnumLibrarySpec specification)
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
            throw new NotImplementedException();
        }

        public ICdtLibrary CreateCdtLibrary(CdtLibrarySpec specification)
        {
            var libraryPackage = (Package) package.Packages.AddNew(specification.Name, string.Empty);
            libraryPackage.Update();
            libraryPackage.ParentID = package.PackageID;
            libraryPackage.Element.Stereotype = util.Stereotype.CDTLibrary;

            libraryPackage.Element.SetTaggedValue(TaggedValues.baseURN, specification.BaseURN);
            libraryPackage.Element.SetTaggedValues(TaggedValues.businessTerm, specification.BusinessTerms);
            libraryPackage.Element.SetTaggedValues(TaggedValues.copyright, specification.Copyrights);
            libraryPackage.Element.SetTaggedValue(TaggedValues.namespacePrefix, specification.NamespacePrefix);
            libraryPackage.Element.SetTaggedValues(TaggedValues.owner, specification.Owners);
            libraryPackage.Element.SetTaggedValues(TaggedValues.reference, specification.References);
            libraryPackage.Element.SetTaggedValue(TaggedValues.status, specification.Status);
            libraryPackage.Element.SetTaggedValue(TaggedValues.uniqueIdentifier, specification.UniqueIdentifier);
            libraryPackage.Element.SetTaggedValue(TaggedValues.versionIdentifier, specification.VersionIdentifier);
            libraryPackage.Update();

            var classDiagram = (Diagram) libraryPackage.Diagrams.AddNew(specification.Name, "Class");
            classDiagram.Update();
            libraryPackage.Diagrams.Refresh();

            package.Packages.Refresh();
            AddPackageToDiagram(libraryPackage);
            return new CDTLibrary(repository, libraryPackage);
        }

        public ICdtLibrary UpdateCdtLibrary(ICdtLibrary cdtLibrary, CdtLibrarySpec specification)
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

            libraryPackage.Element.SetTaggedValue(TaggedValues.baseURN, specification.BaseURN);
            libraryPackage.Element.SetTaggedValues(TaggedValues.businessTerm, specification.BusinessTerms);
            libraryPackage.Element.SetTaggedValues(TaggedValues.copyright, specification.Copyrights);
            libraryPackage.Element.SetTaggedValue(TaggedValues.namespacePrefix, specification.NamespacePrefix);
            libraryPackage.Element.SetTaggedValues(TaggedValues.owner, specification.Owners);
            libraryPackage.Element.SetTaggedValues(TaggedValues.reference, specification.References);
            libraryPackage.Element.SetTaggedValue(TaggedValues.status, specification.Status);
            libraryPackage.Element.SetTaggedValue(TaggedValues.uniqueIdentifier, specification.UniqueIdentifier);
            libraryPackage.Element.SetTaggedValue(TaggedValues.versionIdentifier, specification.VersionIdentifier);
            libraryPackage.Update();
            
            var classDiagram = (Diagram) libraryPackage.Diagrams.AddNew(specification.Name, "Class");
            classDiagram.Update();
            libraryPackage.Diagrams.Refresh();

            package.Packages.Refresh();
            AddPackageToDiagram(libraryPackage);
            return new CCLibrary(repository, libraryPackage);
        }

        public ICcLibrary UpdateCcLibrary(ICcLibrary ccLibrary, CcLibrarySpec specification)
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

            libraryPackage.Element.SetTaggedValue(TaggedValues.baseURN, specification.BaseUrn);
            libraryPackage.Element.SetTaggedValues(TaggedValues.businessTerm, specification.BusinessTerms);
            libraryPackage.Element.SetTaggedValues(TaggedValues.copyright, specification.Copyrights);
            libraryPackage.Element.SetTaggedValue(TaggedValues.namespacePrefix, specification.NamespacePrefix);
            libraryPackage.Element.SetTaggedValues(TaggedValues.owner, specification.Owners);
            libraryPackage.Element.SetTaggedValues(TaggedValues.reference, specification.References);
            libraryPackage.Element.SetTaggedValue(TaggedValues.status, specification.Status);
            libraryPackage.Element.SetTaggedValue(TaggedValues.uniqueIdentifier, specification.UniqueIdentifier);
            libraryPackage.Element.SetTaggedValue(TaggedValues.versionIdentifier, specification.VersionIdentifier);
            libraryPackage.Update();
            
            var classDiagram = (Diagram) libraryPackage.Diagrams.AddNew(specification.Name, "Class");
            classDiagram.Update();
            libraryPackage.Diagrams.Refresh();

            package.Packages.Refresh();
            AddPackageToDiagram(libraryPackage);
            return new BDTLibrary(repository, libraryPackage);
        }

        public IBdtLibrary UpdateBdtLibrary(IBdtLibrary bdtLibrary, BdtLibrarySpec specification)
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

            libraryPackage.Element.SetTaggedValue(TaggedValues.baseURN, specification.BaseURN);
            libraryPackage.Element.SetTaggedValues(TaggedValues.businessTerm, specification.BusinessTerms);
            libraryPackage.Element.SetTaggedValues(TaggedValues.copyright, specification.Copyrights);
            libraryPackage.Element.SetTaggedValue(TaggedValues.namespacePrefix, specification.NamespacePrefix);
            libraryPackage.Element.SetTaggedValues(TaggedValues.owner, specification.Owners);
            libraryPackage.Element.SetTaggedValues(TaggedValues.reference, specification.References);
            libraryPackage.Element.SetTaggedValue(TaggedValues.status, specification.Status);
            libraryPackage.Element.SetTaggedValue(TaggedValues.uniqueIdentifier, specification.UniqueIdentifier);
            libraryPackage.Element.SetTaggedValue(TaggedValues.versionIdentifier, specification.VersionIdentifier);
            libraryPackage.Update();
            
            var classDiagram = (Diagram) libraryPackage.Diagrams.AddNew(specification.Name, "Class");
            classDiagram.Update();
            libraryPackage.Diagrams.Refresh();

            package.Packages.Refresh();
            AddPackageToDiagram(libraryPackage);
            return new BIELibrary(repository, libraryPackage);
        }

        public IBieLibrary UpdateBieLibrary(IBieLibrary bieLibrary, BieLibrarySpec specification)
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

            libraryPackage.Element.SetTaggedValue(TaggedValues.baseURN, specification.BaseURN);
            libraryPackage.Element.SetTaggedValues(TaggedValues.businessTerm, specification.BusinessTerms);
            libraryPackage.Element.SetTaggedValues(TaggedValues.copyright, specification.Copyrights);
            libraryPackage.Element.SetTaggedValue(TaggedValues.namespacePrefix, specification.NamespacePrefix);
            libraryPackage.Element.SetTaggedValues(TaggedValues.owner, specification.Owners);
            libraryPackage.Element.SetTaggedValues(TaggedValues.reference, specification.References);
            libraryPackage.Element.SetTaggedValue(TaggedValues.status, specification.Status);
            libraryPackage.Element.SetTaggedValue(TaggedValues.uniqueIdentifier, specification.UniqueIdentifier);
            libraryPackage.Element.SetTaggedValue(TaggedValues.versionIdentifier, specification.VersionIdentifier);
            libraryPackage.Update();
            
            var classDiagram = (Diagram) libraryPackage.Diagrams.AddNew(specification.Name, "Class");
            classDiagram.Update();
            libraryPackage.Diagrams.Refresh();

            package.Packages.Refresh();
            AddPackageToDiagram(libraryPackage);
            return new DOCLibrary(repository, libraryPackage);
        }

        public IDocLibrary UpdateDocLibrary(IDocLibrary docLibrary, DocLibrarySpec specification)
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