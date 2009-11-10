using System.Collections.Generic;
using UPCCRepositoryInterface;

namespace VIENNAAddInUnitTests.upcc3.newrepo.ccts
{
    internal interface ICctsRepository
    {
        IEnumerable<IPRIMLibrary> PRIMLibraries { get; }
    }
}