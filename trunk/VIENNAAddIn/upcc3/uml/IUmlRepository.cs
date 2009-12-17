using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.uml
{
    public interface IUmlRepository
    {
        IEnumerable<IUmlPackage> GetPackagesByStereotype(string stereotype);
    }
}