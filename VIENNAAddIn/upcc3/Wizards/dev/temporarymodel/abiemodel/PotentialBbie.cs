using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.abiemodel
{
    public class PotentialBbie
    {
        private string mName;
        private bool mChecked;
        private List<PotentialBdt> mPotentialBDTs;

        public PotentialBbie()
        {
            mPotentialBDTs = new List<PotentialBdt>();
        }
    }
}