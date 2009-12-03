using System.Collections.Generic;
using CctsRepository.BdtLibrary;
using CctsRepository.CdtLibrary;
using VIENNAAddIn.upcc3.Wizards.dev.cache;

namespace VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.abiemodel
{
    public class PotentialBbie
    {
        private string mName;
        private bool mChecked;
        private bool mSelected;
        private ICdt mCdtUsedInBcc;
        private List<PotentialBdt> mPotentialBdts;

        public PotentialBbie(string bbieName, ICdt cdtOfTheBccWhichTheBbieIsBasedOn)
        {
            mName = bbieName;
            mCdtUsedInBcc = cdtOfTheBccWhichTheBbieIsBasedOn;
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
                if (mPotentialBdts == null)
                {
                    CcCache ccCache = CcCache.GetInstance();

                    mPotentialBdts = new List<PotentialBdt>();

                    foreach (IBdtLibrary bdtLibrary in ccCache.GetBDTLibraries())
                    {
                        foreach (IBdt bdt in ccCache.GetBdtsFromBdtLibrary(bdtLibrary.Name))
                        {
                            if (bdt.BasedOn.Id == mCdtUsedInBcc.Id)
                            {
                                mPotentialBdts.Add(new PotentialBdt(bdt));
                            }
                        }
                    }
                }

                return mPotentialBdts;
            }

            set { mPotentialBdts = value; }
        }
    }
}