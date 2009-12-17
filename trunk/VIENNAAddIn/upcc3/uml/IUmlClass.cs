using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.uml
{
    public interface IUmlClass : IUmlClassifier
    {
        IEnumerable<IUmlDependency<IUmlClass>> GetDependenciesByStereotype(string stereotype);
        IUmlDependency<IUmlClass> GetFirstDependencyByStereotype(string stereotype);
        IUmlDependency<IUmlClass> CreateDependency(UmlDependencySpec<IUmlClass> spec);
    }
}