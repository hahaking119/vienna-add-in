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

    public class RegistryClass {

        public RegistryClass() { }


        private String uniqueIdentifier = "";
        private String dictionaryEntryName = "";
        private String definition = "";

        public String Definition {
            get { return definition; }
            set { definition = value; }
        }

        public String DictionaryEntryName {
            get { return dictionaryEntryName; }
            set { dictionaryEntryName = value; }
        }


        public String UniqueIdentifier {
            get { return uniqueIdentifier; }
            set { uniqueIdentifier = value; }
        }


    }
}
