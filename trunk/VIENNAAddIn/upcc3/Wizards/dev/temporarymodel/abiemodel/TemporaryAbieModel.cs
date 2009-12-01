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
using CctsRepository.BieLibrary;
using CctsRepository.CcLibrary;
using VIENNAAddIn.upcc3.ccts.dra;
using VIENNAAddIn.upcc3.Wizards.dev.util;

namespace VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.abiemodel
{
    public class TemporaryAbieModel : TemporaryModel
    {
        private Dictionary<string, CandidateAbie> candidateAbies;
        private Dictionary<string, CandidateBcc> candidateBccs;
        private string Name;
        private string Prefix;
        public string SelectedBcc{ get; set;}
        public string SelectedBbie{ get; set;}

        public TemporaryAbieModel(IAcc candidateAcc, string name, string prefix)
        {
            Name = name;
            Prefix = prefix;

            PrepareTemporaryABIEModel(candidateAcc);
        }

        public TemporaryAbieModel(CCRepository x)
        {
        }

        public TemporaryAbieModel(IAbie abie)
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

        public IEnumerable<CheckedListItem> Bccs
        {
            get { return GetBccs(); }
            set
            {
                IEnumerator<CheckedListItem> checkedListItemEnumerator = value.GetEnumerator();
                foreach (var keyValuePair in candidateBccs)
                {
                    keyValuePair.Value.Name = checkedListItemEnumerator.Current.Name;
                    checkedListItemEnumerator.MoveNext();
                }
            }
        }
        public IEnumerable<CheckedListItem> Bbies
        {
            get { return GetBbies(); }
            set
            {
                IEnumerator<CheckedListItem> checkedListItemEnumerator = value.GetEnumerator();
                foreach (var keyValuePair in candidateBccs)
                {
                    keyValuePair.Value.Name = checkedListItemEnumerator.Current.Name;
                    checkedListItemEnumerator.MoveNext();
                }
            }
        }
		
        public IEnumerable<CheckedListItem> Bdts
        {
            get { return GetBdts(); }
            set
            {
                IEnumerator<CheckedListItem> checkedListItemEnumerator = value.GetEnumerator();
                foreach (var keyValuePair in candidateBccs[SelectedBcc].PotentialBBIEs[SelectedBbie].PotentialBDTs)
                {
                    keyValuePair.Value.Name = checkedListItemEnumerator.Current.Name;
                    checkedListItemEnumerator.MoveNext();
                }
            }
        }
        private void PrepareTemporaryABIEModel(IAcc acc)
        {
            candidateBccs = new Dictionary<string, CandidateBcc>();
            foreach (IBcc bcc in acc.BCCs)
            {
                var candidateBCC = new CandidateBcc(bcc);
                var potentialBBIE = new PotentialBbie(candidateBCC.OriginalBCC.Name);
                potentialBBIE.PotentialBDTs.Add(candidateBCC.OriginalBCC.Cdt.Name,
                                                new PotentialBdt(candidateBCC.OriginalBCC.Name));
                candidateBCC.PotentialBBIEs.Add(candidateBCC.OriginalBCC.Name, potentialBBIE);
                candidateBccs.Add(bcc.Name, candidateBCC);
            }
        }

        public IEnumerable<CheckedListItem> GetBccs()
        {
            foreach (var keyValuePair in candidateBccs)
            {
                yield return new CheckedListItem(keyValuePair.Key, keyValuePair.Value.Checked);
            }
        }

        public IEnumerable<CheckedListItem> GetBbies()
        {
            foreach (var keyValuePair in candidateBccs[SelectedBcc].PotentialBBIEs)
            {
                yield return new CheckedListItem(keyValuePair.Key, keyValuePair.Value.Checked);
            }
        }

        public IEnumerable<CheckedListItem> GetBdts()
        {
            foreach (var keyValuePair in candidateBccs)
            {
                foreach (var potentialBBIE in keyValuePair.Value.PotentialBBIEs)
                {
                    if (potentialBBIE.Key.Equals(SelectedBbie))
                    {
                        foreach (var potentialBDT in potentialBBIE.Value.PotentialBDTs)
                        {
                            yield return new CheckedListItem(potentialBDT.Key, potentialBDT.Value.Checked);
                        }
                    }
                }
            }
        }

        public void AddBbie(string bcc, string potentialBBIEName)
        {
            candidateBccs[bcc].PotentialBBIEs.Add(potentialBBIEName, new PotentialBbie(potentialBBIEName));
        }

        public void AddBdt(string potentialBDTName)
        {
            foreach (var candidateBCC in candidateBccs)
            {
                foreach (var keyValuePair in candidateBCC.Value.PotentialBBIEs)
                {
                    keyValuePair.Value.PotentialBDTs.Add(potentialBDTName, new PotentialBdt(potentialBDTName));
                }
            }
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

        //public IEnumerable<CheckedListItem> getBCCs()
        //{
        //    //if (BCCs == null)
        //    //{
        //    //    BCCs = new Dictionary<string, TemporaryBCC>();
        //    //    if (GetBasedOnACC() != null)
        //    //    {
        //    //        foreach (IBCC bcc in GetBasedOnACC().BCCs)
        //    //        {
        //    //            BCCs.Add(bcc.Name, new TemporaryBCC(bcc));
        //    //        }
        //    //    }
        //    //}
        //    //foreach (var keyValuePair in BCCs)
        //    //{
        //    //    yield return new CheckedListItem(keyValuePair.Key, keyValuePair.Value.Checkstate);
        //    //}
        //    throw new NotImplementedException();
        //}

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

        public ICcLibrary GetCCLInUse()
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

        public IAcc GetBasedOnACC()
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