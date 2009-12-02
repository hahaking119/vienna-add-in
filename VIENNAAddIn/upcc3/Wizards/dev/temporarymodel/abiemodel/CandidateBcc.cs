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
            mPotentialBbies = new List<PotentialBbie>();
        }
    }
}