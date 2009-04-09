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



    public class AggregateBusinessInformationEntity : BusinessInformationEntity {





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
        
        private String qualifierTerm;
        private ArrayList abies = new ArrayList();

        public ArrayList Abies {
            get { return abies; }
            set { abies = value; }
        }


        public String QualifierTerm {
            get { return qualifierTerm; }
            set { qualifierTerm = value; }
        }
        private String cardinality;

        public String Cardinality {
            get { return cardinality; }
            set { cardinality = value; }
        }

        private AggregateCoreComponent basis;

        internal AggregateCoreComponent Basis {
            get { return basis; }
            set { basis = value; }
        }

        private ArrayList associationBIEProperty = new ArrayList();

        internal ArrayList AssociationBIEProperty {
            get { return associationBIEProperty; }
            set { associationBIEProperty = value; }
        }

        private ArrayList bieProperty = new ArrayList();

        internal ArrayList BIEProperty {
            get { return bieProperty; }
            set { bieProperty = value; }
        }


        private String roleName;

        public String RoleName {
            get { return roleName; }
            set { roleName = value; }
        }

        private String dictionaryEntry;
        public String DictionaryEntry {
            get { return dictionaryEntry; }
            set { dictionaryEntry = value; }
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

        private String associatedObjectClassTerm;
        public String AssociatedObjectClassTerm {
            get { return associatedObjectClassTerm; }
            set { associatedObjectClassTerm = value; }
        }




        private ArrayList taggedValues;

        public ArrayList TaggedValues {
            get { return taggedValues; }
            set { taggedValues = value; }
        }



        /// <sUMM2ary>
        /// Returns the value of the tagged value with the given name
        /// </sUMM2ary>
        /// <param name="name"></param>
        /// <returns></returns>
        internal String getSingleTaggedValue(CCTS_TV c) {
            String name = c.ToString();            
            if (this.taggedValues != null && this.taggedValues.Count != 0) {
                for (int i = 0; i < taggedValues.Count; i++) {
                    TaggedValue tv = (TaggedValue)taggedValues[i];
                    if (tv.Name == name || tv.Name == name.Substring(0,1).ToLower()+name.Substring(1)) {
                        return tv.Value;
                    }
                }                
            }

            return "";
        }






    }
}
