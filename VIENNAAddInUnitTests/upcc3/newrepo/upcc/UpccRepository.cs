using System.Collections.Generic;
using CctsRepository.PrimLibrary;
using VIENNAAddIn.upcc3.ccts.util;
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

        public IEnumerable<IPrimLibrary> PRIMLibraries
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