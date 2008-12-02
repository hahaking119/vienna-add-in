/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;
using System.Collections;
using System.Text;

namespace VIENNAAddIn.CCTS {


    /// <sUMM2ary>
    /// This class holds auxilliary schemas which might be necessary during the
    /// creation of a regular schema
    /// </sUMM2ary>
    internal class AuxilliarySchema {
        
        private String packageOfOrigin;

        public String PackageOfOrigin {
            get { return packageOfOrigin; }
            set { packageOfOrigin = value; }
        }
        private String _namespace;

        public String Namespace {
            get { return _namespace; }
            set { _namespace = value; }
        }
        private ICollection schemas;

        public ICollection Schemas {
            get { return schemas; }
            set { schemas = value; }
        }
        private String namespacePrefix;

        public String NamespacePrefix {
            get { return namespacePrefix; }
            set { namespacePrefix = value; }
        }








    }
}
