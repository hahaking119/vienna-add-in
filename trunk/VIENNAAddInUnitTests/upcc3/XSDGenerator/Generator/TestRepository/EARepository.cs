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
using EA;
using VIENNAAddIn.upcc3.ccts;
using VIENNAAddIn.upcc3.ccts.util;
using Attribute=EA.Attribute;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    /// <summary>
    /// An implementation of the <c>EA.Repository</c> API for unit testing.
    /// 
    /// <para>Even though it is possible to implement unit tests by loading an <c>EA.Repository</c> from a file, 
    /// this process is to slow for efficient unit testing. Therefore, this class offers an efficient 
    /// alternative for testing algorithms operating an <c>EA.Repository</c>.</para>
    /// 
    /// <para>In order to create your own test repository, implement a subclass of <c>EARepository</c> 
    /// and call <see cref="SetContent"/> in the constructor. The content is created using the factory methods defined 
    /// in this class (<see cref="Package"/>, <see cref="Element"/>, <see cref="Attribute"/>, <see cref="Connector"/>, 
    /// <see cref="TaggedValue"/>). See <see cref="EARepository1"/> for an example test repository.</para>
    /// 
    /// <remarks>
    /// Note that this implementation is by no means complete. Instead, it reflects the demands of our unit tests 
    /// up to date and should be extended as required by future tests and in compliance with the <c>EA.Repository</c> API.
    /// </remarks>
    /// </summary>
    public abstract class EARepository : Repository
    {
        private readonly Dictionary<int, EAElement> elementsById = new Dictionary<int, EAElement>();
        private readonly EACollection<EAPackage> models = new EACollection<EAPackage>();
        private readonly Dictionary<int, EAPackage> packagesById = new Dictionary<int, EAPackage>();

        /// <summary>
        /// Counter for unique repository element IDs.
        /// </summary>
        private int nextId = 1;

        #region Repository Members

        public bool OpenFile(string FilePath)
        {
            throw new NotImplementedException();
        }

        public void Exit()
        {
            throw new NotImplementedException();
        }

        public Element GetElementByID(int id)
        {
            return elementsById[id];
        }

        public Package GetPackageByID(int id)
        {
            return packagesById[id];
        }

        public string GetLastError()
        {
            throw new NotImplementedException();
        }

        public Diagram GetDiagramByID(int DiagramID)
        {
            throw new NotImplementedException();
        }

        public Reference GetReferenceList(string ListName)
        {
            throw new NotImplementedException();
        }

        public Project GetProjectInterface()
        {
            throw new NotImplementedException();
        }

        public bool OpenFile2(string FilePath, string Username, string Password)
        {
            throw new NotImplementedException();
        }

        public void ShowWindow(int Show)
        {
            throw new NotImplementedException();
        }

        public Diagram GetCurrentDiagram()
        {
            throw new NotImplementedException();
        }

        public Connector GetConnectorByID(int ConnectorID)
        {
            throw new NotImplementedException();
        }

        public ObjectType GetTreeSelectedItem(out object Item)
        {
            throw new NotImplementedException();
        }

        public Package GetTreeSelectedPackage()
        {
            throw new NotImplementedException();
        }

        public void CloseAddins()
        {
            throw new NotImplementedException();
        }

        public Package GetPackageByGuid(string GUID)
        {
            throw new NotImplementedException();
        }

        public void AdviseElementChange(int ElementID)
        {
            throw new NotImplementedException();
        }

        public void AdviseConnectorChange(int ConnectorID)
        {
            throw new NotImplementedException();
        }

        public void OpenDiagram(int DiagramID)
        {
            throw new NotImplementedException();
        }

        public void CloseDiagram(int DiagramID)
        {
            throw new NotImplementedException();
        }

        public void ActivateDiagram(int DiagramID)
        {
            throw new NotImplementedException();
        }

        public void SaveDiagram(int DiagramID)
        {
            throw new NotImplementedException();
        }

        public void ReloadDiagram(int DiagramID)
        {
            throw new NotImplementedException();
        }

        public void CloseFile()
        {
            throw new NotImplementedException();
        }

        public int __TempDebug(int No, DateTime No2, out int pNo3)
        {
            throw new NotImplementedException();
        }

        public ObjectType GetTreeSelectedItemType()
        {
            throw new NotImplementedException();
        }

        public string GetTechnologyVersion(string ID)
        {
            throw new NotImplementedException();
        }

        public bool IsTechnologyLoaded(string ID)
        {
            throw new NotImplementedException();
        }

        public bool ImportTechnology(string Technology)
        {
            throw new NotImplementedException();
        }

        public string GetCounts()
        {
            throw new NotImplementedException();
        }

        public Collection GetElementSet(string IDList, int Unused)
        {
            throw new NotImplementedException();
        }

        public bool DeleteTechnology(string ID)
        {
            throw new NotImplementedException();
        }

        public object AddTab(string TabName, string ControlID)
        {
            throw new NotImplementedException();
        }

        public void CreateOutputTab(string Name)
        {
            throw new NotImplementedException();
        }

        public void RemoveOutputTab(string Name)
        {
            throw new NotImplementedException();
        }

        public void WriteOutput(string Name, string String, int ID)
        {
            throw new NotImplementedException();
        }

        public void ClearOutput(string Name)
        {
            throw new NotImplementedException();
        }

        public void EnsureOutputVisible(string Name)
        {
            throw new NotImplementedException();
        }

        public Element GetElementByGuid(string GUID)
        {
            throw new NotImplementedException();
        }

        public Connector GetConnectorByGuid(string GUID)
        {
            throw new NotImplementedException();
        }

        public void ShowDynamicHelp(string Topic)
        {
            throw new NotImplementedException();
        }

        public ObjectType GetContextItem(out object Item)
        {
            throw new NotImplementedException();
        }

        public ObjectType GetContextItemType()
        {
            throw new NotImplementedException();
        }

        public void RefreshModelView(int PackageID)
        {
            throw new NotImplementedException();
        }

        public void ActivateTab(string Name)
        {
            throw new NotImplementedException();
        }

        public void ShowInProjectView(object Object)
        {
            throw new NotImplementedException();
        }

        public object GetDiagramByGuid(string GUID)
        {
            throw new NotImplementedException();
        }

        public string GetCurrentLoginUser(bool GetGuid)
        {
            throw new NotImplementedException();
        }

        public bool ChangeLoginUser(string Name, string Password)
        {
            throw new NotImplementedException();
        }

        public object GetTreeSelectedObject()
        {
            throw new NotImplementedException();
        }

        public void Execute(string SQL)
        {
            throw new NotImplementedException();
        }

        public void ShowProfileToolbox(string Technology, string Profile, bool Show)
        {
            throw new NotImplementedException();
        }

        public void SetUIPerspective(string Perspective)
        {
            throw new NotImplementedException();
        }

        public bool ActivatePerspective(string Perspective, int Options)
        {
            throw new NotImplementedException();
        }

        public bool AddPerspective(string Perspective, int Options)
        {
            throw new NotImplementedException();
        }

        public bool DeletePerspective(string Perspective, int Options)
        {
            throw new NotImplementedException();
        }

        public string GetActivePerspective()
        {
            throw new NotImplementedException();
        }

        public string HasPerspective(string Perspective)
        {
            throw new NotImplementedException();
        }

        public bool CreateModel(CreateModelType CreateType, string FilePath, int ParentWnd)
        {
            throw new NotImplementedException();
        }

        public Attribute GetAttributeByGuid(string GUID)
        {
            throw new NotImplementedException();
        }

        public Method GetMethodByGuid(string GUID)
        {
            throw new NotImplementedException();
        }

        public void RemoveTab(string Name)
        {
            throw new NotImplementedException();
        }

        public string CustomCommand(string ClassName, string MethodName, string Parameters)
        {
            throw new NotImplementedException();
        }

        public int IsTabOpen(string TabName)
        {
            throw new NotImplementedException();
        }

        public bool AddDefinedSearches(string sXML)
        {
            throw new NotImplementedException();
        }

        public void ShowBrowser(string TabName, string URL)
        {
            throw new NotImplementedException();
        }

        public Collection GetElementsByQuery(string QueryName, string SearchTerm)
        {
            throw new NotImplementedException();
        }

        public void RunModelSearch(string QueryName, string SearchTerm, string SearchOptions, string SearchData)
        {
            throw new NotImplementedException();
        }

        public string SQLQuery(string SQL)
        {
            throw new NotImplementedException();
        }

        public string GetTreeXML(int RootPackageID)
        {
            throw new NotImplementedException();
        }

        public string GetTreeXMLByGUID(string GUID)
        {
            throw new NotImplementedException();
        }

        public void RefreshOpenDiagrams(bool FullReload)
        {
            throw new NotImplementedException();
        }

        public string GetTreeXMLForElement(int ElementID)
        {
            throw new NotImplementedException();
        }

        public void ImportPackageBuildScripts(string PackageGUID, string BuildScriptXML)
        {
            throw new NotImplementedException();
        }

        public void ExecutePackageBuildScript(int ScriptOptions, string PackageGUID)
        {
            throw new NotImplementedException();
        }

        public bool ActivateToolbox(string Toolbox, int Options)
        {
            throw new NotImplementedException();
        }

        public bool SaveAuditLogs(string FilePath, object StateDateTime, object EndDateTime)
        {
            throw new NotImplementedException();
        }

        public bool ClearAuditLogs(object StateDateTime, object EndDateTime)
        {
            throw new NotImplementedException();
        }

        public ModelWatcher CreateModelWatcher()
        {
            throw new NotImplementedException();
        }

        public bool ActivateTechnology(string ID)
        {
            throw new NotImplementedException();
        }

        public void SaveAllDiagrams()
        {
            throw new NotImplementedException();
        }

        public string GetFormatFromField(string Format, string Text)
        {
            throw new NotImplementedException();
        }

        public string GetFieldFromFormat(string Format, string Text)
        {
            throw new NotImplementedException();
        }

        public bool IsTechnologyEnabled(string ID)
        {
            throw new NotImplementedException();
        }

        public Attribute GetAttributeByID(int AttributeID)
        {
            throw new NotImplementedException();
        }

        public Method GetMethodByID(int MethodID)
        {
            throw new NotImplementedException();
        }

        public object GetContextObject()
        {
            throw new NotImplementedException();
        }

        public Collection Models
        {
            get { return models; }
        }

        public Collection Terms
        {
            get { throw new NotImplementedException(); }
        }

        public Collection Issues
        {
            get { throw new NotImplementedException(); }
        }

        public Collection Authors
        {
            get { throw new NotImplementedException(); }
        }

        public Collection Clients
        {
            get { throw new NotImplementedException(); }
        }

        public Collection Tasks
        {
            get { throw new NotImplementedException(); }
        }

        public Collection Datatypes
        {
            get { throw new NotImplementedException(); }
        }

        public Collection Resources
        {
            get { throw new NotImplementedException(); }
        }

        public Collection Stereotypes
        {
            get { throw new NotImplementedException(); }
        }

        public ObjectType ObjectType
        {
            get { throw new NotImplementedException(); }
        }

        public int LibraryVersion
        {
            get { throw new NotImplementedException(); }
        }

        public string LastUpdate
        {
            get { throw new NotImplementedException(); }
        }

        public bool FlagUpdate
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string InstanceGUID
        {
            get { throw new NotImplementedException(); }
        }

        public string ConnectionString
        {
            get { throw new NotImplementedException(); }
        }

        public Collection PropertyTypes
        {
            get { throw new NotImplementedException(); }
        }

        public bool EnableUIUpdates
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool BatchAppend
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool IsSecurityEnabled
        {
            get { throw new NotImplementedException(); }
        }

        public App App
        {
            get { throw new NotImplementedException(); }
        }

        public EAEditionTypes EAEdition
        {
            get { throw new NotImplementedException(); }
        }

        public bool SuppressEADialogs
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool EnableCache
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int EnableEventFlags
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string ProjectGUID
        {
            get { throw new NotImplementedException(); }
        }

        public bool SuppressSecurityDialog
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        #endregion

        /// <summary>
        /// Set the content of this repository.
        /// </summary>
        /// <param name="models">The models contained in the repository.</param>
        protected void SetContent(params PackageBuilder[] models)
        {
            foreach (PackageBuilder model in models)
            {
                var eaPackage = (EAPackage) this.models.AddNew(model.GetName(), "Package");
                eaPackage.PackageID = nextId++;
                IndexPackage(eaPackage);
                foreach (TaggedValueBuilder tv in model.GetTaggedValues())
                {
                    eaPackage.Element.AddTaggedValue(tv.Name, tv.Value);
                }
                AddPackages(eaPackage.Packages, eaPackage.PackageID, model.GetPackages());
            }
        }

        /// <summary>
        /// Recursively adds the given <paramref name="packages"/> and all their elements to the repository's contents.
        /// </summary>
        /// <param name="packageCollection">An EA.Collection of EA.Package.</param>
        /// <param name="parentId">The ID of the parent package.</param>
        /// <param name="packages">The packages to be added to the collection.</param>
        private void AddPackages(Collection packageCollection, int parentId, IEnumerable<PackageBuilder> packages)
        {
            foreach (PackageBuilder package in packages)
            {
                var eaPackage = (EAPackage) packageCollection.AddNew(package.GetName(), "Package");
                eaPackage.PackageID = nextId++;
                IndexPackage(eaPackage);
                eaPackage.ParentID = parentId;
                eaPackage.Element.Stereotype = package.GetStereotype();
                foreach (TaggedValueBuilder tv in package.GetTaggedValues())
                {
                    eaPackage.Element.AddTaggedValue(tv.Name, tv.Value);
                }
                foreach (ElementBuilder element in package.GetElements())
                {
                    var eaElement = (EAElement) eaPackage.Elements.AddNew(element.GetName(), "Class");
                    eaElement.ElementID = nextId++;
                    eaElement.PackageID = eaPackage.PackageID;
                    IndexElement(eaElement);
                    eaElement.Stereotype = element.GetStereotype();
                    foreach (TaggedValueBuilder tv in element.GetTaggedValues())
                    {
                        eaElement.AddTaggedValue(tv.Name, tv.Value);
                    }
                    Collection attributes = eaElement.Attributes;
                    foreach (AttributeBuilder attribute in element.GetAttributes())
                    {
                        var eaAttribute = (EAAttribute) attributes.AddNew(attribute.GetName(), null);
                        eaAttribute.Repository = this;
                        eaAttribute.ClassifierPath = attribute.GetPathToType();
                        eaAttribute.Stereotype = attribute.GetStereotype();
                        eaAttribute.LowerBound = attribute.GetLowerBound();
                        eaAttribute.UpperBound = attribute.GetUpperBound();
                    }
                    Collection connectors = eaElement.Connectors;
                    foreach (ConnectorBuilder connector in element.GetConnectors())
                    {
                        var eaConnector = (EAConnector) connectors.AddNew(connector.GetName(), "Association");
                        eaConnector.Repository = this;
                        eaConnector.SupplierPath = connector.GetPathToSupplier();
                        eaConnector.Stereotype = connector.GetStereotype();
                    }
                }
                AddPackages(eaPackage.Packages, eaPackage.PackageID, package.GetPackages());
            }
        }

        private void IndexPackage(EAPackage package)
        {
            packagesById[package.PackageID] = package;
        }

        private void IndexElement(EAElement element)
        {
            elementsById[element.ElementID] = element;
        }

        /// <summary>
        /// Creates a new <see cref="ConnectorBuilder"/>.
        /// </summary>
        /// <param name="name"><see cref="ConnectorBuilder(string, string, Path)"/></param>
        /// <param name="stereotype"><see cref="ConnectorBuilder(string, string, Path)"/></param>
        /// <param name="pathToSupplier"><see cref="ConnectorBuilder(string, string, Path)"/></param>
        /// <returns>A new <see cref="ConnectorBuilder"/>.</returns>
        protected static ConnectorBuilder Connector(string name, string stereotype, Path pathToSupplier)
        {
            return new ConnectorBuilder(name, stereotype, pathToSupplier);
        }

        /// <summary>
        /// Creates a new <see cref="AttributeBuilder"/>.
        /// </summary>
        /// <param name="name"><see cref="AttributeBuilder(string, string, Path)"/></param>
        /// <param name="stereotype"><see cref="AttributeBuilder(string, string, Path)"/></param>
        /// <param name="pathToType"><see cref="AttributeBuilder(string, string, Path)"/></param>
        /// <returns>A new <see cref="AttributeBuilder"/>.</returns>
        protected static AttributeBuilder Attribute(string name, string stereotype, Path pathToType)
        {
            return new AttributeBuilder(name, stereotype, pathToType);
        }

        /// <summary>
        /// Creates a new <see cref="TaggedValueBuilder"/>.
        /// </summary>
        /// <param name="key"><see cref="TaggedValueBuilder(TaggedValues, string)"/></param>
        /// <param name="value"><see cref="TaggedValueBuilder(TaggedValues, string)"/></param>
        /// <returns>A new <see cref="TaggedValueBuilder"/>.</returns>
        protected static TaggedValueBuilder TaggedValue(TaggedValues key, string value)
        {
            return new TaggedValueBuilder(key, value);
        }

        /// <summary>
        /// Creates a new <see cref="ElementBuilder"/>.
        /// </summary>
        /// <param name="name"><see cref="ElementBuilder(string, string)"/></param>
        /// <param name="stereotype"><see cref="PackageBuilder(string, string)"/></param>
        /// <returns>A new <see cref="ElementBuilder"/>.</returns>
        protected static ElementBuilder Element(string name, string stereotype)
        {
            return new ElementBuilder(name, stereotype);
        }

        /// <summary>
        /// Creates a new <see cref="PackageBuilder"/>.
        /// </summary>
        /// <param name="name"><see cref="PackageBuilder(string, string)"/></param>
        /// <param name="stereotype"><see cref="PackageBuilder(string, string)"/></param>
        /// <returns>A new <see cref="PackageBuilder"/>.</returns>
        protected static PackageBuilder Package(string name, string stereotype)
        {
            return new PackageBuilder(name, stereotype);
        }
    }

    public static class EaElementExtensions
    {
        public static void AddTaggedValue(this Element element, string name, string value)
        {
            Collection taggedValues = element.TaggedValues;
            var tv = (TaggedValue) taggedValues.AddNew(name, "");
            tv.Value = value;
        }
    }
}