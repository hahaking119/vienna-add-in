using EA;
using VIENNAAddIn.upcc3.uml;

namespace VIENNAAddIn.upcc3.ea
{
    internal class EaUmlEnumeration : EaUmlClassifier, IUmlEnumeration
    {
        public EaUmlEnumeration(Repository eaRepository, Element eaElement) : base(eaRepository, eaElement)
        {
        }
    }
}