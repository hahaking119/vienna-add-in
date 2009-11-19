using System.Collections.Generic;
using CctsRepository;

namespace VIENNAAddInUnitTests.upcc3.newrepo.upcc.uml
{
    internal interface IUmlPackage
    {
        int Id { get; }
        string Name { get; }
        IEnumerable<IUmlClass> Classes { get; }
        IUmlPackage Parent { get; }
        IUmlTaggedValue GetTaggedValue(TaggedValues name);
    }
}