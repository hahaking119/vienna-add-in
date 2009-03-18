/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace VIENNAAddIn.CCTS {

    /// <sUMM2ary>
    /// This class serves as a helper when trying to find connectors
    /// between two abies which have the same role name (what is not allowed)
    /// </sUMM2ary>
    internal class BIEConnector {

        private String connectorClient;
        private int connectorClientID;

        public int ConnectorClientID {
            get { return connectorClientID; }
            set { connectorClientID = value; }
        }
        
        public String ConnectorClient {
            get { return connectorClient; }
            set { connectorClient = value; }
        }

        private String connectorSupplier;
        private int connectorSupplierID;

        public int ConnectorSupplierID {
            get { return connectorSupplierID; }
            set { connectorSupplierID = value; }
        }

        public String ConnectorSupplier{
            get { return connectorSupplier; }
            set { connectorSupplier = value; }
        }



        private String roleName;

        public String RoleName {
            get { return roleName; }
            set { roleName = value; }
        }

        //The default value
        private bool isChecked = false;

        public bool IsChecked {
            get { return isChecked; }
            set { isChecked = value; }
        }




    }
}
