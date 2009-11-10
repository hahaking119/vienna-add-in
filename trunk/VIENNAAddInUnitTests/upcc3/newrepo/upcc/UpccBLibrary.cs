using System;
using System.Collections.Generic;
using UPCCRepositoryInterface;
using VIENNAAddInUnitTests.upcc3.newrepo.upcc.uml;

namespace VIENNAAddInUnitTests.upcc3.newrepo.upcc
{
    internal class UpccBLibrary : IBLibrary
    {
        private readonly IUmlPackage umlPackage;

        public UpccBLibrary(IUmlPackage umlPackage)
        {
            this.umlPackage = umlPackage;
        }

        #region IBLibrary Members

        public int Id
        {
            get { return umlPackage.Id; }
        }

        public string Name
        {
            get { return umlPackage.Name; }
        }

        public IBLibrary Parent
        {
            get { throw new NotImplementedException(); }
        }

        public string Status
        {
            get { throw new NotImplementedException(); }
        }

        public string UniqueIdentifier
        {
            get { throw new NotImplementedException(); }
        }

        public string VersionIdentifier
        {
            get { throw new NotImplementedException(); }
        }

        public string BaseURN
        {
            get { throw new NotImplementedException(); }
        }

        public string NamespacePrefix
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<string> BusinessTerms
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<string> Copyrights
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<string> Owners
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<string> References
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<IBusinessLibrary> Children
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<IBusinessLibrary> AllChildren
        {
            get { throw new NotImplementedException(); }
        }

        public IBusinessLibrary FindChildByName(string name)
        {
            throw new NotImplementedException();
        }

        public IBLibrary CreateBLibrary(LibrarySpec spec)
        {
            throw new NotImplementedException();
        }

        public ICDTLibrary CreateCDTLibrary(LibrarySpec spec)
        {
            throw new NotImplementedException();
        }

        public ICCLibrary CreateCCLibrary(LibrarySpec spec)
        {
            throw new NotImplementedException();
        }

        public IBDTLibrary CreateBDTLibrary(LibrarySpec spec)
        {
            throw new NotImplementedException();
        }

        public IBIELibrary CreateBIELibrary(LibrarySpec spec)
        {
            throw new NotImplementedException();
        }

        public IPRIMLibrary CreatePRIMLibrary(LibrarySpec spec)
        {
            throw new NotImplementedException();
        }

        public IENUMLibrary CreateENUMLibrary(LibrarySpec spec)
        {
            throw new NotImplementedException();
        }

        public IDOCLibrary CreateDOCLibrary(LibrarySpec spec)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}