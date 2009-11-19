// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System.Collections.Generic;
using CctsRepository;
using EA;
using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddInUtils;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    ///<summary>
    ///</summary>
    public abstract class BusinessLibrary: IBusinessLibrary
    {
        protected readonly Package package;
        protected readonly CCRepository repository;

        protected BusinessLibrary(CCRepository repository, Package package)
        {
            this.repository = repository;
            this.package = package;
        }

        ///<summary>
        ///</summary>
        public string Stereotype
        {
            get { return package.Element.Stereotype; }
            set { package.Element.Stereotype = value; }
        }

        #region IBusinessLibrary Members

        ///<summary>
        ///</summary>
        public int Id
        {
            get { return package.PackageID; }
        }

        ///<summary>
        ///</summary>
        public string Name
        {
            get { return package.Name; }
        }

        ///<summary>
        ///</summary>
        public IBLibrary Parent
        {
            get { return repository.GetLibrary(package.ParentID) as IBLibrary; }
        }

        public Path Path
        {
            get { return package.GetPath(repository.EARepository); }
        }

        ///<summary>
        ///</summary>
        public string Status
        {
            get { return GetTaggedValue(TaggedValues.status); }
            set { package.SetTaggedValue(TaggedValues.status, value); }
        }

        ///<summary>
        ///</summary>
        public string UniqueIdentifier
        {
            get { return GetTaggedValue(TaggedValues.uniqueIdentifier); }
            set { package.SetTaggedValue(TaggedValues.uniqueIdentifier, value); }
        }

        ///<summary>
        ///</summary>
        public string VersionIdentifier
        {
            get { return GetTaggedValue(TaggedValues.versionIdentifier); }
            set { package.SetTaggedValue(TaggedValues.versionIdentifier, value); }
        }

        ///<summary>
        ///</summary>
        public string BaseURN
        {
            get { return GetTaggedValue(TaggedValues.baseURN); }
            set { package.SetTaggedValue(TaggedValues.baseURN, value); }
        }

        ///<summary>
        ///</summary>
        public string NamespacePrefix
        {
            get { return GetTaggedValue(TaggedValues.namespacePrefix); }
            set { package.SetTaggedValue(TaggedValues.namespacePrefix, value); }
        }

        ///<summary>
        ///</summary>
        public IEnumerable<string> BusinessTerms
        {
            get { return package.GetTaggedValues(TaggedValues.businessTerm); }
            set { package.SetTaggedValues(TaggedValues.businessTerm, value); }
        }

        ///<summary>
        ///</summary>
        public IEnumerable<string> Copyrights
        {
            get { return package.GetTaggedValues(TaggedValues.copyright); }
        }

        ///<summary>
        ///</summary>
        public IEnumerable<string> Owners
        {
            get { return package.GetTaggedValues(TaggedValues.owner); }
        }

        ///<summary>
        ///</summary>
        public IEnumerable<string> References
        {
            get { return package.GetTaggedValues(TaggedValues.reference); }
        }

        #endregion

        private string GetTaggedValue(TaggedValues key)
        {
            return package.GetTaggedValue(key) ?? string.Empty;
        }

        ///<summary>
        ///</summary>
        ///<param name="spec"></param>
        ///<param name="parentPackage"></param>
        ///<param name="stereotype"></param>
        ///<returns></returns>
        public static Package CreateLibraryPackage(LibrarySpec spec, Package parentPackage, string stereotype)
        {
            var libraryPackage = (Package) parentPackage.Packages.AddNew(spec.Name, "");
            libraryPackage.Update();
            libraryPackage.ParentID = parentPackage.PackageID;
            libraryPackage.Element.Stereotype = stereotype;

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

            if (CctsRepository.Stereotype.bLibrary == stereotype)
            {
                var packageDiagram = (Diagram) libraryPackage.Diagrams.AddNew(spec.Name, "Package");
                packageDiagram.Update();
            }
            else
            {
                var classDiagram = (Diagram)libraryPackage.Diagrams.AddNew(spec.Name, "Class");
                classDiagram.Update();
            }
            libraryPackage.Diagrams.Refresh();

            parentPackage.Packages.Refresh();

            return libraryPackage;
        }

        protected void AddElementToDiagram(Element element)
        {
            var diagram = (Diagram)package.Diagrams.GetByName(Name);
            if (diagram != null)
            {
                var newDiagramObject = (DiagramObject) diagram.DiagramObjects.AddNew("", "");
                newDiagramObject.DiagramID = diagram.DiagramID;
                newDiagramObject.ElementID = element.ElementID;
                newDiagramObject.Update();
            }
        }
    }
}