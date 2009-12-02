using System.Collections.Generic;
using CctsRepository.BieLibrary;

namespace VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.abiemodel
{
    public class CandidateAbie
    {
        private IAbie mOriginalAbie;
        private bool mChecked;
        private List<PotentialAsbie> mPotentialAsbies;

        public CandidateAbie()
        {
            mPotentialAsbies = new List<PotentialAsbie>();
        }
    }
}