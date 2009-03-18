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
    public class QDT : DT
    {


        string underlyingCDT;

        public string UnderlyingCDT
        {
            get { return underlyingCDT; }
            set { underlyingCDT = value; }
        }





        string dataTypeQualifier;

        public string DataTypeQualifier
        {
            get { return dataTypeQualifier; }
            set { dataTypeQualifier = value; }
        }


         

    }
}
