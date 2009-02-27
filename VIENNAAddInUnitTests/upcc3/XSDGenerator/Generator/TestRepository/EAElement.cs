using System;
using EA;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal class EAElement: Element, IEACollectionElement
    {
        private readonly UpccClass upccClass;

        public EAElement(UpccClass upccClass)
        {
            this.upccClass = upccClass;
        }

        public string Name
        {
            get { return upccClass.Name; }
        }
        #region Element Members

        public bool Update()
        {
            throw new NotImplementedException();
        }

        public string GetLastError()
        {
            throw new NotImplementedException();
        }

        public void Refresh()
        {
            throw new NotImplementedException();
        }

        public void SetAppearance(int Scope, int Item, int Value)
        {
            throw new NotImplementedException();
        }

        public string GetRelationSet(EnumRelationSetType Type)
        {
            throw new NotImplementedException();
        }

        public string GetStereotypeList()
        {
            throw new NotImplementedException();
        }

        public string GetLinkedDocument()
        {
            throw new NotImplementedException();
        }

        public bool LoadLinkedDocument(string FileName)
        {
            throw new NotImplementedException();
        }

        public bool SaveLinkedDocument(string FileName)
        {
            throw new NotImplementedException();
        }

        public bool ApplyUserLock()
        {
            throw new NotImplementedException();
        }

        public bool ReleaseUserLock()
        {
            throw new NotImplementedException();
        }

        public bool ApplyGroupLock(string aGroupName)
        {
            throw new NotImplementedException();
        }

        string IDualElement.Name
        {
            get { return Name; }
            set { throw new NotImplementedException(); }
        }

        public Collection Requirements
        {
            get { throw new NotImplementedException(); }
        }

        public Collection Constraints
        {
            get { throw new NotImplementedException(); }
        }

        public Collection Scenarios
        {
            get { throw new NotImplementedException(); }
        }

        public Collection Files
        {
            get { throw new NotImplementedException(); }
        }

        public Collection Efforts
        {
            get { throw new NotImplementedException(); }
        }

        public Collection Risks
        {
            get { throw new NotImplementedException(); }
        }

        public Collection Metrics
        {
            get { throw new NotImplementedException(); }
        }

        public Collection Issues
        {
            get { throw new NotImplementedException(); }
        }

        public Collection Tests
        {
            get { throw new NotImplementedException(); }
        }

        public Collection TaggedValues
        {
            get { throw new NotImplementedException(); }
        }

        public Collection Connectors
        {
            get { return EACollection<EAConnector>.Wrap(upccClass.Connectors); }
        }

        public string Notes
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool Locked
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Version
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Multiplicity
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string ElementGUID
        {
            get { throw new NotImplementedException(); }
        }

        public string ExtensionPoints
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Tablespace
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Tag
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        string IDualElement.Stereotype
        {
            get { return upccClass.Stereotype; }
            set { throw new NotImplementedException(); }
        }

        public string Genlinks
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Abstract
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Alias
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Author
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Complexity
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Visibility
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Priority
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Phase
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Persistence
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool IsActive
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool IsLeaf
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool IsNew
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool IsSpec
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int Subtype
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        string IDualElement.Type
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int ClassfierID
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string ClassifierName
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string ClassifierType
        {
            get { throw new NotImplementedException(); }
        }

        public DateTime Created
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public DateTime Modified
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Difficulty
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Genfile
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Gentype
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public object Header1
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public object Header2
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int ElementID
        {
            get { return upccClass.Id; }
        }

        public int PackageID
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public Collection Methods
        {
            get { throw new NotImplementedException(); }
        }

        public Collection Attributes
        {
            get { return EACollection<EAAttribute>.Wrap(upccClass.Attributes); }
        }

        public Collection Resources
        {
            get { throw new NotImplementedException(); }
        }

        public string StyleEx
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string EventFlags
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string ActionFlags
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int ClassifierID
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Status
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int TreePos
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public Collection Elements
        {
            get { throw new NotImplementedException(); }
        }

        public Collection Diagrams
        {
            get { throw new NotImplementedException(); }
        }

        public int ParentID
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public ObjectType ObjectType
        {
            get { throw new NotImplementedException(); }
        }

        public Collection Partitions
        {
            get { throw new NotImplementedException(); }
        }

        public Collection CustomProperties
        {
            get { throw new NotImplementedException(); }
        }

        public Collection StateTransitions
        {
            get { throw new NotImplementedException(); }
        }

        public Collection EmbeddedElements
        {
            get { throw new NotImplementedException(); }
        }

        public Collection BaseClasses
        {
            get { throw new NotImplementedException(); }
        }

        public Collection Realizes
        {
            get { throw new NotImplementedException(); }
        }

        public Collection TaggedValuesEx
        {
            get { throw new NotImplementedException(); }
        }

        public Collection AttributesEx
        {
            get { throw new NotImplementedException(); }
        }

        public Collection MethodsEx
        {
            get { throw new NotImplementedException(); }
        }

        public Collection ConstraintsEx
        {
            get { throw new NotImplementedException(); }
        }

        public Collection RequirementsEx
        {
            get { throw new NotImplementedException(); }
        }

        public string StereotypeEx
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int PropertyType
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public Properties Properties
        {
            get { throw new NotImplementedException(); }
        }

        public string MetaType
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string RunState
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public object CompositeDiagram
        {
            get { throw new NotImplementedException(); }
        }

        public string get_MiscData(int index)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}