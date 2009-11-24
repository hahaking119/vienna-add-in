using System.Collections.Generic;
using CctsRepository.prim;

namespace VIENNAAddInUnitTests.upcc3.newrepo.ccts
{
    internal interface ICctsRepository
    {
        IEnumerable<IPRIMLibrary> PRIMLibraries { get; }
    }
}