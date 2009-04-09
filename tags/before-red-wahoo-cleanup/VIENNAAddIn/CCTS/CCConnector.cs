using System;
using System.Collections.Generic;
using System.Text;

namespace UMM2.CCTS
{
    internal class CCConnector
    {

        private String connectorClient;
        private int connectorClientID;

        public int ConnectorClientID
        {
            get { return connectorClientID; }
            set { connectorClientID = value; }
        }

        public String ConnectorClient
        {
            get { return connectorClient; }
            set { connectorClient = value; }
        }

        private String connectorSupplier;
        private int connectorSupplierID;

        public int ConnectorSupplierID
        {
            get { return connectorSupplierID; }
            set { connectorSupplierID = value; }
        }

        public String ConnectorSupplier
        {
            get { return connectorSupplier; }
            set { connectorSupplier = value; }
        }



        private String roleName;

        public String RoleName
        {
            get { return roleName; }
            set { roleName = value; }
        }

        //The default value
        private bool isChecked = false;

        public bool IsChecked
        {
            get { return isChecked; }
            set { isChecked = value; }
        }




    }
}
