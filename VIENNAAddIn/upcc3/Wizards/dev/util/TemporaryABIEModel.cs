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
using System.Collections.ObjectModel;
using CctsRepository;
using VIENNAAddIn.upcc3.ccts.dra;
using VIENNAAddIn.upcc3.Wizards.dev.cache;

namespace VIENNAAddIn.upcc3.Wizards.dev.util
{
   
    public class TemporaryABIEModel
    { 
        private IABIE existingABIE;
        private string ABIEName;
        private string ABIEPrefix;
        private Dictionary<string, TemporaryACC> ACCs;
        private Dictionary<string, TemporaryBBIE> BBIEs;
        private Dictionary<string, TemporaryBCC> BCCs;
        private Dictionary<string, TemporaryBDT> BDTs;
        private Dictionary<string, TemporaryCCL> CCLs;
        private Dictionary<string, TemporaryBIEL> BIELibStore;
        private Dictionary<string, TemporaryBDTL> BDTLibStore;
        private CCCache temporaryCache;

        private Dictionary<string, TemporaryASBIE> potentialASBIEs;

        private ListModel listModelCCL;
        private CheckedListModel checkedListModelCCL;

        public void Initialize(ListModel listModel, CheckedListModel checkedListModel)
        {
            this.listModelCCL = listModel;
            this.listModelCCL.Content = new ObservableCollection<string>(new List<string>(this.getCCLs()));
            this.checkedListModelCCL = checkedListModel;
            this.checkedListModelCCL.Content = new ObservableCollection<CheckedListItem>();
            foreach(string name in getCCLs())
            {
                this.checkedListModelCCL.Content.Add(new CheckedListItem(name, true));
            }
        }

        public TemporaryABIEModel(CCRepository ccRepository)
        {
            temporaryCache = CCCache.GetInstance(ccRepository);
            existingABIE = null;
        }
        public IEnumerable<string> getCCLs()
        {
            if(CCLs ==null)
            {
                CCLs = new Dictionary<string, TemporaryCCL>();
                foreach (CCLibrary ccLibrary in temporaryCache.GetCCLibraries())
                {
                    CCLs.Add(ccLibrary.Name,new TemporaryCCL(ccLibrary));
                }
            }
            foreach (KeyValuePair<string, TemporaryCCL> keyValuePair in CCLs)
            {
                yield return keyValuePair.Key;
            }
        }

        public TemporaryABIEModel(IABIE abie)
        {
            existingABIE = abie;
            string[] tempArray = abie.Name.Split('_');
            ABIEName = tempArray[0];
            ABIEPrefix = tempArray[1];
            SetTargetACC(abie.BasedOn.Name);

            foreach (IBBIE bbie in abie.BBIEs)
            {
                //createBBIE(bbie.Type, bbie.Name, );
            }

        }

        public IEnumerator<string> GetBBIEs()
        {
            foreach (KeyValuePair<string, TemporaryBBIE> keyValuePair in BBIEs)
            {
                yield return keyValuePair.Key;
            }
        }
        
        public ICCLibrary GetCCLInUse()
        {
            foreach (KeyValuePair<string, TemporaryCCL> keyValuePair in CCLs)
            {
                if (keyValuePair.Value.Checkstate)
                {
                    return keyValuePair.Value.ccl;
                }
            }
            return null;
        }
        public void setCCLInUse(string CCL)
        {
            foreach (KeyValuePair<string, TemporaryCCL> keyValuePair in CCLs)
            {
                if (keyValuePair.Key.Equals(CCL))
                {
                    keyValuePair.Value.Checkstate = true;
                }
            }

        }
        public IACC GetBasedOnACC()
        {
            foreach (KeyValuePair<string, TemporaryACC> keyValuePair in ACCs)
            {
                if (keyValuePair.Value.Checkstate)
                {
                    return keyValuePair.Value.acc;
                }
            }
            return null;
        }

        public void SetTargetACC(string acc)
        {
            foreach (KeyValuePair<string, TemporaryACC> keyValuePair in ACCs)
            {
                if(keyValuePair.Key.Equals(acc))
                {
                    keyValuePair.Value.Checkstate = true;
                }
            }
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
        public class TemporaryBBIE
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

        public class TemporaryCCL
        {
            internal ICCLibrary ccl { get; set; }
            internal Boolean Checkstate { get; set; }
            public TemporaryCCL(ICCLibrary newccl)
            {
                ccl = newccl;
                Checkstate = false;
            }
        }

        public class TemporaryBIEL
        {
            internal IBIELibrary bieLibrary { get; set; }
            internal Boolean Checkstate { get; set; }
            public TemporaryBIEL(IBIELibrary newbiel)
            {
                bieLibrary = newbiel;
                Checkstate = false;
            }
        }

        public class TemporaryBDTL
        {
            internal IBDTLibrary bdtLibrary { get; set; }
            internal Boolean Checkstate { get; set; }
            public TemporaryBDTL(IBDTLibrary newbdtl)
            {
                bdtLibrary = newbdtl;
                Checkstate = false;
            }
        }

        public class TemporaryACC
        {
            internal IACC acc { get; set; }
            internal Boolean Checkstate { get; set; }
            public TemporaryACC(IACC newacc)
            {
                acc = newacc;
                Checkstate = false;
            }
        }
    }
}
