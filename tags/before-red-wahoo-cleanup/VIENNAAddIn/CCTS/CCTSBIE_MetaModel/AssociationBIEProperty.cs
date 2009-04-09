/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace VIENNAAddIn.CCTS.CCTSBIE_MetaModel {





    public class AssociationBIEProperty : BIEProperty {

        private AggregateBusinessInformationEntity aggregateBusinessInformationEntity;

        internal AggregateBusinessInformationEntity AggregateBusinessInformationEntity {
            get { return aggregateBusinessInformationEntity; }
            set { aggregateBusinessInformationEntity = value; }
        }

        private ASBIE asbie;

        internal ASBIE Asbie {
            get { return asbie; }
            set { asbie = value; }
        }



    }
}
