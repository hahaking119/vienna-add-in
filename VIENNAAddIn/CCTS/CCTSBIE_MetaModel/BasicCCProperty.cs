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



    public class BasicCCProperty : CCProperty {


        private DataType datatype;

        internal DataType Datatype {
            get { return datatype; }
            set { datatype = value; }
        }

        private BCC bcc;

        internal BCC Bcc {
            get { return bcc; }
            set { bcc = value; }
        }



    }
}
