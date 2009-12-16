using System.Collections.Generic;

namespace VIENNAAddInUnitTests.upcc3.newrepo.upcc.uml
{
    public interface IUmlDataType : IUmlClassifier
    {
        IEnumerable<IUmlDependency<IUmlDataType>> GetDependenciesByStereotype(string stereotype);
        IUmlDependency<IUmlDataType> GetFirstDependencyByStereotype(string stereotype);
        IUmlDependency<IUmlDataType> CreateDependency(UmlDependencySpec<IUmlDataType> spec);
    }
}