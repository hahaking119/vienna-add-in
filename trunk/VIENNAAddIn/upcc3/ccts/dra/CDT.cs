// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System.Linq;
using EA;
using UPCCRepositoryInterface;
using VIENNAAddIn.upcc3.ccts.util;
using Stereotype=UPCCRepositoryInterface.Stereotype;

namespace VIENNAAddIn.upcc3.ccts.dra
{
    public class CDT : AbstractDT<CDTSpec>, ICDT
    {
        public CDT(CCRepository repository, Element element) : base(repository, element, Stereotype.CDT)
        {
        }

        #region ICDT Members

        public ICDT IsEquivalentTo
        {
            get
            {
                Connector connector = Connectors.FirstOrDefault(EAConnectorExtensions.IsIsEquivalentTo);
                return connector != null ? repository.GetCDT(connector.SupplierID) : null;
            }
        }

        #endregion

        protected override bool DeleteConnectorOnUpdate(Connector connector)
        {
            return false;
        }
    }
}