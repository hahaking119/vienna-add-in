using System;
using EA;

namespace VIENNAAddInUnitTests.upcc3.XSDGenerator.Generator.TestRepository
{
    public class EAPackage : Package, IEACollectionElement
    {
        private readonly Collection packages = new EACollection<EAPackage>();
        private readonly Collection elements = new EACollection<EAElement>();
        private readonly Element element = new EAElement();

        #region IEACollectionElement Members

        string IEACollectionElement.Name
        {
            get { return Name; }
            set { Name = value; }
        }

        #endregion

        #region Package Members

        public string Name { get; set; }

        public Collection Packages
        {
            get { return packages; }
        }

        public Collection Elements
        {
            get { return elements; }
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

        public int PackageID { get; set; }

        public string PackageGUID
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int ParentID { get; set; }

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
            return true;
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
    }
}