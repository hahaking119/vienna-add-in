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
    public  class ABIE : BIE
    {




        List<BBIE> bbies;

        public List<BBIE> Bbies
        {
            get { return bbies; }
            set { bbies = value; }
        }


        List<ASBIE> asbies;

        public List<ASBIE> Asbies
        {
            get { return asbies; }
            set { asbies = value; }
        }


        /// <summary>
        /// Add a BBIE to the list
        /// </summary>
        /// <param name="bcc"></param>
        public void addBBIE(BBIE bbie)
        {
            if (this.bbies == null)
            {
                this.bbies = new List<BBIE>();
            }

            this.bbies.Add(bbie);
        }



        /// <summary>
        /// Add a ASCC to the list
        /// </summary>
        /// <param name="ascc"></param>
        public void addASBIE(ASBIE asbie)
        {
            if (this.asbies == null)
            {
                this.asbies = new List<ASBIE>();
            }

            this.asbies.Add(asbie);
        }









    }
}
