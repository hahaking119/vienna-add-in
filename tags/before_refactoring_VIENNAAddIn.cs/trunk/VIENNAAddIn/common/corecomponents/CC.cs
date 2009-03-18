/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VIENNAAddIn.common.corecomponents
{
    public class CC
    {

        string definition;

        public string Definition
        {
            get { return definition; }
            set { definition = value; }
        }



        string objectClassTerm;

        public string ObjectClassTerm
        {
            get { return objectClassTerm; }
            set { objectClassTerm = value; }
        }
        string propertyTerm;

        public string PropertyTerm
        {
            get { return propertyTerm; }
            set { propertyTerm = value; }
        }
        string representationTerm;

        public string RepresentationTerm
        {
            get { return representationTerm; }
            set { representationTerm = value; }
        }


        

    }
}
