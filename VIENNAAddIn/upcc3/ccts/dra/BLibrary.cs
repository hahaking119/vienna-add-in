// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System.Collections.Generic;
using System.Linq;
using CctsRepository.BdtLibrary;
using CctsRepository.BieLibrary;
using CctsRepository.bLibrary;
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

        public IEnumerable<IPRIMLibrary> GetPrimLibraries()
        {
            return from p in ContainedPackages
                   where p.IsPRIMLibrary()
                   select (IPRIMLibrary) new PRIMLibrary(repository, p);
        }

        public IEnumerable<IENUMLibrary> GetEnumLibraries()
        {
            return from p in ContainedPackages
                   where p.IsENUMLibrary()
                   select (IENUMLibrary) new ENUMLibrary(repository, p);
        }

        public IEnumerable<ICDTLibrary> GetCdtLibraries()
        {
            return from p in ContainedPackages
                   where p.IsCDTLibrary()
                   select (ICDTLibrary) new CDTLibrary(repository, p);
        }

        public IEnumerable<ICCLibrary> GetCcLibraries()
        {
            return from p in ContainedPackages
                   where p.IsCCLibrary()
                   select (ICCLibrary) new CCLibrary(repository, p);
        }

        public IEnumerable<IBdtLibrary> GetBdtLibraries()
        {
            return from p in ContainedPackages
                   where p.IsBDTLibrary()
                   select (IBdtLibrary) new BDTLibrary(repository, p);
        }

        public IEnumerable<IBieLibrary> GetBieLibraries()
        {
            return from p in ContainedPackages
                   where p.IsBIELibrary()
                   select (IBieLibrary) new BIELibrary(repository, p);
        }

        public IEnumerable<IDOCLibrary> GetDocLibraries()
        {
            return from p in ContainedPackages
                   where p.IsDOCLibrary()
                   select (IDOCLibrary) new DOCLibrary(repository, p);
        }

        public IBLibrary CreateBLibrary(BLibrarySpec spec)
        {
            var libraryPackage = (Package) package.Packages.AddNew(spec.Name, string.Empty);
            libraryPackage.Update();
            libraryPackage.ParentID = package.PackageID;
            libraryPackage.Element.Stereotype = util.Stereotype.bLibrary;

            libraryPackage.Element.SetTaggedValue(TaggedValues.baseURN, spec.BaseURN);
            libraryPackage.Element.SetTaggedValues(TaggedValues.businessTerm, spec.BusinessTerms);
            libraryPackage.Element.SetTaggedValues(TaggedValues.copyright, spec.Copyrights);
            libraryPackage.Element.SetTaggedValue(TaggedValues.namespacePrefix, spec.NamespacePrefix);
            libraryPackage.Element.SetTaggedValues(TaggedValues.owner, spec.Owners);
            libraryPackage.Element.SetTaggedValues(TaggedValues.reference, spec.References);
            libraryPackage.Element.SetTaggedValue(TaggedValues.status, spec.Status);
            libraryPackage.Element.SetTaggedValue(TaggedValues.uniqueIdentifier, spec.UniqueIdentifier);
            libraryPackage.Element.SetTaggedValue(TaggedValues.versionIdentifier, spec.VersionIdentifier);
            libraryPackage.Update();

            var packageDiagram = (Diagram) libraryPackage.Diagrams.AddNew(spec.Name, "Package");
            packageDiagram.Update();
            libraryPackage.Diagrams.Refresh();

            package.Packages.Refresh();
            AddPackageToDiagram(libraryPackage);
            return new BLibrary(repository, libraryPackage);
        }

        public ICDTLibrary CreateCDTLibrary(CdtLibrarySpec spec)
        {
            var libraryPackage = (Package) package.Packages.AddNew(spec.Name, string.Empty);
            libraryPackage.Update();
            libraryPackage.ParentID = package.PackageID;
            libraryPackage.Element.Stereotype = util.Stereotype.CDTLibrary;

            libraryPackage.Element.SetTaggedValue(TaggedValues.baseURN, spec.BaseURN);
            libraryPackage.Element.SetTaggedValues(TaggedValues.businessTerm, spec.BusinessTerms);
            libraryPackage.Element.SetTaggedValues(TaggedValues.copyright, spec.Copyrights);
            libraryPackage.Element.SetTaggedValue(TaggedValues.namespacePrefix, spec.NamespacePrefix);
            libraryPackage.Element.SetTaggedValues(TaggedValues.owner, spec.Owners);
            libraryPackage.Element.SetTaggedValues(TaggedValues.reference, spec.References);
            libraryPackage.Element.SetTaggedValue(TaggedValues.status, spec.Status);
            libraryPackage.Element.SetTaggedValue(TaggedValues.uniqueIdentifier, spec.UniqueIdentifier);
            libraryPackage.Element.SetTaggedValue(TaggedValues.versionIdentifier, spec.VersionIdentifier);
            libraryPackage.Update();

            var classDiagram = (Diagram) libraryPackage.Diagrams.AddNew(spec.Name, "Class");
            classDiagram.Update();
            libraryPackage.Diagrams.Refresh();

            package.Packages.Refresh();
            AddPackageToDiagram(libraryPackage);
            return new CDTLibrary(repository, libraryPackage);
        }

        public ICCLibrary CreateCCLibrary(CcLibrarySpec spec)
        {
            var libraryPackage = (Package) package.Packages.AddNew(spec.Name, string.Empty);
            libraryPackage.Update();
            libraryPackage.ParentID = package.PackageID;
            libraryPackage.Element.Stereotype = util.Stereotype.CCLibrary;

            libraryPackage.Element.SetTaggedValue(TaggedValues.baseURN, spec.BaseURN);
            libraryPackage.Element.SetTaggedValues(TaggedValues.businessTerm, spec.BusinessTerms);
            libraryPackage.Element.SetTaggedValues(TaggedValues.copyright, spec.Copyrights);
            libraryPackage.Element.SetTaggedValue(TaggedValues.namespacePrefix, spec.NamespacePrefix);
            libraryPackage.Element.SetTaggedValues(TaggedValues.owner, spec.Owners);
            libraryPackage.Element.SetTaggedValues(TaggedValues.reference, spec.References);
            libraryPackage.Element.SetTaggedValue(TaggedValues.status, spec.Status);
            libraryPackage.Element.SetTaggedValue(TaggedValues.uniqueIdentifier, spec.UniqueIdentifier);
            libraryPackage.Element.SetTaggedValue(TaggedValues.versionIdentifier, spec.VersionIdentifier);
            libraryPackage.Update();
            
            var classDiagram = (Diagram) libraryPackage.Diagrams.AddNew(spec.Name, "Class");
            classDiagram.Update();
            libraryPackage.Diagrams.Refresh();

            package.Packages.Refresh();
            AddPackageToDiagram(libraryPackage);
            return new CCLibrary(repository, libraryPackage);
        }

        public IBdtLibrary CreateBDTLibrary(BdtLibrarySpec spec)
        {
            var libraryPackage = (Package) package.Packages.AddNew(spec.Name, string.Empty);
            libraryPackage.Update();
            libraryPackage.ParentID = package.PackageID;
            libraryPackage.Element.Stereotype = util.Stereotype.BDTLibrary;

            libraryPackage.Element.SetTaggedValue(TaggedValues.baseURN, spec.BaseURN);
            libraryPackage.Element.SetTaggedValues(TaggedValues.businessTerm, spec.BusinessTerms);
            libraryPackage.Element.SetTaggedValues(TaggedValues.copyright, spec.Copyrights);
            libraryPackage.Element.SetTaggedValue(TaggedValues.namespacePrefix, spec.NamespacePrefix);
            libraryPackage.Element.SetTaggedValues(TaggedValues.owner, spec.Owners);
            libraryPackage.Element.SetTaggedValues(TaggedValues.reference, spec.References);
            libraryPackage.Element.SetTaggedValue(TaggedValues.status, spec.Status);
            libraryPackage.Element.SetTaggedValue(TaggedValues.uniqueIdentifier, spec.UniqueIdentifier);
            libraryPackage.Element.SetTaggedValue(TaggedValues.versionIdentifier, spec.VersionIdentifier);
            libraryPackage.Update();
            
            var classDiagram = (Diagram) libraryPackage.Diagrams.AddNew(spec.Name, "Class");
            classDiagram.Update();
            libraryPackage.Diagrams.Refresh();

            package.Packages.Refresh();
            AddPackageToDiagram(libraryPackage);
            return new BDTLibrary(repository, libraryPackage);
        }

        public IBieLibrary CreateBIELibrary(BieLibrarySpec spec)
        {
            var libraryPackage = (Package) package.Packages.AddNew(spec.Name, string.Empty);
            libraryPackage.Update();
            libraryPackage.ParentID = package.PackageID;
            libraryPackage.Element.Stereotype = util.Stereotype.BIELibrary;

            libraryPackage.Element.SetTaggedValue(TaggedValues.baseURN, spec.BaseURN);
            libraryPackage.Element.SetTaggedValues(TaggedValues.businessTerm, spec.BusinessTerms);
            libraryPackage.Element.SetTaggedValues(TaggedValues.copyright, spec.Copyrights);
            libraryPackage.Element.SetTaggedValue(TaggedValues.namespacePrefix, spec.NamespacePrefix);
            libraryPackage.Element.SetTaggedValues(TaggedValues.owner, spec.Owners);
            libraryPackage.Element.SetTaggedValues(TaggedValues.reference, spec.References);
            libraryPackage.Element.SetTaggedValue(TaggedValues.status, spec.Status);
            libraryPackage.Element.SetTaggedValue(TaggedValues.uniqueIdentifier, spec.UniqueIdentifier);
            libraryPackage.Element.SetTaggedValue(TaggedValues.versionIdentifier, spec.VersionIdentifier);
            libraryPackage.Update();
            
            var classDiagram = (Diagram) libraryPackage.Diagrams.AddNew(spec.Name, "Class");
            classDiagram.Update();
            libraryPackage.Diagrams.Refresh();

            package.Packages.Refresh();
            AddPackageToDiagram(libraryPackage);
            return new BIELibrary(repository, libraryPackage);
        }

        public IPRIMLibrary CreatePRIMLibrary(PrimLibrarySpec spec)
        {
            var libraryPackage = (Package) package.Packages.AddNew(spec.Name, string.Empty);
            libraryPackage.Update();
            libraryPackage.ParentID = package.PackageID;
            libraryPackage.Element.Stereotype = util.Stereotype.PRIMLibrary;

            libraryPackage.Element.SetTaggedValue(TaggedValues.baseURN, spec.BaseURN);
            libraryPackage.Element.SetTaggedValues(TaggedValues.businessTerm, spec.BusinessTerms);
            libraryPackage.Element.SetTaggedValues(TaggedValues.copyright, spec.Copyrights);
            libraryPackage.Element.SetTaggedValue(TaggedValues.namespacePrefix, spec.NamespacePrefix);
            libraryPackage.Element.SetTaggedValues(TaggedValues.owner, spec.Owners);
            libraryPackage.Element.SetTaggedValues(TaggedValues.reference, spec.References);
            libraryPackage.Element.SetTaggedValue(TaggedValues.status, spec.Status);
            libraryPackage.Element.SetTaggedValue(TaggedValues.uniqueIdentifier, spec.UniqueIdentifier);
            libraryPackage.Element.SetTaggedValue(TaggedValues.versionIdentifier, spec.VersionIdentifier);
            libraryPackage.Update();
            
            var classDiagram = (Diagram) libraryPackage.Diagrams.AddNew(spec.Name, "Class");
            classDiagram.Update();
            libraryPackage.Diagrams.Refresh();

            package.Packages.Refresh();
            AddPackageToDiagram(libraryPackage);
            return new PRIMLibrary(repository, libraryPackage);
        }

        public IENUMLibrary CreateENUMLibrary(EnumLibrarySpec spec)
        {
            var libraryPackage = (Package) package.Packages.AddNew(spec.Name, string.Empty);
            libraryPackage.Update();
            libraryPackage.ParentID = package.PackageID;
            libraryPackage.Element.Stereotype = util.Stereotype.ENUMLibrary;

            libraryPackage.Element.SetTaggedValue(TaggedValues.baseURN, spec.BaseURN);
            libraryPackage.Element.SetTaggedValues(TaggedValues.businessTerm, spec.BusinessTerms);
            libraryPackage.Element.SetTaggedValues(TaggedValues.copyright, spec.Copyrights);
            libraryPackage.Element.SetTaggedValue(TaggedValues.namespacePrefix, spec.NamespacePrefix);
            libraryPackage.Element.SetTaggedValues(TaggedValues.owner, spec.Owners);
            libraryPackage.Element.SetTaggedValues(TaggedValues.reference, spec.References);
            libraryPackage.Element.SetTaggedValue(TaggedValues.status, spec.Status);
            libraryPackage.Element.SetTaggedValue(TaggedValues.uniqueIdentifier, spec.UniqueIdentifier);
            libraryPackage.Element.SetTaggedValue(TaggedValues.versionIdentifier, spec.VersionIdentifier);
            libraryPackage.Update();
            
            var classDiagram = (Diagram) libraryPackage.Diagrams.AddNew(spec.Name, "Class");
            classDiagram.Update();
            libraryPackage.Diagrams.Refresh();

            package.Packages.Refresh();
            AddPackageToDiagram(libraryPackage);
            return new ENUMLibrary(repository, libraryPackage);
        }

        public IDOCLibrary CreateDOCLibrary(DocLibrarySpec spec)
        {
            var libraryPackage = (Package) package.Packages.AddNew(spec.Name, string.Empty);
            libraryPackage.Update();
            libraryPackage.ParentID = package.PackageID;
            libraryPackage.Element.Stereotype = util.Stereotype.DOCLibrary;

            libraryPackage.Element.SetTaggedValue(TaggedValues.baseURN, spec.BaseURN);
            libraryPackage.Element.SetTaggedValues(TaggedValues.businessTerm, spec.BusinessTerms);
            libraryPackage.Element.SetTaggedValues(TaggedValues.copyright, spec.Copyrights);
            libraryPackage.Element.SetTaggedValue(TaggedValues.namespacePrefix, spec.NamespacePrefix);
            libraryPackage.Element.SetTaggedValues(TaggedValues.owner, spec.Owners);
            libraryPackage.Element.SetTaggedValues(TaggedValues.reference, spec.References);
            libraryPackage.Element.SetTaggedValue(TaggedValues.status, spec.Status);
            libraryPackage.Element.SetTaggedValue(TaggedValues.uniqueIdentifier, spec.UniqueIdentifier);
            libraryPackage.Element.SetTaggedValue(TaggedValues.versionIdentifier, spec.VersionIdentifier);
            libraryPackage.Update();
            
            var classDiagram = (Diagram) libraryPackage.Diagrams.AddNew(spec.Name, "Class");
            classDiagram.Update();
            libraryPackage.Diagrams.Refresh();

            package.Packages.Refresh();
            AddPackageToDiagram(libraryPackage);
            return new DOCLibrary(repository, libraryPackage);
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