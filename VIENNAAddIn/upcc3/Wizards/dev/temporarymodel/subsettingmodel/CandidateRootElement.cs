using System.Collections.Generic;
using CctsRepository.DocLibrary;

namespace VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.subsettingmodel
{
    public class CandidateRootElement
    {
        private IMa mOriginalMa;
        private bool mSelected;
        private List<CandidateAbie> mCandidateAbies;

        public CandidateRootElement(IMa ma)
        {
            mOriginalMa = ma;
            mSelected = false;
            mCandidateAbies = null;
        }

        public IMa OriginalMa
        {
            set { mOriginalMa = value; }
            get { return mOriginalMa; }
        }

        public bool Selected
        {
            set { mSelected = value; }
            get { return mSelected; }
        }

        public List<CandidateAbie> CandidateAbies
        {
            set { mCandidateAbies = value; }

            get { return mCandidateAbies; }
        }
    }
}