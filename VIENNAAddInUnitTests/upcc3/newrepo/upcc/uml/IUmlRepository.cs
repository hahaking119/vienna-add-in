using System.Collections.Generic;

namespace VIENNAAddInUnitTests.upcc3.newrepo.upcc.uml
{
    internal interface IUmlRepository
    {
        IEnumerable<IUmlPackage> GetPackagesByStereotype(string stereotype);
    }
}