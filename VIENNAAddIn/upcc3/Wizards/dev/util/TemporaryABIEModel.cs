// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UPCCRepositoryInterface;
using VIENNAAddIn.upcc3.ccts.dra;

namespace VIENNAAddIn.upcc3.Wizards.dev.util
{
   
    public class TemporaryABIEModel
    { 
        private IACC BasedOnACC { get; set; }
        private IABIE existingABIE;
        private string ABIEName;
        private string ABIEPrefix;
        private Dictionary<string, TemporaryBBIE> BBIEs;
        private Dictionary<string, TemporaryBCC> BCCs;
        private Dictionary<string, TemporaryBDT> BDTs;
        private string BIELibStore;
        private string CCLinUse;
        private 

        private Dictionary<string, TemporaryASBIE> potentialASBIEs;

        public TemporaryABIEModel()
        {
            existingABIE = null;
        }

        public TemporaryABIEModel(IABIE abie)
        {
            existingABIE = abie;
            string[] tempArray = abie.Name.Split('_');
            ABIEName = tempArray[0];
            ABIEPrefix = tempArray[1];
            SetTargetACC(abie.BasedOn);

            foreach (IBBIE bbie in abie.BBIEs)
            {
                //createBBIE(bbie.Type, bbie.Name, );
            }

        }

        public Dictionary<string, TemporaryBBIE> GetBBIEs()
        {
            return BBIEs;
        }
        
        public IACC GetBasedOnACC()
        {
            return BasedOnACC;
        }

        public void SetTargetACC(IACC acc)
        {
            BasedOnACC = acc;
        }
        internal class TemporaryASBIE
        {
            internal IASBIE asbie { get; set;}
            internal Boolean Checkstate { get; set; }
            public TemporaryASBIE(IASBIE newasbie)
            {
                asbie = newasbie;
                Checkstate = false;
            }
        }
        public internal class TemporaryBBIE
        {
            internal IBBIE bbie { get; set; }
            internal Boolean Checkstate { get; set; }
            public TemporaryBBIE(IBBIE newbbie)
            {
                bbie = newbbie;
                Checkstate = false;
            }
        }
        internal class TemporaryBDT
        {
            internal IBDT bdt { get; set; }
            internal Boolean Checkstate { get; set; }
            public TemporaryBDT(IBDT newbdt)
            {
                bdt = newbdt;
                Checkstate = false;
            }
        }
        internal class TemporaryBCC
        {
            internal IBCC bcc { get; set; }
            internal Boolean Checkstate { get; set; }
            public TemporaryBCC(IBCC newbcc)
            {
                bcc = newbcc;
                Checkstate = false;
            }
        }

        public void createBBIE(string underlyingBCCName, string BBIEName, List<string> bdts)
        {
            throw new NotImplementedException();
        }
    }
}
