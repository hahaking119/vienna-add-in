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



    public class AggregateCoreComponent : CoreComponent {

        private String objectClassTerm;
        private ArrayList  associationCCProperty;
        private ArrayList ccProperty;

        internal ArrayList CcProperty {
            get { return ccProperty; }
            set { ccProperty = value; }
        }

        internal ArrayList  AssociationCCProperty {
            get { return associationCCProperty; }
            set { associationCCProperty = value; }
        }

        internal String ObjectClassTerm {
            get { return objectClassTerm; }
            set { objectClassTerm = value; }
        }

    }
}
