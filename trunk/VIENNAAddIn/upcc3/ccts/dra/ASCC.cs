// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using EA;
using UPCCRepositoryInterface;
using VIENNAAddIn.upcc3.ccts.util;

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
            get { return repository.GetACC(connector.GetAssociatedElementId(AssociatingElement.Id)); }
        }

        #endregion
    }
}