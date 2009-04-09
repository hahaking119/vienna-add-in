/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;
using System.Collections;
using System.Text;

namespace VIENNAAddIn.CCTS.CCTSBIE_MetaModel {




    public class DataType {

        ArrayList  basicBIEProperty;

        internal ArrayList  BasicBIEProperty {
            get { return basicBIEProperty; }
            set { basicBIEProperty = value; }
        }

        ArrayList  basicCCProperty;

        internal ArrayList  BasicCCProperty {
            get { return basicCCProperty; }
            set { basicCCProperty = value; }
        }



    }
}
