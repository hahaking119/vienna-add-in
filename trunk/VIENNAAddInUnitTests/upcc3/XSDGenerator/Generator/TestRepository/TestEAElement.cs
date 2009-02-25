using System;
using System.Collections.Generic;
using EA;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal abstract class TestEAElement : TestRepositoryElement, Element
    {
        private readonly List<TestEAAttribute> attributes = new List<TestEAAttribute>();

        protected TestEAAttribute AddAttribute(TestEAAttribute attribute)
        {
            attributes.Add(attribute);
            return attribute;
        }

        public bool Update()
        {
            throw new System.NotImplementedException();
        }

        public string GetLastError()
        {
            throw new System.NotImplementedException();
        }

        public void Refresh()
        {
            throw new System.NotImplementedException();
        }

        public void SetAppearance(int Scope, int Item, int Value)
        {
            throw new System.NotImplementedException();
        }

        public string GetRelationSet(EnumRelationSetType Type)
        {
            throw new System.NotImplementedException();
        }

        public string GetStereotypeList()
        {
            throw new System.NotImplementedException();
        }

        public string GetLinkedDocument()
        {
            throw new System.NotImplementedException();
        }

        public bool LoadLinkedDocument(string FileName)
        {
            throw new System.NotImplementedException();
        }

        public bool SaveLinkedDocument(string FileName)
        {
            throw new System.NotImplementedException();
        }

        public bool ApplyUserLock()
        {
            throw new System.NotImplementedException();
        }

        public bool ReleaseUserLock()
        {
            throw new System.NotImplementedException();
        }

        public bool ApplyGroupLock(string aGroupName)
        {
            throw new System.NotImplementedException();
        }

        public Collection Requirements
        {
            get { throw new System.NotImplementedException(); }
        }

        public Collection Constraints
        {
            get { throw new System.NotImplementedException(); }
        }

        public Collection Scenarios
        {
            get { throw new System.NotImplementedException(); }
        }

        public Collection Files
        {
            get { throw new System.NotImplementedException(); }
        }

        public Collection Efforts
        {
            get { throw new System.NotImplementedException(); }
        }

        public Collection Risks
        {
            get { throw new System.NotImplementedException(); }
        }

        public Collection Metrics
        {
            get { throw new System.NotImplementedException(); }
        }

        public Collection Issues
        {
            get { throw new System.NotImplementedException(); }
        }

        public Collection Tests
        {
            get { throw new System.NotImplementedException(); }
        }

        public Collection Connectors
        {
            get { throw new System.NotImplementedException(); }
        }

        public string Notes
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public bool Locked
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public string Version
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public string Multiplicity
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public string ElementGUID
        {
            get { throw new System.NotImplementedException(); }
        }

        public string ExtensionPoints
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public string Tablespace
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public string Tag
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        string IDualElement.Stereotype
        {
            get { return Stereotype; }
            set { throw new System.NotImplementedException(); }
        }

        public string Genlinks
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public string Abstract
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public string Alias
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public string Author
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public string Complexity
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public string Visibility
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public string Priority
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public string Phase
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public string Persistence
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public bool IsActive
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public bool IsLeaf
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public bool IsNew
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public bool IsSpec
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public int Subtype
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        string IDualElement.Type
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public int ClassfierID
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public string ClassifierName
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public string ClassifierType
        {
            get { throw new System.NotImplementedException(); }
        }

        public DateTime Created
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public DateTime Modified
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public string Difficulty
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public string Genfile
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public string Gentype
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public object Header1
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public object Header2
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public int ElementID
        {
            get { return Id; }
        }

        public int PackageID
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public Collection Methods
        {
            get { throw new System.NotImplementedException(); }
        }

        public Collection Attributes
        {
            get { return new TestEACollection<TestEAAttribute>(attributes); }
        }

        public Collection Resources
        {
            get { throw new System.NotImplementedException(); }
        }

        public string StyleEx
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public string EventFlags
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public string ActionFlags
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public int ClassifierID
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public string Status
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public int TreePos
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public Collection Elements
        {
            get { throw new System.NotImplementedException(); }
        }

        public Collection Diagrams
        {
            get { throw new System.NotImplementedException(); }
        }

        public int ParentID
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public ObjectType ObjectType
        {
            get { throw new System.NotImplementedException(); }
        }

        public Collection Partitions
        {
            get { throw new System.NotImplementedException(); }
        }

        public Collection CustomProperties
        {
            get { throw new System.NotImplementedException(); }
        }

        public Collection StateTransitions
        {
            get { throw new System.NotImplementedException(); }
        }

        public Collection EmbeddedElements
        {
            get { throw new System.NotImplementedException(); }
        }

        public Collection BaseClasses
        {
            get { throw new System.NotImplementedException(); }
        }

        public Collection Realizes
        {
            get { throw new System.NotImplementedException(); }
        }

        public Collection TaggedValuesEx
        {
            get { throw new System.NotImplementedException(); }
        }

        public Collection AttributesEx
        {
            get { throw new System.NotImplementedException(); }
        }

        public Collection MethodsEx
        {
            get { throw new System.NotImplementedException(); }
        }

        public Collection ConstraintsEx
        {
            get { throw new System.NotImplementedException(); }
        }

        public Collection RequirementsEx
        {
            get { throw new System.NotImplementedException(); }
        }

        public string StereotypeEx
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public int PropertyType
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public Properties Properties
        {
            get { throw new System.NotImplementedException(); }
        }

        public string MetaType
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public string RunState
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public object CompositeDiagram
        {
            get { throw new System.NotImplementedException(); }
        }

        public string get_MiscData(int index)
        {
            throw new System.NotImplementedException();
        }
    }
}