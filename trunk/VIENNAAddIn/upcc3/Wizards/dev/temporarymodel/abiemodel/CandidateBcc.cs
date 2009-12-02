using System;
using System.Collections.Generic;
using CctsRepository.CcLibrary;

namespace VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.abiemodel
{
    public class CandidateBcc
    {
        private string mName;
        private bool mChecked;
        private IBcc mOriginalBcc;
        private bool mSelected;
        private List<PotentialBbie> mPotentialBbies;

        public CandidateBcc(IBcc originalBcc)
        {
            mName = originalBcc.Name;
            mChecked = false;
            mOriginalBcc = originalBcc;
            mSelected = false;
            mPotentialBbies = null;
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

        public IBcc OriginalBcc
        {
            get { return mOriginalBcc; }
            set { mOriginalBcc = value; }
        }

        public bool Selected
        {
            get { return mSelected; }
            set { mSelected = value; }
        }

        public List<PotentialBbie> PotentialBbies
        {
            get
            {
                throw new NotImplementedException();

                if (mPotentialBbies == null)
                {

                }

                return mPotentialBbies;
            }
        }
    }
}