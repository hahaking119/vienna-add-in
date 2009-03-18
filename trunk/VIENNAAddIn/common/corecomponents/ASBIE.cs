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
    public class ASBIE : BIE
    {


        string associatedObjectClassTermQualifier;

        public string AssociatedObjectClassTermQualifier
        {
            get { return associatedObjectClassTermQualifier; }
            set { associatedObjectClassTermQualifier = value; }
        }



        string associatedObjectClass;

        public string AssociatedObjectClass
        {
            get { return associatedObjectClass; }
            set { associatedObjectClass = value; }
        }



        String lowerBoundMultiplicity;

        public String LowerBoundMultiplicity
        {
            get { return lowerBoundMultiplicity; }
            set { lowerBoundMultiplicity = value; }
        }
        String upperBoundMultiplicity;

        public String UpperBoundMultiplicity
        {
            get { return upperBoundMultiplicity; }
            set { upperBoundMultiplicity = value; }
        }



    }
}
