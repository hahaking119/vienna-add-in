using System;
using System.Collections.Generic;
using CctsRepository;
using CctsRepository.BdtLibrary;
using CctsRepository.BieLibrary;
using CctsRepository.BLibrary;
using CctsRepository.CcLibrary;
using CctsRepository.CdtLibrary;
using CctsRepository.DocLibrary;
using CctsRepository.EnumLibrary;
using CctsRepository.PrimLibrary;
using VIENNAAddIn.upcc3.ccts.util;
using VIENNAAddInUnitTests.upcc3.newrepo.upcc.uml;
using VIENNAAddInUtils;

namespace VIENNAAddInUnitTests.upcc3.newrepo.upcc
{
    internal class UpccRepository : ICctsRepository
    {
        private readonly IUmlRepository umlRepository;

        public UpccRepository(IUmlRepository umlRepository)
        {
            this.umlRepository = umlRepository;
        }

        #region ICctsRepository Members

        public IEnumerable<IBLibrary> GetBLibraries()
        {
            foreach (IUmlPackage umlPackage in umlRepository.GetPackagesByStereotype(Stereotype.bLibrary))
            {
                yield return new UpccBLibrary(umlPackage);
            }
        }

        public IEnumerable<IPrimLibrary> GetPrimLibraries()
        {
            foreach (IUmlPackage primPackage in umlRepository.GetPackagesByStereotype(Stereotype.PRIMLibrary))
            {
                yield return new UpccPrimLibrary(primPackage);
            }
        }

        public IEnumerable<IEnumLibrary> GetEnumLibraries()
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
            foreach (IUmlPackage umlPackage in umlRepository.GetPackagesByStereotype(Stereotype.BDTLibrary))
            {
                yield return new UpccBdtLibrary(umlPackage);
            }
        }

        public IEnumerable<IBieLibrary> GetBieLibraries()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IDocLibrary> GetDocLibraries()
        {
            throw new NotImplementedException();
        }

        public IBLibrary GetBLibraryById(int id)
        {
            throw new NotImplementedException();
        }

        public IPrimLibrary GetPrimLibraryById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumLibrary GetEnumLibraryById(int id)
        {
            throw new NotImplementedException();
        }

        public ICdtLibrary GetCdtLibraryById(int id)
        {
            throw new NotImplementedException();
        }

        public ICcLibrary GetCcLibraryById(int id)
        {
            throw new NotImplementedException();
        }

        public IBdtLibrary GetBdtLibraryById(int id)
        {
            throw new NotImplementedException();
        }

        public IBieLibrary GetBieLibraryById(int id)
        {
            throw new NotImplementedException();
        }

        public IDocLibrary GetDocLibraryById(int id)
        {
            throw new NotImplementedException();
        }

        public IIdScheme GetIdSchemeByPath(Path path)
        {
            throw new NotImplementedException();
        }

        public IPrim GetPrimById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnum GetEnumById(int id)
        {
            throw new NotImplementedException();
        }

        public ICdt GetCdtById(int id)
        {
            throw new NotImplementedException();
        }

        public IAcc GetAccById(int id)
        {
            throw new NotImplementedException();
        }

        public IBdt GetBdtById(int id)
        {
            throw new NotImplementedException();
        }

        public IAbie GetAbieById(int id)
        {
            throw new NotImplementedException();
        }

        public IBLibrary GetBLibraryByPath(Path path)
        {
            throw new NotImplementedException();
        }

        public IPrimLibrary GetPrimLibraryByPath(Path path)
        {
            throw new NotImplementedException();
        }

        public IIdScheme GetIdSchemeById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumLibrary GetEnumLibraryByPath(Path path)
        {
            throw new NotImplementedException();
        }

        public ICdtLibrary GetCdtLibraryByPath(Path path)
        {
            throw new NotImplementedException();
        }

        public ICcLibrary GetCcLibraryByPath(Path path)
        {
            throw new NotImplementedException();
        }

        public IBdtLibrary GetBdtLibraryByPath(Path path)
        {
            throw new NotImplementedException();
        }

        public IBieLibrary GetBieLibraryByPath(Path path)
        {
            throw new NotImplementedException();
        }

        public IDocLibrary GetDocLibraryByPath(Path path)
        {
            throw new NotImplementedException();
        }

        public IPrim GetPrimByPath(Path path)
        {
            throw new NotImplementedException();
        }

        public IEnum GetEnumByPath(Path path)
        {
            throw new NotImplementedException();
        }

        public ICdt GetCdtByPath(Path path)
        {
            throw new NotImplementedException();
        }

        public IMa GetMaById(int id)
        {
            throw new NotImplementedException();
        }

        public IMa GetMaByPath(Path path)
        {
            throw new NotImplementedException();
        }

        public IAcc GetAccByPath(Path path)
        {
            throw new NotImplementedException();
        }

        public IBdt GetBdtByPath(Path path)
        {
            throw new NotImplementedException();
        }

        public IAbie GetAbieByPath(Path path)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Path> GetRootLocations()
        {
            throw new NotImplementedException();
        }

        public IBLibrary CreateRootBLibrary(Path rootLocation, BLibrarySpec spec)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}