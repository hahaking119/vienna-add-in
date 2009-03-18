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
    public class EnumLibrary
    {
        private ArrayList enumList;

        public EnumLibrary()
        {

        }

        public void setEnumList(ArrayList enumList)
        {
            this.enumList = enumList;
        }

        public ArrayList getEnumList()
        {
            return this.enumList;
        }

    }

    
}
