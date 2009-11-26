using System;
using System.Collections.Generic;
using CctsRepository.BdtLibrary;
using CctsRepository.BieLibrary;
using CctsRepository.bLibrary;
using CctsRepository.CcLibrary;
using CctsRepository.CdtLibrary;
using CctsRepository.DocLibrary;
using CctsRepository.EnumLibrary;
using CctsRepository.PrimLibrary;
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

        public IEnumerable<IBLibrary> GetBLibraries()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IPRIMLibrary> GetPrimLibraries()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IENUMLibrary> GetEnumLibraries()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ICDTLibrary> GetCdtLibraries()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ICCLibrary> GetCcLibraries()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IBdtLibrary> GetBdtLibraries()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IBieLibrary> GetBieLibraries()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IDOCLibrary> GetDocLibraries()
        {
            throw new NotImplementedException();
        }

        public IBLibrary CreateBLibrary(BLibrarySpec spec)
        {
            throw new NotImplementedException();
        }

        public ICDTLibrary CreateCDTLibrary(CdtLibrarySpec spec)
        {
            throw new NotImplementedException();
        }

        public ICCLibrary CreateCCLibrary(CcLibrarySpec spec)
        {
            throw new NotImplementedException();
        }

        public IBdtLibrary CreateBDTLibrary(BdtLibrarySpec spec)
        {
            throw new NotImplementedException();
        }

        public IBieLibrary CreateBIELibrary(BieLibrarySpec spec)
        {
            throw new NotImplementedException();
        }

        public IPRIMLibrary CreatePRIMLibrary(PrimLibrarySpec spec)
        {
            throw new NotImplementedException();
        }

        public IENUMLibrary CreateENUMLibrary(EnumLibrarySpec spec)
        {
            throw new NotImplementedException();
        }

        public IDOCLibrary CreateDOCLibrary(DocLibrarySpec spec)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}