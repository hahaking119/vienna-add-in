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
using Stereotype=VIENNAAddIn.upcc3.ccts.util.Stereotype;

namespace VIENNAAddInUnitTests.TestRepository
{
    /// <summary>
    /// An implementation of the <c>EA.Repository</c> API for unit testing.
    /// 
    /// <para>Even though it is possible to implement unit tests by loading an <c>EA.Repository</c> from a file, 
    /// this process is to slow for efficient unit testing. Therefore, this class offers an efficient 
    /// alternative for testing algorithms operating an <c>EA.Repository</c>.</para>
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
        protected readonly EACollection models;
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

        public static EAAttributeTag CreateAttributeTag(string name, string type, int attributeId)
        {
            return new EAAttributeTag { Name = name, AttributeID = attributeId };
        }

        public static EADiagramObject CreateDiagramObject(string name, string type, int diagramId)
        {
            return new EADiagramObject{Name = name, DiagramID = diagramId};
        }

        public static IEACollectionElement CreateTaggedValue(string name, string type, int elementId)
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

        protected static Attribute AddCON(Element e, Element type)
        {
            return AddAttribute(e, "Content", type, Stereotype.CON);
        }

        protected static Attribute AddSUP(Element e, Element type, string name)
        {
            return AddAttribute(e, name, type, Stereotype.SUP);
        }

        protected static void AddSUPs(Element e, Element type, params string[] names)
        {
            foreach (var name in names)
            {
                AddAttribute(e, name, type, Stereotype.SUP);
            }
        }

        protected static Attribute AddAttribute(Element e, string name, Element type, string stereotype)
        {
            return AddAttribute(e, name, type, stereotype, a => { });
        }

        protected static Action<Attribute> SetAttributeStereotype(string stereotype)
        {
            return attribute => attribute.Stereotype = stereotype;
        }

        protected static Attribute AddAttribute(Element element, string name, Element type, params Action<Attribute>[] attributeManipulations)
        {
            return element.AddAttribute(name, type).With(attribute =>
            {
                foreach (var manipulate in attributeManipulations)
                {
                    manipulate(attribute);
                }
            });
        }

        protected static Attribute AddAttribute(Element element, string name, Element type, string stereotype, Action<Attribute> initAttribute)
        {
            return element.AddAttribute(name, type).With(attribute =>
                                                         {
                                                             attribute.Stereotype = stereotype.ToString();
                                                             initAttribute(attribute);
                                                         });
        }

        protected static Element AddPRIM(Package primLib1, string name)
        {
            return primLib1.AddElement(name, "Class").With(e => { e.Stereotype = Stereotype.PRIM; });
        }

        protected static Element AddENUM(Package enumLib1, string name, Element type, params string[] values)
        {
            return enumLib1.AddElement(name, "Class").With(ElementStereotype(Stereotype.ENUM),
                                                           e =>
                                                           {
                                                               for (int i = 0; i < values.Length; i += 2)
                                                               {
                                                                   AddENUMValue(e, values[i], values[i + 1], type);
                                                               }
                                                           });
        }

        private static Action<Element> ElementStereotype(string stereotype)
        {
            return e =>
                   {
                       e.Stereotype = stereotype;
                   };
        }

        private static void AddENUMValue(Element e, string name, string value, Element type)
        {
            EAElementExtensions.AddAttribute(e, name, type).With(a => { a.Default = value; });
        }

        protected static void AddASCC(Element client, Element supplier, string name)
        {
            AddASCC(client, supplier, name, "1", "1");
        }

        protected static void AddASCC(Element client, Element supplier, string name, string lowerBound, string upperBound)
        {
            client.AddConnector(name, EAConnectorTypes.Aggregation.ToString(), c =>
                                                                  {
                                                                      c.Stereotype = Stereotype.ASCC;
                                                                      c.ClientEnd.Aggregation = (int) EAAggregationKind.Shared;
                                                                      c.SupplierID = supplier.ElementID;
                                                                      c.SupplierEnd.Role = name;
                                                                      c.SupplierEnd.Cardinality = lowerBound + ".." + upperBound;
                                                                  });
        }

        protected static void AddASBIE(Element client, Element supplier, string name, EAAggregationKind aggregationKind)
        {
            AddASBIE(client, supplier, name, aggregationKind, "1", "1");
        }

        protected static void AddASBIE(Element client, Element supplier, string name, EAAggregationKind aggregationKind, string lowerBound, string upperBound)
        {
            client.AddConnector(name, EAConnectorTypes.Aggregation.ToString(), c =>
                                                                  {
                                                                      c.Stereotype = Stereotype.ASBIE;
                                                                      c.ClientEnd.Aggregation = (int) aggregationKind;
                                                                      c.SupplierID = supplier.ElementID;
                                                                      c.SupplierEnd.Role = name;
                                                                      c.SupplierEnd.Cardinality = lowerBound + ".." + upperBound;
                                                                  });
        }

        protected static void AddBasedOnDependency(Element client, Element supplier)
        {
            client.AddConnector("basedOn", EAConnectorTypes.Dependency.ToString(), c =>
                                                                      {
                                                                          c.Stereotype = Stereotype.BasedOn;
                                                                          c.ClientEnd.Aggregation = (int) EAAggregationKind.None;
                                                                          c.SupplierID = supplier.ElementID;
                                                                          c.SupplierEnd.Role = "basedOn";
                                                                          c.SupplierEnd.Cardinality = "1";
                                                                      });
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