using EA;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    internal class ASCC : UpccAssociation<IACC>, IASCC
    {
        public ASCC(CCRepository repository, Connector connector, IACC associatingCC)
            : base(repository, connector, associatingCC)
        {
        }

        #region IASCC Members

        public IACC AssociatedElement
        {
            get { return repository.GetACC(connector.SupplierID); }
        }

        #endregion
    }
}