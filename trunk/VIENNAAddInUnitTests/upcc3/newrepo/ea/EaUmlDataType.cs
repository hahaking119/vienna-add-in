using EA;
using VIENNAAddInUnitTests.upcc3.newrepo.upcc.uml;

namespace VIENNAAddInUnitTests.upcc3.newrepo.ea
{
    internal class EaUmlDataType : EaUmlClassifier, IUmlDataType
    {
        public EaUmlDataType(Repository eaRepository, Element eaElement) : base(eaRepository, eaElement)
        {
        }

        #region IUmlDataType Members

        public override UmlClassifierType Type
        {
            get { return UmlClassifierType.DataType; }
        }

        #endregion

        protected override IUmlClassifier CreateUmlClassifier(Element eaElement)
        {
            return new EaUmlDataType(eaRepository, eaElement);
        }
    }
}