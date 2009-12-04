// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System.Collections.Generic;
using CctsRepository.BLibrary;
using EA;
using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddInUtils;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    ///<summary>
    ///</summary>
    public abstract class BusinessLibrary
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
        public IBLibrary BLibrary
        {
            get { return repository.GetBLibraryById(package.ParentID); }
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
        }

        ///<summary>
        ///</summary>
        public string UniqueIdentifier
        {
            get { return GetTaggedValue(TaggedValues.uniqueIdentifier); }
        }

        ///<summary>
        ///</summary>
        public string VersionIdentifier
        {
            get { return GetTaggedValue(TaggedValues.versionIdentifier); }
        }

        ///<summary>
        ///</summary>
        public string BaseURN
        {
            get { return GetTaggedValue(TaggedValues.baseURN); }
        }

        ///<summary>
        ///</summary>
        public string NamespacePrefix
        {
            get { return GetTaggedValue(TaggedValues.namespacePrefix); }
        }

        ///<summary>
        ///</summary>
        public IEnumerable<string> BusinessTerms
        {
            get { return package.GetTaggedValues(TaggedValues.businessTerm); }
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

        private string GetTaggedValue(TaggedValues key)
        {
            return package.GetTaggedValue(key) ?? string.Empty;
        }

        protected void AddElementToDiagram(Element element)
        {
            var diagram = (Diagram) package.Diagrams.GetByName(Name);
            if (diagram != null)
            {
                var newDiagramObject = (DiagramObject) diagram.DiagramObjects.AddNew(string.Empty, string.Empty);
                newDiagramObject.DiagramID = diagram.DiagramID;
                newDiagramObject.ElementID = element.ElementID;
                newDiagramObject.Update();
            }
        }
    }
}