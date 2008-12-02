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

namespace VIENNAAddIn.CCTS.CCTSBIE_MetaModel
{
    public class ENUM
    {
        private ArrayList attributeList;
        private String EnumName;

        public ENUM()
        {

        }

        public void setAttributeList(ArrayList attributeList)
        {
            this.attributeList = attributeList;
        }

        public ArrayList getAttributeList()
        {
            return this.attributeList;
        }

        public void setEnumName(String EnumName)
        {
            this.EnumName = EnumName;
        }

        public String getEnumName()
        {
            return this.EnumName;
        }

    }


}
