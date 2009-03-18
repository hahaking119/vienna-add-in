// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
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