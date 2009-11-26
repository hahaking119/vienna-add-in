using System.Collections.Generic;

namespace VIENNAAddInUnitTests.upcc3.newrepo.upcc.uml
{
    public interface IUmlRepository
    {
        IEnumerable<IUmlPackage> GetPackagesByStereotype(string stereotype);
    }
}