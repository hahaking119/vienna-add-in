using System;
using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.abiemodel
{
    public class PotentialBbie
    {
        private string mName;
        private bool mChecked;
        private bool mSelected;
        private List<PotentialBdt> mPotentialBdts;

        public PotentialBbie(string bbieName)
        {
            mName = bbieName;
            mChecked = false;
            mSelected = false;
            mPotentialBdts = null;                       
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

        public List<PotentialBdt> PotentialBdts
        {
            get
            {
                throw new NotImplementedException();

                if (mPotentialBdts== null)
                {

                }

                return mPotentialBdts;
            }
        }
    }
}