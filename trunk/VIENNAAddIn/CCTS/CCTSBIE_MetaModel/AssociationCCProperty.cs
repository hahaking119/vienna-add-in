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







    public class AssociationCCProperty : CCProperty {

        private AggregateCoreComponent aggregateCoreComponent;

        internal AggregateCoreComponent AggregateCoreComponent {
            get { return aggregateCoreComponent; }
            set { aggregateCoreComponent = value; }
        }

        private ASCC ascc;

        internal ASCC Ascc {
            get { return ascc; }
            set { ascc = value; }
        }




    }
}
