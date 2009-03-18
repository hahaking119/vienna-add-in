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

  

    public class ASBIE : BusinessInformationEntity {




        private String name;
        public String Name {
            get { return name; }
            set { name = value; }
        }
        private int elementID;
        public int ElementID {
            get { return elementID; }
            set { elementID = value; }
        }


        private String roleName;

        public String RoleName {
            get { return roleName; }
            set { roleName = value; }
        }        


        private AssociationBIEProperty associationBIEProperty;

        internal AssociationBIEProperty AssociationBIEProperty {
            get { return associationBIEProperty; }
            set { associationBIEProperty = value; }
        }

        private ASCC basis;

        internal ASCC Basis {
            get { return basis; }
            set { basis = value; }
        }


    }
}
