using System;
using System.Collections.Generic;
using CctsRepository.BdtLibrary;
using CctsRepository.BieLibrary;
using CctsRepository.BLibrary;
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

        public IDocLibrary UpdateDocLibrary(IDocLibrary docLibrary, DocLibrarySpec specification)
        {
            throw new NotImplementedException();
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

        public IBLibrary GetBLibraryByName(string name)
        {
            throw new NotImplementedException();
        }

        public IBLibrary CreateBLibrary(BLibrarySpec specification)
        {
            throw new NotImplementedException();
        }

        public IBLibrary UpdateBLibrary(IBLibrary bLibrary, BLibrarySpec specification)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IPrimLibrary> GetPrimLibraries()
        {
            throw new NotImplementedException();
        }

        public IPrimLibrary GetPrimLibraryByName(string name)
        {
            throw new NotImplementedException();
        }

        public IPrimLibrary CreatePrimLibrary(PrimLibrarySpec specification)
        {
            throw new NotImplementedException();
        }

        public IPrimLibrary UpdatePrimLibrary(IPrimLibrary primLibrary, PrimLibrarySpec specification)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IEnumLibrary> GetEnumLibraries()
        {
            throw new NotImplementedException();
        }

        public IEnumLibrary GetEnumLibraryByName(string name)
        {
            throw new NotImplementedException();
        }

        public IEnumLibrary CreateEnumLibrary(EnumLibrarySpec specification)
        {
            throw new NotImplementedException();
        }

        public IEnumLibrary UpdateEnumLibrary(IEnumLibrary enumLibrary, EnumLibrarySpec specification)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ICdtLibrary> GetCdtLibraries()
        {
            throw new NotImplementedException();
        }

        public ICdtLibrary GetCdtLibraryByName(string name)
        {
            throw new NotImplementedException();
        }

        public ICdtLibrary CreateCdtLibrary(CdtLibrarySpec specification)
        {
            throw new NotImplementedException();
        }

        public ICdtLibrary UpdateCdtLibrary(ICdtLibrary cdtLibrary, CdtLibrarySpec specification)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ICcLibrary> GetCcLibraries()
        {
            throw new NotImplementedException();
        }

        public ICcLibrary GetCcLibraryByName(string name)
        {
            throw new NotImplementedException();
        }

        public ICcLibrary CreateCcLibrary(CcLibrarySpec specification)
        {
            throw new NotImplementedException();
        }

        public ICcLibrary UpdateCcLibrary(ICcLibrary ccLibrary, CcLibrarySpec specification)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IBdtLibrary> GetBdtLibraries()
        {
            throw new NotImplementedException();
        }

        public IBdtLibrary GetBdtLibraryByName(string name)
        {
            throw new NotImplementedException();
        }

        public IBdtLibrary CreateBdtLibrary(BdtLibrarySpec specification)
        {
            throw new NotImplementedException();
        }

        public IBdtLibrary UpdateBdtLibrary(IBdtLibrary bdtLibrary, BdtLibrarySpec specification)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IBieLibrary> GetBieLibraries()
        {
            throw new NotImplementedException();
        }

        public IBieLibrary GetBieLibraryByName(string name)
        {
            throw new NotImplementedException();
        }

        public IBieLibrary CreateBieLibrary(BieLibrarySpec specification)
        {
            throw new NotImplementedException();
        }

        public IBieLibrary UpdateBieLibrary(IBieLibrary bieLibrary, BieLibrarySpec specification)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IDocLibrary> GetDocLibraries()
        {
            throw new NotImplementedException();
        }

        public IDocLibrary GetDocLibraryByName(string name)
        {
            throw new NotImplementedException();
        }

        public IDocLibrary CreateDocLibrary(DocLibrarySpec specification)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}