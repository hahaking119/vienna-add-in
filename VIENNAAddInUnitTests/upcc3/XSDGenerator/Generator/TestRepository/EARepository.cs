using System;
using System.Collections.Generic;
using EA;
using Attribute=EA.Attribute;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    public abstract class EARepository : Repository
    {
        private readonly Dictionary<int, EAElement> elementsById = new Dictionary<int, EAElement>();
        private readonly Collection models = new EACollection<EAPackage>();
        private readonly Dictionary<int, EAPackage> packagesById = new Dictionary<int, EAPackage>();
        private int nextId;

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

        protected void SetContent(List<Model> content)
        {
            foreach (Model model in content)
            {
                var modelPackage = (EAPackage) models.AddNew(model.Name, "Package");
                modelPackage.PackageID = nextId++;
                IndexPackage(modelPackage);
                foreach (UpccTaggedValue tv in model.GetTaggedValues())
                {
                    modelPackage.Element.AddTaggedValue(tv.Name, tv.Value);
                }
                AddPackages(model, modelPackage.Packages);
            }
        }

        private void IndexPackage(EAPackage package)
        {
            packagesById[package.PackageID] = package;
        }

        private void AddPackages(UpccLibrary library, Collection packages)
        {
            foreach (UpccLibrary child in library.GetLibraries())
            {
                var childPackage = (EAPackage) packages.AddNew(child.Name, "Package");
                childPackage.PackageID = nextId++;
                IndexPackage(childPackage);
                childPackage.Element.Stereotype = child.GetStereotype();
                foreach (UpccTaggedValue tv in child.GetTaggedValues())
                {
                    childPackage.Element.AddTaggedValue(tv.Name, tv.Value);
                }
                if (child is BLibrary)
                {
                    AddPackages(child, childPackage.Packages);
                }
                else
                {
                    AddClasses(child, childPackage.PackageID, childPackage.Elements);
                }
            }
        }

        private void AddClasses(UpccLibrary library, int packageId, Collection elements)
        {
//            Console.WriteLine("Adding classes for library " + library.Name);
            foreach (UpccClass c in library.GetClasses())
            {
//                Console.WriteLine("  Adding class " + c.Name);
                var element = (EAElement) elements.AddNew(c.Name, "Class");
                element.ElementID = nextId++;
                element.PackageID = packageId;
                IndexElement(element);
                element.Stereotype = c.GetStereotype();
                Collection attributes = element.Attributes;
                foreach (var attribute in c.GetAttributes())
                {
                    var a = (EAAttribute) attributes.AddNew(attribute.Name, null);
                    a.Repository = this;
                    a.ClassifierPath = attribute.Type;
                    a.Stereotype = attribute.GetStereotype();
                    a.LowerBound = attribute.LowerBound;
                    a.UpperBound = attribute.UpperBound;
                }
                Collection connectors = element.Connectors;
                foreach (var connector in c.GetConnectors())
                {
                    var con = (EAConnector) connectors.AddNew(connector.Name, "Association");
                    con.Repository = this;
                    con.SupplierPath = connector.PathToSupplier;
                    con.Stereotype = connector.GetStereotype();
                }
            }
        }

        private void IndexElement(EAElement element)
        {
            elementsById[element.ElementID] = element;
        }
    }

    public static class EaElementExtensions
    {
        public static TaggedValue AddTaggedValue(this Element element, string name, string value)
        {
            Collection taggedValues = element.TaggedValues;
            var tv = (TaggedValue) taggedValues.AddNew(name, "");
            tv.Value = value;
            tv.Update();
            taggedValues.Refresh();
            return tv;
        }
    }
}