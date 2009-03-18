/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace VIENNAAddIn.CCTS.CCTSBIE_MetaModel {


    public class BusinessInformationEntity : RegistryClass {

        private ArrayList businessTerm;
        private ArrayList context;
        private CoreComponent basis;

        internal CoreComponent Basis {
            get { return basis; }
            set { basis = value; }
        }

        internal ArrayList Context {
            get { return context; }
            set { context = value; }
        }

        public ArrayList BusinessTerm {
            get { return businessTerm; }
            set { businessTerm = value; }
        }

     



    }
}
