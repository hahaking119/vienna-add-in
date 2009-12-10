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
using System.Windows.Input;
using CctsRepository;
using VIENNAAddIn.upcc3.Wizards.dev.cache;
using VIENNAAddIn.upcc3.Wizards.dev.util;
namespace VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.bdtmodel
{
    public class TemporaryBdtModel : TemporaryModel
    {

        private List<string> mCandidateBdtLibraryNames;
        private List<string> mCandidateCdtLibraryNames;
        private List<string> mCandidateCdtNames;
        private List<CheckableItem> mCandidateConItems;
        private List<CheckableItem> mCandidateSupItems;
        private string mName;
        private string mPrefix;
        private CcCache ccCache;
        public List<CandidateBdtLibrary> mCandidateBdtLibraries;
        public List<CandidateCdtLibrary> mCandidateCdtLibraries;

        public TemporaryBdtModel(ICctsRepository cctsRepository)
        {
            ccCache = CcCache.GetInstance(cctsRepository);

            mCandidateBdtLibraries = new List<CandidateBdtLibrary>(ccCache.GetBdtLibraries().ConvertAll(bdtl => new CandidateBdtLibrary(bdtl)));
            mCandidateBdtLibraryNames = new List<string>(mCandidateBdtLibraries.ConvertAll(bdtlname => bdtlname.OriginalBdtLibrary.Name));
            mCandidateCdtLibraries = new List<CandidateCdtLibrary>(ccCache.GetCdtLibraries().ConvertAll(cdtl => new CandidateCdtLibrary(cdtl)));
            mCandidateCdtLibraryNames = new List<string>(mCandidateCdtLibraries.ConvertAll(cdtlname => cdtlname.OriginalCdtLibrary.Name));
        }

        /// 
        /// <param name="selectedBdtLibrary"></param>
        public void setSelectedCandidateBdtLibrary(string selectedBdtLibrary)
        {
            foreach (CandidateBdtLibrary candidateBdtLibrary in mCandidateBdtLibraries)
            {
                if(candidateBdtLibrary.OriginalBdtLibrary.Name.Equals(selectedBdtLibrary))
                {
                    candidateBdtLibrary.Selected = true;
                }
            }
        }

        /// 
        /// <param name="selectedCdtLibrary"></param>
        public void setSelectedCandidateCdtLibrary(string selectedCdtLibrary)
        {
            foreach (CandidateCdtLibrary candidateCdtLibrary in mCandidateCdtLibraries)
            {
                if(candidateCdtLibrary.OriginalCdtLibrary.Name.Equals(selectedCdtLibrary))
                {
                    candidateCdtLibrary.Selected = true;
                }
            }
        }

        /// 
        /// <param name="selectedCdt"></param>
        public void setSelectedCandidateCdt(string selectedCdt)
        {
            foreach (CandidateCdtLibrary candidateCdtLibrary in mCandidateCdtLibraries)
            {
                if(candidateCdtLibrary.Selected.Equals(true))
                {
                    foreach (CandidateCdt candidateCdt in candidateCdtLibrary.CandidateCdts)
                    {
                        if(candidateCdt.OriginalCdt.Name.Equals(selectedCdt))
                        {
                            candidateCdt.Selected = true;
                        }
                    }
                }
            }
        }

        /// 
        /// <param name="potentialCon"></param>
        public CheckableItem PotentialConToCheckableItem(PotentialCon potentialCon)
        {
           return new CheckableItem(potentialCon.Checked, potentialCon.OriginalCdtSup.Name,true,false,Cursors.Arrow);
        }

        /// 
        /// <param name="potentialSup"></param>
        public CheckableItem PotentialSupToCheckableItem(PotentialSup potentialSup)
        {
            return new CheckableItem(potentialSup.Checked, potentialSup.OriginalCdtSup.Name, true, false, Cursors.Arrow);
        }

        /// 
        /// <param name="checkedValue"></param>
        /// <param name="selectedSup"></param>
        public void setCheckedPotentialSup(bool checkedValue, string selectedSup)
        {
            foreach (CandidateCdtLibrary candidateCdtLibrary in mCandidateCdtLibraries)
            {
                if (candidateCdtLibrary.Selected.Equals(true))
                {
                    foreach (CandidateCdt candidateCdt in candidateCdtLibrary.CandidateCdts)
                    {
                        if (candidateCdt.Selected.Equals(true))
                        {
                            foreach (PotentialSup potentialSup in candidateCdt.mPotentialSups)
                            {
                                if(potentialSup.OriginalCdtSup.Name.Equals(selectedSup))
                                {
                                    potentialSup.Checked = checkedValue;
                                }
                            }
                        }
                    }
                }
            }
        }

    }
}