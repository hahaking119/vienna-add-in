using System;
using System.Collections.Generic;
using EA;
using System.Linq;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    internal abstract class TestLibrary : TestRepositoryElement, Package
    {
        private readonly List<TestEAElement> elements = new List<TestEAElement>();

        private readonly List<TestLibrary> libraries = new List<TestLibrary>();

        private readonly Element element;

        protected TestLibrary()
        {
            element = new TestEAPackageElement(this);
        }

        protected TestRepositoryElement AddLibrary(TestLibrary library)
        {
            libraries.Add(library);
            return library;
        }

        protected TestRepositoryElement AddElement(TestEAElement element)
        {
            elements.Add(element);
            return element;
        }

        protected TestLibrary LibraryByName(string name)
        {
            return libraries.First(l => l.Name == name);
        }

        protected TestEAElement ElementByName(string name)
        {
            return elements.First(e => e.Name == name);
        }

        public override string BaseURN
        {
            set { AddTaggedValue("baseURN", value); }
        }

        #region Package Members

        public Collection Packages
        {
            get { return new TestEACollection<TestLibrary>(libraries); }
        }

        public Collection Elements
        {
            get { return new TestEACollection<TestEAElement>(elements); }
        }

        public Collection Diagrams
        {
            get { throw new NotImplementedException(); }
        }

        public string Notes
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool IsNamespace
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int PackageID
        {
            get { throw new NotImplementedException(); }
        }

        public string PackageGUID
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int ParentID
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
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

        public bool IsControlled
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool IsProtected
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool UseDTD
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool LogXML
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string XMLPath
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Version
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public DateTime LastLoadDate
        {
            get { throw new NotImplementedException(); }
        }

        public DateTime LastSaveDate
        {
            get { throw new NotImplementedException(); }
        }

        public string Flags
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Owner
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string CodePath
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string UMLVersion
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int TreePos
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool IsModel
        {
            get { throw new NotImplementedException(); }
        }

        public Element Element
        {
            get { return element; }
        }

        public int BatchLoad
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int BatchSave
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public Collection Connectors
        {
            get { throw new NotImplementedException(); }
        }

        public string Alias
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool IsVersionControlled
        {
            get { throw new NotImplementedException(); }
        }

        public ObjectType ObjectType
        {
            get { throw new NotImplementedException(); }
        }

        public string StereotypeEx
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string GetLastError()
        {
            throw new NotImplementedException();
        }

        public bool Update()
        {
            throw new NotImplementedException();
        }

        public CodeObject GetCodeObject(string CodeID)
        {
            throw new NotImplementedException();
        }

        public void SetCodeProject(string GUID, string ProjectType)
        {
            throw new NotImplementedException();
        }

        public CodeObject GetClassCodeObjects(string CodeIDs)
        {
            throw new NotImplementedException();
        }

        public void GenerateSourceCode()
        {
            throw new NotImplementedException();
        }

        public void GetCodeProject(out string GUID, out string ProjectType)
        {
            throw new NotImplementedException();
        }

        public CodeObject ShallowGetClassCodeObjects(string CodeIDs)
        {
            throw new NotImplementedException();
        }

        public object FindObject(string DottedID)
        {
            throw new NotImplementedException();
        }

        public void VersionControlAdd(string ConfigGuid, string XMLFile, string Comment, bool KeepCheckedOut)
        {
            throw new NotImplementedException();
        }

        public void VersionControlRemove()
        {
            throw new NotImplementedException();
        }

        public void VersionControlCheckout(string Comment)
        {
            throw new NotImplementedException();
        }

        public void VersionControlCheckin(string Comment)
        {
            throw new NotImplementedException();
        }

        public int VersionControlGetStatus()
        {
            throw new NotImplementedException();
        }

        public Package Clone()
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

        #endregion

        public Element ResolvePath(List<string> parts)
        {
            if (parts.Count == 0)
            {
                throw new Exception("cannot resolve path");
            }
            if (parts.Count == 1)
            {
                return ElementByName(parts[0]);
            }
            return LibraryByName(parts[0]).ResolvePath(parts.GetRange(1, parts.Count - 1));
        }
    }

    internal class TestEAPackageElement : TestEAElement
    {
        private readonly TestLibrary library;

        public TestEAPackageElement(TestLibrary library)
        {
            this.library = library;
        }

        public override string Stereotype
        {
            get { return library.Stereotype; }
        }

        public override Collection TaggedValues
        {
            get { return library.TaggedValues; }
        }
    }
}