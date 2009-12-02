using CctsRepository.BieLibrary;

namespace VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.abiemodel
{
    public class CandidateBieLibrary
    {
        private IBieLibrary mOriginalBieLibrary;
        private bool mSelected;

        public CandidateBieLibrary(IBieLibrary bieLibrary)
        {
            mOriginalBieLibrary = bieLibrary;
            mSelected = false;            
        }

        public IBieLibrary OriginalBieLibrary
        {
            set { mOriginalBieLibrary = value; }
            get { return mOriginalBieLibrary; }
        }

        public bool Selected
        {
            set { mSelected = value; }
            get { return mSelected; }
        }
    }
}