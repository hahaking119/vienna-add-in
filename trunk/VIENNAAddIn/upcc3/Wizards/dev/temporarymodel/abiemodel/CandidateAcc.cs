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
            mCandidateBccs = null;
            mCandidateAbies = null;
        }

        public IAcc OriginalAcc
        {
            get { return mOriginalAcc; }
            set { mOriginalAcc = value;}
        }

        public bool Selected
        {
            get { return mSelected; }
            set { mSelected = value; }
        }

        public List<CandidateBcc> CandidateBccs
        {
            get
            {
                if (mCandidateBccs == null)
                {                    
                    mCandidateBccs = new List<CandidateBcc>();                    
                    
                    foreach (IBcc bcc in OriginalAcc.BCCs)
                    {
                        mCandidateBccs.Add(new CandidateBcc(bcc));
                    }
                    
                }
                
                return mCandidateBccs;
            }
        }
    }
}