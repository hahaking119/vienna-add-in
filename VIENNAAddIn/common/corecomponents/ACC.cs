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
    public class ACC : CC
    {

    
        List<BCC> bccs;

        public List<BCC> Bccs
        {
            get { return bccs; }
            set { bccs = value; }
        }


        List<ASCC> asccs;

        public List<ASCC> Asccs
        {
            get { return asccs; }
            set { asccs = value; }
        }


        /// <summary>
        /// Add a BCC to the list
        /// </summary>
        /// <param name="bcc"></param>
        public void addBCC(BCC bcc)
        {
            if (this.bccs == null)
            {
                this.bccs = new List<BCC>();
            }

            this.bccs.Add(bcc);
        }


  
        /// <summary>
        /// Add a ASCC to the list
        /// </summary>
        /// <param name="ascc"></param>
        public void addASCC(ASCC ascc)
        {
            if (this.asccs == null)
            {
                this.asccs = new List<ASCC>();
            }

            this.asccs.Add(ascc);
        }


                
    }
}
