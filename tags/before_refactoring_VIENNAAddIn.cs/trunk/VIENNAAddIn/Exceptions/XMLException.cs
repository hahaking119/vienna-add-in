/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;
using VIENNAAddIn.CCTS.CCTSBIE_MetaModel;

namespace VIENNAAddIn.Exceptions { 
    /// <sUMM2ary>
    /// SUMM2ary description for XMLException.
    /// </sUMM2ary>
    internal class XMLException : AddInException{

        private String errorString = "";

        internal XMLException()
            : base() {
            errorString = "No message specified.";
            //
            // TODO: Add constructor logic here
            //
        }
        internal XMLException(String message)
            : base(message) {
            errorString = message;
        }

        internal XMLException(String message, AggregateBusinessInformationEntity abie) 
            :base(message) {
            errorString = message + ". \n Affected abie: " + abie.Name;            
        }

        internal XMLException(String message, EA.Package p)
            : base(message) {
            if (p.Element.Stereotype == "")
                errorString = message + ". \n Package name: " + p.Name;
            else
                errorString = message + ". \n Package name: <<"+p.Element.Stereotype.ToString()+">> " + p.Name;
        }


        public String getErrorString() {
            return this.errorString;
        }


    }
}