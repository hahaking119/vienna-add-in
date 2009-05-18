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
using EARepositoryExtensions=VIENNAAddIn.EARepositoryExtensions;

namespace VIENNAAddInUnitTests.TestRepository
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
    public class EARepository : Repository
    {
        /// <summary>
        /// Counter for unique repository element IDs.
        /// </summary>
        private readonly IDFactory idFactory = new IDFactory();

        private readonly Dictionary<int, EAElement> elementsById = new Dictionary<int, EAElement>();
        private readonly EACollection models;
        private readonly Dictionary<int, EAPackage> packagesById = new Dictionary<int, EAPackage>();
        private readonly Project project = new EAProject();
        private readonly List<EAConnector> connectors = new List<EAConnector>();

        public EARepository()
        {
            models = new EAPackageCollection(this, null);
        }

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
            return project;
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
            // do nothing
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

        internal IEnumerable<EAConnector> Connectors
        {
            get { return connectors; }
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
                eaPackage.ParentID = parentId;
                eaPackage.Element.Stereotype = package.GetStereotype();

                foreach (DiagramBuilder diagram in package.GetDiagrams())
                {
                    eaPackage.Diagrams.AddNew(diagram.GetName(), diagram.GetDiagramType());
                }

                foreach (TaggedValueBuilder tv in package.GetTaggedValues())
                {
                    eaPackage.Element.AddTaggedValue(tv.Name, tv.Value);
                }

                foreach (ElementBuilder element in package.GetElements())
                {
                    var eaElement = (EAElement) eaPackage.Elements.AddNew(element.GetName(), "Class");
                    eaElement.PackageID = eaPackage.PackageID;
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
                        eaAttribute.Default = attribute.GetDefaultValue();
                        foreach (TaggedValueBuilder tv in attribute.GetTaggedValues())
                        {
                            eaAttribute.AddTaggedValue(tv.Name, tv.Value);
                        }
                    }
                }
                AddPackages(eaPackage.Packages, eaPackage.PackageID, package.GetPackages());
            }
        }

        protected void SetConnectors(params ConnectorBuilder[] connectors)
        {
            foreach (ConnectorBuilder connector in connectors)
            {
                var client = EARepositoryExtensions.Resolve<Element>(this, connector.GetPathToClient());
                if (client == null)
                {
                    throw new Exception("Path cannot be resolved: " + connector.GetPathToClient());
                }
                var supplier = EARepositoryExtensions.Resolve<Element>(this, connector.GetPathToSupplier());
                if (supplier == null)
                {
                    throw new Exception("Path cannot be resolved: " + connector.GetPathToSupplier());
                }
                AddConnector(CreateConnector(connector, client.Connectors, client, supplier));
//                CreateConnector(connector, supplier.Connectors, client, supplier);
            }
        }

        private EAConnector CreateConnector(ConnectorBuilder connector, Collection connectors, Element client, Element supplier)
        {
            var eaConnector = (EAConnector) CreateConnector(connector.GetName(), "Association", client.ElementID);
                //(EAConnector)connectors.AddNew(connector.GetName(), "Association");
            eaConnector.Repository = this;
            eaConnector.ClientID = client.ElementID;
            eaConnector.ClientEnd.Aggregation = (int)connector.GetAggregationKind();
            eaConnector.SupplierID = supplier.ElementID;
            eaConnector.SupplierEnd.Role = connector.GetName();
            eaConnector.SupplierEnd.Cardinality = connector.GetLowerBound() + ".." +
                                                  connector.GetUpperBound();
            eaConnector.Stereotype = connector.GetStereotype();
            foreach (TaggedValueBuilder tv in connector.GetTaggedValues())
            {
                eaConnector.AddTaggedValue(tv.Name, tv.Value);
            }
            return eaConnector;
        }

        /// <summary>
        /// Creates a new <see cref="ConnectorBuilder"/>.
        /// </summary>
        /// <param name="name"><see cref="ConnectorBuilder(string,string,VIENNAAddIn.upcc3.ccts.Path)"/></param>
        /// <param name="stereotype"><see cref="ConnectorBuilder(string,string,VIENNAAddIn.upcc3.ccts.Path)"/></param>
        /// <param name="pathToSupplier"><see cref="ConnectorBuilder(string,string,VIENNAAddIn.upcc3.ccts.Path)"/></param>
        /// <returns>A new <see cref="ConnectorBuilder"/>.</returns>
        protected static ConnectorBuilder Connector(string name, string stereotype, Path pathToClient, Path pathToSupplier)
        {
            return new ConnectorBuilder(name, stereotype, pathToClient, pathToSupplier);
        }

        /// <summary>
        /// Creates a new <see cref="AttributeBuilder"/>.
        /// </summary>
        /// <param name="name"><see cref="AttributeBuilder(string,string,VIENNAAddIn.upcc3.ccts.Path)"/></param>
        /// <param name="stereotype"><see cref="AttributeBuilder(string,string,VIENNAAddIn.upcc3.ccts.Path)"/></param>
        /// <param name="pathToType"><see cref="AttributeBuilder(string,string,VIENNAAddIn.upcc3.ccts.Path)"/></param>
        /// <returns>A new <see cref="AttributeBuilder"/>.</returns>
        protected static AttributeBuilder Attribute(string name, string stereotype, Path pathToType)
        {
            return new AttributeBuilder(name, stereotype, pathToType);
        }

        /// <summary>
        /// Creates a new <see cref="TaggedValueBuilder"/>.
        /// </summary>
        /// <param name="key"><see cref="TaggedValueBuilder(VIENNAAddIn.upcc3.ccts.util.TaggedValues,string)"/></param>
        /// <param name="value"><see cref="TaggedValueBuilder(VIENNAAddIn.upcc3.ccts.util.TaggedValues,string)"/></param>
        /// <returns>A new <see cref="TaggedValueBuilder"/>.</returns>
        protected static TaggedValueBuilder TaggedValue(TaggedValues key, string value)
        {
            return new TaggedValueBuilder(key, value);
        }

        /// <summary>
        /// Creates a new <see cref="ElementBuilder"/>.
        /// </summary>
        /// <param name="name"><see cref="ElementBuilder(string,string)"/></param>
        /// <param name="stereotype"><see cref="PackageBuilder(string,string)"/></param>
        /// <returns>A new <see cref="ElementBuilder"/>.</returns>
        protected static ElementBuilder Element(string name, string stereotype)
        {
            return new ElementBuilder(name, stereotype);
        }

        /// <summary>
        /// Creates a new <see cref="PackageBuilder"/>.
        /// </summary>
        /// <param name="name"><see cref="PackageBuilder(string,string)"/></param>
        /// <param name="stereotype"><see cref="PackageBuilder(string,string)"/></param>
        /// <returns>A new <see cref="PackageBuilder"/>.</returns>
        protected static PackageBuilder Package(string name, string stereotype)
        {
            return new PackageBuilder(name, stereotype);
        }

        /// <summary>
        /// Creates a new <see cref="DiagramBuilder"/>.
        /// </summary>
        /// <param name="name"><see cref="DiagramBuilder"/></param>
        /// <param name="diagramType"></param>
        /// <returns>A new <see cref="DiagramBuilder"/>.</returns>
        protected static DiagramBuilder Diagram(string name, string diagramType)
        {
            return new DiagramBuilder(name, diagramType);
        }

        public EAPackage CreatePackage(string name, string type, int parentId)
        {
            int id = idFactory.NextID;
            var package = new EAPackage(this) { Name = name, PackageID = id, ParentID = parentId};
            packagesById[id] = package;
            return package;
        }

        public EAConnectorTag CreateConnectorTag(string name, string type, int connectorId)
        {
            return new EAConnectorTag { Name = name, TagID = idFactory.NextID, ConnectorID = connectorId};
        }

        public EAAttributeTag CreateAttributeTag(string name, string type, int attributeId)
        {
            return new EAAttributeTag { Name = name, AttributeID = attributeId };
        }

        public EADiagramObject CreateDiagramObject(string name, string type, int diagramId)
        {
            return new EADiagramObject{Name = name, DiagramID = diagramId};
        }

        public IEACollectionElement CreateTaggedValue(string name, string type, int elementId)
        {
            return new EATaggedValue {Name = name, ElementID = elementId};
        }

        public IEACollectionElement CreateAttribute(string name, string type, int elementId)
        {
            return new EAAttribute(this, elementId) {Name = name};
        }

        public IEACollectionElement CreateConnector(string name, string type, int clientId)
        {
            return new EAConnector(this) {Name = name, Type = type, ClientID = clientId};
        }

        public IEACollectionElement CreateElement(string name, string type, int packageId)
        {
            int id = idFactory.NextID;
            var element = new EAElement(this) { Name = name, Type = type, ElementID = id, PackageID = packageId};
            elementsById[id] = element;
            return element;
        }

        public IEACollectionElement CreateDiagram(string name, string type, int packageId)
        {
            return new EADiagram(this) {Name = name, Type = type, PackageID = packageId};
        }

        internal void AddConnector(EAConnector connector)
        {
            connectors.Add(connector);
        }

        internal void RemoveConnector(EAConnector connector)
        {
            connectors.Remove(connector);
        }
    }

    internal class EAProject : Project
    {
        public bool LoadProject(string FileName)
        {
            throw new NotImplementedException();
        }

        public bool ReloadProject()
        {
            throw new NotImplementedException();
        }

        public bool LoadDiagram(string DiagramGUID)
        {
            throw new NotImplementedException();
        }

        public string SaveDiagramImageToFile(string FileName)
        {
            throw new NotImplementedException();
        }

        public string GetElement(string ElementGUID)
        {
            throw new NotImplementedException();
        }

        public string EnumViews()
        {
            throw new NotImplementedException();
        }

        public string EnumPackages(string PackageGUID)
        {
            throw new NotImplementedException();
        }

        public string EnumElements(string PackageGUID)
        {
            throw new NotImplementedException();
        }

        public string EnumLinks(string PackageID)
        {
            throw new NotImplementedException();
        }

        public string EnumDiagrams(string PackageGUID)
        {
            throw new NotImplementedException();
        }

        public string EnumDiagramElements(string DiagramGUID)
        {
            throw new NotImplementedException();
        }

        public string EnumDiagramLinks(string DiagramID)
        {
            throw new NotImplementedException();
        }

        public string GetLink(string LinkGUID)
        {
            throw new NotImplementedException();
        }

        public string GetDiagram(string DiagramGUID)
        {
            throw new NotImplementedException();
        }

        public string GetElementConstraints(string ElementGUID)
        {
            throw new NotImplementedException();
        }

        public string GetElementEffort(string ElementGUID)
        {
            throw new NotImplementedException();
        }

        public string GetElementMetrics(string ElementGUID)
        {
            throw new NotImplementedException();
        }

        public string GetElementFiles(string ElementGUID)
        {
            throw new NotImplementedException();
        }

        public string GetElementRequirements(string ElementGUID)
        {
            throw new NotImplementedException();
        }

        public string GetElementProblems(string ElementGUID)
        {
            throw new NotImplementedException();
        }

        public string GetElementResources(string ElementGUID)
        {
            throw new NotImplementedException();
        }

        public string GetElementRisks(string ElementGUID)
        {
            throw new NotImplementedException();
        }

        public string GetElementScenarios(string ElementGUID)
        {
            throw new NotImplementedException();
        }

        public string GetElementTests(string ElementGUID)
        {
            throw new NotImplementedException();
        }

        public void ShowWindow(int Show)
        {
            throw new NotImplementedException();
        }

        public void Exit()
        {
            throw new NotImplementedException();
        }

        public bool PutDiagramImageOnClipboard(string DiagramGUID, int Type)
        {
            throw new NotImplementedException();
        }

        public bool PutDiagramImageToFile(string DiagramGUID, string FilePath, int Type)
        {
            throw new NotImplementedException();
        }

        public string ExportPackageXMI(string PackageGUID, EnumXMIType XMIType, int DiagramXML, int DiagramImage, int FormatXML, int UseDTD, string FileName)
        {
            throw new NotImplementedException();
        }

        public string EnumProjects()
        {
            throw new NotImplementedException();
        }

        public string EnumViewEx(string ProjectGUID)
        {
            throw new NotImplementedException();
        }

        public void RunReport(string PackageGUID, string TemplateName, string FileName)
        {
            throw new NotImplementedException();
        }

        public string GetLastError()
        {
            throw new NotImplementedException();
        }

        public string GetElementProperties(string ElementGUID)
        {
            throw new NotImplementedException();
        }

        public string GUIDtoXML(string GUID)
        {
            throw new NotImplementedException();
        }

        public string XMLtoGUID(string GUID)
        {
            throw new NotImplementedException();
        }

        public void RunHTMLReport(string PackageGUID, string ExportPath, string ImageFormat, string Style, string Extension)
        {
            throw new NotImplementedException();
        }

        public string ImportPackageXMI(string PackageGUID, string FileName, int ImportDiagrams, int StripGUID)
        {
            throw new NotImplementedException();
        }

        public string SaveControlledPackage(string PackageGUID)
        {
            throw new NotImplementedException();
        }

        public string LoadControlledPackage(string PackageGUID)
        {
            throw new NotImplementedException();
        }

        public bool LayoutDiagram(string DiagramGUID, int LayoutStyle)
        {
            throw new NotImplementedException();
        }

        public bool GenerateXSD(string PackageGUID, string FileName, string Encoding, string Options)
        {
            throw new NotImplementedException();
        }

        public bool LayoutDiagramEx(string DiagramGUID, int LayoutStyle, int Iterations, int LayerSpacing, int ColumnSpacing, bool SavetoDiagram)
        {
            throw new NotImplementedException();
        }

        public string DefineRule(string CategoryID, EnumMVErrorType Severity, string ErrorMsg)
        {
            throw new NotImplementedException();
        }

        public string DefineRuleCategory(string Description)
        {
            throw new NotImplementedException();
        }

        public bool PublishResult(string RuleID, EnumMVErrorType Severity, string ErrorMsg)
        {
            throw new NotImplementedException();
        }

        public bool ValidateElement(string ElementGUID)
        {
            throw new NotImplementedException();
        }

        public bool ValidatePackage(string PackageGUID)
        {
            throw new NotImplementedException();
        }

        public bool ValidateDiagram(string DiagramGUID)
        {
            throw new NotImplementedException();
        }

        public bool IsValidating()
        {
            throw new NotImplementedException();
        }

        public bool CanValidate()
        {
            throw new NotImplementedException();
        }

        public void CancelValidation()
        {
            throw new NotImplementedException();
        }

        public void RunModelSearch(string QueryName, string SearchTerm, bool ShowInEA)
        {
            throw new NotImplementedException();
        }

        public bool GenerateClass(string ElementGUID, string ExtraOptions)
        {
            throw new NotImplementedException();
        }

        public bool GeneratePackage(string PackageGUID, string ExtraOptions)
        {
            throw new NotImplementedException();
        }

        public bool TransformElement(string transformName, string ElementGUID, string TargetPackageGUID, string ExtraOptions)
        {
            throw new NotImplementedException();
        }

        public bool TransformPackage(string transformName, string SourcePackageGUID, string TargetPackageGUID, string ExtraOptions)
        {
            throw new NotImplementedException();
        }

        public bool SynchronizeClass(string ElementGUID, string ExtraOptions)
        {
            throw new NotImplementedException();
        }

        public bool SynchronizePackage(string PackageGUID, string ExtraOptions)
        {
            throw new NotImplementedException();
        }

        public bool ImportDirectory(string PackageGUID, string Language, string DirectoryPath, string ExtraOptions)
        {
            throw new NotImplementedException();
        }

        public bool ImportFile(string PackageGUID, string Language, string FileName, string ExtraOptions)
        {
            throw new NotImplementedException();
        }

        public string DoBaselineCompare(string PackageGUID, string Baseline, string ConnectString)
        {
            throw new NotImplementedException();
        }

        public string DoBaselineMerge(string PackageGUID, string Baseline, string MergeInstructions, string ConnectString)
        {
            throw new NotImplementedException();
        }

        public string GetBaselines(string PackageGUID, string ConnectString)
        {
            throw new NotImplementedException();
        }

        public bool CreateBaseline(string PackageGUID, string Version, string Notes)
        {
            throw new NotImplementedException();
        }

        public ObjectType ObjectType
        {
            get { throw new NotImplementedException(); }
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

    public static class EaAttributeExtensions
    {
        public static void AddTaggedValue(this Attribute attribute, string name, string value)
        {
            var tv = (AttributeTag) attribute.TaggedValues.AddNew(name, "");
            tv.Value = value;
        }
    }

    public static class EaConnectorExtensions
    {
        public static void AddTaggedValue(this Connector connector, string name, string value)
        {
            var tv = (TaggedValue) connector.TaggedValues.AddNew(name, "");
            tv.Value = value;
        }
    }

    public class IDFactory
    {
        private int nextID = 1;

        public int NextID
        {
            get
            {
                return nextID++;
            }
        }
    }
}