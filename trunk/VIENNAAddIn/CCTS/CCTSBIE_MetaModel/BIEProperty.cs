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




    public class BIEProperty {

        private String qualifierTerm;

        public String QualifierTerm {
            get { return qualifierTerm; }
            set { qualifierTerm = value; }
        }

        private CCProperty basis;

        internal CCProperty Basis {
            get { return basis; }
            set { basis = value; }
        }


    }
}
