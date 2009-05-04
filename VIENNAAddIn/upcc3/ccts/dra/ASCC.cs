// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System;
using EA;
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
            get { return repository.GetACC(connector.SupplierID); }
        }

        public AggregationKind AggregationKind
        {
            get
            {
                var value = connector.ClientEnd.Aggregation;
                if (Enum.IsDefined(typeof(AggregationKind), value))
                {
                    return (AggregationKind)Enum.ToObject(typeof(AggregationKind), value);
                }
                return AggregationKind.None;
            }
        }

        #endregion
    }
}