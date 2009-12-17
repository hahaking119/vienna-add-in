using EA;
using VIENNAAddInUnitTests.upcc3.newrepo.upcc.uml;

namespace VIENNAAddInUnitTests.upcc3.newrepo.ea
{
    internal class EaUmlEnumeration : EaUmlClassifier, IUmlEnumeration
    {
        public EaUmlEnumeration(Repository eaRepository, Element eaElement) : base(eaRepository, eaElement)
        {
        }
    }
}