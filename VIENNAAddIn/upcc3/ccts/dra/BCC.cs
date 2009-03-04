using EA;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    internal class BCC : UpccAttribute<IACC>, IBCC
    {
        public BCC(CCRepository repository, Attribute attribute, IACC container)
            : base(repository, attribute, container)
        {
        }

        #region IBCC Members

        public ICDT Type
        {
            get { return repository.GetCDT(attribute.ClassifierID); }
        }

        #endregion
    }
}