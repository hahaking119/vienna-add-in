using System.Collections.Generic;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddInUnitTests.upcc3.newrepo.upcc.uml
{
    public interface IUmlClassifier
    {
        int Id { get; }
        string GUID { get; }
        string Name { get; }
        UmlClassifierType Type { get; }
        IUmlPackage Package { get; }
        IEnumerable<IUmlDependency<IUmlClassifier>> GetDependenciesByStereotype(string stereotype);
        IUmlTaggedValue GetTaggedValue(TaggedValues name);
        IUmlDependency<TTarget> CreateDependency<TTarget>(UmlDependencySpec<TTarget> spec);
    }

    public enum UmlClassifierType
    {
        Class,
        DataType,
    }
}