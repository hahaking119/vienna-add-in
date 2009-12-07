using System;
using System.Collections.Generic;
using System.Windows.Input;
using CctsRepository.CcLibrary;

namespace VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.abiemodel
{
    public class CandidateBcc
    {
        private bool mChecked;
        private IBcc mOriginalBcc;
        private bool mSelected;
        private List<PotentialBbie> mPotentialBbies;
        private bool mItemReadOnly;
        private Cursor mItemCursor;
        private bool mItemFocusable;

        public CandidateBcc(IBcc originalBcc)
        {
            mChecked = false;
            mOriginalBcc = originalBcc;
            mSelected = false;
            mPotentialBbies = null;

            mItemReadOnly = true;
            mItemCursor = Cursors.Arrow;
            mItemFocusable = false;
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

        public bool ItemReadOnly
        {
            get { return mItemReadOnly; }
        }

        public Cursor ItemCursor
        {
            get { return mItemCursor; }
        }

        public bool ItemFocusable
        {
            get { return mItemFocusable; }
        }

        public void AddPotentialBbieAndCheckIfApplicable()
        {
            for (int i = 1; i != -1; i++)
            {
                bool foundBbieWithTheSameName = false;
                string newBbieName = "New" + i + OriginalBcc.Name;                

                foreach (PotentialBbie potentialBbie in mPotentialBbies)
                {
                    if (potentialBbie.Name.Equals(newBbieName))
                    {
                        foundBbieWithTheSameName = true;
                    }

                }

                if (!foundBbieWithTheSameName)
                {
                    PotentialBbie newPotentialBbie = new PotentialBbie(newBbieName, OriginalBcc.Cdt);

                    if (mChecked)
                    {
                        newPotentialBbie.Checked = true;
                    }

                    mPotentialBbies.Add(newPotentialBbie);

                    break;
                }
            } 
        }

    }
}