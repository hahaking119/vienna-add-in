using System.Collections.Generic;

namespace VIENNAAddInUnitTests.upcc3.newrepo.upcc.uml
{
    public interface IUmlClass : IUmlClassifier
    {
        IEnumerable<IUmlDependency<IUmlClass>> GetDependenciesByStereotype(string stereotype);
        IUmlDependency<IUmlClass> GetFirstDependencyByStereotype(string stereotype);
        IUmlDependency<IUmlClass> CreateDependency(UmlDependencySpec<IUmlClass> spec);
    }
}