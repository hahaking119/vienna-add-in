using System.Collections.Generic;
using CctsRepository.PrimLibrary;

namespace VIENNAAddInUnitTests.upcc3.newrepo.ccts
{
    internal interface ICctsRepository
    {
        IEnumerable<IPRIMLibrary> PRIMLibraries { get; }
    }
}