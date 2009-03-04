using EA;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    internal class ASBIE : UpccAssociation<IABIE>, IASBIE
    {
        public ASBIE(CCRepository repository, Connector connector, IABIE associatingBIE)
            : base(repository, connector, associatingBIE)
        {
        }

        #region IASBIE Members

        public IABIE AssociatedElement
        {
            get { return repository.GetABIE(connector.SupplierID); }
        }

        #endregion
    }
}