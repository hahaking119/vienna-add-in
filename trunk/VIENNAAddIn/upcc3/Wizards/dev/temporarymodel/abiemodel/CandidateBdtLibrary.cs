using CctsRepository.BdtLibrary;

namespace VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.abiemodel
{
    public class CandidateBdtLibrary
    {
        private IBdtLibrary mOriginalBdtLibrary;
        private bool mSelected;

        public CandidateBdtLibrary(IBdtLibrary bdtLibrary)
        {
            mOriginalBdtLibrary = bdtLibrary;
            mSelected = false;            
        }

        public IBdtLibrary OriginalBdtLibrary
        {
            set { mOriginalBdtLibrary = value; }
            get { return mOriginalBdtLibrary; }
        }

        public bool Selected
        {
            set { mSelected = value; }
            get { return mSelected; }
        }
    }
}