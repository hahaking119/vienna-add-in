using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.uml
{
    public interface IUmlDataType : IUmlClassifier
    {
        IEnumerable<IUmlDependency<IUmlDataType>> GetDependenciesByStereotype(string stereotype);
        IUmlDependency<IUmlDataType> GetFirstDependencyByStereotype(string stereotype);
        IUmlDependency<IUmlDataType> CreateDependency(UmlDependencySpec<IUmlDataType> spec);
    }
}