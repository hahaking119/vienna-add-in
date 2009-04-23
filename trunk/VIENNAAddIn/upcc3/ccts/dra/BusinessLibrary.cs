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
    ///<summary>
    ///</summary>
    public abstract class BusinessLibrary : IBusinessLibrary
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
        public IBusinessLibrary Parent
        {
            get { return repository.GetLibrary(package.ParentID); }
        }

        ///<summary>
        ///</summary>
        public string Status
        {
            get { return GetTaggedValue(TaggedValues.Status); }
            set { package.SetTaggedValue(TaggedValues.Status, value); }
        }

        ///<summary>
        ///</summary>
        public string UniqueIdentifier
        {
            get { return GetTaggedValue(TaggedValues.UniqueIdentifier); }
            set { package.SetTaggedValue(TaggedValues.UniqueIdentifier, value); }
        }

        ///<summary>
        ///</summary>
        public string VersionIdentifier
        {
            get { return GetTaggedValue(TaggedValues.VersionIdentifier); }
            set { package.SetTaggedValue(TaggedValues.VersionIdentifier, value); }
        }

        ///<summary>
        ///</summary>
        public string BaseURN
        {
            get { return GetTaggedValue(TaggedValues.BaseURN); }
            set { package.SetTaggedValue(TaggedValues.BaseURN, value); }
        }

        ///<summary>
        ///</summary>
        public string NamespacePrefix
        {
            get { return GetTaggedValue(TaggedValues.NamespacePrefix); }
            set { package.SetTaggedValue(TaggedValues.NamespacePrefix, value); }
        }

        ///<summary>
        ///</summary>
        public IEnumerable<string> BusinessTerms
        {
            get { return package.GetTaggedValues(TaggedValues.BusinessTerm); }
            set { package.SetTaggedValues(TaggedValues.BusinessTerm, value); }
        }

        ///<summary>
        ///</summary>
        public IEnumerable<string> Copyrights
        {
            get { return package.GetTaggedValues(TaggedValues.Copyright); }
        }

        ///<summary>
        ///</summary>
        public IEnumerable<string> Owners
        {
            get { return package.GetTaggedValues(TaggedValues.Owner); }
        }

        ///<summary>
        ///</summary>
        public IEnumerable<string> References
        {
            get { return package.GetTaggedValues(TaggedValues.Reference); }
        }

        #endregion

        private string GetTaggedValue(TaggedValues key)
        {
            return package.GetTaggedValue(key) ?? string.Empty;
        }

        public static Package CreateLibraryPackage(LibrarySpec spec, Package parentPackage, string stereotype)
        {
            var libraryPackage = (Package) parentPackage.Packages.AddNew(spec.Name, "");
            libraryPackage.Update();
            libraryPackage.ParentID = parentPackage.PackageID;
            libraryPackage.Element.Stereotype = stereotype;

            libraryPackage.Element.SetTaggedValue(TaggedValues.BaseURN, spec.BaseURN);
            libraryPackage.Element.SetTaggedValues(TaggedValues.BusinessTerm, spec.BusinessTerms);
            libraryPackage.Element.SetTaggedValues(TaggedValues.Copyright, spec.Copyrights);
            libraryPackage.Element.SetTaggedValue(TaggedValues.NamespacePrefix, spec.NamespacePrefix);
            libraryPackage.Element.SetTaggedValues(TaggedValues.Owner, spec.Owners);
            libraryPackage.Element.SetTaggedValues(TaggedValues.Reference, spec.References);
            libraryPackage.Element.SetTaggedValue(TaggedValues.Status, spec.Status);
            libraryPackage.Element.SetTaggedValue(TaggedValues.UniqueIdentifier, spec.UniqueIdentifier);
            libraryPackage.Element.SetTaggedValue(TaggedValues.VersionIdentifier, spec.VersionIdentifier);
            libraryPackage.Update();

            if (util.Stereotype.BLibrary == stereotype)
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
                newDiagramObject.ElementID = element.ElementID;
                newDiagramObject.Update();
            }
        }
    }
}