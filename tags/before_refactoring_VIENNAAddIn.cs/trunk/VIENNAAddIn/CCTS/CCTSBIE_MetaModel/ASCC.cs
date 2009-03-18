/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace VIENNAAddIn.CCTS.CCTSBIE_MetaModel {


    public class ASCC : CoreComponent {


        private LinkedList<CoreComponent> corecomponents;

        private String name;
        public String Name
        {
            get { return name; }
            set { name = value; }
        }
        private int elementID;
        public int ElementID
        {
            get { return elementID; }
            set { elementID = value; }
        }


        private String roleName;
        public String RoleName
        {
            get { return roleName; }
            set { roleName = value; }
        }

        private int supplierID;
        public int SupplierID
        {
            get { return supplierID; }
            set { supplierID = value; }
        }

        private int clientID;
        public int ClientID
        {
            get { return clientID; }
            set { clientID = value; }
        }

        private string notes;
        public string Notes
        {
            get { return notes; }
            set { notes = value; }
        }

        private ArrayList taggedValues;

        public ArrayList TaggedValues
        {
            get { return taggedValues; }
            set { taggedValues = value; }
        }

        private AssociationCCProperty associationCCProperty;

        internal AssociationCCProperty AssociationCCProperty {
            get { return associationCCProperty; }
            set { associationCCProperty = value; }
        }




    }
}
