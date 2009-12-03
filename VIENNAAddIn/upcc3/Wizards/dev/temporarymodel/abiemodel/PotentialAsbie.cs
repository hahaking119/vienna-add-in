using System;
using CctsRepository.CcLibrary;

namespace VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.abiemodel
{
    public class PotentialAsbie
    {        
        private string mName;
        private bool mChecked;
        private IAscc mBasedOn;

        public PotentialAsbie(IAscc originalAscc)
        {
            mName = originalAscc.Name;
            mChecked = false;
            mBasedOn = originalAscc;
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

        public IAscc BasedOn
        {
            get { return mBasedOn; }
            set { mBasedOn = value; }
        }
    }
}