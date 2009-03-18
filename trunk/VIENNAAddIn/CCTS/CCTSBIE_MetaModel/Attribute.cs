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
using EA;

namespace VIENNAAddIn.CCTS.CCTSBIE_MetaModel {


     public class Attribute {

         private int classifierID;
         private String stereotype;

         public String Stereotype {
             get { return stereotype; }
             set { stereotype = value; }
         }

         public int ClassifierID {
             get { return classifierID; }
             set { classifierID = value; }
         }
            private String name;
            private String upperBound;

            public String UpperBound {
                get { return upperBound; }
                set { upperBound = value; }
            }
            private String lowerBound;

            public String LowerBound {
                get { return lowerBound; }
                set { lowerBound = value; }
            }

            public String Name {
                get { return name; }
                set { name = value; }
            }
            private String type;

            public String Type {
                get { return type; }
                set { type = value; }
            }

             private String notes;
             public String Notes
             {
                 get { return notes; }
                 set { notes = value; }
             }

            private ArrayList taggedValues;

            public ArrayList TaggedValues {
                get { return taggedValues; }
                set { taggedValues = value; }
            }


        }   

}
