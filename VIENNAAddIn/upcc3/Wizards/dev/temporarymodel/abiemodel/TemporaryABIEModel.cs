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
using CctsRepository.BieLibrary;
using CctsRepository.CcLibrary;
using VIENNAAddIn.upcc3.ccts.dra;
using VIENNAAddIn.upcc3.Wizards.dev.util;

namespace VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.abiemodel
{
    public class TemporaryABIEModel : TemporaryModel
    {
        private string Name;
        private string Prefix;
        private Dictionary<string, CandidateBCC> CandidateBCCs;
        private Dictionary<string, CandidateABIE> CandidateABIEs;

        public TemporaryABIEModel(IACC candidateACC, string name, string prefix)
        {
            Name = name;
            Prefix = prefix;

            PrepareTemporaryABIEModel(candidateACC);
        }

        private void PrepareTemporaryABIEModel(IACC acc)
        {
            CandidateBCCs = new Dictionary<string, CandidateBCC>();
            foreach (IBCC bcc in acc.BCCs)
            {
                var candidateBCC = new CandidateBCC(bcc);
                var potentialBBIE = new PotentialBBIE(candidateBCC.OriginalBCC.Name);
                potentialBBIE.PotentialBDTs.Add(candidateBCC.OriginalBCC.Type.Name,new PotentialBDT(candidateBCC.OriginalBCC.Name));
                candidateBCC.PotentialBBIEs.Add(candidateBCC.OriginalBCC.Name,potentialBBIE);
                CandidateBCCs.Add(bcc.Name,candidateBCC);
            }
        }

        private void AddBBIE(string bcc, string potentialBBIEName)
        {
            CandidateBCCs[bcc].PotentialBBIEs.Add(potentialBBIEName,new PotentialBBIE(potentialBBIEName));
        }

        private void AddBDT (string potentialBDTName)
        {
            foreach (KeyValuePair<string, CandidateBCC> candidateBCC in CandidateBCCs)
            {
                foreach (KeyValuePair<string, PotentialBBIE> keyValuePair in candidateBCC.Value.PotentialBBIEs)
                {
                    keyValuePair.Value.PotentialBDTs.Add(potentialBDTName,new PotentialBDT(potentialBDTName));
                }
            }
        }
//        private CCCache temporaryCache;

        private ListModel listModelCCL;
        private ListModel listModelACC;
        private CheckedListModel checkedListModelBCC;
        private CheckedListModel checkedListModelBBIE;
        private CheckedListModel checkedListModelBDT;
        private CheckedListModel checkedListModelABIE;
        private CheckedListModel checkedListModelASCC;
        private ListModel listModelBIEL;
        private ListModel listModelBDTL;




        public void Initialize(ListModel _listModelCCL, ListModel _listModelACC, CheckedListModel _checkedListModelBCC, CheckedListModel _checkedListModelBBIE, CheckedListModel _checkedListModelBDT, CheckedListModel _checkedListModelABIE, CheckedListModel _checkedListModelASCC, ListModel _listModelBIEL, ListModel _listModelBDTL)
        {
            listModelCCL = _listModelCCL;
            listModelCCL.Content = new ObservableCollection<string>(new List<string>(getCCLs()));
            listModelACC = _listModelACC;
            checkedListModelBCC = _checkedListModelBCC;
            checkedListModelBBIE = _checkedListModelBBIE;
            checkedListModelBBIE.Content = new ObservableCollection<CheckedListItem>(new List<CheckedListItem>(/*getBBIEs()*/));
            checkedListModelBDT = _checkedListModelBDT;
            checkedListModelBDT.Content = new ObservableCollection<CheckedListItem>(new List<CheckedListItem>(/*getBDTs()*/));
            checkedListModelABIE = _checkedListModelABIE;
            checkedListModelABIE.Content = new ObservableCollection<CheckedListItem>(new List<CheckedListItem>(/*getABIEs()*/));
            checkedListModelASCC = _checkedListModelASCC;
            checkedListModelASCC.Content = new ObservableCollection<CheckedListItem>(new List<CheckedListItem>(/*getASCCs()*/));
            listModelBIEL = _listModelBIEL;
            listModelBIEL.Content = new ObservableCollection<string>(new List<string>(/*getBIELs()*/));
            listModelBDTL = _listModelBDTL;
            listModelBDTL.Content = new ObservableCollection<string>(new List<string>(/*getBDTLs()*/));
        }


