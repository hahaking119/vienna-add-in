using EA;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    internal class BBIE : UpccAttribute<IABIE>, IBBIE
    {
        public BBIE(CCRepository repository, Attribute attribute, IABIE container)
            : base(repository, attribute, container)
        {
        }

        #region IBBIE Members

        public IBDT Type
        {
            get { return repository.GetBDT(attribute.ClassifierID); }
        }

        #endregion
    }
}