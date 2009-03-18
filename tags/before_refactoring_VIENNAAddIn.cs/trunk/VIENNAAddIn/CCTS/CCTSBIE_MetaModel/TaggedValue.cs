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

    public class TaggedValue {

        private String value;
        private String type;
        private String name;

        public String Type {
            get { return type; }
            set { type = value; }
        }

        public String Value {
            get { return this.value; }
            set { this.value = value; }
        }
        

        public String Name {
            get { return name; }
            set { name = value; }
        }

    }


}
