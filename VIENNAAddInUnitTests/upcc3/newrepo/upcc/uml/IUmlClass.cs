using System.Collections.Generic;
using VIENNAAddIn.upcc3.ccts.util;

namespace VIENNAAddInUnitTests.upcc3.newrepo.upcc.uml
{
    internal interface IUmlClass
    {
        int Id { get; }
        string GUID { get; }
        string Name { get; }
        IUmlPackage Package { get; }
        IEnumerable<IUmlDependency> GetDependenciesByStereotype(string stereotype);
        IUmlTaggedValue GetTaggedValue(TaggedValues name);
    }
}