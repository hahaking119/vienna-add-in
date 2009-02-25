using System;
using System.Collections.Generic;
using EA;
using Attribute=EA.Attribute;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal abstract class TestEARepository : Repository
    {
        private readonly List<TestModel> models = new List<TestModel>();
        private readonly Stack<TestRepositoryElement> currentElementStack =
            new Stack<TestRepositoryElement>();

        protected TestEARepository()
        {
            models.Add(new TestModel());
            currentElementStack.Push(models[0]);
        }

        protected void ContentType(Path pathToPRIM)
        {
            OfType(pathToPRIM);
        }

        protected void OfType(Path pathToPRIM)
        {
            CurrentElement.Type = pathToPRIM;
        }

        protected void BaseURN(string baseURN)
        {
            CurrentElement.BaseURN = baseURN;
        }

        protected void UpperBound(string upperBound)
        {
            CurrentElement.UpperBound = upperBound;
        }

        protected void LowerBound(string lowerBound)
        {
            CurrentElement.LowerBound = lowerBound;
        }

        protected void PRIM(Action continueConstruction)
        {
            ProcessChild(CurrentElement.AddPRIM(), continueConstruction);
        }

        protected void PRIMLibrary(Action continueConstruction)
        {
            ProcessChild(CurrentElement.AddPRIMLibrary(), continueConstruction);
        }

        protected void CDT(Action continueConstruction)
        {
            ProcessChild(CurrentElement.AddCDT(), continueConstruction);
        }

        protected void SUP(Action continueConstruction)
        {
            ProcessChild(CurrentElement.AddSUP(), continueConstruction);
        }

        protected void CDTLibrary(Action continueConstruction)
        {
            ProcessChild(CurrentElement.AddCDTLibrary(), continueConstruction);
        }

        protected void Name(string name)
        {
            CurrentElement.Name = name;
        }

        protected void BLibrary(Action continueConstruction)
        {
            ProcessChild(CurrentElement.AddBLibrary(), continueConstruction);
        }

        private void ProcessChild(TestRepositoryElement child, Action continueConstruction)
        {
            currentElementStack.Push(child);
            continueConstruction();
            currentElementStack.Pop();
        }

//        private void Add(BLibrary library)
//        {
//            throw new NotImplementedException();
//        }

        public bool OpenFile(string FilePath)
        {
            throw new System.NotImplementedException();
        }

        public void Exit()
        {
            throw new System.NotImplementedException();
        }

        public Element GetElementByID(int ElementID)
        {
            throw new System.NotImplementedException();
        }

        public Package GetPackageByID(int PackageID)
        {
            throw new System.NotImplementedException();
        }

        public string GetLastError()
        {
            throw new System.NotImplementedException();
        }

        public Diagram GetDiagramByID(int DiagramID)
        {
            throw new System.NotImplementedException();
        }

        public Reference GetReferenceList(string ListName)
        {
            throw new System.NotImplementedException();
        }

        public Project GetProjectInterface()
        {
            throw new System.NotImplementedException();
        }

        public bool OpenFile2(string FilePath, string Username, string Password)
        {
            throw new System.NotImplementedException();
        }

        public void ShowWindow(int Show)
        {
            throw new System.NotImplementedException();
        }

        public Diagram GetCurrentDiagram()
        {
            throw new System.NotImplementedException();
        }

        public Connector GetConnectorByID(int ConnectorID)
        {
            throw new System.NotImplementedException();
        }

        public ObjectType GetTreeSelectedItem(out object Item)
        {
            throw new System.NotImplementedException();
        }

        public Package GetTreeSelectedPackage()
        {
            throw new System.NotImplementedException();
        }

        public void CloseAddins()
        {
            throw new System.NotImplementedException();
        }

        public Package GetPackageByGuid(string GUID)
        {
            throw new System.NotImplementedException();
        }

        public void AdviseElementChange(int ElementID)
        {
            throw new System.NotImplementedException();
        }

        public void AdviseConnectorChange(int ConnectorID)
        {
            throw new System.NotImplementedException();
        }

        public void OpenDiagram(int DiagramID)
        {
            throw new System.NotImplementedException();
        }

        public void CloseDiagram(int DiagramID)
        {
            throw new System.NotImplementedException();
        }

        public void ActivateDiagram(int DiagramID)
        {
            throw new System.NotImplementedException();
        }

        public void SaveDiagram(int DiagramID)
        {
            throw new System.NotImplementedException();
        }

        public void ReloadDiagram(int DiagramID)
        {
            throw new System.NotImplementedException();
        }

        public void CloseFile()
        {
            throw new System.NotImplementedException();
        }

        public int __TempDebug(int No, DateTime No2, out int pNo3)
        {
            throw new System.NotImplementedException();
        }

        public ObjectType GetTreeSelectedItemType()
        {
            throw new System.NotImplementedException();
        }

        public string GetTechnologyVersion(string ID)
        {
            throw new System.NotImplementedException();
        }

        public bool IsTechnologyLoaded(string ID)
        {
            throw new System.NotImplementedException();
        }

        public bool ImportTechnology(string Technology)
        {
            throw new System.NotImplementedException();
        }

        public string GetCounts()
        {
            throw new System.NotImplementedException();
        }

        public Collection GetElementSet(string IDList, int Unused)
        {
            throw new System.NotImplementedException();
        }

        public bool DeleteTechnology(string ID)
        {
            throw new System.NotImplementedException();
        }

        public object AddTab(string TabName, string ControlID)
        {
            throw new System.NotImplementedException();
        }

        public void CreateOutputTab(string Name)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveOutputTab(string Name)
        {
            throw new System.NotImplementedException();
        }

        public void WriteOutput(string Name, string String, int ID)
        {
            throw new System.NotImplementedException();
        }

        public void ClearOutput(string Name)
        {
            throw new System.NotImplementedException();
        }

        public void EnsureOutputVisible(string Name)
        {
            throw new System.NotImplementedException();
        }

        public Element GetElementByGuid(string GUID)
        {
            throw new System.NotImplementedException();
        }

        public Connector GetConnectorByGuid(string GUID)
        {
            throw new System.NotImplementedException();
        }

        public void ShowDynamicHelp(string Topic)
        {
            throw new System.NotImplementedException();
        }

        public ObjectType GetContextItem(out object Item)
        {
            throw new System.NotImplementedException();
        }

        public ObjectType GetContextItemType()
        {
            throw new System.NotImplementedException();
        }

        public void RefreshModelView(int PackageID)
        {
            throw new System.NotImplementedException();
        }

        public void ActivateTab(string Name)
        {
            throw new System.NotImplementedException();
        }

        public void ShowInProjectView(object Object)
        {
            throw new System.NotImplementedException();
        }

        public object GetDiagramByGuid(string GUID)
        {
            throw new System.NotImplementedException();
        }

        public string GetCurrentLoginUser(bool GetGuid)
        {
            throw new System.NotImplementedException();
        }

        public bool ChangeLoginUser(string Name, string Password)
        {
            throw new System.NotImplementedException();
        }

        public object GetTreeSelectedObject()
        {
            throw new System.NotImplementedException();
        }

        public void Execute(string SQL)
        {
            throw new System.NotImplementedException();
        }

        public void ShowProfileToolbox(string Technology, string Profile, bool Show)
        {
            throw new System.NotImplementedException();
        }

        public void SetUIPerspective(string Perspective)
        {
            throw new System.NotImplementedException();
        }

        public bool ActivatePerspective(string Perspective, int Options)
        {
            throw new System.NotImplementedException();
        }

        public bool AddPerspective(string Perspective, int Options)
        {
            throw new System.NotImplementedException();
        }

        public bool DeletePerspective(string Perspective, int Options)
        {
            throw new System.NotImplementedException();
        }

        public string GetActivePerspective()
        {
            throw new System.NotImplementedException();
        }

        public string HasPerspective(string Perspective)
        {
            throw new System.NotImplementedException();
        }

        public bool CreateModel(CreateModelType CreateType, string FilePath, int ParentWnd)
        {
            throw new System.NotImplementedException();
        }

        public Attribute GetAttributeByGuid(string GUID)
        {
            throw new System.NotImplementedException();
        }

        public Method GetMethodByGuid(string GUID)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveTab(string Name)
        {
            throw new System.NotImplementedException();
        }

        public string CustomCommand(string ClassName, string MethodName, string Parameters)
        {
            throw new System.NotImplementedException();
        }

        public int IsTabOpen(string TabName)
        {
            throw new System.NotImplementedException();
        }

        public bool AddDefinedSearches(string sXML)
        {
            throw new System.NotImplementedException();
        }

        public void ShowBrowser(string TabName, string URL)
        {
            throw new System.NotImplementedException();
        }

        public Collection GetElementsByQuery(string QueryName, string SearchTerm)
        {
            throw new System.NotImplementedException();
        }

        public void RunModelSearch(string QueryName, string SearchTerm, string SearchOptions, string SearchData)
        {
            throw new System.NotImplementedException();
        }

        public string SQLQuery(string SQL)
        {
            throw new System.NotImplementedException();
        }

        public string GetTreeXML(int RootPackageID)
        {
            throw new System.NotImplementedException();
        }

        public string GetTreeXMLByGUID(string GUID)
        {
            throw new System.NotImplementedException();
        }

        public void RefreshOpenDiagrams(bool FullReload)
        {
            throw new System.NotImplementedException();
        }

        public string GetTreeXMLForElement(int ElementID)
        {
            throw new System.NotImplementedException();
        }

        public void ImportPackageBuildScripts(string PackageGUID, string BuildScriptXML)
        {
            throw new System.NotImplementedException();
        }

        public void ExecutePackageBuildScript(int ScriptOptions, string PackageGUID)
        {
            throw new System.NotImplementedException();
        }

        public bool ActivateToolbox(string Toolbox, int Options)
        {
            throw new System.NotImplementedException();
        }

        public bool SaveAuditLogs(string FilePath, object StateDateTime, object EndDateTime)
        {
            throw new System.NotImplementedException();
        }

        public bool ClearAuditLogs(object StateDateTime, object EndDateTime)
        {
            throw new System.NotImplementedException();
        }

        public ModelWatcher CreateModelWatcher()
        {
            throw new System.NotImplementedException();
        }

        public bool ActivateTechnology(string ID)
        {
            throw new System.NotImplementedException();
        }

        public void SaveAllDiagrams()
        {
            throw new System.NotImplementedException();
        }

        public string GetFormatFromField(string Format, string Text)
        {
            throw new System.NotImplementedException();
        }

        public string GetFieldFromFormat(string Format, string Text)
        {
            throw new System.NotImplementedException();
        }

        public bool IsTechnologyEnabled(string ID)
        {
            throw new System.NotImplementedException();
        }

        public Attribute GetAttributeByID(int AttributeID)
        {
            throw new System.NotImplementedException();
        }

        public Method GetMethodByID(int MethodID)
        {
            throw new System.NotImplementedException();
        }

        public object GetContextObject()
        {
            throw new System.NotImplementedException();
        }

        public Collection Models
        {
            get
            {
                return new TestEACollection<TestModel>(models);
            }
        }

        public Collection Terms
        {
            get { throw new System.NotImplementedException(); }
        }

        public Collection Issues
        {
            get { throw new System.NotImplementedException(); }
        }

        public Collection Authors
        {
            get { throw new System.NotImplementedException(); }
        }

        public Collection Clients
        {
            get { throw new System.NotImplementedException(); }
        }

        public Collection Tasks
        {
            get { throw new System.NotImplementedException(); }
        }

        public Collection Datatypes
        {
            get { throw new System.NotImplementedException(); }
        }

        public Collection Resources
        {
            get { throw new System.NotImplementedException(); }
        }

        public Collection Stereotypes
        {
            get { throw new System.NotImplementedException(); }
        }

        public ObjectType ObjectType
        {
            get { throw new System.NotImplementedException(); }
        }

        public int LibraryVersion
        {
            get { throw new System.NotImplementedException(); }
        }

        public string LastUpdate
        {
            get { throw new System.NotImplementedException(); }
        }

        public bool FlagUpdate
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public string InstanceGUID
        {
            get { throw new System.NotImplementedException(); }
        }

        public string ConnectionString
        {
            get { throw new System.NotImplementedException(); }
        }

        public Collection PropertyTypes
        {
            get { throw new System.NotImplementedException(); }
        }

        public bool EnableUIUpdates
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public bool BatchAppend
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public bool IsSecurityEnabled
        {
            get { throw new System.NotImplementedException(); }
        }

        public App App
        {
            get { throw new System.NotImplementedException(); }
        }

        public EAEditionTypes EAEdition
        {
            get { throw new System.NotImplementedException(); }
        }

        public bool SuppressEADialogs
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public bool EnableCache
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public int EnableEventFlags
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public string ProjectGUID
        {
            get { throw new System.NotImplementedException(); }
        }

        public bool SuppressSecurityDialog
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        private TestRepositoryElement CurrentElement
        {
            get { return currentElementStack.Peek(); }
        }

        public Element ResolvePath(List<string> parts)
        {
            return models[0].ResolvePath(parts);
        }

    }
}