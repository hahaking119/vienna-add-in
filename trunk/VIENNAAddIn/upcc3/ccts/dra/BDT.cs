using EA;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    public class BDT : AbstractDT, IBDT
    {
        public BDT(CCRepository repository, Element element) : base(repository, element)
        {
        }

        public ICDT BasedOn
        {
            get { throw new System.NotImplementedException(); }
        }
    }
}