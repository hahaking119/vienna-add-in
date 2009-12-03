using System;
using System.Collections.Generic;
using CctsRepository.CcLibrary;

namespace VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.abiemodel
{
    public class CandidateBcc
    {
        private bool mChecked;
        private IBcc mOriginalBcc;
        private bool mSelected;
        private List<PotentialBbie> mPotentialBbies;

        public CandidateBcc(IBcc originalBcc)
        {
            mChecked = false;
            mOriginalBcc = originalBcc;
            mSelected = false;
            mPotentialBbies = null;
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
                if (mPotentialBbies == null)
                {
                    mPotentialBbies = new List<PotentialBbie> { new PotentialBbie(mOriginalBcc.Name, mOriginalBcc.Cdt) };
                }
                
                return mPotentialBbies;
            }

            set { mPotentialBbies = value; }
        }

        public void AddPotentialBbie()
        {
            for (int i = 1; i != -1; i++)
            {
                bool foundBbieWithTheSameName = false;
                string newBbieName = OriginalBcc.Name + i;                

                foreach (PotentialBbie potentialBbie in PotentialBbies)
                {
                    if (potentialBbie.Name.Equals(newBbieName))
                    {
                        foundBbieWithTheSameName = true;
                        break;
                    }
                }

                if (!foundBbieWithTheSameName)
                {
                    PotentialBbies.Add(new PotentialBbie(newBbieName, OriginalBcc.Cdt));                    
                    break;
                }
            } 
        }
    }
}