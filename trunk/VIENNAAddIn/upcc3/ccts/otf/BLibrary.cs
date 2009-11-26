using System;
using System.Collections.Generic;
using CctsRepository;
using CctsRepository.BdtLibrary;
using CctsRepository.BieLibrary;
using CctsRepository.bLibrary;
using CctsRepository.CcLibrary;
using CctsRepository.CdtLibrary;
using CctsRepository.DocLibrary;
using CctsRepository.EnumLibrary;
using CctsRepository.PrimLibrary;

namespace VIENNAAddIn.upcc3.ccts.otf
{
    public class BLibrary : BusinessLibrary, IBLibrary
    {
        public BLibrary(ItemId id, string name, ItemId parentId, string status, string uniqueIdentifier, string versionIdentifier, string baseUrn, string namespacePrefix, IEnumerable<string> businessTerms, IEnumerable<string> copyrights, IEnumerable<string> owners, IEnumerable<string> references) : base(id, name, parentId, status, uniqueIdentifier, versionIdentifier, baseUrn, namespacePrefix, businessTerms, copyrights, owners, references)
        {
        }

        int IBLibrary.Id
        {
            get { return Id.Value; }
        }

        #region IBLibrary Members

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

        public IEnumerable<ICdtLibrary> GetCdtLibraries()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ICcLibrary> GetCcLibraries()
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

        public ICdtLibrary CreateCDTLibrary(CdtLibrarySpec spec)
        {
            throw new NotImplementedException();
        }

        public ICcLibrary CreateCCLibrary(CcLibrarySpec spec)
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