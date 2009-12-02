using System;
using System.Collections.Generic;
using CctsRepository.BieLibrary;

namespace VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.abiemodel
{
    public class CandidateAbie
    {
        private string mName;
        private bool mChecked;
        private IAbie mOriginalAbie;
        private bool mSelected; 
        private List<PotentialAsbie> mPotentialAsbies;

        public CandidateAbie(IAbie originalAbie)
        {
            mName = originalAbie.Name;
            mChecked = false;
            mOriginalAbie = originalAbie;
            mSelected = false;
            mPotentialAsbies = null;
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

        public bool Selected
        {
            get { return mSelected; }
            set { mSelected = value; }
        }

        public IAbie OriginalAbie
        {
            get { return mOriginalAbie; }
            set { mOriginalAbie = value; }
        }

        public List<PotentialAsbie> PotentialAsbies
        {
            get
            {
                throw new NotImplementedException();

                if (mPotentialAsbies == null)
                {

                }

                return mPotentialAsbies;
            }
        }
    }
}