        public TemporaryABIEModel(CCRepository x)
        {
            
        }
        public TemporaryABIEModel(IABIE abie)
        {
            //existingABIE = abie;
            //string[] tempArray = abie.Name.Split('_');
            //ABIEName = tempArray[0];
            //ABIEPrefix = tempArray[1];
            //SetTargetACC(abie.BasedOn.Name);

            //foreach (IBBIE bbie in abie.BBIEs)
            //{
            //    //createBBIE(bbie.Type, bbie.Name, );
            //}

        }
        public IEnumerable<string> getCCLs()
        {
            //if (CCLs == null)
            //{
            //    CCLs = new Dictionary<string, TemporaryCCL>();
            //    foreach (CCLibrary ccLibrary in temporaryCache.GetCCLibraries())
            //    {
            //        CCLs.Add(ccLibrary.Name, new TemporaryCCL(ccLibrary));
            //    }
            //}
            //foreach (KeyValuePair<string, TemporaryCCL> keyValuePair in CCLs)
            //{
            //    yield return keyValuePair.Key;
            //}
            throw new NotImplementedException();
        }

        public IEnumerable<string> getACCs()
        {
            //if (ACCs == null)
            //{
            //    ACCs = new Dictionary<string, TemporaryACC>();
            //    if (GetCCLInUse() != null)
            //    {
            //        foreach (ACC acc in temporaryCache.GetCCsFromCCLibrary(GetCCLInUse().Name))
            //        {
            //            ACCs.Add(acc.Name, new TemporaryACC(acc));
            //        }
            //    }
            //}
            //foreach (KeyValuePair<string, TemporaryACC> keyValuePair in ACCs)
            //{
            //    yield return keyValuePair.Key;
            //}
            throw new NotImplementedException();
        }

        public IEnumerable<CheckedListItem> getBCCs()
        {
            //if (BCCs == null)
            //{
            //    BCCs = new Dictionary<string, TemporaryBCC>();
            //    if (GetBasedOnACC() != null)
            //    {
            //        foreach (IBCC bcc in GetBasedOnACC().BCCs)
            //        {
            //            BCCs.Add(bcc.Name, new TemporaryBCC(bcc));
            //        }
            //    }
            //}
            //foreach (var keyValuePair in BCCs)
            //{
            //    yield return new CheckedListItem(keyValuePair.Key, keyValuePair.Value.Checkstate);
            //}
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetBBIEs()
        {
            //if (BBIEs == null)
            //{
            //    BBIEs = new Dictionary<string, TemporaryBBIE>();

            //}
            //foreach (KeyValuePair<string, TemporaryBBIE> keyValuePair in BBIEs)
            //{
            //    yield return keyValuePair.Key;
            //}
            throw new NotImplementedException();
        }

        public ICCLibrary GetCCLInUse()
        {
            //foreach (KeyValuePair<string, TemporaryCCL> keyValuePair in CCLs)
            //{
            //    if (keyValuePair.Value.Checkstate)
            //    {
            //        return keyValuePair.Value.ccl;
            //    }
            //}
            return null;
        }
        public void setCCLInUse(string CCL)
        {
            //foreach (KeyValuePair<string, TemporaryCCL> keyValuePair in CCLs)
            //{
            //    if (keyValuePair.Key.Equals(CCL))
            //    {
            //        keyValuePair.Value.Checkstate = true;
            //    }
            //}
            //listModelACC.Content = new ObservableCollection<string>(new List<string>(getACCs()));
        }
        public IACC GetBasedOnACC()
        {
            //foreach (KeyValuePair<string, TemporaryACC> keyValuePair in ACCs)
            //{
            //    if (keyValuePair.Value.Checkstate)
            //    {
            //        return keyValuePair.Value.acc;
            //    }
            //}
            return null;
        }

        public void SetTargetACC(string acc)
        {
            //ACCs[acc].Checkstate = true;
            //checkedListModelBCC.Content = new ObservableCollection<CheckedListItem>(new List<CheckedListItem>(getBCCs()));
        }
    }
}