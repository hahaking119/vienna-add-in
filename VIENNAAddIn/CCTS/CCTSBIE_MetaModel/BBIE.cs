/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;
using System.Collections;
using System.Text;
using VIENNAAddIn.constants;

namespace VIENNAAddIn.CCTS.CCTSBIE_MetaModel {


    
    public class BBIE : BusinessInformationEntity {



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

        private String type;

        public String Type {
            get { return type; }
            set { type = value; }
        }

        private String lowerBound;

        public String LowerBound {
            get { return lowerBound; }
            set { lowerBound = value; }
        }
        private String upperBound;

        public String UpperBound {
            get { return upperBound; }
            set { upperBound = value; }
        }


        private BasicBIEProperty basicBIEProperty;

        internal BasicBIEProperty BasicBIEProperty {
            get { return basicBIEProperty; }
            set { basicBIEProperty = value; }
        }

        private BCC basis;

        internal BCC Basis {
            get { return basis; }
            set { basis = value; }
        }


        private String dictionaryEntryName;
        public String DictionaryEntryName {
            get { return dictionaryEntryName; }
            set { dictionaryEntryName = value; }
        }


        private String objectClassTerm;
        public String ObjectClassTerm {
            get { return objectClassTerm; }
            set { objectClassTerm = value; }
        }

        private String propertyTerm;
        public String PropertyTerm {
            get { return propertyTerm; }
            set { propertyTerm = value; }
        }

        private String representationTerm;
        public String RepresentationTerm {
            get { return representationTerm; }
            set { representationTerm = value; }
        }


        private String dataType;
        public String DataType {
            get { return dataType; }
            set { dataType = value; }
        }


        private ArrayList taggedValues;

        public ArrayList TaggedValues {
            get { return taggedValues; }
            set { taggedValues = value; }
        }


        /// <summary>
        /// Returns the value of the tagged value with the given name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        internal String getSingleTaggedValue(CCTS_TV c) {
            String name = c.ToString();            
            if (this.taggedValues != null && this.taggedValues.Count != 0) {
                for (int i = 0; i < taggedValues.Count; i++) {
                    TaggedValue tv = (TaggedValue)taggedValues[i];
                    if (tv.Name == name || tv.Name == name.Substring(0, 1).ToLower() + name.Substring(1)) {
                        return tv.Value;
                    }
                }
            }

            return "";
        }




    }
}
