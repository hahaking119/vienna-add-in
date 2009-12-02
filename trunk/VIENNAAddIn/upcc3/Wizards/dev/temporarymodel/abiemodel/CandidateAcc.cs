using System.Collections.Generic;
using CctsRepository.CcLibrary;

namespace VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.abiemodel
{
    public class CandidateAcc
    {
        private IAcc mOriginalAcc;
        private bool mSelected;
        private List<CandidateBcc> mCandidateBccs; 
        private List<CandidateAbie> mCandidateAbies;

        public CandidateAcc(IAcc originalAcc)
        {
            OriginalAcc = originalAcc;
            mCandidateBccs = new List<CandidateBcc>();
            mCandidateAbies = new List<CandidateAbie>();
        }

        public IAcc OriginalAcc
        {
            get { return mOriginalAcc; }
            set { mOriginalAcc = value;}
        }
    }
}