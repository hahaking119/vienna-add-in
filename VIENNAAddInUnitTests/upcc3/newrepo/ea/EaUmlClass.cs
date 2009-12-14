using EA;
using VIENNAAddInUnitTests.upcc3.newrepo.upcc.uml;

namespace VIENNAAddInUnitTests.upcc3.newrepo.ea
{
    internal class EaUmlClass : EaUmlClassifier, IUmlClass
    {
        public EaUmlClass(Repository repository, Element element)
            : base(repository, element)
        {
        }

        #region IUmlClass Members

        public override UmlClassifierType Type
        {
            get { return UmlClassifierType.Class; }
        }

        #endregion

        protected override IUmlClassifier CreateUmlClassifier(Element eaElement)
        {
            return new EaUmlClass(eaRepository, eaElement);
        }
    }
}