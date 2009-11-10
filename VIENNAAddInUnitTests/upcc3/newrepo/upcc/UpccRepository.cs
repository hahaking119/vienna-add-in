using System.Collections.Generic;
using UPCCRepositoryInterface;
using VIENNAAddInUnitTests.upcc3.newrepo.ccts;
using VIENNAAddInUnitTests.upcc3.newrepo.upcc.uml;

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

        public IEnumerable<IPRIMLibrary> PRIMLibraries
        {
            get
            {
                foreach (IUmlPackage primPackage in umlRepository.GetPackagesByStereotype(Stereotype.PRIMLibrary))
                {
                    yield return new UpccPrimLibrary(primPackage);
                }
            }
        }

        #endregion
    }
}