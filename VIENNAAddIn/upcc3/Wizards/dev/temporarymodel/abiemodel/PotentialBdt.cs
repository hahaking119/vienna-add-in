using CctsRepository.BdtLibrary;

namespace VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.abiemodel
{
    public class PotentialBdt
    {
        private string mName;
        private bool mChecked;        
        private IBdt mOriginalBDT;

        public PotentialBdt(string newBdtName)
        {
            mName = newBdtName;
            mChecked = false;
            mOriginalBDT = null;
        }

        public PotentialBdt(IBdt originalBdt)
        {
            mName = originalBdt.Name;
            mChecked = false;
            mOriginalBDT = originalBdt;
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

        public IBdt OriginalBdt
        {
            get { return mOriginalBDT; }
            set { mOriginalBDT = value; }
        }
    }
}