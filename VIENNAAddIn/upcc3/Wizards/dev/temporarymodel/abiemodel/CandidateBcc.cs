using System.Collections.Generic;
using CctsRepository.CcLibrary;

namespace VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.abiemodel
{
    public class CandidateBcc
    {
        private string mName;
        private bool mChecked;
        private IBcc mOriginalBcc;
        private List<PotentialBbie> mPotentialBbies;

        public CandidateBcc(IBcc initOriginalBcc)
        {
            mName = initOriginalBcc.Name;
            mChecked = false;
            mOriginalBcc = initOriginalBcc;
            mPotentialBbies = null;
        }

        public IBcc OriginalBcc
        {
            get { return mOriginalBcc; }
            set { mOriginalBcc = value; }
        }

        public string Name
        {
            get { return mName; }
            set { mName = value; }
        }

        public bool Checked
        {
            get { return mChecked; }
            set { mChecked = value; }
        }
    }
}